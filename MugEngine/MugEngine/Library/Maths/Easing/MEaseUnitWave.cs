namespace MugEngine.Library
{
	/// <summary>
	/// Go smoothly from 0 to 1 to 0
	/// 
	/// |    @@
	/// |   @  @
	/// |  @    @
	/// |@@      @@
	/// -------------
	/// </summary>
	public class MEaseUnitWave : IMEase
	{
		public float Eval(float t)
		{
			return Func(t);
		}

		public static float Func(float t)
		{
			t *= 4.0f;
			t += 1.0f;
			t %= 4.0f;
			t -= 2.0f;
			t *= 2.0f - MathF.Abs(t);
			t += 1.0f;
			t *= 0.5f;
			return t;
		}
	}
}
