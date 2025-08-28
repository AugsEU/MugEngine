using Microsoft.Xna.Framework.Graphics;
using TracyWrapper;

namespace MugEngine.Graphics;

/// <summary>
/// A canvas is the context in which things are drawn. It handles layers, effects, and render targets.
/// </summary>
public class MCanvas2D : IMUpdate
{
	#region rConstants

	const float LAYER_INCREMENT = 0.0000001f;

	#endregion rConstants




	#region rMembers

	static Texture2D sDummyTexture = null;
	static bool sCurrentlyDrawing = false;

	RenderTarget2D mRenderTarget;

	MCamera mCamera;
	Matrix mCamMatrixCache;
	SpriteBatch mBatcher;
	MSpriteBatchOptions mCurrentOptions;

	float mLayerOffset;
	float mDivLayerCount;


	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create a canvas on which to draw.
	/// </summary>
	public MCanvas2D(Point resolution)
	{
		GraphicsDevice device = MugCore.I.GetDevice();

		mRenderTarget = new RenderTarget2D(device, resolution.X, resolution.Y);
		mCamera = new MCamera(resolution.ToVector2());

		InitCanvas(device);
	}



	/// <summary>
	/// Create a canvas on which to draw.
	/// This canvas is a "null canvas" meaning it is rendering to the back buffer.
	/// </summary>
	public MCanvas2D()
	{
		GraphicsDevice device = MugCore.I.GetDevice();

		mRenderTarget = null;
		mCamera = new MCamera(Vector2.Zero);

		InitCanvas(device);
	}



	/// <summary>
	/// Init the canvas
	/// </summary>
	private void InitCanvas(GraphicsDevice device)
	{
		mBatcher = new SpriteBatch(device);

		if (sDummyTexture is null)
		{
			sDummyTexture = new Texture2D(device, 1, 1);
			Color[] data = new Color[] { Color.White };
			sDummyTexture.SetData(data);
		}

		mDivLayerCount = 1.0f / MugCore.I.GetNumLayers();
		mLayerOffset = 0.0f;

		mCurrentOptions = new MSpriteBatchOptions();
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
	public MDrawInfo BeginDraw(float delta)
	{
		MugDebug.Assert(!sCurrentlyDrawing, "Cannot start drawing while another canvas is drawing.");

		GraphicsDevice device = MugCore.I.GetDevice();

		MDrawInfo thisInfo = new MDrawInfo();
		thisInfo.mDelta = delta;
		thisInfo.mCanvas = this;

		device.SetRenderTarget(mRenderTarget);
		device.Clear(Color.Black);

		mCamMatrixCache = mCamera.CalculateMatrix();
		mLayerOffset = 0.0f;

		mBatcher.MugStartSpriteBatch(mCurrentOptions, mCamMatrixCache);
		sCurrentlyDrawing = true;

		return thisInfo;
	}



	/// <summary>
	/// Draws everything that was added to the canvas.
	/// Everything added after this is not going to be drawn.
	/// </summary>
	public void EndDraw()
	{
		Profiler.PushProfileZone("End Canvas Draw Dump");
		MugDebug.Assert(sCurrentlyDrawing, "Ending draw before we started.");
		mBatcher.End();
		sCurrentlyDrawing = false;
		Profiler.PopProfileZone();
	}

	#endregion rDraw





	#region rUtil

	/// <summary>
	/// Get the output that we can draw.
	/// </summary>
	public Texture2D GetOutput()
	{
		return mRenderTarget;
	}



	/// <summary>
	/// Get the size of the render target
	/// </summary>
	public Point GetSize()
	{
		// Simple case
		if (mRenderTarget is not null)
		{
			return new Point(mRenderTarget.Width, mRenderTarget.Height);
		}

		// Null means we are rendering to the back buffer.
		Viewport vp = MugCore.I.GetDevice().Viewport;

		return new Point(vp.Width, vp.Height);
	}



	/// <summary>
	/// Get camera
	/// </summary>
	public MCamera GetCamera()
	{
		return mCamera;
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

		return result;
	}



	/// <summary>
	/// Get depth float between 0 and 1 for d layers.
	/// </summary>
	float GetDDepth(int layer, float depth)
	{
		mLayerOffset += LAYER_INCREMENT;
		float depthOffset = MugMath.SquashToRange(depth + mLayerOffset, -0.5f, 0.5f) * mDivLayerCount;
		float result = layer * mDivLayerCount + 0.5f + depthOffset;

		MugDebug.Assert(mLayerOffset < mDivLayerCount, "Too many objects have been drawn!");
		MugDebug.Assert(0.0f < result && result < 1.0f, "Layer outside of clip bounds.");

		return result;
	}



	/// <summary>
	/// Get z-depth from layer.
	/// </summary>
	public float GetBaseDepth(int layer)
	{
		return layer * mDivLayerCount;
	}



	/// <summary>
	/// Convert a screen space coord to world space.
	/// </summary>
	public Vector2 ScreenSpaceToWorldSpace(Vector2 screenSpace)
	{
		return Vector2.Transform(screenSpace, Matrix.Invert(mCamMatrixCache));
	}

	#endregion rUtil





	#region rOptions

	/// <summary>
	/// Set new spritebatch options.
	/// Will always restart the spritebatcher
	/// </summary>
	public void SetOptions(MSpriteBatchOptions options)
	{
		mCurrentOptions = options;

		if (sCurrentlyDrawing)
		{
			Profiler.PushProfileZone("Restart Batcher Dump");
			mBatcher.End();
			mBatcher.MugStartSpriteBatch(mCurrentOptions, mCamMatrixCache);
			Profiler.PopProfileZone();
		}
	}



	/// <summary>
	/// Bind an effect. Will apply to all things drawn.
	/// Will always restart the sprite batch.
	/// </summary>
	public void BindEffect(Effect effect)
	{
		mCurrentOptions.mEffect = effect;

		if (sCurrentlyDrawing)
		{
			Profiler.PushProfileZone("Restart Batcher Dump");
			mBatcher.End();
			mBatcher.MugStartSpriteBatch(mCurrentOptions, mCamMatrixCache);
			Profiler.PopProfileZone();
		}
	}

	#endregion rOptions





	#region rTexture

	/// <summary>
	/// Draw a texture to the canvas. Vector scaling
	/// </summary>
	public void DrawTextureVs(Texture2D texture, Vector2 pos, int layer, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;
		mBatcher.Draw(texture, pos, srcRect, drawColor, rot, drawOrigin, drawScale, effect, GetDepth(layer));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DrawTexture(Texture2D texture, Vector2 pos, int layer, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, pos, srcRect, drawColor, rot, drawOrigin, scale, effect, GetDepth(layer));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DrawTexture(Texture2D texture, Vector2 pos, float depth, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, pos, srcRect, drawColor, rot, drawOrigin, scale, effect, depth);
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DrawTexture(Texture2D texture, Rectangle destRect, int layer, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, destRect, srcRect, drawColor, rot, drawOrigin, effect, GetDepth(layer));
	}



	/// <summary>
	/// Simple texture draw at centre
	/// </summary>
	public void DrawTexture(Texture2D texture, MAnchorVector2 pos, int layer, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, pos.ToVec(new Point(texture.Width, texture.Height)), srcRect, drawColor, rot, drawOrigin, scale, effect, GetDepth(layer));
	}

