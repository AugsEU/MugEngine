using System.Text;

namespace MugEngine.Graphics
{
	internal struct MDrawCommand
	{
		public string mText;
		public object mTextureInfo; // Either a texture or a spritefont. Must cast it correctly.
		public Rectangle? mSourceRectangle;

		public Vector2 mPosition;
		public Color mColor;
		public float mRotation;
		public Vector2 mOrigin;
		public float mScale;
		public SpriteEffects mEffects;

		public MDrawCommand(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
		{
			mText = null;
			mTextureInfo = texture;
			mSourceRectangle = sourceRect;

			mPosition = position;
			mColor = color;
			mRotation = rotation;
			mOrigin = origin;
			mScale = scale;
			mEffects = effect;
		}

		public MDrawCommand(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
		{
			mText = text;
			mTextureInfo = font;
			mPosition = position;
			mColor = color;
			mRotation = rotation;
			mOrigin = origin;
			mScale = scale;
			mEffects = effect;
		}

		public bool IsStringCommand()
		{
			return mText is not null;
		}
	}
}
