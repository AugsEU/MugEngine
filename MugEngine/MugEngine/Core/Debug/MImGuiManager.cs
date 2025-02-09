using ImGuiNET;

namespace MugEngine.Core
{
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

			foreach (IMImGuiComponent comp in mComponents)
			{
				comp.Update(info);
			}
#endif
		}

		public void RenderImGui(GameTime time)
		{
#if DEBUG
			if(mComponents.Count == 0)
			{
				return;
			}

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
#endif
		}
	}
}
