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

		public GamePadState mGamepadState0; // As 4 values to avoid garbage creation.
		public GamePadState mGamepadState1;
		public GamePadState mGamepadState2;
		public GamePadState mGamepadState3;

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

			mGamepadState0 = GamePad.GetState(0);
			mGamepadState1 = GamePad.GetState(1);
			mGamepadState2 = GamePad.GetState(2);
			mGamepadState3 = GamePad.GetState(3);
		}


		/// <summary>
		/// Create empty snapshot
		/// </summary>
		public MInputSnapshot()
		{
			mTimeStamp = TimeSpan.Zero;
			mKeyboardState = new KeyboardState();
			mMouseState = new MouseState();

			mGamepadState0 = GamePad.GetState(0);
			mGamepadState1 = GamePad.GetState(1);
			mGamepadState2 = GamePad.GetState(2);
			mGamepadState3 = GamePad.GetState(3);
		}



		/// <summary>
		/// Enumerate over gamepad states
		/// </summary>
		public IEnumerable<GamePadState> EnumerateGamepads()
		{
			yield return mGamepadState0;
			yield return mGamepadState1;
			yield return mGamepadState2;
			yield return mGamepadState3;
		}



		/// <summary>
		/// Get a gamepad state by index
		/// </summary>
		public GamePadState GetGamePadState(int idx)
		{
			switch (idx)
			{
				case 0:
					return mGamepadState0;
				case 1: 
					return mGamepadState1;
				case 2: 
					return mGamepadState2;
				case 3: 
					return mGamepadState3;
				default:
					break;
			}

			throw new IndexOutOfRangeException();
		}
	}
}
