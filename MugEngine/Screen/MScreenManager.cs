//using MugEngine.Types;

//namespace MugEngine.Screen
//{
//	/// <summary>
//	/// Class that manages all screens
//	/// </summary>
//	public class MScreenManager : MSingleton<MScreenManager> 
//	{
//		#region rMembers

//		Dictionary<int, MScreen> mScreens = new Dictionary<int, MScreen>();
//		int mActiveScreen = -1;

//		#endregion rMembers





//		#region rInitialise

//		/// <summary>
//		/// Load all the screens, but don't start them.
//		/// </summary>
//		/// <param name="deviceManager">Graphics device</param>
//		public void LoadAllScreens(GraphicsDeviceManager deviceManager)
//		{
//			mScreens.Clear();
//		}



//		/// <summary>
//		/// Load a screen of a specific type.
//		/// </summary>
//		/// <param name="type"></param>
//		/// <param name="screen"></param>
//		private void LoadScreen(int type, MScreen screen)
//		{
//			mScreens.Add(type, screen);
//		}

//		#endregion rInitialise





//		#region rUtility

//		/// <summary>
//		/// Get a screen of a certain type
//		/// </summary>
//		/// <param name="type">Screen type you want to find.</param>
//		/// <returns>Screen of that type, null if that type doesn't exist</returns>
//		public Screen GetScreen(ScreenType type)
//		{
//			if (mScreens.ContainsKey(type))
//			{
//				return mScreens[type];
//			}

//			return null;
//		}



//		/// <summary>
//		/// Get screen of type with cast for free.
//		/// </summary>
//		public T GetScreen<T>() where T : Screen
//		{
//			foreach (var item in mScreens)
//			{
//				if (item.Value is T returnValue)
//				{
//					return returnValue;
//				}
//			}

//			return null;
//		}



//		/// <summary>
//		/// Get the currently active screen
//		/// </summary>
//		/// <returns>Active screen refernece, null if there is none.</returns>
//		public Screen GetActiveScreen()
//		{
//			if (mScreens.ContainsKey(mActiveScreen))
//			{
//				return mScreens[mActiveScreen];
//			}

//			return null;
//		}



//		/// <summary>
//		/// Get the type of screen that is active
//		/// </summary>
//		public ScreenType GetActiveScreenType()
//		{
//			return mActiveScreen;
//		}



//		/// <summary>
//		/// Activates a screen of a certain type
//		/// </summary>
//		/// <param name="type">Screen type you want to actiavet</param>
//		public void ActivateScreen(ScreenType type)
//		{
//			if (!mScreens.ContainsKey(type))
//			{
//				return;
//			}

//			if (mScreens.ContainsKey(mActiveScreen))
//			{
//				mScreens[mActiveScreen].OnDeactivate();
//			}

//			Camera screenCam = CameraManager.I.GetCamera(CameraManager.CameraInstance.ScreenCamera);
//			screenCam.Reset();

//			mActiveScreen = type;

//			mScreens[type].OnActivate();

//		}

//		#endregion rUtility
//	}
//}
