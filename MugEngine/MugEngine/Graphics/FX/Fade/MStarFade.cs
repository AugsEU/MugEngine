namespace MugEngine.Graphics;

public class MStarFade : MFade
{
	float mSpacing;

	public MStarFade(float spacing = 10.0f)
	{
		mSpacing = spacing;
	}

	public override void DrawAtTime(MDrawInfo info, Rectangle area, float time, int layer)
	{
		float xStart = area.X - mSpacing;
		float xEnd = area.X + area.Width + mSpacing;
		float yStart = area.Y - mSpacing;
		float yEnd = area.Y + area.Height + mSpacing;

		for (float x = xStart; x < xEnd; x += mSpacing)
		{
			for (float y = yStart; y < yEnd; y += mSpacing)
			{
				Vector2 min = new Vector2(x, y - time * mSpacing);
				Vector2 max = min + new Vector2(mSpacing, mSpacing) * time;

				info.mCanvas.DrawRect(new MRect2f(min, max), Color.Black, rot: MathF.PI / 4, layer: layer);
			}
		}
	}
}
