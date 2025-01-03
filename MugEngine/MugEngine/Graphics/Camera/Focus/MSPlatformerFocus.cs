

namespace MugEngine.Graphics
{
	public class MSPlatformerFocus : MSmoothPointFocus
	{
		MSPlatformingActor mActor;
		Vector2 mGroundFollowSpeed;
		Vector2 mAirFollowSpeed;

		public MSPlatformerFocus(MSPlatformingActor actor)
		{
			mActor = actor;
			mGroundFollowSpeed = new Vector2(4.5f, 4.2f);
			mAirFollowSpeed = new Vector2(4.5f, 2.9f);
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
			lookAhead *= 20.0f;

			targetPoint += lookAhead;

			return targetPoint;
		}
	}
}
