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
	NE = 1,
	E = 2,
	SE = 3,
	S = 4,
	SW = 5,
	W = 6,
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



	/// <summary>
	/// Gets angle from compass direction
	/// </summary>
	public static float ToAngle(this MCompassDir dir)
	{
		switch (dir)
		{
			case MCompassDir.N:
				return MathHelper.Pi * 0.0f;
			case MCompassDir.NE:
				return MathHelper.Pi * 0.25f;
			case MCompassDir.E:
				return MathHelper.Pi * 0.5f;
			case MCompassDir.SE:
				return MathHelper.Pi * 0.75f;
			case MCompassDir.S:
				return MathHelper.Pi * 1.0f;
			case MCompassDir.SW:
				return MathHelper.Pi * 1.25f;
			case MCompassDir.W:
				return MathHelper.Pi * 1.5f;
			case MCompassDir.NW:
				return MathHelper.Pi * 1.75f;
		}

		throw new NotImplementedException();
	}


	/// <summary>
	/// Reflect compass along axis
	/// </summary>
	public static MCompassDir ReflectAlong(this MCompassDir dir, MCompassDir axis)
	{
		int dirValue = (int)dir;
		int axisValue = (int)axis;

		int reflectedValue = (2 * axisValue - dirValue) % 8;

		if (reflectedValue < 0)
		{
			reflectedValue += 8;
		}

		return (MCompassDir)reflectedValue;
	}



	/// <summary>
	/// Is facing cardinal direction(N, E, S, W)
	/// </summary>
	public static bool IsCardinal(this MCompassDir dir)
	{
		switch (dir)
		{
			case MCompassDir.N:
			case MCompassDir.E:
			case MCompassDir.S:
			case MCompassDir.W:
				return true;
			case MCompassDir.NE:
			case MCompassDir.SE:
			case MCompassDir.SW:
			case MCompassDir.NW:
				return false;
		}

		throw new NotImplementedException();
	}



	/// <summary>
	/// Is facing cardinal direction(N, E, S, W)
	/// </summary>
	public static bool IsDiagonal(this MCompassDir dir)
	{
		switch (dir)
		{
			case MCompassDir.N:
			case MCompassDir.E:
			case MCompassDir.S:
			case MCompassDir.W:
				return false;
			case MCompassDir.NE:
			case MCompassDir.SE:
			case MCompassDir.SW:
			case MCompassDir.NW:
				return true;
		}

		throw new NotImplementedException();
	}
}

