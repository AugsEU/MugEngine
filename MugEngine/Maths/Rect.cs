namespace MugEngine.Maths
{
	/// <summary>
	/// Rectangle in world space used for colliders
	/// </summary>
	struct Rect2f
	{
		public Rect2f(Vector2 vec1, Vector2 vec2)
		{
			mMin = new Vector2(MathF.Min(vec1.X, vec2.X), MathF.Min(vec1.Y, vec2.Y));
			mMax = new Vector2(MathF.Max(vec1.X, vec2.X), MathF.Max(vec1.Y, vec2.Y));
		}

		public Rect2f(Rectangle rect)
		{
			mMin = new Vector2(rect.X, rect.Y);
			mMax = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
		}

		public Rect2f(Vector2 _min, Texture2D texture)
		{
			mMin = _min;
			mMax = new Vector2(_min.X + texture.Width, _min.Y + texture.Height);
		}

		public Rect2f(Vector2 _min, float width, float height)
		{
			mMin = _min;
			mMax = new Vector2(_min.X + width, _min.Y + height);
		}

		public float GetWidth()
		{
			return Math.Abs(mMax.X - mMin.X);
		}

		public float GetHeight()
		{
			return Math.Abs(mMax.Y - mMin.Y);
		}

		public Vector2 GetCentre()
		{
			return (mMin + mMax) / 2.0f;
		}

		public static Rect2f operator +(Rect2f a, Rect2f b)
		{
			float minX = Math.Min(a.mMin.X, b.mMin.X);
			float minY = Math.Min(a.mMin.Y, b.mMin.Y);
			float maxX = Math.Max(a.mMax.X, b.mMax.X);
			float maxY = Math.Max(a.mMax.Y, b.mMax.Y);

			return new Rect2f(new Vector2(minX, minY), new Vector2(maxX, maxY));
		}

		public static Rect2f operator +(Rect2f rect, Vector2 vec)
		{
			rect.mMin += vec;
			rect.mMax += vec;

			return rect;
		}

		public Rectangle ToRectangle()
		{
			Point rMin = new Point((int)MathF.Round(mMin.X), (int)MathF.Round(mMin.Y));
			Point rMax = new Point((int)MathF.Round(mMax.X), (int)MathF.Round(mMax.Y));

			return new Rectangle(rMin, rMax - rMin);
		}

		public Vector2 mMin;
		public Vector2 mMax;
	}
}
