using MugEngine.Core;

namespace MugEngine.Library
{
	/// <summary>
	/// Simple timer/stopwatch class.
	/// </summary>
	public class MTimer : IMUpdate
	{
		#region rMembers

		bool mPlaying;
		protected float mElapsedTime;

		#endregion rMembers





		#region rInitialisation

		/// <summary>
		/// Make timer
		/// </summary>
		public MTimer()
		{
			mElapsedTime = 0.0f;
			mPlaying = false;
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
		/// <returns>Time in MS</returns>
		public float GetElapsedMs()
		{
			return mElapsedTime;
		}



		/// <summary>
		/// Set elapsed milliseconds.
		/// </summary>
		public void SetElapsedMs(float time)
		{
			mElapsedTime = time;
		}

		#endregion rUtility
	}
}
