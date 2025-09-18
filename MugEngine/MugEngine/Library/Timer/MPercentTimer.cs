namespace MugEngine.Library
{
	/// <summary>
	/// Timer that runs on a portion of time.
	/// </summary>
	public struct MPercentTimer : IMUpdate
	{
		#region rMembers

		bool mPlaying;
		float mElapsedTime;
		float mTotalTime;

		#endregion rMembers





		#region rInitialisation

		/// <summary>
		/// Construct percentage timer
		/// </summary>
		/// <param name="totalTime">Time for it to reach 100%</param>
		public MPercentTimer(float totalTime)
		{
			mElapsedTime = 0.0f;
			mPlaying = false;
			mTotalTime = totalTime;
		}

		#endregion rInitialisation





		#region rUpdate

		/// <summary>
		/// Update timer. This needs to be called for it to play.
		/// </summary>
		/// <param name="info">Update information</param>
		public void Update(MUpdateInfo info)
		{
			if (mPlaying)
			{
				mElapsedTime += info.mDelta;
			}
		}



		/// <summary>
		/// Start the timer from 0
		/// </summary>
		public void Start()
		{
			Reset();
			mPlaying = true;
		}



		/// <summary>
		/// Resume playing the timer from where it left off.
		/// </summary>
		public void Resume()
		{
			mPlaying = true;
		}



		/// <summary>
		/// Stop the timer. Keeps it's place.
		/// </summary>
		public void Stop()
		{
			mPlaying = false;
		}



		/// <summary>
		/// Stop the timer and reset it.
		/// </summary>
		public void Reset()
		{
			mPlaying = false;
			mElapsedTime = 0.0f;
		}

		#endregion rUpdate




		#region rUtility

		/// <summary>
		/// Get percentage as a fraction of 1.0
		/// </summary>
		/// <returns>Number in the range [0.0,1.0]</returns>
		public float GetPercentage()
		{
			if (GetElapsed() < mTotalTime)
			{
				return GetElapsed() / mTotalTime;
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
			return GetElapsed() / mTotalTime;
		}



		/// <summary>
		/// Has the timer reached the goal time?
		/// </summary>
		public bool IsComplete()
		{
			return GetElapsed() >= mTotalTime;
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



		/// <summary>
		/// Get the "goal" amount of time.
		/// </summary>
		public float GetTotalTime()
		{
			return mTotalTime;
		}



		/// <summary>
		/// Get remaining time until complete
		/// </summary>
		public float GetRemainingTime()
		{
			return (mTotalTime - mElapsedTime);
		}



		/// <summary>
		/// Set amount this timer is aiming towards.
		/// </summary>
		public void SetTotalTime(float time)
		{
			mTotalTime = time;
		}
		
		
		
		/// <summary>
		/// Is the timer playing?
		/// </summary>
		/// <returns>True if the timer is playing</returns>
		public bool IsPlaying()
		{
			return mPlaying;
		}



		/// <summary>
		/// Get's elapsed time.
		/// </summary>
		/// <returns>Time in seconds</returns>
		public float GetElapsed()
		{
			return mElapsedTime;
		}



		/// <summary>
		/// Set elapsed seconds.
		/// </summary>
		public void SetElapsed(float time)
		{
			mElapsedTime = time;
		}



		/// <summary>
		/// Used for flashing effects
		/// </summary>
		public bool FlashOnOff(float flashTime)
		{
			return mPlaying && (int)(mElapsedTime / flashTime) % 2 == 0;
		}

		#endregion rUtility
	}
}
