namespace MugEngine.Input
{
	public class MGamepadButton : MButton
	{
		Buttons mButton;
		int mIndex;

		public MGamepadButton(Buttons button, int gamepadIdx)
		{
			mIndex = gamepadIdx;
			mButton = button;
		}

		public override bool IsPressed(ref MInputSnapshot inputSnapshot)
		{
			int padIdx = mIndex;
			if (padIdx == -1)
			{
				padIdx = MugInput.I.GetMainGamepadIdx();
			}

			return inputSnapshot.mGamepadStates[padIdx].IsButtonDown(mButton);
		}
	}
}