	#endregion rTexture




	#region rTexturePart

	/// <summary>
	/// Draw a texture to the canvas. Vector scale
	/// </summary>
	public void DrawTextureVs(MTexturePart texture, Vector2 pos, int layer, Color? color = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;
		mBatcher.Draw(texture.mTexture, pos, texture.mUV, drawColor, rot, drawOrigin, drawScale, effect, GetDepth(layer));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DrawTexture(MTexturePart texture, Vector2 pos, int layer, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture.mTexture, pos, texture.mUV, drawColor, rot, drawOrigin, scale, effect, GetDepth(layer));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DrawTexture(MTexturePart texture, Vector2 pos, float depth, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture.mTexture, pos, texture.mUV, drawColor, rot, drawOrigin, scale, effect, depth);
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DrawTexture(MTexturePart texture, Rectangle destRect, int layer, Color? color = null, float rot = 0.0f, Vector2? origin = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture.mTexture, destRect, texture.mUV, drawColor, rot, drawOrigin, effect, GetDepth(layer));
	}



	/// <summary>
	/// Draw a texture to the canvas.
	/// </summary>
	public void DrawTexture(MTexturePart texture, MAnchorVector2 pos, int layer, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Texture2D tex = texture.mTexture;
		mBatcher.Draw(tex, pos.ToVec(new Point(tex.Width, tex.Height)), texture.mUV, drawColor, rot, drawOrigin, scale, effect, GetDepth(layer));
	}

	#endregion rTexturePart




	#region rDTexture

	/// <summary>
	/// Draw a texture to the canvas. Vector scaling
	/// </summary>
	public void DDrawTextureVs(Texture2D texture, Vector2 pos, int layer, float depth, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;
		mBatcher.Draw(texture, pos, srcRect, drawColor, rot, drawOrigin, drawScale, effect, GetDDepth(layer, depth));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DDrawTexture(Texture2D texture, Vector2 pos, int layer, float depth, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, pos, srcRect, drawColor, rot, drawOrigin, scale, effect, GetDDepth(layer, depth));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DDrawTexture(Texture2D texture, Rectangle destRect, int layer, float depth, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, destRect, srcRect, drawColor, rot, drawOrigin, effect, GetDDepth(layer, depth));
	}



