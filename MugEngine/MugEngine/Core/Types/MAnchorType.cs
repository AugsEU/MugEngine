namespace MugEngine.Core
{
	public enum MAnchorType
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Centre,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	public static class MAnchorTypeImpl
	{
		/// <summary>
		/// To a position on a rectangle.
		/// </summary>
		public static Vector2 ToPosOnRect(this MAnchorType anchor, MRect2f rect)
		{
			Vector2 size = rect.GetSize();
			switch (anchor)
			{
				case MAnchorType.TopLeft:
					return rect.mMin;
				case MAnchorType.Top:
					return rect.mMin + new Vector2(0.5f * size.X, 0.0f);
				case MAnchorType.TopRight:
					return rect.mMin + new Vector2(1.0f * size.X, 0.0f);

				case MAnchorType.Left:
					return rect.mMin + new Vector2(0.0f         , 0.5f * size.Y);
				case MAnchorType.Centre:
					return rect.mMin + new Vector2(0.5f * size.X, 0.5f * size.Y);
				case MAnchorType.Right:
					return rect.mMin + new Vector2(1.0f * size.X, 0.5f * size.Y);

				case MAnchorType.BottomLeft:
					return rect.mMin + new Vector2(0.0f * size.X, 1.0f * size.Y);
				case MAnchorType.Bottom:
					return rect.mMin + new Vector2(0.5f * size.X, 1.0f * size.Y);
				case MAnchorType.BottomRight:
					return rect.mMin + new Vector2(1.0f * size.X, 1.0f * size.Y);
			}

			throw new NotImplementedException();
		}


		/// <summary>
		/// To a position on a rectangle
		/// </summary>
		public static Vector2 ToPosOnRect(this MAnchorType anchor, Rectangle rect)
		{
			return anchor.ToPosOnRect(rect.ToRect2f());
		}
	}
}
