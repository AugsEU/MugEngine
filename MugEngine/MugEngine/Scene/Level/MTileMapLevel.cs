namespace MugEngine.Scene.Level
{
	public class MTileMapLevel<P> : MLevel where P : struct, IMTilePolicy
	{
		MTileMap<P> mTileMap;

		public MTileMapLevel(MTileMap<P> tileMap) : base()
		{
			mTileMap = tileMap;
		}

		public override void Update(MScene scene, MUpdateInfo info)
		{
			mTileMap.Update(scene, info);
		}

		public override bool QueryCollides(Rectangle bounds, MCardDir travelDir)
		{
			return mTileMap.QueryCollides(bounds, travelDir);
		}

		public override void Draw(MScene scene, MDrawInfo info)
		{
			mTileMap.Draw(scene, info);
		}
	}
}
