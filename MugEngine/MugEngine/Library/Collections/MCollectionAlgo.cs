namespace MugEngine.Library;

static public class MCollectionAlgo
{
	/// <summary>
	/// Create iterator over indices of list
	/// </summary>
	//public static T GetMin<T>(this IList<T> list)
	//{
	//	return Enumerable.Range(0, list.Count);
	//}

	/// <summary>
	/// Mirror array horizontally.
	/// </summary>
	public static void MirrorHoriz<T>(T[,] array)
	{
		int width = array.GetLength(0);
		int height = array.GetLength(1);

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < (width / 2); ++x)
			{
				T temp = array[x, y];
				array[x, y] = array[width - x - 1, y];
				array[width - x - 1, y] = temp;
			}
		}
	}
}
