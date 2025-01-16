namespace MugEngine.Core
{
	/// <summary>
	/// Reprents a direction in 1 of 4 directions.
	/// </summary>
	public enum MCardDir
	{
		Up = 0,
		Right = 1,
		Down = 2,
		Left = 3,
	}




	public static class MCardDirImpl
	{
		/// <summary>
		/// Convert cardinal direction enum to unit vector
		/// </summary>
		/// <param name="dir">Cardinal direction</param>
		/// <returns>Cardinal direction unit vector</returns>
		/// <exception cref="NotImplementedException">Requires a valid cardinal direction</exception>
		public static Vector2 ToVec(this MCardDir dir)
		{
			switch (dir)
			{
				case MCardDir.Up:
					return new Vector2(0.0f, -1.0f);
				case MCardDir.Down:
					return new Vector2(0.0f, 1.0f);
				case MCardDir.Left:
					return new Vector2(-1.0f, 0.0f);
				case MCardDir.Right:
					return new Vector2(1.0f, 0.0f);
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Convert cardinal direction enum to unit point
		/// </summary>
		/// <param name="dir">Cardinal direction</param>
		/// <returns>Cardinal direction unit vector</returns>
		/// <exception cref="NotImplementedException">Requires a valid cardinal direction</exception>
		public static Point ToPoint(this MCardDir dir)
		{
			switch (dir)
			{
				case MCardDir.Up:
					return new Point(0, -1);
				case MCardDir.Down:
					return new Point(0, 1);
				case MCardDir.Left:
					return new Point(-1, 0);
				case MCardDir.Right:
					return new Point(1, 0);
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Gets angle from cardinal direction
		/// </summary>
		public static float ToAngle(this MCardDir dir)
		{
			switch (dir)
			{
				case MCardDir.Up:
					return 0.0f;
				case MCardDir.Right:
					return MathHelper.PiOver2;
				case MCardDir.Down:
					return MathHelper.Pi;
				case MCardDir.Left:
					return MathHelper.PiOver2 * 3.0f;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Round angle to cardinal direction
		/// </summary>
		public static MCardDir FromAngle(float angle)
		{
			angle = MugMath.MainBranchRadian(angle);
			float PI8 = MathF.PI / 4.0f;

			if (angle < PI8 || angle > 7.0f * PI8)
			{
				return MCardDir.Up;
			}
			else if (angle < 3.0f * PI8)
			{
				return MCardDir.Left;
			}
			else if (angle < 5.0f * PI8)
			{
				return MCardDir.Down;
			}

			return MCardDir.Right;
		}



		/// <summary>
		/// Card from a vector.
		/// </summary>
		public static MCardDir FromVec(Vector2 vector)
		{
			if (vector.Y < vector.X)
			{
				return -vector.Y < vector.X ? MCardDir.Right : MCardDir.Up;
			}

			return -vector.Y < vector.X ? MCardDir.Down : MCardDir.Left;
		}



		/// <summary>
		/// Swap cardinal direction for it's opposite
		/// </summary>
		/// <param name="dir">Cardinal direction</param>
		/// <returns>Opposite cardinal direction of input</returns>
		/// <exception cref="NotImplementedException">Requires a valid cardinal direction</exception>
		public static MCardDir Inverted(this MCardDir dir)
		{
			switch (dir)
			{
				case MCardDir.Up:
					return MCardDir.Down;
				case MCardDir.Right:
					return MCardDir.Left;
				case MCardDir.Down:
					return MCardDir.Up;
				case MCardDir.Left:
					return MCardDir.Right;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Reflect card by a reflection normal
		/// </summary>
		public static MCardDir Reflected(this MCardDir direction, Vector2 normal)
		{
			Vector2 cardVec = direction.ToVec();
			cardVec = MugMath.Reflect(cardVec, normal);
			return FromVec(cardVec);
		}



		/// <summary>
		/// Get walk direction from cardinal direction
		/// </summary>
		public static MWalkDir CardinalToWalkDirection(this MCardDir direction, MCardDir gravity = MCardDir.Down)
		{
			switch (gravity)
			{
				case MCardDir.Up:
				case MCardDir.Down:
					return direction == MCardDir.Left ? MWalkDir.Left : MWalkDir.Right;
				case MCardDir.Right:
				case MCardDir.Left:
					return direction == MCardDir.Up ? MWalkDir.Left : MWalkDir.Right;
			}

			throw new NotImplementedException();
		}
	}
}
