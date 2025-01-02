namespace MugEngine.Scene
{
	public class MGameObjectManager : MComponent
	{
		#region rMembers

		MDelayChangeList<MGameObject> mObjects;
		MLevel mLevel;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a game object manager.
		/// </summary>
		public MGameObjectManager()
		{
			mObjects = new MDelayChangeList<MGameObject>(new List<MGameObject>());
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update game objects
		/// </summary>
		public override void Update(MUpdateInfo info)
		{
			mLevel?.Update(GetParent(), info);

			for (int i = 0; i < mObjects.Count; i++)
			{
				mObjects[i].Update(info);
			}

			mObjects.ProcessAddsDeletes();
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw all the game objects
		/// </summary>
		public override void Draw(MDrawInfo info)
		{
			mLevel?.Draw(GetParent(), info);

			for (int i = 0; i < mObjects.Count; i++)
			{
				mObjects[i].Draw(info);
			}
		}

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Queue an object to be deleted at the end of the frame.
		/// </summary>
		public void QueueDelete(MGameObject go)
		{
			mObjects.Remove(go);
		}



		/// <summary>
		/// Queue an object to be deleted at the end of the frame.
		/// </summary>
		public void QueueAdd(MGameObject go)
		{
			mObjects.Add(go);
			go.SetScene(GetParent());
			go.PostInitSetup();
		}



		/// <summary>
		/// Make sure queued entites get added immediately.
		/// </summary>
		public void FlushQueues()
		{
			mObjects.ProcessAddsDeletes();
		}



		/// <summary>
		/// Deletes all game objects. Doesn't clear out level.
		/// </summary>
		public void ClearAllGameObjects()
		{
			mObjects.ForceClearAll();
		}

		#endregion rUtil





		#region rAccess

		/// <summary>
		/// Get number of game objects
		/// </summary>
		public int GetCount()
		{
			return mObjects.Count;
		}



		/// <summary>
		/// Get a game object at index
		/// </summary>
		public MGameObject Get(int index)
		{
			return mObjects[index];
		}



		/// <summary>
		/// Get iterator over active objects.
		/// </summary>
		public IEnumerable<MGameObject> ActiveObjects()
		{
			return mObjects.Where(e => e.IsEnabled());
		}



		/// <summary>
		/// Get iterator over active objects on a layer.
		/// </summary>
		public IEnumerable<MGameObject> ActiveObjects(MLayerMask mask)
		{
			return mObjects.Where(e => e.IsEnabled() && e.InteractsWith(mask));
		}



		/// <summary>
		/// Get the first game object of a certain type.
		/// </summary>
		public MGameObject GetFirst<T>()
		{
			return ActiveObjects().FirstOrDefault(go => go.GetType() == typeof(T));
		}



		/// <summary>
		/// Get all game objects of a certain type.
		/// </summary>
		public IEnumerable<MGameObject> GetAll<T>()
		{
			return ActiveObjects().Where(o => o is T);
		}



		/// <summary>
		/// Get all game objects in a rectangle.
		/// </summary>
		public IEnumerable<MGameObject> GetInRect(Rectangle rect)
		{
			return ActiveObjects().Where(o => o.BoundsRect().Intersects(rect));
		}



		/// <summary>
		/// Get all game objects in a rectangle.
		/// </summary>
		public IEnumerable<MGameObject> GetInRect(Rectangle rect, MLayerMask layers)
		{
			return ActiveObjects(layers).Where(o => (o.BoundsRect().Intersects(rect)));
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
		public bool QueryLevelCollision(Rectangle rect, MCardDir direction)
		{
			if (mLevel is null)
			{
				return false;
			}

			return mLevel.QueryCollides(rect, direction);
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
