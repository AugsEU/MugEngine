

namespace MugEngine.Scene;

public abstract class MSPhysicalActor : MSActor
{
	protected Vector2 mVelocity = Vector2.Zero;

	protected MCardDir mGravityDir = MCardDir.Down;
	protected float mGravityStrength = 0.0f;

	public MSPhysicalActor(Vector2 position) : base(position)
	{
	}

	public override void Update(MUpdateInfo info)
	{
		mVelocity += mGravityDir.ToVec() * mGravityStrength * info.mDelta;

		Vector2 delta = info.mDelta * mVelocity;

		MoveX(delta.X, false);
		MoveY(delta.Y, false);
	}

	public MCardDir GetGravityDir()
	{
		return mGravityDir;
	}

	public void SetGravityDir(MCardDir dir)
	{
		mGravityDir = dir;
	}

	public Vector2 GetVelocity()
	{
		return mVelocity;
	}

	public Vector2 GetHorzVelocity()
	{
		Vector2 sideVec = mGravityDir.ToVec().Perpendicular();
		return Vector2.Dot(sideVec, mVelocity) * sideVec;
	}

	public float GetHorzSpeed()
	{
		Vector2 sideVec = mGravityDir.ToVec().Perpendicular();
		return Vector2.Dot(sideVec, mVelocity);
	}

	public void SetHorzSpeed(float speed)
	{
		Vector2 sideVec = mGravityDir.ToVec().Perpendicular();
		mVelocity += (speed - Vector2.Dot(mVelocity, sideVec)) * sideVec;
	}

	public Vector2 GetVertVelocity()
	{
		Vector2 downVec = mGravityDir.ToVec();
		return Vector2.Dot(downVec, mVelocity) * downVec;
	}

	public float GetVertSpeed()
	{
		Vector2 downVec = mGravityDir.ToVec();
		return Vector2.Dot(downVec, mVelocity);
	}

	public void SetVertSpeed(float speed)
	{
		Vector2 downVec = mGravityDir.ToVec();
		mVelocity += (speed - Vector2.Dot(mVelocity, downVec)) * downVec;
	}

	public (Vector2, Vector2) GetVelocityBasis()
	{
		return (GetHorzVelocity(), GetVertVelocity());
	}
}

