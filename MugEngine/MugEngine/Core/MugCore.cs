#if DEBUG
#define IMGUI_ON
#endif

using ImGuiNET;

namespace MugEngine.Core;

public class MugCore : MSingleton<MugCore>
{
	#region rMembers

	MugEngineSettings mSettings;
	MCanvas2D mBackBufferCanvas;
	GraphicsDeviceManager mGraphics;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Initialise the engine.
	/// </summary>
	public void InitEngine(MugEngineSettings settings, MugEngineInitParams init)
	{
		// Init params
		mGraphics = init.mGraphics;

		// Load data
		MData.I.Init(init.mContentManager);

		// General setup
		mSettings = settings;
		MScreenManager.I.AddScreenTypes(settings.mResolution, settings.mScreenTypes);
		MScreenManager.I.LoadScreens(settings.mStartScreen);

		MugInput.I.Init(mSettings.mInputHistorySize);

		mBackBufferCanvas = new MCanvas2D();
	}

	#endregion rInit





	#region rUpdate

	/// <summary>
	/// Update game engine
	/// </summary>
	public void UpdateEngine(GameTime gameTime)
	{
		TracyWrapper.Profiler.PushProfileZone("Update");

		MUpdateInfo info = new MUpdateInfo();
		info.mDelta = MugUtil.ToDelta(gameTime);

		// Poll inputs
		MugInput.I.Update(gameTime.TotalGameTime);

		// Update game state
		MScreenManager.I.Update(info);

#if IMGUI_ON
		MImGuiManager.I.Update(info);
#endif

		TracyWrapper.Profiler.PopProfileZone();
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

#if IMGUI_ON
		MImGuiManager.I.RenderImGui(gameTime);
#endif
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
		return mGraphics.GraphicsDevice;
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

