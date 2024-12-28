namespace MugEngine.Scene
{
	/// <summary>
	/// A collection of things that make up our game.
	/// </summary>
	public class MScene : IMUpdate, IMDraw
	{
		#region rMembers

		Dictionary<Type, MComponent> mUniqueEntities;
		List<MComponent> mEntities;
		bool mEntityOrderDirty = true;

		HashSet<MComponent> mDeletePool;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a scene
		/// </summary>
		public MScene()
		{
			mUniqueEntities = new Dictionary<Type, MComponent>();
			mEntities = new List<MComponent>();

			mDeletePool = new HashSet<MComponent>();
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update all entities in the scene.
		/// </summary>
		public void Update(MUpdateInfo info)
		{
			if (mEntityOrderDirty)
			{
				mEntities.Sort(new MEntityUpdateOrderComparer());
			}

			// Update entities in scene.
			for (int i = 0; i < mEntities.Count; i++)
			{
				mEntities[i].Update(info);
			}

			ResolveDeleteQueue();
		}



		/// <summary>
		/// Delete entities requested for deletion.
		/// </summary>
		public void ResolveDeleteQueue()
		{
			for (int i = 0; i < mEntities.Count && mDeletePool.Count > 0; i++)
			{
				MComponent entity = mEntities[i];
				if (mDeletePool.Contains(entity))
				{
					mDeletePool.Remove(entity);
					mEntities.RemoveAt(i);

					Type entType = entity.GetType();
					if (mUniqueEntities.ContainsKey(entType))
					{
						mUniqueEntities.Remove(entType);
					}

					i--;
				}
			}

			MugDebug.Assert(mDeletePool.Count == 0, "Queued entities to delete that aren't in the list.");
			mDeletePool.Clear();
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw the scene
		/// </summary>
		public void Draw(MDrawInfo info)
		{
			for (int i = 0; i < mEntities.Count; i++)
			{
				mEntities[i].Draw(info);
			}
		}

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Add an entity to the scene.
		/// </summary>
		public void AddEntity(MComponent entity)
		{
			mEntities.Add(entity);
			entity.SetScene(this);

			mEntityOrderDirty = true;
		}



		/// <summary>
		/// Add a unique entity.
		/// </summary>
		public void AddUnique(MComponent entity)
		{
			Type entType = entity.GetType();
			MugDebug.Assert(!mUniqueEntities.ContainsKey(entType), "Cannot add unique entity of type more than once.");

			mUniqueEntities.Add(entType, entity);

			// Also add it to big list.
			AddEntity(entity);
		}



		/// <summary>
		/// Prepare entity for deletion.
		/// </summary>
		public void QueueRemove(MComponent entity)
		{
			mDeletePool.Add(entity);
		}

		#endregion rUtil





		#region rAccess

		/// <summary>
		/// Get a unique entity.
		/// </summary>
		public T Get<T>() where T : MComponent
		{
			return (T)mUniqueEntities.GetValueOrDefault(typeof(T), default(T));
		}



		/// <summary>
		/// Get an entity
		/// </summary>
		public MComponent Get(int index)
		{
			return mEntities[index];
		}



		/// <summary>
		/// Get number of entities
		/// </summary>
		public int GetNum()
		{
			return mEntities.Count;
		}



		/// <summary>
		/// Get tile map
		/// </summary>
		public MLevel LVL { get { return Get<MGameObjectManager>().GetLevel(); } }



		/// <summary>
		/// Game game object manager.
		/// </summary>
		public MGameObjectManager GO { get { return Get<MGameObjectManager>(); } }


#if MUG_PHYSICS
		/// <summary>
		/// Physics world
		/// </summary>
		public MPhysicsWorld PW { get { return Get<MPhysicsWorld>(); } }
#endif

		#endregion rAccess
	}
}
