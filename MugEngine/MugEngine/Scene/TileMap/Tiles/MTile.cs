using MugEngine.Core;
using MugEngine.Graphics;

namespace MugEngine.Scene
{
	public abstract class MTile : IMSceneUpdate
	{
		#region rMembers

		protected bool mEnabled = true;
		protected MTileAdjacency mAdjacency = MTileAdjacency.Ad0;
		protected MCardDir mRotation;

		private Rectangle mLocalBounds;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a tile with default bounds.
		/// </summary>
		public MTile(Point tileSize)
		{
			mLocalBounds = new Rectangle(0, 0, tileSize.X, tileSize.Y);
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



		/// <summary>
		/// Is this tile the same type as us?
		/// </summary>
		public virtual bool IsSameType(MTile neighbour)
		{
			return neighbour.GetType() == GetType();
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Called once per frame to update the tile.
		/// </summary>
		public virtual void Update(MScene scene, MUpdateInfo info)
		{
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Get texture we want to draw.
		/// </summary>
		public virtual MTexturePart GetTexture()
		{
			return MTexturePart.Empty;
		}



		/// <summary>
		/// Sprite effects can mirror or flip a tile when drawing.
		/// </summary>
		/// <returns>Sprite effect</returns>
		public virtual SpriteEffects GetEffect()
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





		#region rUtil

		/// <summary>
		/// Get bounds in coordinates relative to the tile.
		/// </summary>
		public Rectangle GetLocalBounds()
		{
			return mLocalBounds;
		}

		public virtual bool RegisterCollider()
		{
			return false;
		}

		#endregion rUtil





		#region rUtil

		/// <summary>
		/// Is the tile enabled?
		/// </summary>
		public bool IsEnabled()
		{
			return mEnabled;
		}



		/// <summary>
		/// Get rotation of tile. E.g. platforms can be rotated
		/// </summary>
		/// <returns>Rotation in radians</returns>
		public virtual float GetRotation()
		{
			return MugEnum.GetRotation(mRotation);
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
