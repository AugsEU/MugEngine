namespace MugEngine.Graphics
{
	/// <summary>
	/// Struct to specify the camera parameters
	/// </summary>
	public struct MCameraSpec
	{
		public static MCameraSpec Zero { get { return new MCameraSpec(); } }

		public Vector2 mPosition;
		public float mRotation;
		public float mZoom;

		public MCameraSpec()
		{
			mPosition = Vector2.Zero;
			mRotation = 0.0f;
			mZoom = 1.0f;
		}

		public static MCameraSpec operator +(MCameraSpec a, MCameraSpec b)
		{
			return new MCameraSpec
			{
				mPosition = a.mPosition + b.mPosition,
				mRotation = a.mRotation + b.mRotation,
				mZoom = a.mZoom * b.mZoom
			};
		}
	}
}
