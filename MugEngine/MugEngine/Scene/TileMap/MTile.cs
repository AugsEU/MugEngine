using System.Collections.Generic;

namespace MugEngine.Scene;

public struct MTile
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
	public byte mAnimIdx = 0;
	public ushort mType = 0;
	public ulong mFlags = 0;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create a tile with default bounds.
	/// </summary>
	public MTile(ushort type, MCardDir rot)
	{
		mType = type;

		mFlags |= ANIM_OFFSET_MASK; // I think we always want this but maybe one day we want to toggle it.
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
	/// Does this use animation offset?
	/// </summary>
	public bool UsesAnimOffset()
	{
		return (mFlags & ANIM_OFFSET_MASK) != 0;
	}

	#endregion rDraw





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

