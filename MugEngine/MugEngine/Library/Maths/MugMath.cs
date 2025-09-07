namespace MugEngine
{
	/// <summary>
	/// Math utility functions
	/// </summary>
	public static class MugMath
	{
		#region rConstants

		public const float SQRT_2 = 1.41421356237f;
		public const float I_SQRT_2 = 0.70710678118f;

		#endregion rConstants




		#region rVector

		/// <summary>
		/// Cross two vectors
		/// </summary>
		/// <param name="a">Vector a</param>
		/// <param name="b">Vector b</param>
		/// <returns>Cross product of a and b</returns>
		public static float Cross(Vector2 a, Vector2 b)
		{
			return a.X * b.Y - a.Y * b.X;
		}



		/// <summary>
		/// Generate a perpendicular to vector
		/// </summary>
		/// <param name="a">Input vector</param>
		/// <returns>Clockwise perpendicular vector</returns>
		public static Vector2 Perpendicular(this Vector2 a)
		{
			return new Vector2(a.Y, -a.X);
		}



		/// <summary>
		/// Rotate vector clockwise
		/// </summary>
		public static Vector2 Rotate(Vector2 a, float angle)
		{
			float s = MathF.Sin(angle);
			float c = MathF.Cos(angle);
			return new Vector2(c * a.X - s * a.Y, s * a.X + c * a.Y);
		}



		/// <summary>
		/// Rotate vector clockwise
		/// </summary>
		public static Vector2 RotateDeg(Vector2 a, float angle)
		{
			return Rotate(a, DegToRad(angle));
		}



		/// <summary>
		/// Vector 2 from angle.
		/// </summary>
		public static Vector2 FromAngle(float angle)
		{
			return Rotate(new Vector2(1.0f, 0.0f), angle);
		}



		/// <summary>
		/// Lerp two vectors
		/// </summary>
		public static Vector2 Lerp(Vector2 p1, Vector2 p2, float t)
		{
			return p1 * (1.0f - t) + p2 * t;
		}



		/// <summary>
		/// Project V2 to V3
		/// </summary>
		public static Vector3 ToVec3(this Vector2 vec)
		{
			return new Vector3(vec, 0.0f);
		}



		/// <summary>
		/// Gets diff in direction between two vectors. 0 is no diff, 1 is max diff, 0.5 is perp
		/// </summary>
		public static float VectorDiff(Vector2 p1, Vector2 p2)
		{
			p1.Normalize();
			p2.Normalize();

			return 0.5f * (1.0f - Vector2.Dot(p1, p2));
		}



		/// <summary>
		/// Round a float to an int.
		/// </summary>
		public static Vector2 Round(this Vector2 v)
		{
			return new Vector2(MathF.Round(v.X), MathF.Round(v.Y));
		}



		/// <summary>
		/// Reflect vector along normal
		/// </summary>
		public static Vector2 Reflect(Vector2 vec, Vector2 normal)
		{
			return vec - 2.0f * Vector2.Dot(vec, normal) * normal;
		}



		/// <summary>
		/// Reflect vector along normal and centre of reflection
		/// </summary>
		public static Vector2 Reflect(Vector2 vec, Vector2 normal, Vector2 centre)
		{
			vec -= centre;
			return vec - 2.0f * Vector2.Dot(vec, normal) * normal + centre;
		}



		/// <summary>
		/// Truncate annoying float stuff
		/// </summary>
		public static Vector2 TruncateSmall(this Vector2 a)
		{
			const float THRESH = 0.000001f;
			if (-THRESH < a.X && a.X < THRESH)
			{
				a.X = 0.0f;
			}
			else if (-THRESH < a.Y && a.Y < THRESH)
			{
				a.Y = 0.0f;
			}

			return a;
		}



		/// <summary>
		/// Spaceship operator on length. 
		/// </summary>
		public static int CmpLength(this Vector2 v, float l)
		{
			return v.LengthSquared().CompareTo(l*l);
		}

		#endregion rVector




		#region rColor

		/// <summary>
		/// Lerp two colours
		/// </summary>
		public static Color Lerp(Color p1, Color p2, float t)
		{
			float R = p1.R * (1.0f - t) + p2.R * t;
			float G = p1.G * (1.0f - t) + p2.G * t;
			float B = p1.B * (1.0f - t) + p2.B * t;

			R = Math.Clamp(R, 0.0f, 255.0f);
			G = Math.Clamp(G, 0.0f, 255.0f);
			B = Math.Clamp(B, 0.0f, 255.0f);

			return new Color((byte)R, (byte)G, (byte)B);
		}

		#endregion rColor





		#region rNumber

		/// <summary>
		/// Rotate vector clockwise
		/// </summary>
		public static float RadToDeg(float angle)
		{
			return angle / (MathF.PI * 2.0f) * 360.0f;
		}


		/// <summary>
		/// Rotate vector clockwise
		/// </summary>
		public static float DegToRad(float angle)
		{
			return angle / 360.0f * MathF.PI * 2.0f;
		}


		/// <summary>
		/// Turn radian into 0 -> 2PI range
		/// </summary>
		public static float MainBranchRadian(float angle)
		{
			while (angle <= 0.0f)
			{
				angle += MathF.PI * 2.0f;
			}
			return angle % (MathF.PI * 2.0f);
		}



		/// <summary>
		/// Clamp number between -absLimit and absLimit
		/// </summary>
		/// <param name="toClamp">Number to clamp</param>
		/// <param name="absLimit">Absolute limit of number</param>
		/// <returns>Number in the range [-absLimitm,absLimit]</returns>
		public static float ClampAbs(float toClamp, float absLimit)
		{
			absLimit = Math.Abs(absLimit);
			if (toClamp > absLimit)
			{
				toClamp = absLimit;
			}
			else if (toClamp < -absLimit)
			{
				toClamp = -absLimit;
			}

			return toClamp;
		}



		/// <summary>
		/// Round a float to an int.
		/// </summary>
		public static int Round(float f)
		{
			return (int)MathF.Round(f);
		}



		/// <summary>
		/// Skews range to lower end, such as with an exponential curve
		/// </summary>
		public static float FakeExpSquash(float min, float max, float t)
		{
			t -= min;
			t /= min - max;
			t *= t;
			return min * (1.0f - t) + max * t;
		}



		/// <summary>
		/// Biject float into a fixed region.
		/// </summary>
		public static float SquashToRange(float value, float min, float max)
		{
			if (value > 0.0f)
			{
				value = 1.0f - 1.0f / (value + 2.0f);
			}
			else
			{
				value = -1.0f / (value - 2.0f);
			}

			return (max - min) * value + min;
		}



		/// <summary>
		/// Get digits of a number as a list
		/// </summary>
		public static int[] GetDigits(int num)
		{
			if (num == 0)
			{
				return new int[] { 0 };
			}

			int count = (int)Math.Log10(num) + 1;
			int[] digits = new int[count];

			// Extract digits
			for (int i = count - 1; i >= 0; --i)
			{
				digits[i] = num % 10;
				num /= 10;
			}

			return digits;
		}



		/// <summary>
		/// Get digits of a number as a list
		/// </summary>
		public static int CountDigits(float num)
		{
			if (num < 0.0f) num = -num;

			int digits = 1;
			while (num >= 10.0f)
			{
				num /= 10.0f;
				digits++;
			}

			return digits;
		}



		/// <summary>
		/// Find min of 3 numbers
		/// </summary>
		public static int Min(int n1, int n2, int n3)
		{
			return Math.Min(Math.Min(n1, n2), n3);
		}



		/// <summary>
		/// Find min of 3 numbers
		/// </summary>
		public static int Min(int n1, int n2, int n3, int n4)
		{
			return Math.Min(Math.Min(Math.Min(n1, n2), n3), n4);
		}



		/// <summary>
		/// Find min of 3 numbers
		/// </summary>
		public static int Max(int n1, int n2, int n3)
		{
			return Math.Max(Math.Max(n1, n2), n3);
		}



		/// <summary>
		/// Find min of 3 numbers
		/// </summary>
		public static int Max(int n1, int n2, int n3, int n4)
		{
			return Math.Max(Math.Max(Math.Max(n1, n2), n3), n4);
		}



		/// <summary>
		/// Move towards zero by amount but not over
		/// </summary>
		public static float MoveToZero(float start, float amount)
		{
			if (start > 0.0f)
			{
				return Math.Max(start - amount, 0.0f);
			}
			else
			{
				return Math.Min(start + amount, 0.0f);
			}

			throw new Exception("How did we get here?");
		}



		/// <summary>
		/// Sort two numbers
		/// </summary>
		public static (T, T) MinMax<T>(T n1, T n2) where T : IComparable<T>
		{
			if(n1.CompareTo(n2) < 0)
			{
				return (n1, n2);
			}
			else
			{
				return (n2, n1);
			}
		}

		#endregion rNumber





		#region rBits

		/// <summary>
		/// Counts the number of bits which are set
		/// </summary>
		public static uint BitCountI32(uint i)
		{
			i = i - (i >> 1 & 0x55555555);                // add pairs of bits
			i = (i & 0x33333333) + (i >> 2 & 0x33333333); // quads
			i = i + (i >> 4) & 0x0F0F0F0F;                // groups of 8
			return i * 0x01010101 >> 24;                  // horizontal sum of bytes
		}

		#endregion rBits





		#region rGeom

		/// <summary>
		/// Create a square centered at a location
		/// </summary>
		public static Rectangle SquareCenteredAt(Vector2 centre, float side)
		{
			int iS = (int)MathF.Round(side);
			side *= 0.5f;

			Vector2 TL = centre;
			if (side > 1.0f)
			{
				TL.X -= side;
				TL.Y -= side;
			}

			int iX = (int)MathF.Round(TL.X);
			int iY = (int)MathF.Round(TL.Y);

			return new Rectangle(iX, iY, iS, iS);
		}



		/// <summary>
		/// Get bounding rectangle.
		/// </summary>
		public static Rectangle GetBoundingRectangle(Rectangle rect1, Rectangle rect2)
		{
			int minX = Math.Min(rect1.Left, rect2.Left);
			int minY = Math.Min(rect1.Top, rect2.Top);
			int maxX = Math.Max(rect1.Right, rect2.Right);
			int maxY = Math.Max(rect1.Bottom, rect2.Bottom);

			return new Rectangle(minX, minY, maxX - minX, maxY - minY);
		}


		/// <summary>
		/// Get minimal rectangle that includes both points
		/// </summary>
		public static Rectangle GetBoundingRectangle(Point pt1, Point pt2)
		{
			(int xMin, int xMax) = MinMax(pt1.X, pt2.X);
			(int yMin, int yMax) = MinMax(pt1.Y, pt2.Y);

			return new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
		}



		/// <summary>
		/// Get minimal rectangle that includes the rectangle and the point
		/// </summary>
		public static Rectangle GetBoundingRectangle(Rectangle rect, Point pt)
		{
			int x = rect.X;
			int y = rect.Y;
			int w = rect.Width;
			int h = rect.Height;

			if(pt.X > rect.X)// Point is to the right of the origin
			{
				// Enlarge rect to fit point
				w = Math.Max(rect.Width, pt.X - rect.X);
			}
			else// Point is to the left of the origin
			{
				int r = rect.Right;
				x = Math.Min(rect.X, pt.X);
				w = r - x;
			}

			if (pt.Y > rect.Y)// Point is below the origin
			{
				// Enlarge rect to fit point
				h = Math.Max(rect.Height, pt.Y - rect.Y);
			}
			else// Point above the origin
			{
				int b = rect.Bottom;
				y = Math.Min(rect.Y, pt.Y);
				h = b - y;
			}

			return new Rectangle(x, y, w, h);
		}


		/// <summary>
		/// Get adjacent points.
		/// </summary>
		public static List<Point> GetAdjacentPoints(Point point)
		{
			List<Point> returnVal = new List<Point>();
			returnVal.Add(new Point(point.X + 1, point.Y));
			returnVal.Add(new Point(point.X - 1, point.Y));
			returnVal.Add(new Point(point.X, point.Y + 1));
			returnVal.Add(new Point(point.X, point.Y - 1));
			return returnVal;
		}




		/// <summary>
		/// Create a rectangle surrounding 3 points
		/// </summary>
		public static Rectangle CreateRectangle(Point pt1, Point pt2, Point pt3)
		{
			Point topLeft = new Point(Min(pt1.X, pt2.X, pt3.X), Min(pt1.Y, pt2.Y, pt3.Y));
			Point botRight = new Point(Max(pt1.X, pt2.X, pt3.X), Max(pt1.Y, pt2.Y, pt3.Y));

			return new Rectangle(topLeft, botRight - topLeft);
		}



		/// <summary>
		/// Create a rectangle surrounding 4 points
		/// </summary>
		public static Rectangle CreateRectangle(Point pt1, Point pt2, Point pt3, Point pt4)
		{
			Point topLeft = new Point(Min(pt1.X, pt2.X, pt3.X, pt4.X), Min(pt1.Y, pt2.Y, pt3.Y, pt4.Y));
			Point botRight = new Point(Max(pt1.X, pt2.X, pt3.X, pt4.X), Max(pt1.Y, pt2.Y, pt3.Y, pt4.Y));

			return new Rectangle(topLeft, botRight - topLeft);
		}


		/// <summary>
		/// Create a rectangle from floating points
		/// </summary>
		public static Rectangle RectFromFloats(float x, float y, float w, float h)
		{
			return new Rectangle(Round(x), Round(y), Round(w), Round(h));
		}



		/// <summary>
		/// Multiply point by scalar
		/// </summary>
		public static Point Multiply(this Point pt, int m)
		{
			return new Point(pt.X * m, pt.Y * m);
		}



		/// <summary>
		/// Get distance squared
		/// </summary>
		public static float DistSq(Point a, Point b)
		{
			int dx = a.X - b.X;
			int dy = a.Y - b.Y;

			return dx * dx + dy * dy;
		}



		/// <summary>
		/// Get distance between points
		/// </summary>
		public static float Dist(Point a, Point b)
		{
			int dx = a.X - b.X;
			int dy = a.Y - b.Y;

			return MathF.Sqrt(dx * dx + dy * dy);
		}



		/// <summary>
		/// Get manhattan distance between points
		/// </summary>
		public static float ManhattanDist(Point a, Point b)
		{
			int dx = a.X - b.X;
			int dy = a.Y - b.Y;

			return Math.Abs(dx) + Math.Abs(dy);
		}



		/// <summary>
		/// Compare the distance between two points
		/// Returns negative if the distance between is less than d
		/// Returns positive if the distance between is more than d
		/// </summary>
		public static int CmpDist(Point a, Point b, float d)
		{
			return DistSq(a, b).CompareTo(d*d);
		}



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
		/// Which compass direction should you go if you are at "from" and going to "to"
		/// Includes margin of error.
		/// </summary>
		public static MCompassDir? GetDirectionToGo(Point from, Point to, int delta = 3)
		{
			bool east = from.X + delta < to.X;
			bool west = from.X > to.X + delta;

			bool north = from.Y > to.Y + delta;
			bool south = from.Y + delta < to.Y;

			if (east)
			{
				if (north)
				{
					return MCompassDir.NE;
				}
				else if (south)
				{
					return MCompassDir.SE;
				}
				else
				{
					return MCompassDir.E;
				}
			}
			else if(west)
			{
				if (north)
				{
					return MCompassDir.NW;
				}
				else if (south)
				{
					return MCompassDir.SW;
				}
				else
				{
					return MCompassDir.W;
				}
			}
			else if(north)
			{
				return MCompassDir.N;
			}
			else if(south)
			{
				return MCompassDir.S;
			}

			return null;
		}

		#endregion rGeom
	}
}
