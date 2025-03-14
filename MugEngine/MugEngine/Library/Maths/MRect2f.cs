using System.Runtime.CompilerServices;

namespace MugEngine.Library;

/// <summary>
/// Rectangle in world space used for colliders
/// </summary>
public struct MRect2f
{
	public MRect2f(Vector2 min, Vector2 max)
	{
		mMin = new Vector2(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y));
		mMax = new Vector2(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y));
	}

	public MRect2f(Rectangle rect)
	{
		mMin = new Vector2(rect.X, rect.Y);
		mMax = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
	}

	public MRect2f(Vector2 min, Texture2D texture)
	{
		mMin = min;
		mMax = new Vector2(min.X + texture.Width, min.Y + texture.Height);
	}

	public MRect2f(Vector2 min, float width, float height)
	{
		mMin = min;
		mMax = new Vector2(min.X + width, min.Y + height);
	}

	public MRect2f(float x, float y, float width, float height)
	{
		mMin = new Vector2(x, y);
		mMax = new Vector2(x + width, y + height);
	}

	public float GetWidth()
	{
		return mMax.X - mMin.X;
	}

	public float GetHeight()
	{
		return mMax.Y - mMin.Y;
	}

	public Vector2 GetSize()
	{
		return mMax - mMin;
	}

	public Vector2 GetCentre()
	{
		return (mMin + mMax) / 2.0f;
	}

	public static MRect2f operator +(MRect2f a, MRect2f b)
	{
		float minX = Math.Min(a.mMin.X, b.mMin.X);
		float minY = Math.Min(a.mMin.Y, b.mMin.Y);
		float maxX = Math.Max(a.mMax.X, b.mMax.X);
		float maxY = Math.Max(a.mMax.Y, b.mMax.Y);

		return new MRect2f(new Vector2(minX, minY), new Vector2(maxX, maxY));
	}

	public static MRect2f operator +(MRect2f rect, Vector2 vec)
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

static class MRect2fImpl
{
	public static MRect2f ToRect2f(this Rectangle rect)
	{
		return new MRect2f(rect.Location.ToVector2(), (rect.Location + rect.Size).ToVector2());
	}
}

