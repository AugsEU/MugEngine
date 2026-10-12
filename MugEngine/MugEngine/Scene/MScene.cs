namespace MugEngine.Scene
{
	/// <summary>
	/// A collection of things that make up our game.
	/// </summary>
	public class MScene : IMUpdate, IMDraw
	{
		#region rMembers
 

		Dictionary<Type, MComponent> mUniqueEntities;
		List<MComponent> mComponents;
		bool mEntityOrderDirty = true;

		HashSet<MComponent> mDeletePool;

		MSceneActionItem mCurrActionItem;
		Stack<MSceneActionItem> mActionItems = new();

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a scene
		/// </summary>
		public MScene()
		{
			mUniqueEntities = new Dictionary<Type, MComponent>();
			mComponents = new List<MComponent>();

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
				mComponents.Sort(new MEntityUpdateOrderComparer());
			}

			// Action item
			// If null we must look for the next item.
			if (mCurrActionItem is null && mActionItems.TryPop(out MSceneActionItem actionItem))
			{
				mCurrActionItem = actionItem;
				mCurrActionItem.OnBegin(this);
			}

			// Update entities in scene.
			for (int i = 0; i < mComponents.Count; i++)
			{
				bool blockUpdate = (mCurrActionItem?.BlockComponentUpdate(mComponents[i])).GetValueOrDefault(false);
				if (blockUpdate)
				{
					continue;
				}

				mComponents[i].Update(info);
			}

			ResolveDeleteQueue();

			// Update action item at the end.
			if (mCurrActionItem is not null)
			{
				bool finished = mCurrActionItem.Update(info);

				if (finished)
				{
					mCurrActionItem = null;
				}
			}
		}



		/// <summary>
		/// Delete entities requested for deletion.
		/// </summary>
		public void ResolveDeleteQueue()
		{
			for (int i = 0; i < mComponents.Count && mDeletePool.Count > 0; i++)
			{
				MComponent entity = mComponents[i];
				if (mDeletePool.Contains(entity))
				{
					mDeletePool.Remove(entity);
					mComponents.RemoveAt(i);

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
			if(mCurrActionItem is not null)
			{
				mCurrActionItem.Draw(info);
			}

			for (int i = 0; i < mComponents.Count; i++)
			{
				bool blockDraw = (mCurrActionItem?.BlockComponentDraw(mComponents[i])).GetValueOrDefault(false);
				if (blockDraw)
				{
					continue;
				}

				mComponents[i].Draw(info);
			}
		}

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Add an entity to the scene.
		/// </summary>
		public void AddEntity(MComponent entity)
		{
			mComponents.Add(entity);
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


		/// <summary>
		/// Push an action item to the stack
		/// </summary>
		public void PushActionItem(MSceneActionItem item)
		{
			mActionItems.Push(item);
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
			return mComponents[index];
		}



		/// <summary>
		/// Get number of entities
		/// </summary>
		public int GetNum()
		{
			return mComponents.Count;
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
