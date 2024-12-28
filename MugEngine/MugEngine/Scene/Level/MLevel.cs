﻿namespace MugEngine.Scene
{
	/// <summary>
	/// A level is responsible for the terrain.
	/// It also spawns in all the components.
	/// It is sort of like the "master puppeteer" of a scene.
	/// </summary>
	public abstract class MLevel : IMSceneUpdate, IMSceneDraw
	{
		public virtual void BeginLevel()
		{
		}

		public abstract void Update(MScene scene, MUpdateInfo info);

		public abstract bool QueryCollides(Rectangle bounds, MCardDir travelDir);

		public abstract void Draw(MScene scene, MDrawInfo info);
	}
}