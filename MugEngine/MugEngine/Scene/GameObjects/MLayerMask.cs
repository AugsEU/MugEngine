namespace MugEngine.Scene
{
	public struct MLayerMask
	{
		public static MLayerMask All { get { return new MLayerMask(UInt64.MaxValue); } }

		UInt64 mMask;


		/// <summary>
		/// Create a layer mask
		/// </summary>
		public MLayerMask(UInt64 mask)
		{
			mMask = mask;
		}



		/// <summary>
		/// Create from Enum
		/// </summary>
		public static MLayerMask From<T>(T mask) where T : Enum
		{
			UInt64 uflags = Convert.ToUInt64(mask);

			return new MLayerMask(uflags);
		}



		/// <summary>
		/// Get layer this interacts on.
		/// </summary>
		public bool InteractsWith(MLayerMask other)
		{
			return (other.mMask & mMask) != 0;
		}



		/// <summary>
		/// Set interaction layers specifically.
		/// Clears all other layers.
		/// </summary>
		public void SetLayer<T>(T layer) where T : Enum
		{
			UInt64 mask = Convert.ToUInt64(layer);

			mMask = 0;
			mMask |= mask;
		}



		/// <summary>
		/// Add to an interaction layer.
		/// Does not clear other layers
		/// </summary>
		public void AddLayer<T>(T layer) where T : Enum
		{
			UInt64 mask = Convert.ToUInt64(layer);

			mMask |= mask;
		}



		/// <summary>
		/// Remove from an interaction layer.
		/// Does not clear other layers.
		/// </summary>
		public void RemoveLayer<T>(T layer) where T : Enum
		{
			UInt64 mask = Convert.ToUInt64(layer);

			mMask &= (UInt64.MaxValue ^ mask);
		}
	}
}
