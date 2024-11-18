using System.Runtime.CompilerServices;
using bottlenoselabs.C2CS.Runtime;
using Tracy;

namespace MugEngine.Core
{
	static class MugProfile
	{
		public static void HeartBeat()
		{
			Tracy.PInvoke.TracyEmitFrameMarkStart(new CString("UpdateLoop"));
		}
	}

	public struct MProfileScope
	{
		private Tracy.PInvoke.TracyCZoneContext mZoneID;

		// Add CallerMemberName to automatically capture the function name
		public MProfileScope(Color color, string name = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string function = null, [CallerFilePath] string sourceFile = null)
		{
			ulong srcloc = PInvoke.TracyAllocSrclocName(
											(uint)lineNumber,
											CString.FromString(sourceFile),
											(ulong)sourceFile.Length,
											CString.FromString(function),
											(ulong)function.Length,
											CString.FromString(name),
											(ulong)name.Length);


			mZoneID = Tracy.PInvoke.TracyEmitZoneBeginAlloc(srcloc, 1);
		}

		public void End()
		{
			Tracy.PInvoke.TracyEmitZoneEnd(mZoneID);
		}
	}
}
