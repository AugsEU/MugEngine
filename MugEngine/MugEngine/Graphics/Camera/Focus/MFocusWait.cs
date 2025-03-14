
namespace MugEngine.Graphics;

/// <summary>
/// The most basic focus system, we focus on a point and just wait.
/// </summary>
class MFocusWait : MCameraFocus
{
	public override MCameraSpec UpdateFocusPoint(MUpdateInfo info, MCameraSpec curr)
	{
		return curr;
	}
}

