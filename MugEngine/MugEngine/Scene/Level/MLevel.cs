namespace MugEngine.Scene
{
	/// <summary>
	/// A level is responsible for the terrain.
	/// It also spawns in all the components.
	/// It is sort of like the "master puppeteer" of a scene.
	/// </summary>
	public abstract class MLevel : IMSceneUpdate, IMSceneDraw, IMCollisionQueryable
	{
		public virtual void BeginLevel(MGameObjectManager gameObjects)
		{
		}

		public virtual void EndLevel(MGameObjectManager gameObjects)
		{
		}

		public abstract void Update(MScene scene, MUpdateInfo info);

		public abstract bool QueryCollides(Rectangle bounds, MCardDir travelDir, CollisionFlags flags);

		public abstract void Draw(MScene scene, MDrawInfo info);
	}
}
