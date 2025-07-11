using LDtk;
using TracyWrapper;

namespace MugEngine.Scene;

/// <summary>
/// Represents a map of square tiles.
/// </summary>
public class MTileMap<P> : IMCollisionQueryable, IMSceneUpdate, IMSceneDraw 
	where P : struct, IMTilePolicy
{
	#region rConstants

	const byte INVALID_ANIM = byte.MaxValue;

	#endregion rConstants





	#region rMembers

	Vector2 mBasePosition;
	Point mTileSize;
	MTile[,] mTileMap;
	List<MAnimation> mTileAnimations;
	P mPolicy = default;
	int mDrawLayer;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create an empty tilemap.
	/// </summary>
	public MTileMap(Vector2 tileMapPos, Point tileSize, int drawLayer)
	{
		mTileSize = tileSize;
		mBasePosition = tileMapPos;
		mDrawLayer = drawLayer;
		mTileMap = null;

		mTileAnimations = new();
	}



	/// <summary>
	/// Tell which tiles are adjacent. Used for textures.
	/// </summary>
	private void CalculateTileAdjacency()
	{
		for (int x = 0; x < mTileMap.GetLength(0); x++)
		{
			for (int y = 0; y < mTileMap.GetLength(1); y++)
			{
				mTileMap[x, y].mAdjacency = MTileAdjacency.Ad0;
			}
		}

		for (int x = 0; x < mTileMap.GetLength(0); x++)
		{
			for (int y = 0; y < mTileMap.GetLength(1); y++)
			{
				if (x + 1 < mTileMap.GetLength(0))
				{
					if (mTileMap[x, y].mType == mTileMap[x + 1, y].mType)
					{
						mTileMap[x, y].InformAdjacent(ref mTileMap[x + 1, y], MTileAdjacency.Ad6);
					}

					if (y + 1 < mTileMap.GetLength(1))
					{
						if (mTileMap[x, y].mType == mTileMap[x + 1, y + 1].mType)
						{
							mTileMap[x, y].InformAdjacent(ref mTileMap[x + 1, y + 1], MTileAdjacency.Ad3);
						}
					}

					if (y - 1 >= 0)
					{
						if (mTileMap[x, y].mType == mTileMap[x + 1, y - 1].mType)
						{
							mTileMap[x, y].InformAdjacent(ref mTileMap[x + 1, y - 1], MTileAdjacency.Ad9);
						}
					}

				}

				if (y + 1 < mTileMap.GetLength(1))
				{
					if (mTileMap[x, y].mType == mTileMap[x, y + 1].mType)
					{
						mTileMap[x, y].InformAdjacent(ref mTileMap[x, y + 1], MTileAdjacency.Ad2);
					}
				}
			}
		}
	}

	#endregion rInit





	#region rLoad

	/// <summary>
	/// Load tilemap from ldtk level.
	/// </summary>
	public void LoadFromLDtkLevel(LDtkLevel level, bool loadPositionFromFile = false)
	{
		LDtkIntGrid typeGrid = level.GetIntGrid("Type");
		LDtkIntGrid rotGrid = level.GetIntGrid("Rotation");
		LDtkIntGrid paramGrid = level.GetIntGrid("Param");

		if (loadPositionFromFile)
		{
			mBasePosition = new Vector2(level.Position.X, level.Position.Y);
		}

		LoadFromIntGrids(typeGrid.Get2DArray(), rotGrid.Get2DArray(), paramGrid.Get2DArray());
	}



	/// <summary>
	/// Load from int grids
	/// </summary>
	public void LoadFromIntGrids(int[,] types, int[,] rot, int[,] param)
	{
		mTileMap = new MTile[types.GetLength(0), types.GetLength(1)];
		Point basePt = mBasePosition.ToPoint();

		Dictionary<string, int> animToIdx = new();

		for (int x = 0; x < mTileMap.GetLength(0); x++)
		{
			for (int y = 0; y < mTileMap.GetLength(1); y++)
			{
				Point tilePos = new Point(x * mTileSize.X, y * mTileSize.Y) + basePt;
				MTile newTile = new MTile((ushort)types[x, y], (MCardDir)rot[x, y]);

				string animPath = mPolicy.GetTileAnimPath(newTile, param[x, y]);

				MAnimation anim = null;

				int idx = INVALID_ANIM;
				if (animPath != "" && !animToIdx.TryGetValue(animPath, out idx))
				{
					anim = MData.I.LoadAnimation(animPath);

					mTileAnimations.Add(anim);
					idx = mTileAnimations.Count - 1;
					MugDebug.Assert(idx != INVALID_ANIM, "Maximum limit of 256 animations exceeded");
					animToIdx.Add(animPath, idx);
				}

				newTile.mAnimIdx = (byte)idx;
				mTileMap[x, y] = newTile;
			}
		}

		CalculateTileAdjacency();
	}

	#endregion rLoad





	#region rUpdate

	/// <summary>
	/// Update all animations on tiles
	/// </summary>
	public void Update(MScene scene, MUpdateInfo info)
	{
		foreach(MAnimation anim in mTileAnimations)
		{
			anim.Update(info);
		}
	}

	#endregion rUpdate





	#region rDraw

	/// <summary>
	/// Draw info
	/// </summary>
	public void Draw(MScene scene, MDrawInfo info)
	{
		// Draw by type for better batching
		for (int i = 0; i < mTileAnimations.Count; i++)
		{
			Draw(scene, info, i);
		}
	}



	/// <summary>
	/// Draw the tilemap. Only draw 1 type of layer for better batching.
	/// </summary>
	public void Draw(MScene scene, MDrawInfo info, int tileAnimIdx)
	{
		if (tileAnimIdx >= mTileAnimations.Count)
		{
			return;
		}

		Rectangle visibleFrustum = info.mCanvas.GetCamera().GetViewportForCull();
		Rectangle tilesToDraw = PossibleIntersectTiles(visibleFrustum);

		float drawDepth = info.mCanvas.GetBaseDepth(mDrawLayer);

		Profiler.PushProfileZone("Tile Draw layer");

		int rngSeed = tileAnimIdx * 13;

		for (int x = tilesToDraw.X; x < tilesToDraw.Right; x++)
		{
			for (int y = tilesToDraw.Y; y < tilesToDraw.Bottom; y++)
			{
				MTile tile = mTileMap[x, y];

				if(tile.mType == 0 || tile.mAnimIdx == INVALID_ANIM || tile.mAnimIdx != tileAnimIdx)
				{
					continue;
				}

				rngSeed = MRandom.NextRng(rngSeed);
				float randomFloat = rngSeed / (float)int.MaxValue;
				float animOffset = tile.UsesAnimOffset() ? randomFloat : 0.0f;

				Vector2 tilePos = mBasePosition + new Vector2(x * mTileSize.X, y * mTileSize.Y);
				TileTexDrawInfo tileDrawInfo = GetTileDrawInfo(tile, animOffset);
				Rectangle sourceRectangle = new Rectangle(tileDrawInfo.mTexturePart.mUV.Location + tileDrawInfo.mTileIndex * mTileSize, mTileSize);

				info.mCanvas.DrawTexture(tileDrawInfo.mTexturePart.mTexture, tilePos, sourceRectangle, Color.White, tileDrawInfo.mRotation, Vector2.Zero, 1.0f, tileDrawInfo.mEffect, drawDepth);
			}
		}

		Profiler.PopProfileZone();
	}



	/// <summary>
	/// Find out which part of the tile atlas to draw
	/// </summary>
	TileTexDrawInfo GetTileDrawInfo(MTile tile, float animOffset)
	{
		MAnimation anim = mTileAnimations[tile.mAnimIdx];
		MTexturePart tileTexture = anim.GetCurrentTexture(animOffset);

		TileTexDrawInfo returnInfo;

		int width = tileTexture.Width();
		int height = tileTexture.Height();

		//Square texture, draw as is.
		if (width == height)
		{
			// Square textures can be rotated freely.
			// Others can't since they need ot be rotated to fit together.
			returnInfo = new TileTexDrawInfo();
			returnInfo.mRotation = tile.GetRotation();
			returnInfo.mEffect = tile.GetEffect();
		}
		// Otherwise, look for texture with different edge types
		else if (width == 6 * height)
		{
			// Needs rotating
			returnInfo = MTileDrawHelpers.SetupTileWithRotation(tile.mAdjacency);
		}
		else if (width == 4 * height)
		{
			returnInfo = MTileDrawHelpers.SetupTileNoRotation(tile.mAdjacency);
		}
		else if (height == 47 * width)
		{
			returnInfo = MTileDrawHelpers.SetupTileForBorderFill(tile.mAdjacency);
		}
		else if (height == 2 * width)
		{
			returnInfo = MTileDrawHelpers.SetupTileForUpDown(tile.mAdjacency);
		}
		else
		{
			throw new Exception("Unhandled texture dimensions");
		}

		returnInfo.mTexturePart = tileTexture;

		return returnInfo;
	}

	#endregion rDraw





	#region rCollision

	/// <summary>
	/// Does this collide?
	/// </summary>
	public bool QueryCollides(Rectangle bounds, MCardDir dir, CollisionFlags flags)
	{
		Rectangle tileBounds = PossibleIntersectTiles(bounds);

		for (int x = tileBounds.X; x <= tileBounds.X + tileBounds.Width; x++)
		{
			for (int y = tileBounds.Y; y <= tileBounds.Y + tileBounds.Height; y++)
			{
				ref MTile tile = ref mTileMap[x, y];

				// Special case for 0
				if (tile.mType == 0)
				{
					continue;
				}

				Rectangle tileRect = new Rectangle(x * mTileSize.X, y * mTileSize.Y, mTileSize.X, mTileSize.Y);
				tileRect.X += (int)mBasePosition.X;
				tileRect.Y += (int)mBasePosition.Y;
				//MugDebug.AddDebugRect(tileRect, Color.Red);

				if (mPolicy.QueryTileCollision(tile, tileRect, bounds, dir, flags))
				{
					return true;
				}
			}
		}

		return false;
	}



	/// <summary>
	/// Find rectangle of tile coordinates that a box will lie in
	/// </summary>
	/// <param name="box">Box to check</param>
	/// <returns>Rectangle of indices to tiles</returns>
	public Rectangle PossibleIntersectTiles(Rectangle bounds)
	{
		Vector2 min = new Vector2(bounds.X, bounds.Y) - mBasePosition;
		Vector2 max = new Vector2(bounds.Right, bounds.Bottom) - mBasePosition;

		min.X /= mTileSize.X;
		min.Y /= mTileSize.Y;

		max.X /= mTileSize.X;
		max.Y /= mTileSize.Y;

		Point rMin = new Point(Math.Max((int)min.X, 0), Math.Max((int)min.Y, 0));
		Point rMax = new Point(Math.Min((int)MathF.Ceiling(max.X), mTileMap.GetLength(0)), Math.Min((int)MathF.Ceiling(max.Y), mTileMap.GetLength(1)));

		return new Rectangle(rMin, rMax - rMin);
	}

	#endregion rCollision





	#region rAccess

	/// <summary>
	/// Move the tile map to a position.
	/// </summary>
	public void SetPosition(Vector2 pos)
	{
		mBasePosition = pos;
	}



	/// <summary>
	/// Get tile at a world position
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public MTile GetTile(Vector2 pos)
	{
		return GetTile(GetTileMapCoord(pos));
	}



	/// <summary>
	/// Get a tile from a coordinate in the tile map.
	/// </summary>
	/// <param name="coord">Coordinate of tile you want.</param>
	/// <returns>Tile reference</returns>
	public MTile GetTile(Point coord)
	{
		return GetTile(coord.X, coord.Y);
	}



	/// <summary>
	/// Get a tile from a coordinate of the tile map
	/// </summary>
	/// <param name="x">Tile x-coordinate</param>
	/// <param name="y">Tile y-coordinate</param>
	/// <returns>Tile reference</returns>
	public MTile GetTile(int x, int y)
	{
		if (0 <= x && x < mTileMap.GetLength(0) &&
			0 <= y && y < mTileMap.GetLength(1))
		{
			return mTileMap[x, y];
		}

		return new MTile(0, MCardDir.Up);
	}



	/// <summary>
	/// Get a tile at a world position with an offset in tiles
	/// </summary>
	/// <param name="pos">World position</param>
	/// <param name="displacement">Number of tiles to offset by</param>
	/// <returns>Tile reference</returns>
	public MTile GetRelativeTile(Vector2 pos, Point displacement)
	{
		return GetRelativeTile(pos, displacement.X, displacement.Y);
	}



	/// <summary>
	/// Get a tile at a world position with an offset in tiles
	/// </summary>
	/// <param name="pos">World position</param>
	/// <param name="dx">Number of horizontal tiles to offset</param>
	/// <param name="dy">Number of vertical tiles to offset</param>
	/// <returns></returns>
	public MTile GetRelativeTile(Vector2 pos, int dx, int dy)
	{
		Point coord = GetTileMapCoord(pos);

		return GetTile(coord.X + dx, coord.Y + dy);
	}



	/// <summary>
	/// Get an N by N square of tiles
	/// </summary>
	/// <param name="pos">World space position of the middle of the square</param>
	/// <param name="n">Side length(in tiles) of tile squares</param>
	/// <returns>Rectangle of indices to tiles</returns>
	public Rectangle GetNbyN(Vector2 pos, int n)
	{
		return GetNbyM(pos, n, n);
	}



	/// <summary>
	/// Get an N by M rectangle of tiles
	/// </summary>
	/// <param name="pos">World space position of the middle of the rectangle</param>
	/// <param name="n">Width(in tiles) of tile rectangle</param>
	/// <param name="m">Height(in tiles) of tile rectangle</param>
	/// <returns>Rectangle of indices to tiles</returns>
	public Rectangle GetNbyM(Vector2 pos, int n, int m)
	{
		Vector2 tileSpacePos = (pos - mBasePosition);
		tileSpacePos.X /= mTileSize.X;
		tileSpacePos.Y /= mTileSize.Y;

		Point point = new Point((int)tileSpacePos.X - (n - 1) / 2, (int)tileSpacePos.Y - (m - 1) / 2);
		Point size = new Point(n, m);

		if (point.X + size.X > mTileMap.GetLength(0))
		{
			size.X = mTileMap.GetLength(0) - point.X;
		}

		if (point.Y + size.Y > mTileMap.GetLength(1))
		{
			size.Y = mTileMap.GetLength(1) - point.Y;
		}

		return new Rectangle(point, size);
	}



	/// <summary>
	/// Convert world space position to tile map
	/// </summary>
	/// <param name="pos">World space position</param>
	/// <returns>Tile map index. Note that this may be out of bounds</returns>
	public Point GetTileMapCoord(Vector2 pos)
	{
		pos = pos - mBasePosition;
		pos.X /= mTileSize.X;
		pos.Y /= mTileSize.Y;

		Point ret = new Point((int)Math.Floor(pos.X), (int)MathF.Floor(pos.Y));

		return ret;
	}


	/// <summary>
	/// Convert index to tile's top left..
	/// </summary>
	public Vector2 GetTileTopLeft(Point index)
	{
		Vector2 result = mBasePosition;

		result.X += (index.X) * (mTileSize.X);
		result.Y += (index.Y) * (mTileSize.Y);

		return result;
	}



	/// <summary>
	/// Convert tile index to position.
	/// </summary>
	public Vector2 GetTileCentre(Point index)
	{
		Vector2 result = mBasePosition;

		result.X += (index.X + 0.5f) * (mTileSize.X);
		result.Y += (index.Y + 0.5f) * (mTileSize.Y);

		return result;
	}



	/// <summary>
	/// Round a position to the centre of a tile
	/// </summary>
	/// <param name="pos">World space position</param>
	/// <returns>World space position that is in the middle of a tile</returns>
	public Vector2 RoundToTileCentre(Vector2 pos)
	{
		pos = pos - mBasePosition;
		pos.X /= mTileSize.X;
		pos.Y /= mTileSize.Y;

		pos.X = (int)Math.Floor(pos.X) + 0.5f;
		pos.Y = (int)Math.Floor(pos.Y) + 0.5f;

		pos.X *= mTileSize.X;
		pos.Y *= mTileSize.Y;

		pos = pos + mBasePosition;

		return pos;
	}



	/// <summary>
	/// Is the point within the map?
	/// </summary>
	public bool IsInTileMap(Vector2 pos)
	{
		pos -= mBasePosition;

		return 0.0f <= pos.X && pos.X <= GetWidth() &&
			0.0f <= pos.Y && pos.Y <= GetHeight();
	}



	/// <summary>
	/// Is the point within the map?
	/// </summary>
	public bool IsInTileMap(Point pos)
	{
		return pos.X >= 0 && pos.X < mTileMap.GetLength(0)
			&& pos.Y >= 0 && pos.Y < mTileMap.GetLength(1);
	}



	/// <summary>
	/// Get width
	/// </summary>
	/// <returns>Width of tile map</returns>
	public float GetWidth()
	{
		return (mTileSize.X * mTileMap.GetLength(0));
	}



	/// <summary>
	/// Get height
	/// </summary>
	/// <returns>Width of tile mpa in pixels</returns>
	public float GetHeight()
	{
		return (mTileSize.Y * mTileMap.GetLength(1));
	}



	/// <summary>
	/// Get bounds of the entire tilemap
	/// </summary>
	public Rectangle GetWholeBounds()
	{
		return new Rectangle((int)mBasePosition.X, (int)mBasePosition.Y, mTileSize.X * mTileMap.GetLength(0), mTileSize.Y * mTileMap.GetLength(1));
	}



	/// <summary>
	/// Get top left position of tilemap
	/// </summary>
	public Vector2 GetTopLeft()
	{
		return mBasePosition;
	}



	/// <summary>
	/// Get approximate size in bytes of this map.
	/// </summary>
	public int GetApproxSize()
	{
		return 0;
	}

	#endregion rAccess
}

