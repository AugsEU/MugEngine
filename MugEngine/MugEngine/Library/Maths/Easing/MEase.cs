namespace MugEngine.Library
{
	/// <summary>
	/// Contains many easing functions.
	/// All take input from 0 to 1, and return
	/// x=0 -> 0
	/// x=1 -> 1
	/// </summary>
	public interface IMEase
	{
		#region rClass

		public float Func(float t);

		#endregion rClass
	}
}
