namespace MugEngine.Input
{
	internal class MKeyboardButton : MButton
	{
		Keys mKey = Keys.None;

		public MKeyboardButton(Keys key)
		{
			mKey = key;
		}

		public override bool IsPressed(ref MInputSnapshot inputSnapshot)
		{
			return inputSnapshot.mKeyboardState.IsKeyDown(mKey);
		}
	}
}
