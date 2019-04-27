using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ASIOChannelControls
	{
		public int channel;

		[MarshalAs(UnmanagedType.Bool)]
		public bool isInput;

		public int gain;

		public int meter;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string future = string.Empty;
	}
}
