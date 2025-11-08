namespace MugEngine.Graphics;

class MWipeFade : MFade
{
	MCardDir mDirection;

	public MWipeFade(MCardDir direction)
	{
		mDirection = direction;
	}

	public override void DrawAtTime(MDrawInfo info, Rectangle area, float time, int layer)
	{
		Rectangle rectToDraw = area;

		switch (mDirection)
		{
			case MCardDir.Down:
				rectToDraw.Height = (int)(rectToDraw.Height * time);
				break;
			case MCardDir.Up:
				rectToDraw.Height = (int)(rectToDraw.Height * time);
				rectToDraw.Y = area.Y + area.Height - rectToDraw.Height;
				break;
			case MCardDir.Right:
				rectToDraw.Width = (int)(rectToDraw.Width * time);
				break;
			case MCardDir.Left:
				rectToDraw.Width = (int)(rectToDraw.Width * time); 
				rectToDraw.X = area.X + area.Width - rectToDraw.Width;
				break;
		}

		info.mCanvas.DrawRect(rectToDraw, Color.Black, layer: layer);
	}
}
