namespace MugEngine.Scene;

public class MTileMapLevel<P> : MLevel where P : struct, IMTilePolicy
{
	MTileMap<P> mTileMap;

	public MTileMapLevel(MTileMap<P> tileMap) : base()
	{
		mTileMap = tileMap;
	}

	public override void Update(MUpdateInfo info)
	{
		mTileMap.Update(GetScene(), info);
	}

	public override bool QueryCollides(Rectangle bounds, MCardDir travelDir, MCollisionFlags flags)
	{
		return mTileMap.QueryCollides(bounds, travelDir, flags);
	}

	public override void Draw(MDrawInfo info)
	{
		mTileMap.Draw(GetScene(), info);
	}
}
