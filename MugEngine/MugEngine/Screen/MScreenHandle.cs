using MugEngine.Core;

namespace MugEngine.Screen
{
	/// <summary>
	/// A handle that refers to a type of screen.
	/// </summary>
	public class MScreenHandle : MTypeHandle<MScreen>
	{
		public MScreenHandle(Type screenType) : base(screenType)
		{
		}

		public static MScreenHandle From<S>() where S : MScreen
		{
			return new MScreenHandle(typeof(S));
		}
	}
}
