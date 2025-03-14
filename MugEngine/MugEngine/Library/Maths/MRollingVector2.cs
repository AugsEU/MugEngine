using Nito.Collections;

namespace MugEngine.Library;

class MRollingVector2 : MRollingDeque<Vector2>
{
	public MRollingVector2(int maxSize) : base(maxSize)
	{
	}
	
	public Vector2 GetAverage()
	{
		float num = 0.0f;
		Vector2 result = Vector2.Zero;
		foreach(Vector2 v in this)
		{
			result += v;
			num += 1.0f;
		}

		if (num > 0.0f)
		{
			result /= num;
		}

		return result;
	}
}

