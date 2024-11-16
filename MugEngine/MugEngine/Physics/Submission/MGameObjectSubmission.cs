using MugEngine.Scene;

namespace MugEngine.Physics
{
	struct MGameObjectSubmission
	{
		public MColliderPoolID mColliderID;
		public Vector2 mVelocity;
		public MGameObject mSenderObject;

		public MGameObjectSubmission(MColliderPoolID id, Vector2 velocity, MGameObject sender)
		{
			mColliderID = id;
			mVelocity = velocity;
			mSenderObject = sender;
		}
	}
}
