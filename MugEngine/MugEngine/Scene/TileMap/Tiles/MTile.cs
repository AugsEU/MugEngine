﻿using System.Collections.Generic;

namespace MugEngine.Scene
{
	public struct MTile : IMCollisionQueryable
	{
		#region rConstants

		/// <summary>
		/// These define how mFlags is used.
		/// </summary>
		const ulong ROTATION_MASK        = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000011u;
		const ulong ANIM_OFFSET_MASK     = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100u;

		#endregion rConstants





		#region rMembers

		public MTileAdjacency mAdjacency = MTileAdjacency.Ad0;
		public ushort mType;

		public Rectangle mBoundingBox;
		public ulong mFlags = 0;
		public MAnimation mAnimation;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a tile with default bounds.
		/// </summary>
		public MTile(int type)
		{
			mType = (ushort)type;

			mFlags |= ANIM_OFFSET_MASK; // I think we always want this but maybe one day we want to toggle it.
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
		/// Inform that a neighbour is to the X of this tile.
		/// </summary>
		public void InformAdjacent(ref MTile neighbour, MTileAdjacency adj)
		{
			//My neighbour is to the right of me
			mAdjacency |= adj;

			//I'm to the left of my neighbour
			neighbour.mAdjacency |= MTileAdjacencyHelper.InvertDir(adj);
		}

		#endregion rInit





		#region rDraw

		/// <summary>
		/// Sprite effects can mirror or flip a tile when drawing.
		/// </summary>
		/// <returns>Sprite effect</returns>
		public SpriteEffects GetEffect()
		{
			// Flipping the tile keeps light direction consistent
			switch (GetRotDir())
			{
				case MCardDir.Down:
				case MCardDir.Left:
					return SpriteEffects.FlipHorizontally;
				default:
					break;
			}
			return SpriteEffects.None;
		}


		/// <summary>
		/// Get animation offset
		/// </summary>
		public float GetAnimOffset()
		{
			bool useAnimOffset = (mFlags & ANIM_OFFSET_MASK) != 0;

			if (!useAnimOffset)
			{
				return 0.0f;
			}

			int bigNum = mBoundingBox.Location.X + 240257 * mBoundingBox.Location.Y;

			return MRandom.NextRng(bigNum) / 2147483647.0f;
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

		#endregion rCollision





		#region rUtil

		/// <summary>
		/// Get rotation of tile. E.g. platforms can be rotated
		/// </summary>
		/// <returns>Rotation in radians</returns>
		public float GetRotation()
		{
			return GetRotDir().ToAngle();
		}




		/// <summary>
		/// Get cardinal direction
		/// </summary>
		public MCardDir GetRotDir()
		{
			return (MCardDir)(mFlags & ROTATION_MASK);
		}



		/// <summary>
		/// Set cardinal rotation
		/// </summary>
		public void SetRotDir(MCardDir dir)
		{
			mFlags &= ~ROTATION_MASK;
			switch (dir)
			{
				case MCardDir.Up:
					mFlags |= (0b00000000);
					break;
				case MCardDir.Right:
					mFlags |= (0b00000001);
					break;
				case MCardDir.Down:
					mFlags |= (0b00000010);
					break;
				case MCardDir.Left:
					mFlags |= (0b00000011);
					break;
			}
		}

		#endregion rUtil
	}
}
