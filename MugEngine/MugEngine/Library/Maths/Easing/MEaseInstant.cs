namespace MugEngine.Library
{
	/// <summary>
	/// Ease function that is instantly one
	/// </summary>
	public class MEaseOne : IMEase
	{
		public float Eval(float t)
		{
			return 1.0f;
		}
	}



	/// <summary>
	/// Ease function that is always 0
	/// </summary>
	public class MEaseZero : IMEase
	{
		public float Eval(float t)
		{
			return 0.0f;
		}
	}
}
