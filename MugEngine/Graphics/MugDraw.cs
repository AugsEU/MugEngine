using Microsoft.Xna.Framework.Graphics;
using MugEngine.Maths;

namespace MugEngine.Graphics
{
	/// <summary>
	/// Simple rendering methods.
	/// </summary>
	static class MonoDraw
	{
		#region rRender

		/// <summary>
		/// Draw a texture at position(rotated about the centre).
		/// </summary>
		public static void MugTextureRotCent(this SpriteBatch spriteBatch, Texture2D texture2D, Vector2 position, float rotation, float depth)
		{
			Vector2 rotationOffset = CalcRotationOffset(rotation, texture2D.Width, texture2D.Height);
			spriteBatch.Draw(texture2D, new Rectangle((int)position.X, (int)position.Y, texture2D.Width, texture2D.Height), null, Color.White, rotation, rotationOffset, SpriteEffects.None, depth);
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public static void MugStringCentred(this SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, string text, float depth)
		{
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position - size * 0.5f;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);

			spriteBatch.DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public static void MugStringCentredRot(this SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, string text, float rotation, float depth)
		{
			Vector2 size = font.MeasureString(text);

			spriteBatch.DrawString(font, text, position, color, rotation, size * 0.5f, 1.0f, SpriteEffects.None, depth);
		}



		/// <summary>  
		/// Draw a string centred at a position with a shadow
		/// </summary>
		public static void MugStringCentredShadow(this SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, string text, float depth)
		{
			Color shadowColor = color * 0.2f;
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position - size * 0.5f;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);
			Vector2 shadowPos = drawPosition + new Vector2(2.0f, 2.0f);

			spriteBatch.DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
			spriteBatch.DrawString(font, text, shadowPos, shadowColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public static void MugParagraphCentred(this SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, string text, float lineHeight, float depth)
		{
			string accumulatedStr = "";
			for (int c = 0; c < text.Length; c++)
			{
				char newChar = text[c];
				if (newChar == '\n')
				{
					MugStringCentred(spriteBatch, font, position, color, accumulatedStr, depth);
					accumulatedStr = "";
					position.Y += lineHeight;
				}
				else
				{
					accumulatedStr += newChar;
				}
			}

			if (accumulatedStr.Length > 0)
			{
				MugStringCentred(spriteBatch, font, position, color, accumulatedStr, depth);
			}
		}



		/// <summary>
		/// Draw a string at position(top left)
		/// </summary>
		public static void MugString(this SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, string text, float depth)
		{
			spriteBatch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
		}


		/// <summary>
		/// Draw a string at a position
		/// </summary>
		public static void MugString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float depth)
		{
			spriteBatch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
		}



		/// <summary>
		/// Draw a string right aligned
		/// </summary>
		public static void DrawStringRight(this SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, string text, float depth)
		{
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position;
			drawPosition.X -= size.X;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);

			spriteBatch.DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
		}

		#endregion rRender





		#region rUtility

		/// <summary>
		/// Calculate rotation offset so we can rotate about the centre. For squares
		/// </summary>
		/// <param name="rotation">Rotation in rads</param>
		/// <param name="height">Square height</param>
		/// <returns>Rotation offset used in draw call</returns>
		public static Vector2 CalcRotationOffset(float rotation, float height)
		{
			return CalcRotationOffset(rotation, height, height);
		}



		/// <summary>
		/// Calculate rotation offset so we can rotate about the centre. For rectangles
		/// </summary>
		/// <param name="rotation">Rotation in rads</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		/// <returns>Rotation offset used in draw call</returns>
		public static Vector2 CalcRotationOffset(float rotation, float width, float height)
		{
			float c = MathF.Cos(rotation);
			float s = MathF.Sin(-rotation);

			Vector2 oldCentre = new Vector2(width / 2.0f, height / 2.0f);
			Vector2 newCentre = new Vector2(oldCentre.X * c - oldCentre.Y * s, oldCentre.X * s + oldCentre.Y * c);

			return oldCentre - newCentre;
		}

		#endregion rUtility
	}
}
