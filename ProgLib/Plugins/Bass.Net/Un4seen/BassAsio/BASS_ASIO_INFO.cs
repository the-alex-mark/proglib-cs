using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_ASIO_INFO
	{
		public override string ToString()
		{
			return this.name;
		}

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string name = string.Empty;

		public int version;

		public int inputs;

		public int outputs;

		public int bufmin;

		public int bufmax;

		public int bufpref;

		public int bufgran;

		public int initflags;
	}
}
