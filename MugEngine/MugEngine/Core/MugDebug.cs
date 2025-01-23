#define FAIL_ON_ASSERT

#define USE_SEPARATE_CONSOLE_OFF
#define USE_BUFFERED_LOG

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MugEngine.Core
{
	public static class MugDebug
	{
		struct DebugRect(Rectangle rect, Color color, bool perm)
		{
			public Rectangle mRectangle = rect;
			public Color mColor = color;
			public bool mPerm = perm;
		}

		private static List<DebugRect> mDebugRectToDraw = new List<DebugRect>();

		static uint mLogLineNum = 0;

#if USE_SEPARATE_CONSOLE
		static bool mConsoleAlloc = false;
#elif USE_BUFFERED_LOG
		static StringBuilder mMessageBuffer = new StringBuilder();
#endif


		/// <summary>
		/// Log message to console. Only if debug is on.
		/// </summary>
		public static void Log(string msg, params object[] args)
		{
#if DEBUG
			string format = string.Format("[{0}]: {1}", mLogLineNum.ToString("X4"), msg);
			mLogLineNum++;

#if USE_SEPARATE_CONSOLE
			if (!mConsoleAlloc)
			{
				AllocConsole();
				mConsoleAlloc = true;

			}
			
			Console.WriteLine(format, args);
#elif USE_BUFFERED_LOG
			string output = string.Format(format, args);
			mMessageBuffer.AppendLine(output);
#else
			Debug.WriteLine(format, args);
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


		public static void AddDebugRect(Rectangle rect, Color color, bool perm = false)
		{
#if DEBUG
			mDebugRectToDraw.Add(new DebugRect(rect, color, perm));
#endif
		}

		public static void AddDebugPoint(Vector2 pos, Color color, bool perm = false)
		{
#if DEBUG
			AddDebugRect(new Rectangle(pos.ToPoint(), new Point(2, 2)), color, perm);
#endif
		}

		public static void FinalizeDebug(MDrawInfo info, int layer)
		{
#if DEBUG
			foreach (DebugRect debugRect in mDebugRectToDraw)
			{
				info.mCanvas.DrawRect(debugRect.mRectangle, debugRect.mColor, layer);
			}
			mDebugRectToDraw.RemoveAll(r => !r.mPerm);

#if USE_BUFFERED_LOG
			string allMessages = mMessageBuffer.ToString();

			if (allMessages.Length > 0)
			{
				Debug.WriteLine(allMessages);
			}
			mMessageBuffer.Clear();
#endif
#endif
		}
	}
}
