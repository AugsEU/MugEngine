namespace MugEngine.Scene
{
	/// <summary>
	/// A level is responsible for the terrain.
	/// It also spawns in all the components.
	/// It is sort of like the "master puppeteer" of a scene.
	/// </summary>
	public abstract class MLevel : IMUpdate, IMDraw, IMCollisionQueryable
	{
		MScene mScene;

		public void SetScene(MScene scene)
		{
			mScene = scene;
		}

		protected MScene GetScene()
		{
			return mScene;
		}

		public virtual void BeginLevel()
		{
		}

		public virtual void EndLevel()
		{
		}

		public abstract void Update(MUpdateInfo info);

		public abstract bool QueryCollides(Rectangle bounds, MCardDir travelDir, MCollisionFlags flags);

		public abstract void Draw(MDrawInfo info);
	}
}
