namespace MugEngine.Input
{
	/// <summary>
	/// Represents a collection of buttons.
	/// </summary>
	internal class MButtonSet
	{
		List<MButton> mButtons;

		public MButtonSet(MButton[] buttons)
		{
			mButtons = new List<MButton>(buttons);
		}

		public bool IsPressed(ref MInputSnapshot shapshot)
		{
			foreach (MButton button in mButtons)
			{
				if (button.IsPressed(ref shapshot))
				{
					return true;
				}
			}

			return false;
		}
	}
}
