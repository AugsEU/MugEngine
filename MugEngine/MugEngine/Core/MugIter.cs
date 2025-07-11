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



	/// <summary>
	/// Pick a random element from a subset of a list.
	/// Default returned if none found.
	/// </summary>
	public static T RandomElementWhere<T>(this IList<T> list, Func<T, bool> predicate, MRandom rng = null)
	{
		T result = default;
		int count = 0;

		for (int i = 0; i < list.Count; i++)
		{
			T item = list[i];
			if (predicate(item))
			{
				// With probability 1/count, replace the current result
				if (count == 0 || rng.GetIntRange(0, count-1) == 0)
				{
					result = item;
				}
				count++;
			}
		}

		return result;
	}
}

