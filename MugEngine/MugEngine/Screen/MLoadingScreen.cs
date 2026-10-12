using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugEngine;

public abstract class MLoadingScreen : MScreen
{
	MHandle<MScreen> mNextScreen;
	int mProgress;
	int mMaxProgress;

	public MLoadingScreen(Point resolution, MHandle<MScreen> nextScreen, int maxProgress) : base(resolution)
	{
		mNextScreen = nextScreen;
		mProgress = 0;
		mMaxProgress = maxProgress;
	}

	public override void Update(MUpdateInfo info)
	{
		if (mProgress < mMaxProgress)
		{
			DoLoadingStep(mProgress);
			mProgress++;
		}

		if (mProgress == mMaxProgress)
		{
			MScreenManager.I.ActivateScreen(mNextScreen);
			mProgress++;
		}

		base.Update(info);
	}

	public abstract void DoLoadingStep(int stepNum);
}
