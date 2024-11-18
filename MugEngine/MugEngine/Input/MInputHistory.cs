using Nito.Collections;

namespace MugEngine.Input
{
	/// <summary>
	/// Holds data about input history
	/// </summary>
	public class MInputHistory
	{
		#region rMembers

		int mMaxHistorySize;
		Deque<MInputSnapshot> mHistory;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create input history class
		/// </summary>
		public MInputHistory(int maxHistorySize)
		{
			const int SIZE_OVERFLOW = 8;
			mHistory = new Deque<MInputSnapshot>(maxHistorySize + SIZE_OVERFLOW);
			mMaxHistorySize = maxHistorySize;
		}

		#endregion rInit





		#region rPoll

		/// <summary>
		/// Polls state of all inputs using MonoGame
		/// </summary>
		public void PollInputs(TimeSpan timeStamp)
		{
			if (mHistory.Count > mMaxHistorySize)
			{
				mHistory.RemoveFromBack();
			}

			mHistory.AddToFront(new MInputSnapshot(timeStamp));
		}

		#endregion rPoll



		#region rAccess

		/// <summary>
		/// Get snapshot N time back
		/// </summary>
		public MInputSnapshot SnapshotFromTime(TimeSpan timeStamp)
		{
			if(mHistory.Count < 0)
			{
				return MInputSnapshot.Default;
			}

			int snapIdx = 0;
			TimeSpan frontTime = mHistory[0].mTimeStamp;
			for(int i = 1; i < mHistory.Count; i++)
			{
				if(frontTime - mHistory[i].mTimeStamp < TimeSpan.Zero)
				{
					snapIdx = i;
					break;
				}
			}

			return mHistory[snapIdx];
		}



		/// <summary>
		/// Get snapshot N frames back
		/// </summary>
		public MInputSnapshot SnapshotFromFrames(int frames)
		{
			if (mHistory.Count < frames)
			{
				return MInputSnapshot.Default;
			}

			return mHistory[frames];
		}

		#endregion rAccess
	}
}
