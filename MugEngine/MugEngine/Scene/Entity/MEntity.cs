using MugEngine.Types;

namespace MugEngine.Scene
{
	/// <summary>
	/// An entity is a thing managed by the scene.
	/// </summary>
	public abstract class MEntity : IMSceneUpdate, IMSceneDraw
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
		public abstract void Update(MScene scene, MUpdateInfo info);


		/// <summary>
		/// Called every frame draw the entity.
		/// </summary>
		public abstract void Draw(MScene scene, MDrawInfo info);
	}
}
