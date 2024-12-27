namespace MugEngine.Library.Maths.Easing
{
	/// <summary>
	/// Reciprocal function that creates a J bend.
	/// </summary>
	internal class MEaseRecipJBend : IMEase
	{
		float mBendValue;

		public MEaseRecipJBend(float bendValue)
		{
			mBendValue = bendValue;
		}

		public float Func(float t)
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
