namespace MugEngine.Input;

/// <summary>
/// Mouse button
/// </summary>
public enum MMouseButtonType
{
	Left,
	Middle,
	Right
}

/// <summary>
/// A mouse button
/// </summary>
public class MMouseButton : MButton
{
	MMouseButtonType mType;

	public MMouseButton(MMouseButtonType type)
	{
		mType = type;
	}

	public override bool IsPressed(ref MInputSnapshot inputSnapshot)
	{
		switch (mType)
		{
			case MMouseButtonType.Left:
				return inputSnapshot.mMouseState.LeftButton == ButtonState.Pressed;
			case MMouseButtonType.Middle:
				return inputSnapshot.mMouseState.MiddleButton == ButtonState.Pressed;
			case MMouseButtonType.Right:
				return inputSnapshot.mMouseState.RightButton == ButtonState.Pressed;
			default:
				break;
		}

		throw new NotImplementedException();
	}
}
