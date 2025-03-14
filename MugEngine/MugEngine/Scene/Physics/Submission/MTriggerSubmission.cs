#if MUG_PHYSICS
namespace MugEngine.Scene;

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

#endif
