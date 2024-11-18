using MugEngine.Scene;

namespace MugEngine.Physics
{
	struct MActorSubmission
	{
		public MColliderPoolID mColliderID;
		public Vector2 mVelocity;
		public MGameObject mSenderObject;

		public MActorSubmission(MColliderPoolID id, Vector2 velocity, MGameObject sender)
		{
			mColliderID = id;
			mVelocity = velocity;
			mSenderObject = sender;
		}
	}
}
