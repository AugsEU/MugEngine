namespace MugEngine.Library;

public class MStateMachine<T> : IMUpdate where T : class, IMStateMachine<T>
{
	MSelector<T> mStateSelector;
	MTimer mTimeSinceEntered;

	public MStateMachine(MHandle<T> startState, params T[] states)
	{
		mStateSelector = new();
		mTimeSinceEntered = new();
		mTimeSinceEntered.Start();

		foreach (T state in states)
		{
			mStateSelector.Add(state);
		}
		
		mStateSelector.SetCurr(startState);
	}

	public void Update(MUpdateInfo info)
	{
		mTimeSinceEntered.Update(info);
		T curr = mStateSelector.GetCurr();
		MHandle<T>? nextState = curr.Update(info, mTimeSinceEntered.GetElapsed());

		if (nextState.HasValue)
		{
			// New state
			curr.OnExitState(info);
			mStateSelector.SetCurr(nextState.Value);
			mStateSelector.GetCurr().OnEnterState(info);
			mTimeSinceEntered.Start();
		}
	}

	public IEnumerable<T> GetStates()
	{
		return mStateSelector.GetStates();
	}

	public T GetCurrentState()
	{
		return mStateSelector.GetCurr();
	}

	public MHandle<T> GetCurrentStateHandle()
	{
		return mStateSelector.GetCurrHandle();
	}
}
