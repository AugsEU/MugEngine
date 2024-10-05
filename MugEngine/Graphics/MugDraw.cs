using MugEngine.Maths;

namespace MugEngine.Graphics
{
	enum DrawLayer
	{
		Background = 0,
		BackgroundElement,
		SubEntity,
		Particle, // Reserved for particles for better batching.
		Default,
		Player,
		Tile,
		TileEffects,
		Projectiles,
		Bubble,
		PauseMenu,
		SequenceBG,
		Front,
		NumLayers
	}

	/// <summary>
	/// Simple rendering methods.
	/// </summary>
	static class MonoDraw
	{
		#region rConstants

		const float TOTAL_LAYERS = (float)DrawLayer.NumLayers;
		const float LAYER_MOVE_EPSILON = 0.000001f;

		static float sCurrentLayerDelta = 0.0f;

		#endregion rConstants


		#region rDummy

		private static Texture2D sDummyTexture = null;

		private static Texture2D GetDummyTexture(DrawInfo info)
		{
			if(sDummyTexture == null)
			{
				sDummyTexture = new Texture2D(info.mGraphics.GraphicsDevice, 1, 1);
				sDummyTexture.SetData<Color>(new Color[] { Color.White });
			}

			return sDummyTexture;
		}

		#endregion rDummy



		#region rRender

		/// <summary>
		/// Draw a texture at a position.
		/// </summary>
		public static void DrawTexture(DrawInfo info, Texture2D texture2D, Vector2 position)
		{
			info.mSpriteBatch.Draw(texture2D, position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(DrawLayer.Default));
		}



		/// <summary>
		/// Draw a texture at to rect.
		/// </summary>
		public static void DrawTexture(DrawInfo info, Texture2D texture2D, Rectangle rect)
		{
			info.mSpriteBatch.Draw(texture2D, rect, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GetDepth(DrawLayer.Default));
		}





		/// <summary>
		/// Draw a texture at position(with effect).
		/// </summary>
		public static void DrawTexture(DrawInfo info, Texture2D texture2D, Vector2 position, SpriteEffects effect)
		{
			info.mSpriteBatch.Draw(texture2D, position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, effect, GetDepth(DrawLayer.Default));
		}


		/// <summary>
		/// Draw a texture at position(rotated about the centre).
		/// </summary>
		public static void DrawTexture(DrawInfo info, Texture2D texture2D, Vector2 position, float rotation)
		{
			info.mSpriteBatch.Draw(texture2D, position, null, Color.White, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(DrawLayer.Default));
		}


		/// <summary>
		/// Draw a texture at position(rotated about the centre).
		/// </summary>
		public static void DrawTextureRotCent(DrawInfo info, Texture2D texture2D, Vector2 position, float rotation)
		{
			Vector2 rotationOffset = CalcRotationOffset(rotation, texture2D.Width, texture2D.Height);
			info.mSpriteBatch.Draw(texture2D, new Rectangle((int)position.X, (int)position.Y, texture2D.Width, texture2D.Height), null, Color.White, rotation, rotationOffset, SpriteEffects.None, GetDepth(DrawLayer.Default));
		}





