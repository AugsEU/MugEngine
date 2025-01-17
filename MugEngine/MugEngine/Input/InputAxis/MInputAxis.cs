namespace MugEngine.Input
{
	public abstract class MInputAxis
	{
		public abstract float AxisValue(ref MInputSnapshot inputSnapshot);

		public abstract void CalibrateToNow();
	}
}
