namespace MugEngine.Graphics
{
	public struct MTextureDrawData
	{
		public Texture2D mTexture;
		public Rectangle? mSourceRectangle;
		public Vector2 mPosition;
		public Color mColor;
		public float mRotation;
		public Vector2 mOrigin;
		public Vector2 mScale;
		public SpriteEffects mEffects;

		public MTextureDrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect)
		{
			mTexture = texture;
			mSourceRectangle = sourceRect;
			mPosition = position;
			mColor = color;
			mRotation = rotation;
			mOrigin = origin;
			mScale = scale;
			mEffects = effect;
		}
	}
}
