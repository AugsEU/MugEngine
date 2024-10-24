using MugEngine.Graphics;
using MugEngine.Screen;
using MugEngine.Types;

namespace MugEngine.Core
{
	public class MugCore : MSingleton<MugCore>
	{
		#region rMembers

		MugEngineSettings mSettings;
		MCanvas2D mBackBufferCanvas;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create mug engine. Called automatically be singleton
		/// </summary>
		public MugCore()
		{
		}



		/// <summary>
		/// Initialise the engine.
		/// </summary>
		public void InitEngine(MugEngineSettings settings)
		{
			mSettings = settings;
			MScreenManager.I.AddScreenTypes(settings.mResolution, settings.mScreenTypes);
			MScreenManager.I.LoadScreens(settings.mStartScreen);

			mBackBufferCanvas = new MCanvas2D();
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update game engine
		/// </summary>
		public void UpdateEngine(GameTime gameTime)
		{
			MUpdateInfo info = new MUpdateInfo();
			info.mDelta = MugUtil.ToDelta(gameTime);

			MScreenManager.I.Update(info);
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw game engine output
		/// </summary>
		public void DrawEngine(GameTime gameTime)
		{
			MDrawInfo info = new MDrawInfo();
			info.mDelta = MugUtil.ToDelta(gameTime);
			info.mCanvas = mBackBufferCanvas;

			MScreenManager.I.Draw(info);
		}

		#endregion rDraw





		#region rUtil

		#endregion rUtil






		#region rAccess

		/// <summary>
		/// Get the graphics device bound to this
		/// </summary>
		public GraphicsDevice GetDevice()
		{
			return mSettings.mDeviceManager.GraphicsDevice;
		}



		/// <summary>
		/// Get number of layers to use.
		/// To do: Rethink this.
		/// </summary>
		public int GetNumLayers()
		{
			return mSettings.mNumLayers;
		}

		#endregion rAccess
	}
}
