namespace MugEngine
{
	/// <summary>
	/// Class that contains utilities for a game.
	/// </summary>
	public class MugMainGame : Game
	{
		#region rRegion

		static protected MugMainGame sSelf = null;

		protected GraphicsDeviceManager mGraphics;
		private Rectangle mPrevWindowedSize;
		private MugEngineSettings mSettings;

		private Thread mDebugThread;

		#endregion rRegion





		#region rInit

		/// <summary>
		/// Create main game.
		/// </summary>
		public MugMainGame(MugEngineSettings settings)
		{
			mGraphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			mSettings = settings;

			MugDebug.Assert(sSelf is null, "Cannot create multiple copies of MugMainGame.");
			sSelf = this;
		}

		private void ThreadTask()
		{
			TracyWrapper.Profiler.InitThread();
			List<byte[]> sillyList = new List<byte[]>();
			while (true)
			{
				for (int i = 0; i < 100; i++) sillyList.Add(new byte[10000]);

				TracyWrapper.Profiler.PushProfileZone("Thread Wait");
				Thread.Sleep(1);
				TracyWrapper.Profiler.PopProfileZone();
				if (sillyList.Count > 200)
				{
					sillyList.Clear();
				}
			}
		}


		/// <summary>
		/// Initialise the engine. Must be called.
		/// </summary>
		protected override void Initialize()
		{
			TracyWrapper.Profiler.InitThread();

			MugEngineInitParams initParams = new MugEngineInitParams(mGraphics, Content);
			MugCore.I.InitEngine(mSettings, initParams);

			// Fix to fps.
			IsFixedTimeStep = true;
			TargetElapsedTime = System.TimeSpan.FromSeconds(1d / mSettings.mFPS);

			Window.ClientSizeChanged += OnResize;

			mPrevWindowedSize = GraphicsDevice.PresentationParameters.Bounds;

			mDebugThread = new Thread(ThreadTask);
			mDebugThread.Name = "DebugThread";
			mDebugThread.Start();
		}

		#endregion rInit





		#region rWindow

		/// <summary>
		/// Enter/leave full screen
		/// </summary>
		private void ToggleFullscreen()
		{
			if (mGraphics.IsFullScreen)
			{
				mGraphics.IsFullScreen = false;
				mGraphics.PreferredBackBufferWidth = mPrevWindowedSize.Width;
				mGraphics.PreferredBackBufferHeight = mPrevWindowedSize.Height;
			}
			else
			{
				mPrevWindowedSize = GraphicsDevice.PresentationParameters.Bounds;
				mGraphics.IsFullScreen = true;

				mGraphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				mGraphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			}

			mGraphics.ApplyChanges();
		}



		/// <summary>
		/// Callback for re-sizing the screen
		/// </summary>
		/// <param name="sender">Sender of this event</param>
		/// <param name="eventArgs">Event args</param>
		private void OnResize(object sender, EventArgs eventArgs)
		{
			if (mGraphics.IsFullScreen)
			{
				return;
			}

			float aspectRatio = (float)mSettings.mResolution.X / mSettings.mResolution.Y;
			int minHeight = mSettings.mResolution.Y;
			int minWidth = (int)(aspectRatio * minHeight);

			if (Window.ClientBounds.Height >= minHeight && Window.ClientBounds.Width >= minWidth)
			{
				return;
			}
			else
			{
				// Set window size to minimum
				mGraphics.PreferredBackBufferWidth = Math.Max(minWidth, Window.ClientBounds.Width);
				mGraphics.PreferredBackBufferHeight = Math.Max(minHeight, Window.ClientBounds.Height);
				mGraphics.ApplyChanges();
			}

			mPrevWindowedSize = GraphicsDevice.PresentationParameters.Bounds;
		}



		/// <summary>
		/// Set window height(width is inferred from aspect ratio)
		/// </summary>
		protected void SetWindowHeight(int height)
		{
			float aspectRatio = (float)mSettings.mResolution.X / mSettings.mResolution.Y;
			height = Math.Max(mSettings.mResolution.Y, height);
			int width = (int)(aspectRatio * height);

			mGraphics.PreferredBackBufferWidth = width;
			mGraphics.PreferredBackBufferHeight = height;
			mGraphics.ApplyChanges();

			mPrevWindowedSize = GraphicsDevice.PresentationParameters.Bounds;
		}


		/// <summary>
		/// Set window size based on multiple of resolution
		/// </summary>
		protected void SetWindowSize(float resMult)
		{
			resMult = Math.Max(1.0f, resMult);
			SetWindowHeight((int)(resMult * mSettings.mResolution.Y));
		}



		/// <summary>
		/// 
		/// </summary>
		private void ForceWindowSize(int height, int width)
		{

		}



		/// <summary>
		/// Set fullscreen directly
		/// </summary>
		public static void SetFullScreen(bool isFullScreen)
		{
			if (isFullScreen != sSelf.mGraphics.IsFullScreen)
			{
				sSelf.ToggleFullscreen();
			}
		}



		/// <summary>
		/// Are we full screen?
		/// </summary>
		public static bool IsFullScreen()
		{
			return sSelf.mGraphics.IsFullScreen;
		}

		#endregion rWindow





		#region rUpdate

		/// <summary>
		/// Update game.
		/// </summary>
		protected override void Update(GameTime gameTime)
		{
			TracyWrapper.Profiler.HeartBeat();

			MugCore.I.UpdateEngine(gameTime);
			base.Update(gameTime);
		}

		#endregion rUpdate





		#region rRegion

		/// <summary>
		/// Draw game.
		/// </summary>
		protected override void Draw(GameTime gameTime)
		{
			MugCore.I.DrawEngine(gameTime);
			base.Draw(gameTime);
		}

		#endregion rRegion


	}
}
