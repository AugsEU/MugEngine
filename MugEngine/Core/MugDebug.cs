#define FAIL_ON_ASSERT

using System.Runtime.InteropServices;

namespace MugEngine.Core
{
	static class MugDebug
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
			if (!mConsoleAlloc)
			{
				AllocConsole();
				mConsoleAlloc = true;
			}
			mLogLineNum++;
			string format = string.Format("[{0}]: {1}", mLogLineNum.ToString("X4"), msg);
			Console.WriteLine(format, args);
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
	}
}
