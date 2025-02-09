
using ImGuiNET;

namespace MugEngine.Core
{
	public class MIGHelloWorld : IMImGuiComponent
	{
		public void Update(MUpdateInfo info)
		{
		}

		public void AddImGuiCommands(GameTime time)
		{
			ImGui.Text("Hello world");
		}
	}
}
