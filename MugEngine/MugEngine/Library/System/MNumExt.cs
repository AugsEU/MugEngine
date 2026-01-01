using System.Globalization;

namespace MugEngine.Library;
/// <summary>
/// Various extensions for number types
/// </summary>

public static class MByte
{
	public static byte Parse(string str)
	{
		return byte.Parse(str, CultureInfo.InvariantCulture);
	}

	public static byte ParseHex(string str)
	{
		return byte.Parse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
	}
}

public static class MSByte
{
	public static sbyte Parse(string str)
	{
		return sbyte.Parse(str, CultureInfo.InvariantCulture);
	}
}

public static class MInt
{
	public static int Parse(string str)
	{
		return int.Parse(str, CultureInfo.InvariantCulture);
	}
}

public static class MUInt
{
	public static uint Parse(string str)
	{
		return uint.Parse(str, CultureInfo.InvariantCulture);
	}
}

public static class MLong
{
	public static long Parse(string str)
	{
		return long.Parse(str, CultureInfo.InvariantCulture);
	}
}

public static class MULong
{
	public static ulong Parse(string str)
	{
		return ulong.Parse(str, CultureInfo.InvariantCulture);
	}
}

public static class MFloat
{
	public static float Parse(string str)
	{
		return float.Parse(str, CultureInfo.InvariantCulture);
	}
}

public static class MDouble
{
	public static double Parse(string str)
	{
		return double.Parse(str, CultureInfo.InvariantCulture);
	}
}

