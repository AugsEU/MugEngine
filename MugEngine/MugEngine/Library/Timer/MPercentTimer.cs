namespace MugEngine.Library.Timer
{
	/// <summary>
	/// Timer that runs on a portion of time.
	/// </summary>
	public class MPercentTimer : MTimer
	{
		#region rMembers

		float mTotalTime;

		#endregion rMembers





		#region rInitialisation

		/// <summary>
		/// Construct percentage timer
		/// </summary>
		/// <param name="totalTime">Time for it to reach 100%</param>
		public MPercentTimer(float totalTime) : base()
		{
			mTotalTime = totalTime;
		}

		#endregion rInitialisation





		#region rUtility

		/// <summary>
		/// Get percentage as a fraction of 1.0
		/// </summary>
		/// <returns>Number in the range [0.0,1.0]</returns>
		public float GetPercentage()
		{
			if (GetElapsedMs() < mTotalTime)
			{
				return GetElapsedMs() / mTotalTime;
			}

			//Return 1.0 after exceeding the total time
			return 1.0f;
		}



		/// <summary>
		/// Get percentage as float but not capped at 1.0
		/// </summary>
		/// <returns>Number in the range [0.0f, inf)</returns>
		public float GetPercentageUncap()
		{
			return GetElapsedMs() / mTotalTime;
		}



		/// <summary>
		/// Has the timer reached the goal time?
		/// </summary>
		public bool IsComplete()
		{
			return GetElapsedMs() >= mTotalTime;
		}



		/// <summary>
		/// Jump to 100%
		/// </summary>
		public void SetComplete()
		{
			mElapsedTime = mTotalTime;
		}



		/// <summary>
		/// Set time as a percentage
		/// </summary>
		public void SetPercentTime(float timePercent)
		{
			mElapsedTime = mTotalTime * timePercent;
		}

		#endregion rUtility
	}
}
