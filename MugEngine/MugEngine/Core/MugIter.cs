using MugEngine.Library;

namespace MugEngine.Core;

/// <summary>
/// Iterator methods
/// </summary>
public static class MugIter
{
	/// <summary>
	/// Create an iterator that is shuffled
	/// </summary>
	public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> source, MRandom rng = null)
	{
		if (source == null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		rng ??= new MRandom(); // Use the provided Random instance or create a new one.

		// Shuffle using LINQ with a random order
		return source.OrderBy(_ => rng.Next());
	}



	/// <summary>
	/// Create iterator over indices of list
	/// </summary>
	public static IEnumerable<int> IdxIter<T>(this IList<T> list)
	{
		return Enumerable.Range(0, list.Count);
	}


	/// <summary>
	/// Get first item in iter
	/// </summary>
	public static T? FirstOptional<T>(this IEnumerable<T> iter) where T : struct
	{
		foreach (var item in iter)
		{
			return item;
		}

		return null;
	}
}

