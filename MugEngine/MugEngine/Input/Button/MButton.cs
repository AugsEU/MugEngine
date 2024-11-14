namespace MugEngine.Input
{
	/// <summary>
	/// Button that can be pressed or not pressed
	/// </summary>
	public abstract class MButton
	{
		public abstract bool IsPressed(ref MInputSnapshot inputSnapshot);
	}
}