	/// <summary>
	/// Simple texture draw at centre
	/// </summary>
	public void DDrawTexture(Texture2D texture, MAnchorVector2 pos, int layer, float depth, Rectangle? srcRect = null, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture, pos.ToVec(new Point(texture.Width, texture.Height)), srcRect, drawColor, rot, drawOrigin, scale, effect, GetDDepth(layer, depth));
	}

	#endregion rDTexture




	#region rDTexturePart

	/// <summary>
	/// Draw a texture to the canvas. Vector scale
	/// </summary>
	public void DDrawTextureVs(MTexturePart texture, Vector2 pos, int layer, float depth, Color? color = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;
		mBatcher.Draw(texture.mTexture, pos, texture.mUV, drawColor, rot, drawOrigin, drawScale, effect, GetDDepth(layer, depth));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DDrawTexture(MTexturePart texture, Vector2 pos, int layer, float depth, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture.mTexture, pos, texture.mUV, drawColor, rot, drawOrigin, scale, effect, GetDDepth(layer, depth));
	}



	/// <summary>
	/// Draw a texture to the canvas
	/// </summary>
	public void DDrawTexture(MTexturePart texture, Rectangle destRect, int layer, float depth, Color? color = null, float rot = 0.0f, Vector2? origin = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		mBatcher.Draw(texture.mTexture, destRect, texture.mUV, drawColor, rot, drawOrigin, effect, GetDDepth(layer, depth));
	}



	/// <summary>
	/// Draw a texture to the canvas.
	/// </summary>
	public void DDrawTexture(MTexturePart texture, MAnchorVector2 pos, int layer, float depth, Color? color = null, float rot = 0.0f, Vector2? origin = null, float scale = 1.0f, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Texture2D tex = texture.mTexture;

		mBatcher.Draw(tex, pos.ToVec(texture.mUV.Size), texture.mUV, drawColor, rot, drawOrigin, scale, effect, GetDDepth(layer, depth));
	}

	#endregion rDTexturePart



	#region rRect

	/// <summary>
	/// Draw a simple rectangle. Used mostly for debugging
	/// </summary>
	public void DrawRect(MRect2f rect2f, Color color, int layer = 0)
	{
		DrawTextureVs(sDummyTexture, rect2f.mMin, layer, scale: rect2f.GetSize());
	}


	/// <summary>
	/// Draw a simple rectangle. Used mostly for debugging
	/// </summary>
	public void DrawRect(Rectangle rect, Color color, int layer = 0)
	{
		Vector2 pos = new Vector2(rect.X, rect.Y);
		Vector2 size = new Vector2(rect.Width, rect.Height);
		DrawTextureVs(sDummyTexture, pos, layer, color: color, scale: size);
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
		DrawTextureVs(sDummyTexture, point, layer, color: color, rot: angle, origin: origin, scale: scale);
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
	public void DrawString(SpriteFont font, string text, Vector2 pos, int layer, Color? color = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;
		mBatcher.DrawString(font, text, pos, drawColor, rot, drawOrigin, drawScale, effect, GetDepth(layer));
	}



	/// <summary>
	/// Draw a string to the canvas
	/// </summary>
	public void DrawString(SpriteFont font, string text, MAnchorVector2 pos, int layer, Color? color = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Vector2 size = font.MeasureString(text);
		Vector2 drawPosition = pos.ToVec(new Point(MugMath.Round(size.X), MugMath.Round(size.Y)));
		drawPosition.X = MathF.Round(drawPosition.X);
		drawPosition.Y = MathF.Round(drawPosition.Y);

		Color drawColor = color ?? Color.White;
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;
		mBatcher.DrawString(font, text, drawPosition, drawColor, rot, drawOrigin, drawScale, effect, GetDepth(layer));
	}



	/// <summary>  
	/// Draw a string to the canvas with a shadow
	/// </summary>
	public void DrawStringShadow(SpriteFont font, string text, MAnchorVector2 pos, int layer, float? dropDist = null, Color? color = null, Color? shadowColor = null, float rot = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
	{
		Vector2 size = font.MeasureString(text);
		Vector2 drawPosition = pos.ToVec(new Point(MugMath.Round(size.X), MugMath.Round(size.Y)));
		drawPosition.X = MathF.Round(drawPosition.X);
		drawPosition.Y = MathF.Round(drawPosition.Y);

		Color drawColor = color ?? Color.White;
		Color drawShadowColor = shadowColor ?? new(Color.Gray, 0.6f);
		Vector2 drawOrigin = origin ?? Vector2.Zero;
		Vector2 drawScale = scale ?? Vector2.One;

		// Draw shadow first so it goes behind.
		float drawDropDist = dropDist ?? size.Y * 0.3f;
		mBatcher.DrawString(font, text, drawPosition + new Vector2(drawDropDist, drawDropDist), drawShadowColor, rot, drawOrigin, drawScale, effect, GetDepth(layer));

		// Draw text
		mBatcher.DrawString(font, text, drawPosition, drawColor, rot, drawOrigin, drawScale, effect, GetDepth(layer));
	}

	#endregion rStringHelpers
}

