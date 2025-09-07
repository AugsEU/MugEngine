namespace MugEngine.Library;

public static class MSpacialPathing
{
	#region rMembers

	private static PriorityQueue<MSpacialGraphNode, float> sOpenSet = new();

	#endregion rMembers


	#region rPath

	/// <summary>
	/// Find path in spacial area.
	/// </summary>
	public static MSpacialPathResults? SpacialPath(Rectangle character, Vector2 end, Point stepSize, IMCollisionQueryable landscape, bool diagonals = false)
	{
		MSpacialGraphNode start = new MSpacialGraphNode(character);
		Point endPoint = end.ToPoint();

		bool found = false;
		MSpacialPathResults result = new();
		sOpenSet.Clear();

		result.mDistances[start] = 0.0f;
		sOpenSet.Enqueue(start, start.PathHeuristic(endPoint));

		while (sOpenSet.Count > 0)
		{
			MSpacialGraphNode curr = sOpenSet.Dequeue();

			if (curr.CanReachDirect(endPoint, landscape))
			{
				found = true;
				result.mEndNode = curr;
				break;
			}

			foreach (MSpacialGraphNode neigh in curr.PathNeighbours(landscape, stepSize, diagonals))
			{
				float newScore = result.DistanceOf(curr) + curr.PathNeighbourDistance(neigh, diagonals);

				if (newScore < result.DistanceOf(neigh))
				{
					result.mCameFrom[neigh] = curr;
					result.mDistances[neigh] = newScore;

					sOpenSet.Enqueue(neigh, newScore + neigh.PathHeuristic(endPoint));
				}
			}
		}

		return found ? result : null;
	}

	#endregion rPath
}
