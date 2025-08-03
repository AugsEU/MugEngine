namespace MugEngine.Scene;

public struct MSolidCollision
{
	public bool mHit;
	public MSSolid mSolid;

	private MSolidCollision(bool hit, MSSolid solid)
	{
		mHit = hit;
		mSolid = solid;
	}

	public static MSolidCollision HitSolid(MSSolid solid)
	{
		return new MSolidCollision(true, solid);
	}

	public static MSolidCollision HitLevel()
	{
		return new MSolidCollision(true, null);
	}

	public static MSolidCollision Empty()
	{
		return new MSolidCollision(false, null);
	}
}
