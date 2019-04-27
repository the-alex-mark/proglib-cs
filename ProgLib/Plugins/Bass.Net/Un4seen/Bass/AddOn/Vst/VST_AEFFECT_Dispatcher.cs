using System;

namespace Un4seen.Bass.AddOn.Vst
{
	public delegate int VST_AEFFECT_Dispatcher(IntPtr effect, BASSVSTDispatcherOpCodes opCode, int index, int value, IntPtr ptr, float opt);
}
