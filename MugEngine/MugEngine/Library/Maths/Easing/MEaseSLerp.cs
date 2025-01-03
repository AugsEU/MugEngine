namespace MugEngine.Library
{
	/// <summary>
	/// Reciprocal function that creates a T bend.
	/// 
	/// |       @@
	/// |     @
	/// |   @
	/// |@@
	/// -------------
	/// </summary>
	public class MEaseSLerp : IMEase
	{
		public float Eval(float t)
		{
			return Func(t);
		}

		public static float Func(float t)
		{
			t *= 2.0f;
			t -= 1.0f;
			t *= 2.0f - MathF.Abs(t);
			t += 1.0f;
			t *= 0.5f;
			return t;
		}
	}
}
