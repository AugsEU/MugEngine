namespace MugEngine.Library;

public interface IMStateMachine<T> where T : class
{
	public MHandle<T>? Update(MUpdateInfo info, float timeSinceEnter);

	public void OnEnterState(MUpdateInfo info);

	public void OnExitState(MUpdateInfo info);
}
