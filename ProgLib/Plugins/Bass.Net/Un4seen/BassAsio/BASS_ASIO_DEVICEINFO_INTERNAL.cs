using System;

namespace Un4seen.BassAsio
{
	[Serializable]
	internal struct BASS_ASIO_DEVICEINFO_INTERNAL
	{
		public IntPtr name;

		public IntPtr driver;
	}
}
