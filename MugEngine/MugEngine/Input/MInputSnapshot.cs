using Microsoft.Xna.Framework.Input;

namespace MugEngine.Input
{
	/// <summary>
	/// All the data for a single frame of inputs
	/// </summary>
	public struct MInputSnapshot
	{
		public static readonly MInputSnapshot Default = new MInputSnapshot();

		public KeyboardState mKeyboardState;
		public MouseState mMouseState;

		public GamePadState[] mGamepadStates;

		public TimeSpan mTimeStamp;

		/// <summary>
		/// Create snap shot by polling all input devices
		/// </summary>
		/// <param name="timeStamp">Time of snapshot</param>
		public MInputSnapshot(TimeSpan timeStamp)
		{
			mTimeStamp = timeStamp;
			mKeyboardState = Keyboard.GetState();
			mMouseState = Mouse.GetState();

			int numGamepads = GamePad.MaximumGamePadCount;

			mGamepadStates = new GamePadState[numGamepads];
			for(int i = 0; i < numGamepads; i++)
			{
				mGamepadStates[i] = GamePad.GetState(i);
			}
		}


		/// <summary>
		/// Create empty snapshot
		/// </summary>
		public MInputSnapshot()
		{
			mTimeStamp = TimeSpan.Zero;
			mKeyboardState = new KeyboardState();
			mMouseState = new MouseState();

			int numGamepads = GamePad.MaximumGamePadCount;
			mGamepadStates = new GamePadState[numGamepads];
			for (int i = 0; i < numGamepads; i++)
			{
				mGamepadStates[i] = GamePadState.Default;
			}
		}
	}
}
