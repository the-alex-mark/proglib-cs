using System;

namespace Un4seen.Bass.AddOn.Vst
{
	public delegate int VSTPROC(int vstHandle, BASSVSTAction action, int param1, int param2, IntPtr user);
}
