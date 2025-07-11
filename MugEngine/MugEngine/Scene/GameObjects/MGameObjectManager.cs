namespace MugEngine.Scene
{
	public class MGameObjectManager : MComponent, IMSubclassFactory<MPooledGameObject>
	{
		#region rMembers

		MObjectPool<MPooledGameObject> mPooledObjects;
		List<MGameObject> mObjects;
		List<MGameObject> mUpdateList;
		MLevel mLevel;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a game object manager.
		/// </summary>
		public MGameObjectManager()
		{
			mPooledObjects = new MObjectPool<MPooledGameObject>();
			mObjects = new List<MGameObject>();
			mUpdateList = new List<MGameObject>(1024);
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update game objects
		/// </summary>
		public override void Update(MUpdateInfo info)
		{
			mLevel?.Update(GetParent(), info);

			// Copy over in case collection changes. @perf
			mUpdateList.Clear();
			foreach(MGameObject go in ActiveObjects())
			{
				mUpdateList.Add(go);
			}

			foreach(MGameObject go in mUpdateList)
			{
				go.Update(info);
			}
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw all the game objects
		/// </summary>
		public override void Draw(MDrawInfo info)
		{
			mLevel?.Draw(GetParent(), info);

			foreach (MGameObject go in ActiveObjects())
			{
				go.Draw(info);
			}
		}

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Queue an object to be deleted at the end of the frame.
		/// </summary>
		public void Delete(MGameObject go)
		{
			if (mObjects.Remove(go))
			{
				// Do nothing.
			}
			else if(go is MPooledGameObject pgo)
			{
				mPooledObjects.Destroy(pgo);
			}
		}



		/// <summary>
		/// Queue an object to be deleted at the end of the frame.
		/// </summary>
		public T CreatePooled<T>() where T : MPooledGameObject, new()
		{
			return mPooledObjects.GetFreshInstance<T>(this);
		}



		/// <summary>
		/// Deletes all game objects. Doesn't clear out level.
		/// </summary>
		public void ClearAllGameObjects()
		{
			mPooledObjects.Clear();
			mObjects.Clear();
		}



		/// <summary>
		/// Deletes all game objects. Doesn't clear out level.
		/// </summary>
		public void ClearPoolGameObjects()
		{
			mPooledObjects.Clear();
		}



		/// <summary>
		/// Create a new gameobject subclass
		/// </summary>
		public U CreateNew<U>() where U : MPooledGameObject, new()
		{
			U newGameObject = new();
			newGameObject.SetScene(GetParent());
			return newGameObject;
		}



		/// <summary>
		/// Add game object to be updated.
		/// </summary>
		/// <param name="go"></param>
		public void Add(MGameObject go)
		{
			mObjects.Add(go);
			go.SetScene(GetParent());
			go.PostInitSetup();
		}

		#endregion rUtil





		#region rAccess

		/// <summary>
		/// Get iterator over active objects.
		/// </summary>
		public IEnumerable<MGameObject> ActiveObjects()
		{
			foreach(MGameObject go in mPooledObjects.GetEnumerator())
			{
				if (go.IsEnabled())
				{
					yield return go;
				}
			}

			foreach(MGameObject go in mObjects)
			{
				if (go.IsEnabled())
				{
					yield return go;
				}
			}
		}



		/// <summary>
		/// Get iterator over active objects on a layer.
		/// </summary>
		public IEnumerable<MGameObject> ActiveObjects(MLayerMask mask)
		{
			foreach (MGameObject go in ActiveObjects())
			{
				if (go.InteractsWith(mask))
				{
					yield return go;
				}
			}
		}



		/// <summary>
		/// Get the first game object of a certain type.
		/// </summary>
		public MGameObject GetFirst<T>()
		{
			foreach (MGameObject go in ActiveObjects())
			{
				if (go.GetType() == typeof(T))
				{
					return go;
				}
			}

			return null;
		}



		/// <summary>
		/// Get all game objects of a certain type.
		/// </summary>
		public IEnumerable<MGameObject> GetAll<T>()
		{
			foreach (MGameObject go in ActiveObjects())
			{
				if (go is T)
				{
					yield return go;
				}
			}
		}



		/// <summary>
		/// Get all game objects in a rectangle.
		/// </summary>
		public IEnumerable<MGameObject> GetInRect(Rectangle rect)
		{
			// TO DO: Make this performant
			foreach (MGameObject go in ActiveObjects())
			{
				if (go.BoundsRect().Intersects(rect))
				{
					yield return go;
				}
			}
		}



		/// <summary>
		/// Get all game objects in a rectangle.
		/// </summary>
		public IEnumerable<MGameObject> GetInRect(Rectangle rect, MLayerMask layers)
		{
			foreach (MGameObject go in ActiveObjects(layers))
			{
				if (go.BoundsRect().Intersects(rect))
				{
					yield return go;
				}
			}
		}



		/// <summary>
		/// Add some terrain that things can collide with.
		/// </summary>
		public void LoadLevel(MLevel level)
		{
			// Unload previous level.
			UnloadLevel();

			// Start new level
			mLevel = level;
			level.BeginLevel(this);
		}



		/// <summary>
		/// Stop a level
		/// </summary>
		public void UnloadLevel()
		{
			if (mLevel is not null)
			{
				mLevel.EndLevel(this);
				mLevel = null;
			}
		}



		/// <summary>
		/// Get terrain things collide with
		/// </summary>
		public MLevel GetLevel()
		{
			return mLevel;
		}

		#endregion rAccess





		#region rCollision

		/// <summary>
		/// Query for collision with the level
		/// </summary>
		public bool QueryLevelCollision(Rectangle rect, MCardDir direction, CollisionFlags flags)
		{
			if (mLevel is null)
			{
				return false;
			}

			return mLevel.QueryCollides(rect, direction, flags);
		}



		/// <summary>
		/// Does the given go collide with the bounds of any other entity?
		/// </summary>
		public bool CheckBoundsCollision(MGameObject go)
		{
			Rectangle goBounds = go.BoundsRect();

			// Check entity v entity
			foreach (MGameObject goOther in GetInRect(goBounds, go.GetLayerMask()))
			{
				if (ReferenceEquals(goOther, go))
				{
					continue;
				}

				if (goOther.BoundsRect().Intersects(goBounds))
				{
					return true;
				}
			}

			// None of the tests hit so we are good.
			return false;
		}

		#endregion rCollision
	}
}
