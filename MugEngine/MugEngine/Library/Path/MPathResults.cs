namespace MugEngine.Library;

/// <summary>
/// Describes the result of a path find.
/// </summary>
public struct MPathResults<T> where T : class, IMGraphNode<T>
{
	public Dictionary<T, T> mCameFrom;
	public Dictionary<T, float> mDistances;


	/// <summary>
	/// Default path results.
	/// </summary>
	public MPathResults()
	{
		mCameFrom = new();
		mDistances = new();
	}



	/// <summary>
	/// Get the shortest distance of a node from the start nodes.
	/// </summary>
	public float DistanceOf(T node)
	{
		if (mDistances.TryGetValue(node, out float currDist))
		{
			return currDist;
		}

		return float.MaxValue;
	}



	/// <summary>
	/// Get the shortest path from a node to the start nodes.
	/// </summary>
	public IEnumerable<T> EnumeratePath(T end)
	{
		yield return end;

		T curr = end;
		while (mCameFrom.TryGetValue(curr, out T next))
		{
			curr = next;
			yield return curr;
		}
	}
}
