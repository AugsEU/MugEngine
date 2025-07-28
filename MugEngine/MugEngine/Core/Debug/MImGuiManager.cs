using ImGuiNET;
using TracyWrapper;

namespace MugEngine.Core;

public class MImGuiManager : MSingleton<MImGuiManager>, IMUpdate
{

	MDelayChangeList<IMImGuiComponent> mComponents;
	MImGuiRenderer mRenderer;

	public MImGuiManager()
	{
	}

	public void Init(Game game)
	{
		mRenderer = new MImGuiRenderer(game);
		mComponents = new MDelayChangeList<IMImGuiComponent>();
	}

	public void AddComponent(IMImGuiComponent component)
	{
#if DEBUG
		mComponents.Add(component);
#endif
	}

	public void Update(MUpdateInfo info)
	{
#if DEBUG
		mComponents.ProcessAddsDeletes();
#endif
	}

	public void RenderImGui(GameTime time)
	{
#if DEBUG
		if(mComponents.Count == 0)
		{
			return;
		}

		Profiler.PushProfileZone("RenderImGui", ZoneC.LIGHT_BLUE);

		mRenderer.BeforeLayout(time);

		// Top level menu bar.
		if (ImGui.BeginMainMenuBar())
		{
			if (ImGui.BeginMenu("Windows"))
			{
				foreach (MImGuiWindow window in mComponents.OfType<MImGuiWindow>())
				{
					bool showWindow = window.Visible;
					if (ImGui.MenuItem(window.GetUniqueTitle(), null, showWindow))
					{
						window.Visible = !window.Visible; // Toggle window visibility
						if(window.Visible)
						{

						}
					}
				}

				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}

		foreach (IMImGuiComponent comp in mComponents)
		{
			comp.AddImGuiCommands(time);
		}

		mRenderer.AfterLayout();

		Profiler.PopProfileZone();
#endif
	}
}

