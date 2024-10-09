using MugEngine.Maths;
using MugEngine.Types;

namespace MugEngine.Graphics
{
	/// <summary>
	/// A canvas is the context in which things are drawn. It handles layers, effects, and render targets.
	/// </summary>
	public class MCanvas2D : IMUpdate
	{
		#region rMembers

		static Texture2D sDummyTexture = null;

		List<MCanvasLayer> mLayers = new List<MCanvasLayer>();
		RenderTarget2D mRenderTarget;

		MCamera mCamera;
		SpriteBatch mBatcher;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a canvas on which to draw.
		/// </summary>
		public MCanvas2D(Point resolution, GraphicsDeviceManager graphics, int numLayers)
		{
			mRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, resolution.X, resolution.Y);
			mBatcher = new SpriteBatch(graphics.GraphicsDevice);

			mCamera = new MCamera(MugMath.PointToVec(resolution));

			while (mLayers.Count < numLayers)
			{
				mLayers.Add(new MCanvasLayer());
			}

			if (sDummyTexture is null)
			{
				sDummyTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
				Color[] data = new Color[] { Color.White };
				sDummyTexture.SetData(data);
			}
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update the canvas.
		/// </summary>
		public void Update(MUpdateInfo info)
		{
			mCamera.Update(info);
		}

		#endregion rUpdate




		#region rDraw

		/// <summary>
		/// Call to ready the canvas for drawing.
		/// Returns draw information for drawing to this specific canvas.
		/// </summary>
		public MDrawInfo BeginDraw(MDrawInfo info)
		{
			MDrawInfo thisInfo = new MDrawInfo();
			thisInfo.mDelta = info.mDelta;
			thisInfo.mCanvas = this;

			return thisInfo;
		}



		/// <summary>
		/// Draws everything that was added to the canvas.
		/// Everything added after this is not going to be drawn.
		/// </summary>
		public void EndDraw(MDrawInfo info)
		{
			info.mDevice.SetRenderTarget(mRenderTarget);
			info.mDevice.Clear(Color.Black);

			Matrix viewPort = mCamera.CalculateMatrix();

			for (int i = 0; i < mLayers.Count; i++)
			{
				mLayers[i].DrawLayer(mBatcher, viewPort);
			}
		}

		#endregion rDraw





		#region rCommands


		/// <summary>
		/// Draw a texture to the canvas
		/// </summary>
		public void DrawTexture(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, int layer)
		{
			MTextureDrawData data = new MTextureDrawData(texture, position, sourceRect, color, rotation, origin, scale, effect);
			mLayers[layer].DrawTexture(ref data);
		}



		/// <summary>
		/// Draw a string to the canvas
		/// </summary>
		public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, int layer)
		{
			MStringDrawData data = new MStringDrawData(font, text, position, color, rotation, origin, scale, effect);
			mLayers[layer].DrawString(ref data);
		}

		#endregion rCommands





		#region rTextureHelpers

