using MugEngine.Library;

namespace MugEngine.Input
{
	/// <summary>
	/// Button press methods for MugInput
	/// </summary>
	public partial class MugInput : MSingleton<MugInput>
	{
		/// <summary>
		/// Is this button currently down?
		/// </summary>
		public bool ButtonDown<T>(T id) where T : Enum
		{
			MButtonSet set = GetButtonSet(id);
			MInputSnapshot snapShot = mHistory.SnapshotFromFrames(0);
			return set.IsPressed(ref snapShot);
		}



		/// <summary>
		/// Is this button currently down but wasn't on the previous frame?
		/// </summary>
		public bool ButtonPressed<T>(T id) where T : Enum
		{
			MButtonSet set = GetButtonSet(id);
			MInputSnapshot snapShotNow = mHistory.SnapshotFromFrames(0);
			MInputSnapshot snapShotBefore = mHistory.SnapshotFromFrames(1);

			return set.IsPressed(ref snapShotNow) && !set.IsPressed(ref snapShotBefore);
		}
	}
}
