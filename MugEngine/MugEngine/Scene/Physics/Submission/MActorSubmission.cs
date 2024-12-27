#if MUG_PHYSICS
namespace MugEngine.Scene
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
#endif
