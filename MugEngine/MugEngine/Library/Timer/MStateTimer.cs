namespace MugEngine.Library
{
	/// <summary>
	/// Info about a state
	/// </summary>
	public struct MStateTimerState
	{
		public MTimer mTimeSinceEntered;
		public int mFramesSinceEntered;

		public MStateTimerState()
		{
			mTimeSinceEntered = new();
			mFramesSinceEntered = 0;
		}
	}

	/// <summary>
	/// Simple state machine that times it's states.
	/// </summary>
	public class MStateTimer<T> : IMUpdate where T : Enum
	{
		#region rMembers

		T mCurrState;
		Dictionary<T, int> mEnumToIndex;
		MStateTimerState[] mStates;

		#endregion rMembers





		#region rInitialisation

		/// <summary>
		/// Construct a state machine with a starting state
		/// </summary>
		/// <param name="start">State to start in</param>
		public MStateTimer(T start)
		{
			mCurrState = start;

			mEnumToIndex = new();
			int acc = 0;
			foreach(T state in MugEnum.EnumIter<T>())
			{
				mEnumToIndex[state] = acc;
				acc += 1;
			}

			mStates = new MStateTimerState[acc];
			for (int i = 0; i < acc; i++)
			{
				mStates[i] = new MStateTimerState();
			}
		}

		#endregion





		#region rUpdate

		/// <summary>
		/// Update state machine timer
		/// </summary>
		public void Update(MUpdateInfo info)
		{
			int idx = mEnumToIndex[mCurrState];
			mStates[idx].mTimeSinceEntered.Update(info);
			mStates[idx].mFramesSinceEntered++;
		}

		#endregion rUpdate





		#region rUtil

		/// <summary>
		/// Enter a state
		/// </summary>
		public void EnterState(T state)
		{
			if(mCurrState.Equals(state))
			{
				return;
			}

			int prevIdx = mEnumToIndex[mCurrState];
			mStates[prevIdx].mTimeSinceEntered.Reset();
			mStates[prevIdx].mFramesSinceEntered = 0;

			mCurrState = state;

			int currIdx = mEnumToIndex[mCurrState];
			mStates[currIdx].mTimeSinceEntered.Start();
			mStates[currIdx].mFramesSinceEntered = 0;
		}


		/// <summary>
		/// Re-enter the current state.
		/// </summary>
		public void ResetCurrState()
		{
			int currIdx = mEnumToIndex[mCurrState];
			mStates[currIdx].mTimeSinceEntered.Start();
			mStates[currIdx].mFramesSinceEntered = 0;
		}



		/// <summary>
		/// Get the current state
		/// </summary>
		public T GetCurrState()
		{
			return mCurrState;
		}



		/// <summary>
		/// Get the current state info
		/// </summary>
		public (T, MStateTimerState) GetCurrStateTimeInfo()
		{
			int idx = mEnumToIndex[mCurrState];
			return (mCurrState, mStates[idx]);
		}



		/// <summary>
		/// Get number of frames we have been in this state.
		/// </summary>
		public int GetFramesInState()
		{
			return GetFramesInState(mCurrState);
		}



		/// <summary>
		/// Get number of frames we have been in this state.
		/// </summary>
		public int GetFramesInState(T state)
		{
			int idx = mEnumToIndex[state];
			return mStates[idx].mFramesSinceEntered;
		}



		/// <summary>
		/// Get number of frames we have been in this state.
		/// </summary>
		public float GetTimeInState()
		{
			int idx = mEnumToIndex[mCurrState];
			return mStates[idx].mTimeSinceEntered.GetElapsed();
		}

		#endregion rUtil



	}
}
