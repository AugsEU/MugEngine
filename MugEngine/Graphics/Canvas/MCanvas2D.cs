﻿using Microsoft.Xna.Framework.Graphics;
using MugEngine.Core;
using MugEngine.Maths;
using MugEngine.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MugEngine.Graphics
{
	/// <summary>
	/// A canvas is the context in which things are drawn. It handles layers, effects, and render targets.
	/// </summary>
	public class MCanvas2D : IMUpdate
	{
		#region rConstants

		static float LAYER_INCREMENT = 0.0000001f;

		#endregion rConstants




		#region rMembers

		static Texture2D sDummyTexture = null;

		RenderTarget2D mRenderTarget;

		MCamera mCamera;
		Matrix mCurrentViewport;
		SpriteBatch mBatcher;
		bool mCurrentlyDrawing;
		MSpriteBatchOptions mCurrentOptions;

		float mLayerOffset;
		float mDivLayerCount;


		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a canvas on which to draw.
		/// </summary>
		public MCanvas2D(Point resolution, GraphicsDeviceManager graphics, int numLayers)
		{
			mRenderTarget = new RenderTarget2D(graphics.GraphicsDevice, resolution.X, resolution.Y);
			mBatcher = new SpriteBatch(graphics.GraphicsDevice);
			mCurrentlyDrawing = false;

			mCamera = new MCamera(MugMath.PointToVec(resolution));

			mDivLayerCount = 1.0f / numLayers;
			mLayerOffset = 0.0f;

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

			info.mDevice.SetRenderTarget(mRenderTarget);
			info.mDevice.Clear(Color.Black);

			mCurrentViewport = mCamera.CalculateMatrix();
			mLayerOffset = 0.0f;

			mBatcher.MugStartSpriteBatch(mCurrentOptions, mCurrentViewport);
			mCurrentlyDrawing = true;

			return thisInfo;
		}



		/// <summary>
		/// Draws everything that was added to the canvas.
		/// Everything added after this is not going to be drawn.
		/// </summary>
		public void EndDraw(MDrawInfo info)
		{
			MugDebug.Assert(mCurrentlyDrawing, "Ending draw before we started.");
			mBatcher.End();
			mCurrentlyDrawing = false;
		}

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Set new spritebatch options.
		/// Will always restart the spritebatcher
		/// </summary>
		public void SetOptions(MSpriteBatchOptions options)
		{
			mCurrentOptions = options;

			if (mCurrentlyDrawing)
			{
				mBatcher.End();
				mBatcher.MugStartSpriteBatch(mCurrentOptions, mCurrentViewport);
			}
		}



		/// <summary>
		/// Bind an effect. Will apply to all things drawn.
		/// Will always restart the sprite batch.
		/// </summary>
		public void BindEffect(Effect effect)
		{
			mCurrentOptions.mEffect = effect;

			if(mCurrentlyDrawing)
			{
				mBatcher.End();
				mBatcher.MugStartSpriteBatch(mCurrentOptions, mCurrentViewport);
			}
		}



		/// <summary>
		/// Get depth float between 0 and 1
		/// </summary>
		float GetDepth(int layer)
		{
			mLayerOffset += LAYER_INCREMENT;
			float result = layer * mDivLayerCount + mLayerOffset;

			MugDebug.Assert(mLayerOffset < mDivLayerCount, "Too many objects have been drawn!");
			MugDebug.Assert(0.0f < result && result < 1.0f, "Layer outside of clip bounds.");

			return result + mLayerOffset;
		}

		#endregion rUtil





		#region rTexture

		/// <summary>
		/// Draw a texture to the canvas
		/// </summary>
		public void DrawTexture(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, int layer)
		{
			mBatcher.Draw(texture, position, sourceRect, color, rotation, origin, scale, effect, GetDepth(layer));
		}

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

		#endregion rTexture





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





		#region rString


		/// <summary>
		/// Draw a string to the canvas
		/// </summary>
		public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, int layer)
		{
			mBatcher.DrawString(font, text, position, color, rotation, origin, scale, effect, GetDepth(layer));
		}



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
