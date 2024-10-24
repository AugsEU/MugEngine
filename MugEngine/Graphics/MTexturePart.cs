namespace MugEngine.Graphics
{
	/// <summary>
	/// Represents a part of a texture.
	/// </summary>
	public struct MTexturePart
	{
		public readonly static MTexturePart Empty = new MTexturePart(null, Rectangle.Empty);

		public Rectangle mUV;
		public Texture2D mTexture;

		public MTexturePart(Texture2D texture, Rectangle uv)
		{
			mUV = uv;
			mTexture = texture;
		}

		public MTexturePart(Texture2D texture, int x, int y, int width, int height)
		{
			mUV = new Rectangle(x, y, width, height);
			mTexture = texture;
		}

		public int Width()
		{
			return mUV.Width;
		}

		public int Height()
		{
			return mUV.Height;
		}

		public bool IsNull()
		{
			return mTexture == null;
		}
	}
}
