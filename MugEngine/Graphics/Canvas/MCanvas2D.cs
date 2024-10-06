using Microsoft.Xna.Framework.Graphics;
using MugEngine.Collections;
using MugEngine.Graphics.Camera;
using MugEngine.Maths;
using MugEngine.Types;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;

namespace MugEngine.Graphics
{
	/// <summary>
	/// A canvas is the context in which things are drawn. It handles layers, effects, and render targets.
	/// </summary>
	public class MCanvas2D : IMUpdate
	{
		#region rConstants

		const int BASE_DRAW_COMMANDS = 512; // Reserved size for each layer.

		#endregion rConstants





		#region rMembers

		List<MStructArray<MDrawCommand>> mDrawCommandLayers = new List<MStructArray<MDrawCommand>>(BASE_DRAW_COMMANDS);
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

			while (mDrawCommandLayers.Count < numLayers)
			{
				mDrawCommandLayers.Add(new MStructArray<MDrawCommand>(BASE_DRAW_COMMANDS));
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
			for(int i = 0; i < mDrawCommandLayers.Count; i++)
			{
				mDrawCommandLayers[i].Clear();
			}

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

			for (int i = 0; i < mDrawCommandLayers.Count; i++)
			{
				DrawLayer(mDrawCommandLayers[i]);
			}
		}



		/// <summary>
		/// Draw a single layer.
		/// </summary>
		private void DrawLayer(MStructArray<MDrawCommand> layer)
		{
			mCamera.StartSpriteBatch(mBatcher);

			for(int c = 0; c < layer.Count; c++)
			{
				MDrawCommand cmd = layer[c];

				if(cmd.IsStringCommand())
				{
					SpriteFont font = (SpriteFont)cmd.mTextureInfo;
					mBatcher.DrawString(font, cmd.mText, cmd.mPosition, cmd.mColor, cmd.mRotation, cmd.mOrigin, cmd.mScale, cmd.mEffects, 0.0f);
				}
				else
				{
					Texture2D texture = (Texture2D)cmd.mTextureInfo;
					mBatcher.Draw(texture, cmd.mPosition, cmd.mSourceRectangle, cmd.mColor, cmd.mRotation, cmd.mOrigin, cmd.mScale, cmd.mEffects, 0.0f);
				}
			}

			mCamera.EndSpriteBatch(mBatcher);
		}

		#endregion rDraw





		#region rCommands

		/// <summary>
		/// Queue a texture draw.
		/// </summary>
		public void DrawTexture(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, int layer)
		{
			MDrawCommand cmd = new MDrawCommand(texture, position, sourceRect, color, rotation, origin, scale, effect);
			mDrawCommandLayers[layer].Add(cmd);
		}



		/// <summary>
		/// Queue a texture draw.
		/// </summary>
		public void DrawTexture<T>(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, T layer) where T : Enum
		{
			int layerInt = (int)(object)layer;

			DrawTexture(texture, position, sourceRect, color, rotation, origin, scale, effect, layerInt);
		}



		/// <summary>
		/// Queue a string draw.
		/// </summary>
		public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, int layer)
		{
			MDrawCommand cmd = new MDrawCommand(font, text, position, color, rotation, origin, scale, effect);
			mDrawCommandLayers[layer].Add(cmd);
		}



		/// <summary>
		/// Queue a string draw.
		/// </summary>
		public void DrawString<T>(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, T layer) where T : Enum
		{
			int layerInt = (int)(object)layer;

			DrawString(font, text, position, color, rotation, origin, scale, effect, layerInt);
		}

		#endregion rCommands
	}
}
