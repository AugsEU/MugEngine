using MugEngine.Types;

namespace MugEngine.Core
{
	public class MugEngine : MSingleton<MugEngine>
	{
		#region rMembers

		GraphicsDeviceManager mGraphicsDeviceManager;

		#endregion rMembers



		public MugEngine()
		{
		}

		public void InitEngine(GraphicsDeviceManager deviceManager)
		{
			mGraphicsDeviceManager = deviceManager;
		}
	}
}
