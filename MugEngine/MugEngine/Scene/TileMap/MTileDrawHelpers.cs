namespace MugEngine.Scene
{
	struct TileTexDrawInfo
	{
		public TileTexDrawInfo()
		{
			mTileIndex = Point.Zero;
			mRotation = 0.0f;
			mEffect = SpriteEffects.None;
		}

		public Point mTileIndex;
		public float mRotation;
		public SpriteEffects mEffect;
		public MTexturePart mTexturePart;
	}

	static class MTileDrawHelpers
	{
		/// <summary>
		/// Used for textures that have all 16 tile types
		/// </summary>
		/// <param name="adjacency">Adjacency type</param>
		/// <param name="tileIndex">Output variable of which sub-section of the texture to use</param>
		public static TileTexDrawInfo SetupTileNoRotation(MTileAdjacency adjacency)
		{
			TileTexDrawInfo ret = new TileTexDrawInfo();

			switch (MTileAdjacencyHelper.GetDirectlyAdjacent(adjacency))
			{
				case MTileAdjacency.Ad0:
					ret.mTileIndex.X = 7;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad8:
					ret.mTileIndex.X = 0;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad2:
					ret.mTileIndex.X = 2;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad4:
					ret.mTileIndex.X = 3;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad6:
					ret.mTileIndex.X = 1;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad28:
					ret.mTileIndex.X = 5;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad48:
					ret.mTileIndex.X = 0;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad68:
					ret.mTileIndex.X = 1;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad248:
					ret.mTileIndex.X = 5;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad268:
					ret.mTileIndex.X = 7;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad468:
					ret.mTileIndex.X = 6;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad26:
					ret.mTileIndex.X = 2;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad24:
					ret.mTileIndex.X = 3;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad246:
					ret.mTileIndex.X = 4;
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad46:
					ret.mTileIndex.X = 4;
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad2468:
					ret.mTileIndex.X = 6;
					ret.mTileIndex.Y = 1;
					break;
			}

			return ret;
		}



		/// <summary>
		/// Used for textures that only have 5 tile types
		/// The rest are generated from rotations
		/// </summary>
		/// <param name="adjacency">Adjacency type</param>
		public static TileTexDrawInfo SetupTileWithRotation(MTileAdjacency adjacency)
		{
			const float PI2 = MathHelper.PiOver2;
			const float PI = MathHelper.Pi;
			const float PI32 = MathHelper.Pi * 1.5f;

			TileTexDrawInfo ret = new TileTexDrawInfo();

			switch (MTileAdjacencyHelper.GetDirectlyAdjacent(adjacency))
			{
				case MTileAdjacency.Ad0:
					ret.mTileIndex.X = 0;
					ret.mRotation = 0.0f;
					break;
				case MTileAdjacency.Ad8:
					ret.mTileIndex.X = 1;
					ret.mRotation = PI32;
					break;
				case MTileAdjacency.Ad2:
					ret.mTileIndex.X = 1;
					ret.mRotation = PI2;
					break;
				case MTileAdjacency.Ad4:
					ret.mTileIndex.X = 1;
					ret.mRotation = PI;
					break;
				case MTileAdjacency.Ad6:
					ret.mTileIndex.X = 1;
					ret.mRotation = 0.0f;
					break;
				case MTileAdjacency.Ad28:
					ret.mTileIndex.X = 2;
					ret.mRotation = PI2;
					break;
				case MTileAdjacency.Ad48:
					ret.mTileIndex.X = 5;
					ret.mRotation = 0.0f;
					break;
				case MTileAdjacency.Ad68:
					ret.mTileIndex.X = 5;
					ret.mRotation = PI2;
					break;
				case MTileAdjacency.Ad248:
					ret.mTileIndex.X = 3;
					ret.mRotation = PI32;
					break;
				case MTileAdjacency.Ad268:
					ret.mTileIndex.X = 3;
					ret.mRotation = PI2;
					break;
				case MTileAdjacency.Ad468:
					ret.mTileIndex.X = 3;
					ret.mRotation = 0.0f;
					break;
				case MTileAdjacency.Ad26:
					ret.mTileIndex.X = 5;
					ret.mRotation = PI;
					break;
				case MTileAdjacency.Ad24:
					ret.mTileIndex.X = 5;
					ret.mRotation = PI32;
					break;
				case MTileAdjacency.Ad246:
					ret.mTileIndex.X = 3;
					ret.mRotation = PI;
					break;
				case MTileAdjacency.Ad46:
					ret.mTileIndex.X = 2;
					ret.mRotation = 0.0f;
					break;
				case MTileAdjacency.Ad2468:
					ret.mTileIndex.X = 4;
					ret.mRotation = 0.0f;
					break;
			}

			return ret;
		}




		/// <summary>
		/// Sets up tile that are of the "border + fill" type
		/// </summary>
		/// <param name="adjacency">Adjacency type</param>
		public static TileTexDrawInfo SetupTileForBorderFill(MTileAdjacency adjacency)
		{
			TileTexDrawInfo ret = new TileTexDrawInfo();

			MTileAdjacency essentialAdjacency = MTileAdjacencyHelper.RemoveRedundantTiles(adjacency);
			switch (essentialAdjacency)
			{
				case MTileAdjacency.Ad12346789:
					ret.mTileIndex.Y = 0;
					break;
				case MTileAdjacency.Ad1234689:
					ret.mTileIndex.Y = 1;
					break;
				case MTileAdjacency.Ad1234678:
					ret.mTileIndex.Y = 2;
					break;
				case MTileAdjacency.Ad1246789:
					ret.mTileIndex.Y = 3;
					break;
				case MTileAdjacency.Ad2346789:
					ret.mTileIndex.Y = 4;
					break;
				case MTileAdjacency.Ad123468:
					ret.mTileIndex.Y = 5;
					break;
				case MTileAdjacency.Ad124689:
					ret.mTileIndex.Y = 6;
					break;
				case MTileAdjacency.Ad234689:
					ret.mTileIndex.Y = 7;
					break;
				case MTileAdjacency.Ad124678:
					ret.mTileIndex.Y = 8;
					break;
				case MTileAdjacency.Ad234678:
					ret.mTileIndex.Y = 9;
					break;
				case MTileAdjacency.Ad246789:
					ret.mTileIndex.Y = 10;
					break;
				case MTileAdjacency.Ad12468:
					ret.mTileIndex.Y = 11;
					break;
				case MTileAdjacency.Ad23468:
					ret.mTileIndex.Y = 12;
					break;
				case MTileAdjacency.Ad24678:
					ret.mTileIndex.Y = 13;
					break;
				case MTileAdjacency.Ad24689:
					ret.mTileIndex.Y = 14;
					break;
				case MTileAdjacency.Ad2468:
					ret.mTileIndex.Y = 15;
					break;
				case MTileAdjacency.Ad12346:
					ret.mTileIndex.Y = 16;
					break;
				case MTileAdjacency.Ad1246:
					ret.mTileIndex.Y = 17;
					break;
				case MTileAdjacency.Ad2346:
					ret.mTileIndex.Y = 18;
					break;
				case MTileAdjacency.Ad246:
					ret.mTileIndex.Y = 19;
					break;
				case MTileAdjacency.Ad12478:
					ret.mTileIndex.Y = 20;
					break;
				case MTileAdjacency.Ad2478:
					ret.mTileIndex.Y = 21;
					break;
				case MTileAdjacency.Ad1248:
					ret.mTileIndex.Y = 22;
					break;
				case MTileAdjacency.Ad248:
					ret.mTileIndex.Y = 23;
					break;
				case MTileAdjacency.Ad46789:
					ret.mTileIndex.Y = 24;
					break;
				case MTileAdjacency.Ad4689:
					ret.mTileIndex.Y = 25;
					break;
				case MTileAdjacency.Ad4678:
					ret.mTileIndex.Y = 26;
					break;
				case MTileAdjacency.Ad468:
					ret.mTileIndex.Y = 27;
					break;
				case MTileAdjacency.Ad23689:
					ret.mTileIndex.Y = 28;
					break;
				case MTileAdjacency.Ad2368:
					ret.mTileIndex.Y = 29;
					break;
				case MTileAdjacency.Ad2689:
					ret.mTileIndex.Y = 30;
					break;
				case MTileAdjacency.Ad268:
					ret.mTileIndex.Y = 31;
					break;
				case MTileAdjacency.Ad28:
					ret.mTileIndex.Y = 32;
					break;
				case MTileAdjacency.Ad46:
					ret.mTileIndex.Y = 33;
					break;
				case MTileAdjacency.Ad124:
					ret.mTileIndex.Y = 34;
					break;
				case MTileAdjacency.Ad24:
					ret.mTileIndex.Y = 35;
					break;
				case MTileAdjacency.Ad478:
					ret.mTileIndex.Y = 36;
					break;
				case MTileAdjacency.Ad48:
					ret.mTileIndex.Y = 37;
					break;
				case MTileAdjacency.Ad689:
					ret.mTileIndex.Y = 38;
					break;
				case MTileAdjacency.Ad68:
					ret.mTileIndex.Y = 39;
					break;
				case MTileAdjacency.Ad236:
					ret.mTileIndex.Y = 40;
					break;
				case MTileAdjacency.Ad26:
					ret.mTileIndex.Y = 41;
					break;
				case MTileAdjacency.Ad2:
					ret.mTileIndex.Y = 42;
					break;
				case MTileAdjacency.Ad4:
					ret.mTileIndex.Y = 43;
					break;
				case MTileAdjacency.Ad8:
					ret.mTileIndex.Y = 44;
					break;
				case MTileAdjacency.Ad6:
					ret.mTileIndex.Y = 45;
					break;
				case MTileAdjacency.Ad0:
					ret.mTileIndex.Y = 46;
					break;
				default:
					throw new Exception("This tile type doesn't work!");
			}

			return ret;
		}


		/// <summary>
		/// Setup basic up/down tile
		/// </summary>
		public static TileTexDrawInfo SetupTileForUpDown(MTileAdjacency adjacency)
		{
			TileTexDrawInfo ret = new TileTexDrawInfo();

			if (adjacency.HasFlag(MTileAdjacency.Ad8))
			{
				ret.mTileIndex.Y = 1;
			}

			return ret;
		}
	}
}
