namespace MugEngine.Input
{
	public class MScrollWheelAxis : MInputAxis
	{
		bool mHorizontal;
		float mStartValue;
		float mScale;

		public MScrollWheelAxis(float scale, bool horizontal = false)
		{
			mHorizontal = horizontal;
			mScale = scale;

			CalibrateToNow();
		}

		/// <summary>
		/// Recalibrate start value to the current value.
		/// </summary>
		public override void CalibrateToNow()
		{
			MInputSnapshot curr = MugInput.I.GetCurrState();
			mStartValue = mHorizontal ? curr.mMouseState.HorizontalScrollWheelValue : curr.mMouseState.ScrollWheelValue;
		}

		public void SetStartValue(int value)
		{
			mStartValue = value;
		}

		public override float AxisValue(ref MInputSnapshot inputSnapshot)
		{
			float value = 0.0f;

			if (mHorizontal)
			{
				value = (float)inputSnapshot.mMouseState.HorizontalScrollWheelValue;
			}
			else
			{
				value = (float)inputSnapshot.mMouseState.ScrollWheelValue;
			}

			return (value - mStartValue) * mScale;
		}
	}
}
