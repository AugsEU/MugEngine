﻿namespace MugEngine.Scene
{
	/// <summary>
	/// Bit map enum of all possible direct adjacencies.
	/// Naming is based on the numpad keys. So Ad2 means
	/// to the left of the tile. The reason for this is because
	/// there's no english words to represent top-left diagonally.
	/// This would mean distinguishing from the top-left diagonal and
	/// having "top AND left" adjacent tiles would be confusing.
	/// Bit choices may seem confusing but they result in nice bit-shifting
	/// algorithms.
	/// </summary>
	public enum MTileAdjacency : byte
	{
		Ad0 = 0b0000_0000,

		// Directly adjacent
		Ad2 = 0b0000_0010,
		Ad4 = 0b0000_0001,
		Ad6 = 0b0000_0100,
		Ad8 = 0b0000_1000,

		// Diagonally adjacent
		Ad1 = 0b0010_0000,
		Ad3 = 0b0100_0000,
		Ad7 = 0b0001_0000,
		Ad9 = 0b1000_0000,

		// Automatically generated for swizzle
		Ad12 = 0b0010_0010,
		Ad13 = 0b0110_0000,
		Ad14 = 0b0010_0001,
		Ad16 = 0b0010_0100,
		Ad17 = 0b0011_0000,
		Ad18 = 0b0010_1000,
		Ad19 = 0b1010_0000,
		Ad23 = 0b0100_0010,
		Ad24 = 0b0000_0011,
		Ad26 = 0b0000_0110,
		Ad27 = 0b0001_0010,
		Ad28 = 0b0000_1010,
		Ad29 = 0b1000_0010,
		Ad34 = 0b0100_0001,
		Ad36 = 0b0100_0100,
		Ad37 = 0b0101_0000,
		Ad38 = 0b0100_1000,
		Ad39 = 0b1100_0000,
		Ad46 = 0b0000_0101,
		Ad47 = 0b0001_0001,
		Ad48 = 0b0000_1001,
		Ad49 = 0b1000_0001,
		Ad67 = 0b0001_0100,
		Ad68 = 0b0000_1100,
		Ad69 = 0b1000_0100,
		Ad78 = 0b0001_1000,
		Ad79 = 0b1001_0000,
		Ad89 = 0b1000_1000,
		Ad123 = 0b0110_0010,
		Ad124 = 0b0010_0011,
		Ad126 = 0b0010_0110,
		Ad127 = 0b0011_0010,
		Ad128 = 0b0010_1010,
		Ad129 = 0b1010_0010,
		Ad134 = 0b0110_0001,
		Ad136 = 0b0110_0100,
		Ad137 = 0b0111_0000,
		Ad138 = 0b0110_1000,
		Ad139 = 0b1110_0000,
		Ad146 = 0b0010_0101,
		Ad147 = 0b0011_0001,
		Ad148 = 0b0010_1001,
		Ad149 = 0b1010_0001,
		Ad167 = 0b0011_0100,
		Ad168 = 0b0010_1100,
		Ad169 = 0b1010_0100,
		Ad178 = 0b0011_1000,
		Ad179 = 0b1011_0000,
		Ad189 = 0b1010_1000,
		Ad234 = 0b0100_0011,
		Ad236 = 0b0100_0110,
		Ad237 = 0b0101_0010,
		Ad238 = 0b0100_1010,
		Ad239 = 0b1100_0010,
		Ad246 = 0b0000_0111,
		Ad247 = 0b0001_0011,
		Ad248 = 0b0000_1011,
		Ad249 = 0b1000_0011,
		Ad267 = 0b0001_0110,
		Ad268 = 0b0000_1110,
		Ad269 = 0b1000_0110,
		Ad278 = 0b0001_1010,
		Ad279 = 0b1001_0010,
		Ad289 = 0b1000_1010,
		Ad346 = 0b0100_0101,
		Ad347 = 0b0101_0001,
		Ad348 = 0b0100_1001,
		Ad349 = 0b1100_0001,
		Ad367 = 0b0101_0100,
		Ad368 = 0b0100_1100,
		Ad369 = 0b1100_0100,
		Ad378 = 0b0101_1000,
		Ad379 = 0b1101_0000,
		Ad389 = 0b1100_1000,
		Ad467 = 0b0001_0101,
		Ad468 = 0b0000_1101,
		Ad469 = 0b1000_0101,
		Ad478 = 0b0001_1001,
		Ad479 = 0b1001_0001,
		Ad489 = 0b1000_1001,
		Ad678 = 0b0001_1100,
		Ad679 = 0b1001_0100,
		Ad689 = 0b1000_1100,
		Ad789 = 0b1001_1000,
		Ad1234 = 0b0110_0011,
		Ad1236 = 0b0110_0110,
		Ad1237 = 0b0111_0010,
		Ad1238 = 0b0110_1010,
		Ad1239 = 0b1110_0010,
		Ad1246 = 0b0010_0111,
		Ad1247 = 0b0011_0011,
		Ad1248 = 0b0010_1011,
		Ad1249 = 0b1010_0011,
		Ad1267 = 0b0011_0110,
		Ad1268 = 0b0010_1110,
		Ad1269 = 0b1010_0110,
		Ad1278 = 0b0011_1010,
		Ad1279 = 0b1011_0010,
		Ad1289 = 0b1010_1010,
		Ad1346 = 0b0110_0101,
		Ad1347 = 0b0111_0001,
		Ad1348 = 0b0110_1001,
		Ad1349 = 0b1110_0001,
		Ad1367 = 0b0111_0100,
		Ad1368 = 0b0110_1100,
		Ad1369 = 0b1110_0100,
		Ad1378 = 0b0111_1000,
		Ad1379 = 0b1111_0000,
		Ad1389 = 0b1110_1000,
		Ad1467 = 0b0011_0101,
		Ad1468 = 0b0010_1101,
		Ad1469 = 0b1010_0101,
		Ad1478 = 0b0011_1001,
		Ad1479 = 0b1011_0001,
		Ad1489 = 0b1010_1001,
		Ad1678 = 0b0011_1100,
		Ad1679 = 0b1011_0100,
		Ad1689 = 0b1010_1100,
		Ad1789 = 0b1011_1000,
		Ad2346 = 0b0100_0111,
		Ad2347 = 0b0101_0011,
		Ad2348 = 0b0100_1011,
		Ad2349 = 0b1100_0011,
		Ad2367 = 0b0101_0110,
		Ad2368 = 0b0100_1110,
		Ad2369 = 0b1100_0110,
		Ad2378 = 0b0101_1010,
		Ad2379 = 0b1101_0010,
		Ad2389 = 0b1100_1010,
		Ad2467 = 0b0001_0111,
		Ad2468 = 0b0000_1111,
		Ad2469 = 0b1000_0111,
		Ad2478 = 0b0001_1011,
		Ad2479 = 0b1001_0011,
		Ad2489 = 0b1000_1011,
		Ad2678 = 0b0001_1110,
		Ad2679 = 0b1001_0110,
		Ad2689 = 0b1000_1110,
		Ad2789 = 0b1001_1010,
		Ad3467 = 0b0101_0101,
		Ad3468 = 0b0100_1101,
		Ad3469 = 0b1100_0101,
		Ad3478 = 0b0101_1001,
		Ad3479 = 0b1101_0001,
		Ad3489 = 0b1100_1001,
		Ad3678 = 0b0101_1100,
		Ad3679 = 0b1101_0100,
		Ad3689 = 0b1100_1100,
		Ad3789 = 0b1101_1000,
		Ad4678 = 0b0001_1101,
		Ad4679 = 0b1001_0101,
		Ad4689 = 0b1000_1101,
		Ad4789 = 0b1001_1001,
		Ad6789 = 0b1001_1100,
		Ad12346 = 0b0110_0111,
		Ad12347 = 0b0111_0011,
		Ad12348 = 0b0110_1011,
		Ad12349 = 0b1110_0011,
		Ad12367 = 0b0111_0110,
		Ad12368 = 0b0110_1110,
		Ad12369 = 0b1110_0110,
		Ad12378 = 0b0111_1010,
		Ad12379 = 0b1111_0010,
		Ad12389 = 0b1110_1010,
		Ad12467 = 0b0011_0111,
		Ad12468 = 0b0010_1111,
		Ad12469 = 0b1010_0111,
		Ad12478 = 0b0011_1011,
		Ad12479 = 0b1011_0011,
		Ad12489 = 0b1010_1011,
		Ad12678 = 0b0011_1110,
		Ad12679 = 0b1011_0110,
		Ad12689 = 0b1010_1110,
		Ad12789 = 0b1011_1010,
		Ad13467 = 0b0111_0101,
		Ad13468 = 0b0110_1101,
		Ad13469 = 0b1110_0101,
		Ad13478 = 0b0111_1001,
		Ad13479 = 0b1111_0001,
		Ad13489 = 0b1110_1001,
		Ad13678 = 0b0111_1100,
		Ad13679 = 0b1111_0100,
		Ad13689 = 0b1110_1100,
		Ad13789 = 0b1111_1000,
		Ad14678 = 0b0011_1101,
		Ad14679 = 0b1011_0101,
		Ad14689 = 0b1010_1101,
		Ad14789 = 0b1011_1001,
		Ad16789 = 0b1011_1100,
		Ad23467 = 0b0101_0111,
		Ad23468 = 0b0100_1111,
		Ad23469 = 0b1100_0111,
		Ad23478 = 0b0101_1011,
		Ad23479 = 0b1101_0011,
		Ad23489 = 0b1100_1011,
		Ad23678 = 0b0101_1110,
		Ad23679 = 0b1101_0110,
		Ad23689 = 0b1100_1110,
		Ad23789 = 0b1101_1010,
		Ad24678 = 0b0001_1111,
		Ad24679 = 0b1001_0111,
		Ad24689 = 0b1000_1111,
		Ad24789 = 0b1001_1011,
		Ad26789 = 0b1001_1110,
		Ad34678 = 0b0101_1101,
		Ad34679 = 0b1101_0101,
		Ad34689 = 0b1100_1101,
		Ad34789 = 0b1101_1001,
		Ad36789 = 0b1101_1100,
		Ad46789 = 0b1001_1101,
		Ad123467 = 0b0111_0111,
		Ad123468 = 0b0110_1111,
		Ad123469 = 0b1110_0111,
		Ad123478 = 0b0111_1011,
		Ad123479 = 0b1111_0011,
		Ad123489 = 0b1110_1011,
		Ad123678 = 0b0111_1110,
		Ad123679 = 0b1111_0110,
		Ad123689 = 0b1110_1110,
		Ad123789 = 0b1111_1010,
		Ad124678 = 0b0011_1111,
		Ad124679 = 0b1011_0111,
		Ad124689 = 0b1010_1111,
		Ad124789 = 0b1011_1011,
		Ad126789 = 0b1011_1110,
		Ad134678 = 0b0111_1101,
		Ad134679 = 0b1111_0101,
		Ad134689 = 0b1110_1101,
		Ad134789 = 0b1111_1001,
		Ad136789 = 0b1111_1100,
		Ad146789 = 0b1011_1101,
		Ad234678 = 0b0101_1111,
		Ad234679 = 0b1101_0111,
		Ad234689 = 0b1100_1111,
		Ad234789 = 0b1101_1011,
		Ad236789 = 0b1101_1110,
		Ad246789 = 0b1001_1111,
		Ad346789 = 0b1101_1101,
		Ad1234678 = 0b0111_1111,
		Ad1234679 = 0b1111_0111,
		Ad1234689 = 0b1110_1111,
		Ad1234789 = 0b1111_1011,
		Ad1236789 = 0b1111_1110,
		Ad1246789 = 0b1011_1111,
		Ad1346789 = 0b1111_1101,
		Ad2346789 = 0b1101_1111,
		Ad12346789 = 0b1111_1111
	}

	static class MTileAdjacencyHelper
	{
		public static MTileAdjacency InvertDir(MTileAdjacency input)
		{
			switch (input)
			{
				case MTileAdjacency.Ad1:
					return MTileAdjacency.Ad9;
				case MTileAdjacency.Ad2:
					return MTileAdjacency.Ad8;
				case MTileAdjacency.Ad3:
					return MTileAdjacency.Ad7;
				case MTileAdjacency.Ad4:
					return MTileAdjacency.Ad6;
				case MTileAdjacency.Ad6:
					return MTileAdjacency.Ad4;
				case MTileAdjacency.Ad7:
					return MTileAdjacency.Ad3;
				case MTileAdjacency.Ad8:
					return MTileAdjacency.Ad2;
				case MTileAdjacency.Ad9:
					return MTileAdjacency.Ad1;
			}

			// Only accept single directions into this.
			throw new NotImplementedException();
		}

		public static MTileAdjacency GetDirectlyAdjacent(MTileAdjacency input)
		{
			byte inputByte = (byte)input;
			return (MTileAdjacency)(inputByte & 0b0000_1111);
		}

		public static MTileAdjacency RemoveRedundantTiles(MTileAdjacency input)
		{
			// Bit stuff haha
			byte value = (byte)input;
			// Combine all operations into a single expression to allow CPU to optimize
			return (MTileAdjacency)(
				(((value & 0x0F) | 0x10) >> 1) & value & ((value << 3) | 0x0F)
			);
		}
	}
}