
namespace MugEngine.Library;

public class LinearTrajectory : Trajectory
{
	Vector2 mPosition;
	Vector2 mVelocity;

	public LinearTrajectory(Vector2 origin, Vector2 vel)
	{
		mPosition = origin;
		mVelocity = vel;
	}

	/// <summary>
	/// Construct linear trajectory from non-normalized dir and speed.
	/// </summary>
	public LinearTrajectory(Vector2 origin, Vector2 dir, float speed)
	{
		mPosition = origin;
		dir.Normalize();
		mVelocity = dir * speed;
	}

	public override Vector2 GetPosition()
	{
		return mPosition;
	}

	public override void Update(MUpdateInfo info)
	{
		mPosition += mVelocity * info.mDelta;
	}
}
