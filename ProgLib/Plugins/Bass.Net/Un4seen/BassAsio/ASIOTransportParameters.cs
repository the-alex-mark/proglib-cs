using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ASIOTransportParameters
	{
		public int command;

		public long samplePosition;

		public int track;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public int[] trackSwitches = new int[16];

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string future = string.Empty;
	}
}
