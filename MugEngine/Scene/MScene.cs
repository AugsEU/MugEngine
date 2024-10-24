using MugEngine.Core;
using MugEngine.Interface;
using MugEngine.Types;
using System.Data.SqlTypes;
using System.Dynamic;

namespace MugEngine.Scene
{
	/// <summary>
	/// A collection of things that make up our game.
	/// </summary>
	public class MScene : IMUpdate, IMDraw
	{
		#region rMembers

		Dictionary<Type, MEntity> mUniqueEntities;
		List<MEntity> mEntities;

		HashSet<MEntity> mDeletePool;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a scene
		/// </summary>
		public MScene()
		{
			mUniqueEntities = new Dictionary<Type, MEntity>();
			mEntities = new List<MEntity>();

			mDeletePool = new HashSet<MEntity>();
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update all entities in the scene.
		/// </summary>
		public void Update(MUpdateInfo info)
		{
			// Update entities in scene.
			for(int i = 0; i < mEntities.Count; i++)
			{
				mEntities[i].Update(this, info);
			}

			ResolveDeleteQueue();
		}



		/// <summary>
		/// Delete entities requested for deletion.
		/// </summary>
		public void ResolveDeleteQueue()
		{
			for(int i = 0; i < mEntities.Count && mDeletePool.Count > 0; i++)
			{
				MEntity entity = mEntities[i];
				if (mDeletePool.Contains(entity))
				{
					mDeletePool.Remove(entity);
					mEntities.RemoveAt(i);

					Type entType = entity.GetType();
					if(mUniqueEntities.ContainsKey(entType))
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
				mEntities[i].Draw(this, info);
			}
		}

		#endregion rDraw



		#region rUtil

		/// <summary>
		/// Add an entity to the scene.
		/// </summary>
		public void AddEntity(MEntity entity)
		{
			mEntities.Add(entity);
			entity.OnSceneAdd(this);
		}



		/// <summary>
		/// Add a unique entity.
		/// </summary>
		public void AddUnique(MEntity entity)
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
		public void QueueRemove(MEntity entity)
		{
			mDeletePool.Add(entity);
		}

		#endregion rUtil



		#region rAccess

		/// <summary>
		/// Get a unique entity.
		/// </summary>
		public T Get<T>() where T : MEntity
		{
			return (T)mUniqueEntities.GetValueOrDefault(typeof(T), default(T));
		}



		/// <summary>
		/// Get an entity
		/// </summary>
		public MEntity Get(int index)
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

		#endregion rAccess
	}
}
