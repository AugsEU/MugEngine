namespace MugEngine.Library;

public struct MSpacialPathResults
{
	public Dictionary<MSpacialGraphNode, MSpacialGraphNode> mCameFrom;
	public Dictionary<MSpacialGraphNode, float> mDistances;
	public MSpacialGraphNode mEndNode;


	/// <summary>
	/// Default path results.
	/// </summary>
	public MSpacialPathResults()
	{
		mCameFrom = new();
		mDistances = new();
	}



	/// <summary>
	/// Get the shortest distance of a node from the start nodes.
	/// </summary>
	public float DistanceOf(MSpacialGraphNode node)
	{
		if (mDistances.TryGetValue(node, out float currDist))
		{
			return currDist;
		}

		return float.MaxValue;
	}



	/// <summary>
	/// Get closest spacial node to a given position.
	/// </summary>
	public MSpacialGraphNode GetClosestCameFromNode(Vector2 pos)
	{
		return GetClosestCameFromNode(pos.ToPoint());
	}



	/// <summary>
	/// Get closest spacial node to a given position
	/// </summary>
	public MSpacialGraphNode GetClosestCameFromNode(Point pos)
	{
		MSpacialGraphNode closest = default;
		float dist = 0.0f;
		foreach (MSpacialGraphNode child in mCameFrom.Keys)
		{
			float newDist = MugMath.DistSq(child.mBounds.Center, pos);
			if (dist == 0.0f || newDist < dist)
			{
				closest = child;
				dist = newDist;
			}
		}

		return closest;
	}



	/// <summary>
	/// Get the shortest path from a node to the start node.
	/// </summary>
	public IEnumerable<MSpacialGraphNode> EnumeratePath(MSpacialGraphNode end)
	{
		yield return end;

		MSpacialGraphNode curr = end;
		while (mCameFrom.TryGetValue(curr, out MSpacialGraphNode next))
		{
			curr = next;
			yield return curr;
		}
	}



	/// <summary>
	/// Get the shortest path from the end node to the start node.
	/// </summary>
	public IEnumerable<MSpacialGraphNode> EnumeratePath()
	{
		yield return mEndNode;

		MSpacialGraphNode curr = mEndNode;
		while (mCameFrom.TryGetValue(curr, out MSpacialGraphNode next))
		{
			curr = next;
			yield return curr;
		}
	}
}