		/// <summary>
		/// Draw a texture at a position(with layer depth).
		/// </summary>
		public static void DrawTextureDepth(DrawInfo info, Texture2D texture2D, Vector2 position, DrawLayer depth)
		{
			info.mSpriteBatch.Draw(texture2D, position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a texture at a position(with layer depth + color).
		/// </summary>
		public static void DrawTextureDepthColor(DrawInfo info, Texture2D texture2D, Vector2 position, Color color, DrawLayer depth)
		{
			info.mSpriteBatch.Draw(texture2D, position, null, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}


		/// <summary>
		/// Draw a texture at a position(with layer depth).
		/// </summary>
		public static void DrawTextureDepthScale(DrawInfo info, Texture2D texture2D, Vector2 position, float scale, DrawLayer depth)
		{
			info.mSpriteBatch.Draw(texture2D, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a texture at a position(all options).
		/// </summary>
		public static void DrawTexture(DrawInfo info, Texture2D texture2D, Vector2 position, Rectangle? sourceRectange, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, DrawLayer depth)
		{
			info.mSpriteBatch.Draw(texture2D, position, sourceRectange, color, rotation, origin, scale, effect, GetDepth(depth));
		}


		/// <summary>
		/// Draw a texture to a rect(all options).
		/// </summary>
		public static void DrawTexture(DrawInfo info, Texture2D texture2D, Rectangle destRect, Rectangle? sourceRectange, Color color, float rotation, Vector2 origin, SpriteEffects effect, DrawLayer depth)
		{
			info.mSpriteBatch.Draw(texture2D, destRect, sourceRectange, color, rotation, origin, effect, GetDepth(depth));
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public static void DrawStringCentred(DrawInfo info, SpriteFont font, Vector2 position, Color color, string text, DrawLayer depth = DrawLayer.Bubble)
		{
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position - size * 0.5f;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);

			info.mSpriteBatch.DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public static void DrawStringCentredRot(DrawInfo info, SpriteFont font, Vector2 position, Color color, string text, float rotation, DrawLayer depth = DrawLayer.Bubble)
		{
			Vector2 size = font.MeasureString(text);

			info.mSpriteBatch.DrawString(font, text, position, color, rotation, size * 0.5f, 1.0f, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>  
		/// Draw a string centred at a position with a shadow
		/// </summary>
		public static void DrawStringCentredShadow(DrawInfo info, SpriteFont font, Vector2 position, Color color, string text, DrawLayer depth = DrawLayer.Bubble)
		{
			Color shadowColor = color * 0.2f;
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position - size * 0.5f;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);
			Vector2 shadowPos = drawPosition + new Vector2(2.0f, 2.0f);

			info.mSpriteBatch.DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
			info.mSpriteBatch.DrawString(font, text, shadowPos, shadowColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public static void DrawParagraphCentred(DrawInfo info, SpriteFont font, Vector2 position, Color color, string text, float lineHeight, DrawLayer depth = DrawLayer.Bubble)
		{
			string accumulatedStr = "";
			for (int c = 0; c < text.Length; c++)
			{
				char newChar = text[c];
				if (newChar == '\n')
				{
					DrawStringCentred(info, font, position, color, accumulatedStr, depth);
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
				DrawStringCentred(info, font, position, color, accumulatedStr, depth);
			}
		}



		/// <summary>
		/// Draw a string at position(top left)
		/// </summary>
		public static void DrawString(DrawInfo info, SpriteFont font, Vector2 position, Color color, string text, DrawLayer depth = DrawLayer.Bubble)
		{
			info.mSpriteBatch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}


		/// <summary>
		/// Draw a string at a position
		/// </summary>
		public static void DrawString(DrawInfo info, SpriteFont font, string text, Vector2 position, Color color, DrawLayer depth)
		{
			info.mSpriteBatch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a string right aligned
		/// </summary>
		public static void DrawStringRight(DrawInfo info, SpriteFont font, Vector2 position, Color color, string text, DrawLayer depth = DrawLayer.Bubble)
		{
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position;
			drawPosition.X -= size.X;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);

			info.mSpriteBatch.DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a simple rectangle. Used mostly for debugging
		/// </summary>
		/// <param name="info">Info needed to draw</param>
		/// <param name="rect2f">Rectangle to draw</param>
		/// <param name="col">Color to draw rectangle in</param>
		public static void DrawRect(DrawInfo info, Rect2f rect2f, Color col)
		{
			Point min = new Point((int)rect2f.mMin.X, (int)rect2f.mMin.Y);
			Point max = new Point((int)rect2f.mMax.X, (int)rect2f.mMax.Y);

			DrawRect(info, new Rectangle(min, max - min), col);
		}



		/// <summary>
		/// Draw a simple rectangle. Used mostly for debugging
		/// </summary>
		public static void DrawRect(DrawInfo info, Rectangle rect, Color col)
		{
			info.mSpriteBatch.Draw(GetDummyTexture(info), rect, col);
		}


		/// <summary>
		/// Draw a simple rectangle. Used mostly for debugging
		/// </summary>
		public static void DrawRectDepth(DrawInfo info, Rectangle rect, Color col, DrawLayer depth)
		{
			info.mSpriteBatch.Draw(GetDummyTexture(info), rect, null, col, 0.0f, Vector2.Zero, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a simple rectangle. Used mostly for debugging
		/// </summary>
		public static void DrawRectDepth(DrawInfo info, Rect2f rect2f, Color col, DrawLayer depth)
		{
			DrawRectDepth(info, rect2f.ToRectangle(), col, depth);
		}



		/// <summary>
		/// Draw a simple rectangle with a shadow.
		/// </summary>
		public static void DrawRectShadow(DrawInfo info, Rect2f rect2f, Color col, Color shadowCol, Vector2 displacement, DrawLayer depth)
		{
			Rect2f shadowRect = rect2f;
			shadowRect.mMin += displacement;
			shadowRect.mMax += displacement;
			DrawRectDepth(info, rect2f.ToRectangle(), col, depth);
			DrawRectDepth(info, shadowRect.ToRectangle(), shadowCol, depth);
		}



		/// <summary>
		/// Draw a simple rectangle hollow. Used mostly for debugging
		/// </summary>
		public static void DrawRectHollow(DrawInfo info, Rect2f rect2f, float thickness, Color col, Color shadowCol, Vector2 shadowDisp, DrawLayer depth)
		{
			Vector2 tl = MugMath.Round(new Vector2(rect2f.mMin.X, rect2f.mMin.Y));
			Vector2 tr = MugMath.Round(new Vector2(rect2f.mMax.X, rect2f.mMin.Y));
			Vector2 br = MugMath.Round(new Vector2(rect2f.mMax.X, rect2f.mMax.Y));
			Vector2 bl = MugMath.Round(new Vector2(rect2f.mMin.X, rect2f.mMax.Y));

			Rect2f topEdge = new Rect2f(tl, tr + new Vector2(-thickness, thickness));
			Rect2f rightEdge = new Rect2f(tr + new Vector2(-thickness, 0.0f), br);
			Rect2f bottomEdge = new Rect2f(bl + new Vector2(0.0f, thickness), br);
			Rect2f leftEdge = new Rect2f(tl + new Vector2(0.0f, thickness), bl + new Vector2(thickness, 0.0f));

			DrawRectShadow(info, topEdge, col, shadowCol, shadowDisp, depth);
			DrawRectShadow(info, rightEdge, col, shadowCol, shadowDisp, depth);
			DrawRectShadow(info, bottomEdge, col, shadowCol, shadowDisp, depth);
			DrawRectShadow(info, leftEdge, col, shadowCol, shadowDisp, depth);
		}


		/// <summary>
		/// Draw a line from point A by an angle.
		/// </summary>
		public static void DrawRectRot(DrawInfo info, Rectangle rect, float angle, Color col, DrawLayer depth = DrawLayer.Default)
		{
			Vector2 origin = new Vector2(0.5f, 0.5f);
			info.mSpriteBatch.Draw(GetDummyTexture(info), rect, null, col, angle, origin, SpriteEffects.None, GetDepth(depth));
		}


		/// <summary>
		/// Draw a dot at a position.
		/// </summary>
		public static void DrawDot(DrawInfo info, Vector2 point, Color col)
		{
			DrawDot(info, new Point((int)point.X, (int)point.Y), col);
		}



		/// <summary>
		/// Draw a dot at a position
		/// </summary>
		public static void DrawDot(DrawInfo info, Point point, Color col)
		{
			Rectangle rectangle = new Rectangle(point.X, point.Y, 1, 1);
			DrawRect(info, rectangle, col);
		}



		/// <summary>
		/// Draw a dot at a position.
		/// </summary>
		public static void DrawDotDepth(DrawInfo info, Vector2 point, Color col, DrawLayer depth)
		{
			DrawDotDepth(info, new Point((int)point.X, (int)point.Y), col, depth);
		}



		/// <summary>
		/// Draw a dot at a position
		/// </summary>
		public static void DrawDotDepth(DrawInfo info, Point point, Color col, DrawLayer depth)
		{
			Rectangle rectangle = new Rectangle(point.X, point.Y, 1, 1);
			DrawRectDepth(info, rectangle, col, depth);
		}



		/// <summary>
		/// Draw a line from point A to B
		/// </summary>
		public static void DrawLine(DrawInfo info, Vector2 point1, Vector2 point2, Color color, float thickness = 1.0f, DrawLayer depth = DrawLayer.Default)
		{
			float distance = Vector2.Distance(point1, point2);
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			DrawLine(info, point1, distance, angle, color, thickness, depth);
		}



		/// <summary>
		/// Draw a line from point A by an angle.
		/// </summary>
		public static void DrawLine(DrawInfo info, Vector2 point, float length, float angle, Color color, float thickness = 1.0f, DrawLayer depth = DrawLayer.Default)
		{
			var origin = new Vector2(0f, 0.5f);
			var scale = new Vector2(length, thickness);
			info.mSpriteBatch.Draw(GetDummyTexture(info), point, null, color, angle, origin, scale, SpriteEffects.None, GetDepth(depth));
		}



		/// <summary>
		/// Draw a line with drop shadow.
		/// </summary>
		public static void DrawLineShadow(DrawInfo info, Vector2 point1, Vector2 point2, Color color, Color shadowColor, float dropDistance, float thickness = 1.0f, DrawLayer depth = DrawLayer.Default)
		{
			Vector2 shadowPt1 = point1;
			Vector2 shadowPt2 = point2;

			shadowPt1.Y += dropDistance;
			shadowPt2.Y += dropDistance;

			DrawLine(info, shadowPt1, shadowPt2, shadowColor, thickness, depth);
			DrawLine(info, point1, point2, color, thickness, depth);
		}



		/// <summary>
		/// Draw rect with a shadow
		/// </summary>
		public static void DrawRectShadow(DrawInfo info, Rectangle rect, Color color, Color shadowColor, float dropDistance, DrawLayer depth)
		{
			int iDropDistance = (int)Math.Round(dropDistance);

			rect.X += iDropDistance;
			rect.Y += iDropDistance;

			DrawRectDepth(info, rect, shadowColor, depth);

			rect.X -= iDropDistance;
			rect.Y -= iDropDistance;

			DrawRectDepth(info, rect, color, depth);
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


		/// <summary>
		/// Increment layer to avoid z-fighting.
		/// </summary>
		static float GetDepth(DrawLayer layer)
		{
			float layerNum = (float)layer;

			sCurrentLayerDelta += LAYER_MOVE_EPSILON;
			return layerNum / TOTAL_LAYERS + sCurrentLayerDelta;
		}


		/// <summary>
		/// Call this to clear out each frame.
		/// </summary>
		public static void FlushRender()
		{
			sCurrentLayerDelta = 0.0f;
		}



		/// <summary>
		/// Memcpy a texture
		/// </summary>
		public static void MemCopyTexture(GraphicsDevice graphics, Texture2D source, Texture2D dest)
		{
			// Recreate texture if size is different
			if (dest is null || dest.Width != source.Width || dest.Height != source.Height)
			{
				dest = new Texture2D(graphics, source.Width, source.Height);
			}

			int count = source.Width * source.Height;
			Color[] data = new Color[count];
			source.GetData<Color>(data);
			dest.SetData(data);
		}



		/// <summary>
		/// Create texture from memcpy
		/// </summary>
		public static Texture2D MemCopyTexture(GraphicsDevice graphics, Texture2D source)
		{
			// Recreate texture if size is different
			Texture2D dest = new Texture2D(graphics, source.Width, source.Height);

			int count = source.Width * source.Height;
			Color[] data = new Color[count];
			source.GetData<Color>(data);
			dest.SetData(data);

			return dest;
		}



		/// <summary>
		/// Make texture greyscale
		/// </summary>
		public static Texture2D MakeTextureGreyscale(GraphicsDevice graphics, Texture2D source)
		{
			// Recreate texture
			Texture2D dest = new Texture2D(graphics, source.Width, source.Height);
			int count = source.Width * source.Height;
			Color[] data = new Color[count];
			source.GetData<Color>(data);

			// Make greyscale
			for (int i = 0; i < count; i++)
			{
				Color col = data[i];
				float lum = 0.2126f * col.R + 0.7152f * col.G + 0.0722f * col.B;
				lum = Math.Clamp(lum, 0.0f, 255.0f) / 255.0f;
				data[i] = new Color(lum, lum, lum, col.A / 255.0f);
			}

			// Save to dest
			dest.SetData(data);

			return dest;
		}

		#endregion rUtility
	}
}
