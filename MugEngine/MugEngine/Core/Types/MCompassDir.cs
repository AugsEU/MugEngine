using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugEngine.Core;

/// <summary>
/// Represents one of the eight ways on a compass
/// </summary>
public enum MCompassDir : byte
{
	N = 0,
	E = 1,
	S = 2,
	W = 3,
	NE = 4,
	SE = 5,
	SW = 6,
	NW = 7
}

public static class MCompassDirImpl
{
	/// <summary>
	/// Convert compass direction enum to unit vector
	/// </summary>
	/// <param name="dir">Compass direction</param>
	/// <returns>Compass direction unit vector</returns>
	/// <exception cref="NotImplementedException">Requires a valid compass direction</exception>
	public static Vector2 ToVec(this MCompassDir dir)
	{
		switch (dir)
		{
			case MCompassDir.N:
				return new Vector2(0.0f, -1.0f);
			case MCompassDir.S:
				return new Vector2(0.0f, 1.0f);
			case MCompassDir.W:
				return new Vector2(-1.0f, 0.0f);
			case MCompassDir.E:
				return new Vector2(1.0f, 0.0f);
			case MCompassDir.NE:
				return new Vector2(MugMath.I_SQRT_2, -MugMath.I_SQRT_2);
			case MCompassDir.SE:
				return new Vector2(MugMath.I_SQRT_2, MugMath.I_SQRT_2);
			case MCompassDir.SW:
				return new Vector2(-MugMath.I_SQRT_2, MugMath.I_SQRT_2);
			case MCompassDir.NW:
				return new Vector2(-MugMath.I_SQRT_2, -MugMath.I_SQRT_2);
		}

		throw new NotImplementedException();
	}

	/// <summary>
	/// Convert Compass direction enum to unit point
	/// </summary>
	/// <param name="dir">Compass direction</param>
	/// <returns>Compass direction point</returns>
	/// <exception cref="NotImplementedException">Requires a valid Compass direction</exception>
	public static Point ToPoint(this MCompassDir dir)
	{
		switch (dir)
		{
			case MCompassDir.N:
				return new Point(0, -1);
			case MCompassDir.S:
				return new Point(0, 1);
			case MCompassDir.W:
				return new Point(-1, 0);
			case MCompassDir.E:
				return new Point(1, 0);
			case MCompassDir.NE:
				return new Point(1, -1);
			case MCompassDir.SE:
				return new Point(1, 1);
			case MCompassDir.SW:
				return new Point(-1, 1);
			case MCompassDir.NW:
				return new Point(-1, -1);
		}

		throw new NotImplementedException();
	}
}

