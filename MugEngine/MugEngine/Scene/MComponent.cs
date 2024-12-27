using MugEngine.Core.Types;

namespace MugEngine.Scene
{
	/// <summary>
	/// An entity is a thing managed by the scene.
	/// </summary>
	public abstract class MComponent : IMSceneUpdate, IMSceneDraw
	{
		/// <summary>
		/// Callled when added to the scene.
		/// </summary>
		public virtual void OnSceneAdd(MScene scene)
		{

		}

		/// <summary>
		/// Called every frame to update the state.
		/// </summary>
		public virtual void Update(MScene scene, MUpdateInfo info)
		{

		}


		/// <summary>
		/// Called every frame draw the entity.
		/// </summary>
		public virtual void Draw(MScene scene, MDrawInfo info)
		{

		}


		/// <summary>
		/// Gets order of updates.
		/// Higher is later.
		/// </summary>
		public virtual int UpdateOrder()
		{
			return MComponentUpdateOrder.DEFAULT_ORDER;
		}
	}
}
