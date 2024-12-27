using MugEngine.Library.Maths;
using System.Reflection;

namespace MugEngine.Types
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



	/// <summary>
	/// A direction we can walk in.
	/// </summary>
	public enum WalkDirection
	{
		Left,
		Right,
		None,
	}


	/// <summary>
	/// Helper function to convert enums into different values.
	/// </summary>
	public static class MugEnum
	{
		#region rGeneral

		/// <summary>
		/// Get an enum from a string
		/// </summary>
		static public T GetEnumFromString<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value);
		}



		/// <summary>
		/// Get number of enums
		/// </summary>
		static public int EnumLength(Type enumType)
		{
			return Enum.GetNames(enumType).Length;
		}



		/// <summary>
		/// Get file path attribute
		/// </summary>
		public static string GetFilePath(this Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());

			MFilePathAttribute attribute = field.GetCustomAttribute<MFilePathAttribute>();

			return attribute?.mPath;
		}

		#endregion rGeneral




		#region rCardDir

		/// <summary>
		/// Convert cardinal direction enum to unit vector
		/// </summary>
		/// <param name="dir">Cardinal direction</param>
		/// <returns>Cardinal direction unit vector</returns>
		/// <exception cref="NotImplementedException">Requires a valid cardinal direction</exception>
		public static Vector2 GetNormal(MCardDir dir)
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
		public static Point GetNormalPoint(MCardDir dir)
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
		public static float GetRotation(MCardDir dir)
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
		/// Swap cardinal direction for it's opposite
		/// </summary>
		/// <param name="dir">Cardinal direction</param>
		/// <returns>Opposite cardinal direction of input</returns>
		/// <exception cref="NotImplementedException">Requires a valid cardinal direction</exception>
		public static MCardDir InvertDirection(MCardDir dir)
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
		/// Invert walking direction to opposite
		/// </summary>
		public static WalkDirection InvertDirection(WalkDirection dir)
		{
			switch (dir)
			{
				case WalkDirection.Left:
					return WalkDirection.Right;
				case WalkDirection.Right:
					return WalkDirection.Left;
				case WalkDirection.None:
					return WalkDirection.None;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Convert a walk direction to a cardinal direction
		/// </summary>
		public static MCardDir WalkDirectionToCardinal(WalkDirection walk, MCardDir gravity)
		{
			switch (gravity)
			{
				case MCardDir.Up:
					return walk == WalkDirection.Right ? MCardDir.Right : MCardDir.Left;
				case MCardDir.Right:
					return walk == WalkDirection.Right ? MCardDir.Down : MCardDir.Up;
				case MCardDir.Down:
					return walk == WalkDirection.Right ? MCardDir.Right : MCardDir.Left;
				case MCardDir.Left:
					return walk == WalkDirection.Right ? MCardDir.Down : MCardDir.Up;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Get walk direction from cardinal direction
		/// </summary>
		public static WalkDirection CardinalToWalkDirection(MCardDir card, MCardDir gravity)
		{
			switch (gravity)
			{
				case MCardDir.Up:
				case MCardDir.Down:
					return card == MCardDir.Left ? WalkDirection.Left : WalkDirection.Right;
				case MCardDir.Right:
				case MCardDir.Left:
					return card == MCardDir.Up ? WalkDirection.Left : WalkDirection.Right;
			}

			throw new NotImplementedException();
		}


		/// <summary>
		/// Round angle to cardinal direction
		/// </summary>
		public static MCardDir MCardDirFromAngle(float angle)
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
		public static MCardDir MCardDirFromVector(Vector2 vector)
		{
			if (vector.Y < vector.X)
			{
				return -vector.Y < vector.X ? MCardDir.Right : MCardDir.Up;
			}

			return -vector.Y < vector.X ? MCardDir.Down : MCardDir.Left;
		}



		/// <summary>
		/// Reflect card by a reflection normal
		/// </summary>
		public static MCardDir ReflectMCardDir(MCardDir direction, Vector2 normal)
		{
			Vector2 cardVec = GetNormal(direction);
			cardVec = MugMath.Reflect(cardVec, normal);
			return MCardDirFromVector(cardVec);
		}

		#endregion rCardDir
	}



	/// <summary>
	/// File path attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	sealed class MFilePathAttribute : Attribute
	{
		public readonly string mPath;

		public MFilePathAttribute(string path)
		{
			mPath = path;
		}
	}
}
