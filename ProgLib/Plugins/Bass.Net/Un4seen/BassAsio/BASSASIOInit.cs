using System;

namespace Un4seen.BassAsio
{
	[Flags]
	public enum BASSASIOInit
	{
		BASS_ASIO_DEFAULT = 0,
		BASS_ASIO_THREAD = 1,
		BASS_ASIO_JOINORDER = 2,
		BASS_ASIO_VOLRAMP = 4
	}
}
