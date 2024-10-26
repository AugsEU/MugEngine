using MugEngine.Collections;
using MugEngine.Types;

namespace MugEngine.Scene
{
	public class MGameObjectManager : MEntity
	{
		#region rMembers

		MDelayDeleteList<MGameObject> mObjects;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a game object manager.
		/// </summary>
		public MGameObjectManager()
		{
			mObjects = new MDelayDeleteList<MGameObject>(new List<MGameObject>());
		}



		/// <summary>
		/// Called when added to the scene.
		/// </summary>
		public override void OnSceneAdd(MScene scene)
		{
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update game objects
		/// </summary>
		public override void Update(MScene scene, MUpdateInfo info)
		{
			for (int i = 0; i < mObjects.Count; i++)
			{
				mObjects[i].Update(scene, info);
			}

			mObjects.ProcessDeletes();
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw all the game objects
		/// </summary>
		public override void Draw(MScene scene, MDrawInfo info)
		{
			for (int i = 0; i < mObjects.Count; i++)
			{
				mObjects[i].Draw(scene, info);
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
		/// Get the first game object of a certain type.
		/// </summary>
		public MGameObject GetFirst<T>()
		{
			foreach (MGameObject go in mObjects)
			{
				if (go.GetType() == typeof(T))
				{
					return go;
				}
			}

			throw new Exception("Could not find game object of type.");
		}



		/// <summary>
		/// Get the first game object of a certain type.
		/// </summary>
		public List<MGameObject> GetAll<T>()
		{
			List<MGameObject> list = new List<MGameObject>();

			foreach (MGameObject go in mObjects)
			{
				if (go is T)
				{
					list.Add(go);
				}
			}

			return list;
		}

		#endregion rAccess
	}
}
