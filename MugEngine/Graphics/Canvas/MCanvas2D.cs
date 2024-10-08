using MugEngine.Graphics.Camera;
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
		public void DrawTexture(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, int layer)
		{
			MTextureDrawData data = new MTextureDrawData(texture, position, sourceRect, color, rotation, origin, scale, effect);
			mLayers[layer].DrawTexture(ref data);
		}



		/// <summary>
		/// Draw a string to the canvas
		/// </summary>
		public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, int layer)
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
			DrawTexture(texture, position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layer);
		}

		#endregion rTextureHelpers





		#region rStringHelpers

		/// <summary>
		/// Simple string draw
		/// </summary>
		public void DrawString(SpriteFont font, string text, Vector2 position, Color color, int layer = 0)
		{
			DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layer);
		}

		#endregion rStringHelpers
	}
}
