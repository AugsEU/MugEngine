namespace MugEngine.Core
{
	/// <summary>
	/// A direction we can walk in.
	/// </summary>
	public enum MWalkDir
	{
		Left,
		Right,
		None,
	}





	public static class MWalkDirImpl
	{
		/// <summary>
		/// Invert walking direction to opposite
		/// </summary>
		public static MWalkDir Inverted(this MWalkDir dir)
		{
			switch (dir)
			{
				case MWalkDir.Left:
					return MWalkDir.Right;
				case MWalkDir.Right:
					return MWalkDir.Left;
				case MWalkDir.None:
					return MWalkDir.None;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Convert a walk direction to a cardinal direction
		/// </summary>
		public static MCardDir ToCardDir(this MWalkDir walk, MCardDir gravity = MCardDir.Down)
		{
			switch (gravity)
			{
				case MCardDir.Up:
					return walk == MWalkDir.Right ? MCardDir.Right : MCardDir.Left;
				case MCardDir.Right:
					return walk == MWalkDir.Right ? MCardDir.Down : MCardDir.Up;
				case MCardDir.Down:
					return walk == MWalkDir.Right ? MCardDir.Right : MCardDir.Left;
				case MCardDir.Left:
					return walk == MWalkDir.Right ? MCardDir.Down : MCardDir.Up;
			}

			throw new NotImplementedException();
		}


		/// <summary>
		/// Convert a walk direction to a cardinal direction
		/// </summary>
		public static Vector2 ToVec(this MWalkDir walk, MCardDir gravity = MCardDir.Down)
		{
			return walk.ToCardDir(gravity).ToVec();
		}
	}
}
