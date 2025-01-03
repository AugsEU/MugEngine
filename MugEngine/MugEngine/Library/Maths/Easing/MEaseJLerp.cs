namespace MugEngine.Library
{
	/// <summary>
	/// Reciprocal function that creates a J bend.
	/// 
	/// |      @
	/// |      @
	/// |    @
	/// |@@@
	/// -------------
	/// </summary>
	public class MEaseJLerp : IMEase
	{
		float mBendValue;

		public MEaseJLerp(float bendValue)
		{
			mBendValue = bendValue;
		}

		public float Eval(float t)
		{
			return Func(t, mBendValue);
		}

		public static float Func(float t, float bend)
		{
			bend -= 1.0f;
			t -= bend;
			t = bend * (bend - 1.0f) / t;
			t += bend;

			return 1.0f - t;
		}
	}
}
