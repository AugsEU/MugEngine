
using System.Diagnostics.CodeAnalysis;

namespace MugEngine;

/// <summary>
/// Graph node for navigating spacial environments
/// </summary>
public struct MSpacialGraphNode : IMGraphNode<MSpacialGraphNode>, IEqualityComparer<MSpacialGraphNode>
{
	public Rectangle mBounds;
	public IMCollisionQueryable mLandScape;

	public MSpacialGraphNode(Rectangle bounds, IMCollisionQueryable landScape)
	{
		mBounds = bounds;
		mLandScape = landScape;
	}

	public bool Equals(MSpacialGraphNode x, MSpacialGraphNode y)
	{
		return x.mBounds == y.mBounds;
	}

	public int GetHashCode([DisallowNull] MSpacialGraphNode obj)
	{
		return obj.mBounds.GetHashCode();
	}

	public bool IsSameNodeAs(MSpacialGraphNode other)
	{
		float minDist = MathF.Min(mBounds.Width, mBounds.Height);
		Point otherCen = other.mBounds.Center;
		Point ourCen = mBounds.Center;

		return MugMath.CmpDist(ourCen, otherCen, minDist) <= 0;
	}

	public float PathHeuristic(MSpacialGraphNode other)
	{
		return PathNeighbourDistance(other);
	}

	public float PathNeighbourDistance(MSpacialGraphNode other)
	{
		return MathF.Sqrt(MugMath.DistSq(other.mBounds.Center, mBounds.Center));
	}

	public IEnumerable<MSpacialGraphNode> PathNeighbours()
	{
		int xStep = mBounds.Width / 2;
		int yStep = mBounds.Height / 2;
		Rectangle copyBounds = mBounds;

		// Right
		copyBounds.X += xStep;
		if(!mLandScape.QueryCollides(copyBounds, MCardDir.Right, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Right Down
		copyBounds.Y += yStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Right, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Down
		copyBounds.X -= xStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Down, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Down Left
		copyBounds.X -= xStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Down, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Left
		copyBounds.Y -= yStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Left, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Left Up
		copyBounds.Y -= yStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Left, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Up
		copyBounds.X += xStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Up, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}

		// Up Right
		copyBounds.X += xStep;
		if (!mLandScape.QueryCollides(copyBounds, MCardDir.Up, MCollisionFlags.None))
		{
			yield return new MSpacialGraphNode(copyBounds, mLandScape);
		}
	}
}
