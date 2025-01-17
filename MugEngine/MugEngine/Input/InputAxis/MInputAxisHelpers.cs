namespace MugEngine.Input
{
	/// <summary>
	/// Axis interpret helpers for MugEngine
	/// </summary>
	public partial class MugInput : MSingleton<MugInput>
	{
		/// <summary>
		/// Set the axis's position as 0, 0
		/// </summary>
		public void CalibrateAxis<T>(T id) where T : Enum
		{
			GetInputAxis(id).CalibrateToNow();
		}

		/// <summary>
		/// Is this button currently down but wasn't on the previous frame?
		/// </summary>
		public float AxisValue<T>(T id) where T : Enum
		{
			MInputAxis axis = GetInputAxis(id);
			MInputSnapshot snapShotNow = mHistory.Now();

			return axis.AxisValue(ref snapShotNow);
		}
	}
}
