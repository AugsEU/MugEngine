namespace MugEngine.Library
{
	/// <summary>
	/// Reciprocal function that creates a T bend.
	/// 
	/// |     @@@@@@@
	/// |   @
	/// | @
	/// |@
	/// -------------
	/// </summary>
	public class MEaseTLerp
	{
		float mBendValue;

		public MEaseTLerp(float bendValue)
		{
			mBendValue = bendValue;
		}

		public float Eval(float t)
		{
			return Func(t, mBendValue);
		}

		public static float Func(float t, float bend)
		{
			bend = -bend;
			t -= bend;
			t = bend * (bend - 1.0f) / t;
			t += bend;

			return 1.0f - t;
		}
	}
}
