namespace MugEngine.Maths
{
	/// <summary>
	/// Reciprocal function that creates a T bend.
	/// </summary>
	internal class MEaseRecipTBend
	{
		float mBendValue;

		public MEaseRecipTBend(float bendValue)
		{
			mBendValue = bendValue;
		}

		public float Func(float t)
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
