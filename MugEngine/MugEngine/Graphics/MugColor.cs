using System.Globalization;

namespace MugEngine.Graphics;

/// <summary>
/// Utility functions for color
/// </summary>
public static class MugColor
{
	/// <summary>
	/// Get uint hex code from color.
	/// </summary>
	public static uint ColorToHEX(Color color)
	{
		return color.B + ((uint)(color.G) << 8) + +((uint)(color.R) << 16);
	}



	/// <summary>
	/// Get color from string hex code.
	/// </summary>
	public static Color HEXToColor(string hex)
	{
		byte r, g, b, a;

		r = MByte.ParseHex(hex.Substring(0, 2));
		g = MByte.ParseHex(hex.Substring(2, 2));
		b = MByte.ParseHex(hex.Substring(4, 2));

		if (hex.Length == 6)
		{
			a = 255;
		}
		else if (hex.Length == 8)
		{
			a = MByte.ParseHex(hex.Substring(6, 2));
		}
		else
		{
			throw new NotImplementedException();
		}

		return new Color(r, g, b, a);
	}



	/// <summary>
	/// Brighten colour linearly
	/// </summary>
	/// <param name="col">Initial colour, output value</param>
	/// <param name="bright">Brightness factor from 0 to 1</param>
	public static void BrightenColour(ref Color col, float bright)
	{
		col.R = (byte)((col.R * (1.0f - bright)) + (255.0f * bright));
		col.G = (byte)((col.G * (1.0f - bright)) + (255.0f * bright));
		col.B = (byte)((col.B * (1.0f - bright)) + (255.0f * bright));
	}
}

