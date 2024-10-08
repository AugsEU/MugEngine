namespace MugEngine.Graphics
{
	public struct MStringDrawData
	{
		public SpriteFont mFont;
		public string mText;
		public Vector2 mPosition;
		public Color mColor;
		public float mRotation;
		public Vector2 mOrigin;
		public float mScale;
		public SpriteEffects mEffects;

		public MStringDrawData(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
		{
			mFont = font;
			mText = text;
			mPosition = position;
			mColor = color;
			mRotation = rotation;
			mOrigin = origin;
			mScale = scale;
			mEffects = effect;
		}
	}
}
