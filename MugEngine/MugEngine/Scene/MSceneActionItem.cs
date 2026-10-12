namespace MugEngine.Scene;

/// <summary>
/// An action item is a task to be done at the end of the scene's frame
/// </summary>
public abstract class MSceneActionItem : IMDraw
{
	protected MScene mScene;

	/// <summary>
	/// Called when added to the scene and work begins
	/// </summary>
	public virtual void OnBegin(MScene parentScene)
	{
		mScene = parentScene;
	}

	/// <summary>
	/// Returns true if task is done.
	/// </summary>
	public abstract bool Update(MUpdateInfo info);

	/// <summary>
	/// Draw extra effects
	/// </summary>
	public abstract void Draw(MDrawInfo info);

	/// <summary>
	/// Should we block this component from updating?
	/// </summary>
	public virtual bool BlockComponentUpdate(MComponent comp)
	{
		return false;
	}

	/// <summary>
	/// Should we block this component from drawing?
	/// </summary>
	public virtual bool BlockComponentDraw(MComponent comp)
	{
		return false;
	}
}
