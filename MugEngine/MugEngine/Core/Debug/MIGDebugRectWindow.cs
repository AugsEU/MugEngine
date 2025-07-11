
using ImGuiNET;

namespace MugEngine.Core;

public class MIGDebugRectWindow : MImGuiWindow
{
	public MIGDebugRectWindow() : base("Debug Rect")
	{
	}

	protected override void AddWindowCommands(GameTime time)
	{
		List<(int, string)> layerNames = MugDebug.GetDebugRectLayerNames();
		foreach((int id, string name) in layerNames)
		{
			bool vis = MugDebug.GetDebugRectLayerVisible(id);
			bool setVis = ImGui.Checkbox(name, ref vis);
			MugDebug.SetDebugRectLayerVisible(id, vis);
		}
	}
}
