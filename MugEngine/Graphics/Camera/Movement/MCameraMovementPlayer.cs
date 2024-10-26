using MugEngine.Core;
using MugEngine.Types;

namespace MugEngine.Graphics
{
	/// <summary>
	/// Class that handles playing camera movement
	/// </summary>
	internal class MCameraMovementPlayer : IMUpdate
	{
		MCameraMovement mMovement;
		MPercentTimer mTimer;

		public MCameraMovementPlayer(MCameraMovement movement, float time)
		{
			mMovement = movement;
			mTimer = new MPercentTimer(time);
			mTimer.Start();
		}

		public void Update(MUpdateInfo info)
		{
			mTimer.Update(info);
		}

		public MCameraSpec GetSpecDelta()
		{
			return mMovement.GetSpecDelta(mTimer.GetPercentage());
		}

		public bool IsFinished()
		{
			return mTimer.IsComplete();
		}

		public MCameraSpec GetFinalDelta()
		{
			return mMovement.GetSpecDelta(1.0f);
		}
	}
}
