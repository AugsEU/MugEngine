namespace MugEngine.Graphics.Camera
{
	/// <summary>
	/// Struct to specify the camera parameters
	/// </summary>
	public struct MCameraSpec
	{
		public MCameraSpec()
		{
			mPosition = Vector2.Zero;
			mRotation = 0.0f;
			mZoom = 1.0f;
		}

		public Vector2 mPosition;
		public float mRotation;
		public float mZoom;
	}
}
