using System;
using System.Runtime.InteropServices;

namespace radio42.Multimedia.Midi
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class MIDI_INCAPS
	{
		public override string ToString()
		{
			return this.name;
		}

		public short mid;

		public short pid;

		public int driverVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string name = string.Empty;

		public int support;
	}
}
