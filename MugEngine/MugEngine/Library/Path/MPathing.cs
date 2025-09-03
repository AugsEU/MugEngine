namespace MugEngine.Library;

public static class MPathing<T> where T : IMGraphNode<T>
{
	#region rMembers

	private static PriorityQueue<T, float> sOpenSet = new();

	#endregion rMembers



	#region rAlgorithms

	/// <summary>
	/// Find path using A-Star algorithm. Efficient for single path but
	/// does not calculate all distances.
	/// </summary>
	public static MPathResults<T>? AStar(T start, T end)
	{
		bool found = false;
		MPathResults<T> result = new();
		sOpenSet.Clear();

		result.mDistances[start] = 0.0f;
		sOpenSet.Enqueue(start, start.PathHeuristic(end));

		while (sOpenSet.Count > 0)
		{
			T curr = sOpenSet.Dequeue();

			if (curr.IsSameNodeAs(end))
			{
				// Found path.
				found = true;
				break;
			}

			foreach (T neigh in curr.PathNeighbours())
			{
				float newScore = result.DistanceOf(curr) + curr.PathNeighbourDistance(neigh);

				if (newScore < result.DistanceOf(neigh))
				{
					result.mCameFrom[neigh] = curr;
					result.mDistances[neigh] = newScore;

					sOpenSet.Enqueue(neigh, newScore + neigh.PathHeuristic(end));
				}
			}
		}

		// Clear to avoid memory leaks.
		sOpenSet.Clear();
		return found ? result : null;
	}



	/// <summary>
	/// Calculate all distances from a start node.
	/// </summary>
	public static MPathResults<T> FindDistances(T start)
	{
		MPathResults<T> result = new();
		sOpenSet.Clear();

		sOpenSet.Enqueue(start, 0.0f);
		result.mDistances[start] = 0.0f;

		while (sOpenSet.Count > 0)
		{
			T curr = sOpenSet.Dequeue();

			foreach (T neigh in curr.PathNeighbours())
			{
				float newScore = result.DistanceOf(curr) + curr.PathNeighbourDistance(neigh);

				if (newScore < result.DistanceOf(neigh))
				{
					result.mCameFrom[neigh] = curr;
					result.mDistances[neigh] = newScore;

					sOpenSet.Enqueue(neigh, newScore);
				}
			}
		}

		sOpenSet.Clear();
		return result;
	}


	/// <summary>
	/// Calculate all distances from many start nodes.
	/// </summary>
	public static MPathResults<T> FindDistances(IEnumerable<T> starts)
	{
		MPathResults<T> result = new();
		sOpenSet.Clear();

		foreach(T start in starts)
		{
			sOpenSet.Enqueue(start, 0.0f);
			result.mDistances[start] = 0.0f;
		}

		while (sOpenSet.Count > 0)
		{
			T curr = sOpenSet.Dequeue();

			foreach (T neigh in curr.PathNeighbours())
			{
				float newScore = result.DistanceOf(curr) + curr.PathNeighbourDistance(neigh);

				if (newScore < result.DistanceOf(neigh))
				{
					result.mCameFrom[neigh] = curr;
					result.mDistances[neigh] = newScore;

					sOpenSet.Enqueue(neigh, newScore);
				}
			}
		}

		sOpenSet.Clear();
		return result;
	}

	#endregion rAlgorithms
}
