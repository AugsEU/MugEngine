namespace MugEngine.Library;

/// <summary>
/// Class that describes movement along a 2d plane.
/// </summary>
public abstract class Trajectory : IMUpdate
{
	public abstract void Update(MUpdateInfo info);

	public abstract Vector2 GetPosition();
}
