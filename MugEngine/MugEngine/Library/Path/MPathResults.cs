namespace MugEngine.Library;

/// <summary>
/// Describes the result of a path find.
/// </summary>
public struct MPathResults<T> where T : IMGraphNode<T>
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


	public T GetClosestCameFromNode(T node)
	{
		T closest = node;
		float dist = 0.0f;
		foreach(T child in mCameFrom.Keys)
		{
			float newDist = node.PathNeighbourDistance(child);
			if (dist == 0.0f || newDist < dist)
			{
				closest = child;
				dist = newDist;
			}
		}

		return closest;
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
