
namespace MugEngine.Graphics;

public class MGameObjectFocus : MSmoothPointFocus
{
	MGameObject mTarget;

	public MGameObjectFocus(MGameObject target)
	{
		mTarget = target;
	}

	protected override Vector2 GetTargetPoint()
	{
		return mTarget.GetCentreOfMass();
	}
}
