namespace MugEngine.Graphics
{
	public abstract class MCameraFocus
	{
		MCameraSpec mStartSpec;
		MCameraSpec mCurrSpec;

		public void StartFocus(MCameraSpec startSpec)
		{
			mStartSpec = startSpec;
		}

		public abstract MCameraSpec UpdateFocusPoint(MUpdateInfo info, MCameraSpec curr);

		public MCameraSpec GetStartSpec()
		{
			return mStartSpec;
		}

		public MCameraSpec GetSpec()
		{
			return mCurrSpec;
		}
	}
}
