namespace MugEngine.Scene
{
	public struct MTile : IMCollisionQueryable, IMBounds
	{
		#region rMembers

		public MTileAdjacency mAdjacency = MTileAdjacency.Ad0;
		public MCardDir mRotation;
		public Rectangle mBoundingBox;
		public MAnimation mAnimation;
		public ulong mFlags = 0;
		public int mType;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a tile with default bounds.
		/// </summary>
		public MTile(int type)
		{
		}



		/// <summary>
		/// Inform tile is at this position.
		/// This should not change.
		/// </summary>
		public void PlaceAt(Point pos, Point size)
		{
			mBoundingBox = new Rectangle(pos, size);
		}



		/// <summary>
		/// Clear adjacency info.
		/// </summary>
		public void ClearAdjacent()
		{
			mAdjacency = MTileAdjacency.Ad0;
		}



		/// <summary>
		/// Inform that a neighbour is to the X of this tile.
		/// </summary>
		public void InformAdjacent(MTile neighbour, MTileAdjacency adj)
		{
			//My neighbour is to the right of me
			mAdjacency |= adj;

			//I'm to the left of my neighbour
			neighbour.mAdjacency |= MTileAdjacencyHelper.InvertDir(adj);
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Called once per frame to update the tile.
		/// </summary>
		public void Update(MScene scene, MUpdateInfo info)
		{
			// TO DO: Remove this?
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Get animated texture of tile
		/// </summary>
		public MTexturePart GetTexture()
		{
			if (mAnimation is null)
			{
				return MTexturePart.Empty;
			}
			return mAnimation.GetCurrentTexture();
		}



		/// <summary>
		/// Sprite effects can mirror or flip a tile when drawing.
		/// </summary>
		/// <returns>Sprite effect</returns>
		public SpriteEffects GetEffect()
		{
			// Flipping the tile keeps light direction consistent
			switch (mRotation)
			{
				case MCardDir.Down:
				case MCardDir.Left:
					return SpriteEffects.FlipHorizontally;
				default:
					break;
			}
			return SpriteEffects.None;
		}

		#endregion rDraw





		#region rCollision

		/// <summary>
		/// Query collision
		/// </summary>
		public bool QueryCollides(Rectangle bounds, MCardDir travelDir)
		{
			return mBoundingBox.Intersects(bounds);
		}



		/// <summary>
		/// Get bounding box of this tile
		/// </summary>
		public Rectangle BoundsRect()
		{
			return mBoundingBox;
		}

		#endregion rCollision





		#region rUtil

		/// <summary>
		/// Get rotation of tile. E.g. platforms can be rotated
		/// </summary>
		/// <returns>Rotation in radians</returns>
		public float GetRotation()
		{
			return mRotation.ToAngle();
		}



		/// <summary>
		/// Get type of tile adjacency.
		/// </summary>
		/// <returns>Tile adjacency</returns>
		public MTileAdjacency GetAdjacency()
		{
			return mAdjacency;
		}

		#endregion rUtil
	}
}
