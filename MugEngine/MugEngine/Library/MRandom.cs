﻿namespace MugEngine.Library
{
	/// <summary>
	/// Special random class we can control
	/// </summary>
	public class MRandom
	{
		#region rConstants

		private const int A = 16807;
		private const int M = 2147483647;
		private const int Q = 127773;
		private const int R = 2836;

		#endregion rConstants





		#region rMembers

		private static Random sStdRandom = new Random();
		private static int sInitRandomSeed = 0;
		private int mSeed;

		#endregion rMembers





		#region rSeedConfig

		/// <summary>
		/// Create random with non-deterministic seed
		/// </summary>
		public static MRandom NonDetRng()
		{
			return new MRandom(sStdRandom.Next());
		}



		/// <summary>
		/// Construct random with auto seed.
		/// </summary>
		public MRandom()
		{
			mSeed = sInitRandomSeed;
			sInitRandomSeed = Next();
		}



		/// <summary>
		/// Consctruct random from a seed
		/// </summary>
		/// <param name="seed">Start seed</param>
		public MRandom(int seed)
		{
			mSeed = seed;
		}



		/// <summary>
		/// Set the random seed
		/// </summary>
		/// <param name="_seed">Seed to set</param>
		public void SetSeed(int _seed)
		{
			mSeed = _seed;
		}



		/// <summary>
		/// Incorperate number into our seed, changes seed in deterministic way based on the number and the seed.
		/// </summary>
		/// <param name="number">Number to chug</param>
		public void ChugNumber(int number)
		{
			mSeed += number;
			Next();
		}



		/// <summary>
		/// Get next random number
		/// </summary>
		/// <returns>Pseudo-random number</returns>
		public int Next()
		{
			int hi = mSeed / Q;
			int lo = mSeed % Q;

			mSeed = (A * lo) - (R * hi);

			if (mSeed < 0)
			{
				mSeed = mSeed + M;
			}

			if (mSeed == 0)
			{
				mSeed = 1;
			}

			return mSeed;
		}

		#endregion rSeedConfig





		#region rAccessors

		/// <summary>
		/// Get integer within range(inclusive)
		/// </summary>
		/// <param name="low">Minimum value</param>
		/// <param name="high">Maximum value</param>
		/// <returns>Integer within range(inclusive)</returns>
		public int GetIntRange(int low, int high)
		{
			int num = Next();
			num = (num % (high - low + 1)) + low;

			return num;
		}



		/// <summary>
		/// Get float within the range 0-1
		/// </summary>
		/// <returns>Get float within the range (0,1]</returns>
		public float GetUnitFloat()
		{
			int num = Next();

			return (num * 1.0f) / M;
		}



		/// <summary>
		/// Get float within range(low,high]
		/// </summary>
		/// <param name="low">Minimum number</param>
		/// <param name="high">Maximum number</param>
		/// <returns>Float within range(low,high]</returns>
		public float GetFloatRange(float low, float high)
		{
			return GetUnitFloat() * (high - low) + low;
		}



		/// <summary>
		/// Random event with percent chance of being true.
		/// </summary>
		/// <param name="percent">Percent change of being true</param>
		/// <returns>True percent% of the time</returns>
		public bool PercentChance(float percent)
		{
			return GetFloatRange(0.0f, 100.0f) < percent;
		}



		/// <summary>
		/// Gets point in rectangle
		/// </summary>
		public Vector2 PointIn(Rectangle rect)
		{
			float x = GetFloatRange(rect.X, rect.X + rect.Width);
			float y = GetFloatRange(rect.Y, rect.Y + rect.Height);

			return new Vector2(x, y);
		}



		/// <summary>
		/// Get random element in list
		/// </summary>
		public T InList<T>(IList<T> values)
		{
			int idx = GetIntRange(0, values.Count - 1);
			return values[idx];
		}



		/// <summary>
		/// Generate a random color
		/// </summary>
		public Color GetColor()
		{
			int r = GetIntRange(0, 255);
			int g = GetIntRange(0, 255);
			int b = GetIntRange(0, 255);

			return new Color(r, g, b);
		}

		#endregion rAccessors
	}
}
