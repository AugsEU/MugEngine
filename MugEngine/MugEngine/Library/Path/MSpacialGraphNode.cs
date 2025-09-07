
using System.Diagnostics.CodeAnalysis;

namespace MugEngine.Library;

/// <summary>
/// Graph node for navigating spacial environments
/// </summary>
public struct MSpacialGraphNode : IEqualityComparer<MSpacialGraphNode>
{
	#region rMembers

	public Rectangle mBounds;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create new node at position
	/// </summary>
	public MSpacialGraphNode(Rectangle bounds)
	{
		mBounds = bounds;
	}

	#endregion rInit





	#region rEquality

	/// <summary>
	/// Are these two x and y equal?
	/// </summary>
	public bool Equals(MSpacialGraphNode x, MSpacialGraphNode y)
	{
		return x.mBounds == y.mBounds;
	}

	/// <summary>
	/// Get the hash code of an object
	/// </summary>
	public int GetHashCode([DisallowNull] MSpacialGraphNode obj)
	{
		return obj.mBounds.GetHashCode();
	}

	#endregion rEquality





	#region rPathing

	/// <summary>
	/// Path heuristic to end goal
	/// </summary>
	public float PathHeuristic(Point end)
	{
		return MugMath.Dist(mBounds.Center, end);
	}



	/// <summary>
	/// Get distance to another node.
	/// </summary>
	public float PathNeighbourDistance(MSpacialGraphNode other, bool diagnonals = false)
	{
		if(!diagnonals)
		{
			return MugMath.ManhattanDist(other.mBounds.Center, mBounds.Center);
		}

		return MugMath.Dist(other.mBounds.Center, mBounds.Center);
	}



	/// <summary>
	/// Get neighbours of this node
	/// </summary>
	public IEnumerable<MSpacialGraphNode> PathNeighbours(IMCollisionQueryable landscape, Point stepSize, bool diagonals = false)
	{
		MugDebug.AddDebugPoint(mBounds.Center.ToVector2(), Color.Yellow);
		int xStep = stepSize.X;
		int yStep = stepSize.Y;
		Rectangle copyBounds = mBounds;

		// Right
		copyBounds.X += xStep;
		if (!landscape.QueryCollides(copyBounds, MCardDir.Right, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Right Down
		copyBounds.Y += yStep;
		if (diagonals && !landscape.QueryCollides(copyBounds, MCardDir.Right, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Down
		copyBounds.X -= xStep;
		if (!landscape.QueryCollides(copyBounds, MCardDir.Down, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Down Left
		copyBounds.X -= xStep;
		if (diagonals && !landscape.QueryCollides(copyBounds, MCardDir.Down, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Left
		copyBounds.Y -= yStep;
		if (!landscape.QueryCollides(copyBounds, MCardDir.Left, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Left Up
		copyBounds.Y -= yStep;
		if (diagonals && !landscape.QueryCollides(copyBounds, MCardDir.Left, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Up
		copyBounds.X += xStep;
		if (!landscape.QueryCollides(copyBounds, MCardDir.Up, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}

		// Up Right
		copyBounds.X += xStep;
		if (diagonals && !landscape.QueryCollides(copyBounds, MCardDir.Up, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds);
		}
	}



	/// <summary>
	/// Can we just walk to the end?
	/// </summary>
	public bool CanReachDirect(Point endPt, IMCollisionQueryable landscape)
	{
		Rectangle walkDirectRect = MugMath.GetBoundingRectangle(mBounds, endPt);

		MCardDir walkDir = MCardDir.Up;

		// This doesn't make any sense but approximates the truth enough that I don't care.
		if (mBounds.Location == walkDirectRect.Location)
		{
			walkDir = MCardDir.Right;
		}
		else if (mBounds.X < walkDirectRect.X)
		{
			walkDir = MCardDir.Left;
		}
		else if (mBounds.Y < walkDirectRect.Y)
		{
			walkDir = MCardDir.Up;
		}
		else
		{
			walkDir = MCardDir.Down;
		}

		// Check if this is all clear.
		bool allClear = !landscape.QueryCollides(walkDirectRect, walkDir, MCollisionFlags.None);

		return allClear;
	}

	#endregion rPathing
}
