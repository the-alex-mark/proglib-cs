using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_ASIO_CHANNELINFO
	{
		public override string ToString()
		{
			return this.name;
		}

		public int group;

		public BASSASIOFormat format = BASSASIOFormat.BASS_ASIO_FORMAT_UNKNOWN;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string name = string.Empty;
	}
}
