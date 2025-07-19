namespace MugEngine.Screen
{
	/// <summary>
	/// Class that manages all screens
	/// </summary>
	public class MScreenManager : MSingleton<MScreenManager>, IMUpdate, IMDraw
	{
		#region rMembers

		Dictionary<MHandle<MScreen>, MScreen> mScreens = new Dictionary<MHandle<MScreen>, MScreen>();
		MScreen mActiveScreen = null;
		MScreen mNextScreen = null;

		MScreenFit mFitSetting = MScreenFit.IntegerScale;

		#endregion rMembers





		#region rInitialise

		/// <summary>
		/// Load a screens of a specific types.
		/// Screens must be registered here to work.
		/// </summary>
		public void AddScreenTypes(Point resolution, params Type[] screenTypes)
		{
			if (mActiveScreen is not null)
			{
				throw new Exception("Can't add more screen types after initialisation.");
			}

			foreach (var type in screenTypes)
			{
				if (!typeof(MScreen).IsAssignableFrom(type))
				{
					throw new Exception("Invalid screen type.");
				}

				MScreen screen = Activator.CreateInstance(type, resolution) as MScreen;

				if (screen is null)
				{
					throw new Exception(string.Format("Failed to create {0}", type.ToString()));
				}

				mScreens.Add(new MHandle<MScreen>(type), screen);
			}
		}



		/// <summary>
		/// Do final init for screens.
		/// </summary>
		public void LoadScreens(MHandle<MScreen> startScreen)
		{
			foreach (MScreen screen in mScreens.Values)
			{
				screen.Initialise();
			}

			mActiveScreen = mScreens[startScreen];
			mActiveScreen.OnActivate();
		}

		#endregion rInitialise





		#region rUpdate

		/// <summary>
		/// Update the screens
		/// </summary>
		public void Update(MUpdateInfo updateInfo)
		{
			mActiveScreen.Update(updateInfo);

			if (mNextScreen is not null)
			{
				if (mActiveScreen.AllowQuit())
				{
					mActiveScreen = mNextScreen;
					mNextScreen = null;
					mActiveScreen.OnActivate();
				}
			}
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw main game with the largest possible integer scaling
		/// </summary>
		public void Draw(MDrawInfo info)
		{
			if (mActiveScreen is null)
			{
				return;
			}

			mActiveScreen.Draw(info);

			GraphicsDevice device = MugCore.I.GetDevice();
			Texture2D finalFrame = mActiveScreen.GetCanvas().GetOutput();
			Rectangle backBuffer = device.PresentationParameters.Bounds;

			Rectangle destBounds = GetDestScreenBounds(backBuffer, finalFrame);

			Point mouseWindowPoint = MugInput.I.GetCurrState().mMouseState.Position - destBounds.Location;
			Vector2 mouseScreenPos = mouseWindowPoint.ToVector2();
			mouseScreenPos.X *= ((float)finalFrame.Width / (float)destBounds.Width);
			mouseScreenPos.Y *= ((float)finalFrame.Height / (float)destBounds.Height);
			MugInput.I.UpdateMousePosition(0, mouseScreenPos);

			// Draw the frame
			MDrawInfo canvasInfo = info.mCanvas.BeginDraw(info.mDelta);

			info.mCanvas.DrawTexture(finalFrame, destBounds, 0);

			info.mCanvas.EndDraw();
		}



		/// <summary>
		/// Calc the bounds the screen should draw to.
		/// </summary>
		private Rectangle GetDestScreenBounds(Rectangle backBuffer, Texture2D outputTexture)
		{
			switch (mFitSetting)
			{
				case MScreenFit.IntegerScale:
					return IntegerBounds(backBuffer, outputTexture);
				case MScreenFit.StretchNearest:
					return StretchNearestBounds(backBuffer, outputTexture);
				default:
					break;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Calc bounds with the largest possible integer scaling
		/// </summary>
		private Rectangle IntegerBounds(Rectangle backBuffer, Texture2D outputTexture)
		{
			int multiplier = (int)MathF.Min(backBuffer.Width / outputTexture.Width, backBuffer.Height / outputTexture.Height);

			int finalWidth = outputTexture.Width * multiplier;
			int finalHeight = outputTexture.Height * multiplier;

			return new Rectangle((backBuffer.Width - finalWidth) / 2, (backBuffer.Height - finalHeight) / 2, finalWidth, finalHeight);
		}



		/// <summary>
		/// Calc bounds with the largest possible scaling
		/// </summary>
		private Rectangle StretchNearestBounds(Rectangle backBuffer, Texture2D outputTexture)
		{
			float multiplier = MathF.Min((float)backBuffer.Width / outputTexture.Width, (float)backBuffer.Height / outputTexture.Height);

			int finalWidth = (int)(outputTexture.Width * multiplier);
			int finalHeight = (int)(outputTexture.Height * multiplier);

			return new Rectangle((backBuffer.Width - finalWidth) / 2, (backBuffer.Height - finalHeight) / 2, finalWidth, finalHeight);
		}

		#endregion rDraw





		#region rUtility

		/// <summary>
		/// Get screen from handle
		/// </summary>
		public MScreen GetScreen(MHandle<MScreen> handle)
		{
			MScreen retScreen = null;
			if (mScreens.TryGetValue(handle, out retScreen))
			{
				return retScreen;
			}

			throw new Exception("Can't find screen of type.");
		}



		/// <summary>
		/// Get the currently active screen
		/// </summary>
		/// <returns>Active screen refernece, null if there is none.</returns>
		public MScreen GetActiveScreen()
		{
			return mActiveScreen;
		}



		/// <summary>
		/// Activates a screen of a certain type
		/// </summary>
		public void ActivateScreen(MHandle<MScreen> handle)
		{
			if (mNextScreen is null)
			{
				// Only do this if we haven't already started.
				mActiveScreen.BeginDeactivate();
			}

			mNextScreen = GetScreen(handle);
		}

		#endregion rUtility
	}
}
