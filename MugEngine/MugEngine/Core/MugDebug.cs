#define FAIL_ON_ASSERT
#define USE_SEPARATE_CONSOLE_OFF

using System.Diagnostics;
using System.Runtime.InteropServices;
using TracyWrapper;

namespace MugEngine.Core
{
	public static class MugDebug
	{
		struct DebugRect
		{
			public Rectangle mRectangle;
			public Color mColor;
		}

		private static List<DebugRect> mDebugRectToDraw = new List<DebugRect>();

		public static bool mConsoleAlloc = false;
		public static bool mDebugFlag1 = false;
		static uint mLogLineNum = 0;

		/// <summary>
		/// Log message to console. Only if debug is on.
		/// </summary>
		public static void Log(string msg, params object[] args)
		{
#if DEBUG
#if USE_SEPARATE_CONSOLE
			if (!mConsoleAlloc)
			{
				AllocConsole();
				mConsoleAlloc = true;

			}
			mLogLineNum++;
			string format = string.Format("[{0}]: {1}", mLogLineNum.ToString("X4"), msg);
			
			Console.WriteLine(format, args);
#else
			Debug.WriteLine(msg, args);
#endif
#endif
		}



		/// <summary>
		/// Open console window
		/// </summary>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AllocConsole();



		/// <summary>
		/// Debug break.
		/// </summary>
		public static void Break(string msg = "", params object[] args)
		{
			if (msg != "")
			{
				Log(msg, args);
			}
			System.Diagnostics.Debugger.Break();
		}



		/// <summary>
		/// Conditional debug break.
		/// </summary>
		public static void Assert(bool condition, string msg = "", params object[] args)
		{
#if DEBUG
			if (!condition)
			{
				Log(msg, args);
				Break("Assertion failed.");
			}
#elif FAIL_ON_ASSERT
			if (!condition)
			{
				string errorMsg = string.Format(msg, args);
				throw new Exception(errorMsg);
			}
#endif
		}


		public static void AddDebugRect(Rectangle rect, Color color)
		{
#if DEBUG
			DebugRect debugRect;
			debugRect.mRectangle = rect;
			debugRect.mColor = color;

			mDebugRectToDraw.Add(debugRect);
#endif
		}

		public static void AddDebugPoint(Vector2 pos, Color color)
		{
#if DEBUG
			AddDebugRect(new Rectangle(MugMath.ToPoint(pos), new Point(2, 2)), color);
#endif
		}

		public static void DrawDebugRects(MDrawInfo info, int layer)
		{
#if DEBUG
			foreach (DebugRect debugRect in mDebugRectToDraw)
			{
				info.mCanvas.DrawRect(debugRect.mRectangle, debugRect.mColor, layer);
			}
			mDebugRectToDraw.Clear();
#endif
		}
	}
}