		/// <summary>
		/// Simple texture draw
		/// </summary>
		public void DrawTexture(Texture2D texture, Vector2 position, int layer = 0)
		{
			DrawTexture(texture, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
		}



		/// <summary>
		/// Draw a texture at position(with effect).
		/// </summary>
		public void DrawTexture(Texture2D texture2D, Vector2 position, SpriteEffects effect, int layer = 0)
		{
			DrawTexture(texture2D, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, effect, layer);
		}




		/// <summary>
		/// Draw a texture at position(with effect).
		/// </summary>
		public void DrawTexture(Texture2D texture2D, Vector2 position, Vector2 scale, int layer = 0)
		{
			DrawTexture(texture2D, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layer);
		}



		/// <summary>
		/// Simple texture draw
		/// </summary>
		public void DrawTexture(Texture2D texture, Vector2 position, Color color, int layer = 0)
		{
			DrawTexture(texture, position, null, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
		}







		#endregion rTextureHelpers
		#region rRect
		/// <summary>
		/// Draw a simple rectangle. Used mostly for debugging
		/// </summary>
		public void DrawRect(MRect2f rect2f, Color color, int layer = 0)
		{
			DrawTexture(sDummyTexture, rect2f.mMin, null, color, 0.0f, Vector2.Zero, rect2f.GetSize(), SpriteEffects.None, layer);
		}



		/// <summary>
		/// Draw a simple rectangle with a shadow.
		/// </summary>
		public void DrawRectShadow(MRect2f rect2f, Color col, Color shadowCol, Vector2 displacement, int layer = 0)
		{
			MRect2f shadowRect = rect2f;
			shadowRect.mMin += displacement;
			shadowRect.mMax += displacement;
			DrawRect(rect2f, col, layer);
			DrawRect(shadowRect, shadowCol, layer);
		}



		/// <summary>
		/// Draw a simple rectangle hollow.
		/// </summary>
		public void DrawRectHollow(MRect2f rect2f, float thickness, Color col, Color shadowCol, Vector2 shadowDisp, int layer = 0)
		{
			Vector2 tl = MugMath.Round(new Vector2(rect2f.mMin.X, rect2f.mMin.Y));
			Vector2 tr = MugMath.Round(new Vector2(rect2f.mMax.X, rect2f.mMin.Y));
			Vector2 br = MugMath.Round(new Vector2(rect2f.mMax.X, rect2f.mMax.Y));
			Vector2 bl = MugMath.Round(new Vector2(rect2f.mMin.X, rect2f.mMax.Y));

			MRect2f topEdge = new MRect2f(tl, tr + new Vector2(-thickness, thickness));
			MRect2f rightEdge = new MRect2f(tr + new Vector2(-thickness, 0.0f), br);
			MRect2f bottomEdge = new MRect2f(bl + new Vector2(0.0f, thickness), br);
			MRect2f leftEdge = new MRect2f(tl + new Vector2(0.0f, thickness), bl + new Vector2(thickness, 0.0f));

			DrawRectShadow(topEdge, col, shadowCol, shadowDisp, layer);
			DrawRectShadow(rightEdge, col, shadowCol, shadowDisp, layer);
			DrawRectShadow(bottomEdge, col, shadowCol, shadowDisp, layer);
			DrawRectShadow(leftEdge, col, shadowCol, shadowDisp, layer);
		}





		#endregion rRect





		#region rLine

		/// <summary>
		/// Draw a line from point A by an angle.
		/// </summary>
		public void DrawLine(Vector2 point, float length, float angle, Color color, float thickness = 1.0f, int layer = 0)
		{
			var origin = new Vector2(0f, 0.5f);
			var scale = new Vector2(length, thickness);
			DrawTexture(sDummyTexture, point, null, color, angle, origin, scale, SpriteEffects.None, layer);
		}



		/// <summary>
		/// Draw a line from point A to B
		/// </summary>
		public void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness = 1.0f, int layer = 0)
		{
			float distance = Vector2.Distance(point1, point2);
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			DrawLine(point1, distance, angle, color, thickness, layer);
		}



		/// <summary>
		/// Draw a line with drop shadow.
		/// </summary>
		public void DrawLineShadow(Vector2 point1, Vector2 point2, Color color, Color shadowColor, float dropDistance, float thickness = 1.0f, int layer = 0)
		{
			Vector2 shadowPt1 = point1;
			Vector2 shadowPt2 = point2;

			shadowPt1.Y += dropDistance;
			shadowPt2.Y += dropDistance;

			DrawLine(shadowPt1, shadowPt2, shadowColor, thickness, layer);
			DrawLine(point1, point2, color, thickness, layer);
		}

		#endregion rLine





		#region rStringHelpers

		/// <summary>
		/// Simple string draw
		/// </summary>
		public void DrawString(SpriteFont font, string text, Vector2 position, Color color, int layer = 0)
		{
			DrawString(font, text, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
		}



		/// <summary>
		/// Draw a string centred at a position
		/// </summary>
		public void DrawStringCentred(SpriteFont font, Vector2 position, Color color, string text, int layer)
		{
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position - size * 0.5f;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);

			DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
		}



		/// <summary>  
		/// Draw a string centred at a position with a shadow
		/// </summary>
		public void DrawStringCentredShadow(SpriteFont font, Vector2 position, Color color, string text, int layer)
		{
			Color shadowColor = color * 0.2f;
			Vector2 size = font.MeasureString(text);
			Vector2 drawPosition = position - size * 0.5f;
			drawPosition.X = MathF.Round(drawPosition.X);
			drawPosition.Y = MathF.Round(drawPosition.Y);
			Vector2 shadowPos = drawPosition + new Vector2(2.0f, 2.0f);

			DrawString(font, text, drawPosition, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
			DrawString(font, text, shadowPos, shadowColor, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
		}

		#endregion rStringHelpers
	}
}
