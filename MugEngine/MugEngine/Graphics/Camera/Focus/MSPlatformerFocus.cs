

namespace MugEngine.Graphics
{
	public class MSPlatformerFocus : MSmoothPointFocus
	{
		MSPlatformingActor mActor;
		Vector4 mGroundFollowSpeed;
		Vector4 mAirFollowSpeed;

		MRollingVector2 mRollingTargetWindow;

		public MSPlatformerFocus(MSPlatformingActor actor)
		{
			mActor = actor;
			mGroundFollowSpeed = new Vector4(7.5f, 4.2f, 7.5f, 6.2f);
			mAirFollowSpeed = new Vector4(7.5f, 12.0f, 7.5f, 2.9f);

			mRollingTargetWindow = new MRollingVector2(4);
		}

		public override MCameraSpec UpdateFocusPoint(MUpdateInfo info, MCameraSpec curr)
		{
			if(mActor.OnGround())
			{
				pSpeed = mGroundFollowSpeed;
			}
			else
			{
				pSpeed = mAirFollowSpeed;
			}

			return base.UpdateFocusPoint(info, curr);
		}

		protected override Vector2 GetTargetPoint()
		{
			Vector2 targetPoint = mActor.GetCentreOfMass();

			targetPoint += mActor.GetVelocity() / 10.0f;

			Vector2 lookAhead = mActor.GetFacingDir().ToVec(mActor.GetGravityDir());
			lookAhead *= 18.0f;

			targetPoint += lookAhead;

			mRollingTargetWindow.Add(targetPoint);
			return mRollingTargetWindow.GetAverage();
		}
	}
}
