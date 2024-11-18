using MugEngine.Scene;

namespace MugEngine.Physics.Submission
{
	struct MTriggerSubmission
	{
		public MColliderPoolID mColliderID;
		public MGameObject mSenderObject;

		public MTriggerSubmission(MColliderPoolID id, Vector2 velocity, MGameObject sender)
		{
			mColliderID = id;
			mSenderObject = sender;
		}
	}
}
