

namespace MugEngine.Scene
{
	public abstract class MSPhysicalActor : MSActor
	{
		protected Vector2 mVelocity = Vector2.Zero;

		protected MCardDir mGravityDir = MCardDir.Down;
		protected float mGravityStrength = -9.8f;

		public MSPhysicalActor(Vector2 position) : base(position)
		{

		}

		public MSPhysicalActor(Vector2 position, Point size) : base(position, size)
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
	}
}
