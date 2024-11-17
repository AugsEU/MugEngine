using MugEngine.Scene;

namespace MugEngine.Physics
{
	struct MKinematicSubmission
	{
		public MColliderPoolID mColliderID;
		public Vector2 mVelocity;
		public MGameObject mSenderObject;

		public MKinematicSubmission(MColliderPoolID id, Vector2 velocity, MGameObject sender)
		{
			mColliderID = id;
			mVelocity = velocity;
			mSenderObject = sender;
		}
	}
}
