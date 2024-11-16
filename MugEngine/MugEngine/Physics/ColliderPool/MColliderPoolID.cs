namespace MugEngine.Physics
{
	/// <summary>
	/// An ID for a collider pool
	/// </summary>
	struct MColliderPoolID
	{
		public MColliderType mColliderType;
		public int mIndex;

		public MColliderPoolID(MColliderType type, int index)
		{
			mColliderType = type;
			mIndex = index;
		}
	}
}
