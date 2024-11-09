namespace MugEngine.Types
{
	/// <summary>
	/// Simple singleton implementation. It uses lazy initialisation.
	/// </summary>
	/// <typeparam name="TClass">Use CRTP to make singleton of yourself</typeparam>
	public abstract class MSingleton<TClass> where TClass : class, new()
	{
		protected MSingleton()
		{
		}

		public static TClass I { get { return Nested.mInstance; } }

		private class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly TClass mInstance = new TClass();
		}
	}
}
