using MugEngine.Types;

namespace MugEngine
{
	/// <summary>
	/// A thing that can be updated.
	/// </summary>
	public interface IMUpdate
	{
		public void Update(MUpdateInfo info);
	}
}
