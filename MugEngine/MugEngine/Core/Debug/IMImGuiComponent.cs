namespace MugEngine.Core
{
	public interface IMImGuiComponent : IMUpdate
	{
		public void AddImGuiCommands(GameTime time);
	}
}
