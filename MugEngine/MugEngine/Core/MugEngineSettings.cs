namespace MugEngine.Core
{
	public enum MScreenFit
	{
		/// <summary>
		/// Find maximum integer to fit window.
		/// </summary>
		IntegerScale,

		/// <summary>
		/// Fit regardless of distortions made.
		/// </summary>
		StretchNearest
	}

	/// <summary>
	/// Settings to configure the engine.
	/// </summary>
	public struct MugEngineSettings
	{
		// Screen
		public Type[] mScreenTypes;
		public Type mStartScreen;

		// Settings
		public int mFPS;
		public Point mResolution;
		public int mNumLayers;
		public int mInputHistorySize;

		public MugEngineSettings()
		{
			SetDefaultSettings();
		}

		private void SetDefaultSettings()
		{
			mFPS = 60;
			mResolution = new Point(640, 360);
			mNumLayers = 1;

			mInputHistorySize = 180; // About 3 seconds
		}
	}

	/// <summary>
	/// Things needed to initalise the engine.
	/// </summary>
	public struct MugEngineInitParams
	{
		public GraphicsDeviceManager mGraphics;
		public ContentManager mContentManager;

		public MugEngineInitParams(GraphicsDeviceManager graphics, ContentManager content)
		{
			mGraphics = graphics;
			mContentManager = content;
		}
	}
}
