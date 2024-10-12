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

	public struct MugEngineSettings
	{
		// Init requirements
		public GraphicsDeviceManager mDeviceManager;
		public Type[] mScreenTypes;
		public Type mStartScreen;

		// Settings
		public int mFPS;
		public Point mResolution;
		public int mNumLayers;

		public MugEngineSettings(GraphicsDeviceManager deviceManager)
		{
			mDeviceManager = deviceManager;
			SetDefaultSettings();
		}

		private void SetDefaultSettings()
		{
			mFPS = 60;
			mResolution = new Point(640, 360);
			mNumLayers = 1;
		}
	}
}
