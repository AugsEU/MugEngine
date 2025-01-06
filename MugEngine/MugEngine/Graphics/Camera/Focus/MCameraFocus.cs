namespace MugEngine.Graphics
{
	public abstract class MCameraFocus
	{
		MCameraSpec mStartSpec;

		public void StartFocus(MCameraSpec startSpec)
		{
			mStartSpec = startSpec;
		}

		public abstract MCameraSpec UpdateFocusPoint(MUpdateInfo info, MCameraSpec curr);

		public MCameraSpec GetStartSpec()
		{
			return mStartSpec;
		}
	}
}
