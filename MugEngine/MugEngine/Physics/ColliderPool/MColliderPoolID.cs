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

		public override bool Equals(object obj)
		{
			return obj is MColliderPoolID id && Equals(id);
		}

		public bool Equals(MColliderPoolID other)
		{
			return mColliderType == other.mColliderType &&
				   mIndex == other.mIndex;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(mColliderType, mIndex);
		}

		public static bool operator ==(MColliderPoolID left, MColliderPoolID right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MColliderPoolID left, MColliderPoolID right)
		{
			return !(left == right);
		}
	}
}
