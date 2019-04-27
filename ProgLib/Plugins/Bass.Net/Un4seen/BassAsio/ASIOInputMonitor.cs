using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ASIOInputMonitor
	{
		public int input;

		public int output;

		public int gain;

		[MarshalAs(UnmanagedType.Bool)]
		public bool state;

		public int pan;
	}
}
