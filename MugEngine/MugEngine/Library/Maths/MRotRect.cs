namespace MugEngine;

/// <summary>
/// Represents a rectangle that can rotate
/// </summary>
public struct MRotRect : IMBounds
{
	public Vector2 mPos = Vector2.Zero;
	public Vector2 mSize = Vector2.Zero;
	public float mRot = 0.0f;

	public MRotRect(Vector2 pos, Vector2 size, float rot)
	{
		mPos = pos; mSize = size; mRot = rot;
	}

	public MRotRect(Vector2 size)
	{
		mSize = size; 
	}

	public void SetCenter(Vector2 center)
	{
		Vector2 x = GetSideVec();
		Vector2 y = GetForwardVec();

		mPos = center - (x + y) * 0.5f;
	}

	public Vector2 GetNPoint(int n)
	{
		Vector2 x = GetSideVec();
		Vector2 y = GetForwardVec();

		switch (n)
		{
			case 0:
				return mPos;
			case 1:
				return mPos + x;
			case 2:
				return mPos + x + y;
			case 3:
				return mPos + y;
			default:
				break;
		}

		throw new NotImplementedException();
	}

	public IEnumerable<Vector2> GetPoints()
	{
		Vector2 x = GetSideVec();
		Vector2 y = GetForwardVec();

		yield return mPos;
		yield return mPos + x;
		yield return mPos + x + y;
		yield return mPos + y;
	}

	public Rectangle BoundsRect()
	{
		Rectangle r = MugMath.RectFromFloats(mPos.X, mPos.Y, mSize.X, mSize.Y);

		foreach(Vector2 pt in GetPoints())
		{
			r = MugMath.GetBoundingRectangle(r, pt.ToPoint());
		}

		return r;
	}

	public Vector2 GetSideVec()
	{
		Vector2 x = new Vector2(mSize.X, 0.0f);
		x.Rotate(mRot);
		return x;
	}

	public Vector2 GetForwardVec()
	{
		Vector2 y = new Vector2(0.0f, mSize.Y);
		y.Rotate(mRot);
		return y;
	}
}
