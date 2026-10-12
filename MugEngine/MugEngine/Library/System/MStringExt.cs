using System.Globalization;

namespace MugEngine.Library;

public static class MStringExt
{
	public static string MToLower(this string str)
	{
		return str.ToLower(CultureInfo.InvariantCulture);
	}
}
