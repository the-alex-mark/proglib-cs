using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Vst
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_VST_AUDIO_MASTER_PARAM
	{
		public IntPtr aeffect = IntPtr.Zero;

		public BASSVSTDispatcherOpCodes opcode;

		public int index;

		public int value;

		public IntPtr ptr = IntPtr.Zero;

		public float opt;

		public long doDefault;
	}
}
