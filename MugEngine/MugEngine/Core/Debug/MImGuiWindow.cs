
using ImGuiNET;

namespace MugEngine.Core
{
	public abstract class MImGuiWindow : IMImGuiComponent
	{
		public bool Visible { get; set; }

		protected string mUniqueTitle;

		public MImGuiWindow(string title)
		{
			mUniqueTitle = title;
			Visible = false;
		}

		public void AddImGuiCommands(GameTime time)
		{
			if (!Visible)
			{
				return;
			}

			ImGui.Begin(mUniqueTitle);

			AddWindowCommands(time);

			ImGui.End();
		}

		protected abstract void AddWindowCommands(GameTime time);

		public string GetUniqueTitle()
		{
			return mUniqueTitle;
		}
	}
}
