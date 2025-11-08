namespace MugEngine.Graphics;


/// <summary>
/// Utility methods for drawing fade effects
/// </summary>
public abstract class MFade
{
	/// <summary>
	/// Draw the fade at a specific percentage(0.0f to 1.0f) to an area
	/// </summary>
	public abstract void DrawAtTime(MDrawInfo info, Rectangle area, float time, int layer);
}
