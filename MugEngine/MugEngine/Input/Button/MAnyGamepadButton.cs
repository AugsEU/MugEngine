using System.Reflection.Metadata.Ecma335;

namespace MugEngine.Input
{
	/// <summary>
	/// Represnets the same button across all gamepads.
	/// E.g. "A" button but pressing it on any gamepad works.
	/// </summary>
	public class MAnyGamepadButton : MButton
	{
		Buttons mButton;

		public MAnyGamepadButton(Buttons button, int gamepadIdx)
		{
			mButton = button;
		}

		public override bool IsPressed(ref MInputSnapshot inputSnapshot)
		{
			for(int i = 0; i < inputSnapshot.mGamepadStates.Length; i++)
			{
				if(inputSnapshot.mGamepadStates[i].IsButtonDown(mButton))
				{
					return true;
				}
			}
			return false;
		}
	}
}
