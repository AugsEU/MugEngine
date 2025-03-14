using ImGuiNET;

namespace MugEngine.Core;

public class MIGDebugInfo : MImGuiWindow
{
	MRollingDeque<float> mFPSHistory;

	public MIGDebugInfo() : base("DebugInfo")
	{
		mFPSHistory = new MRollingDeque<float>(120);
	}

	protected override void AddWindowCommands(GameTime time)
	{
		// FPS
		float fps = 1.0f / (float)time.ElapsedGameTime.TotalSeconds;

		mFPSHistory.Add(fps);

		ImGui.Text($"FPS: {fps:F1}");

		float[] fpsArray = mFPSHistory.ToArray();

		// Plot FPS
		ImGui.PlotLines("Frame Rate", ref fpsArray[0], fpsArray.Length, 0, null, 0, 120, new System.Numerics.Vector2(0, 80));
	}
}

