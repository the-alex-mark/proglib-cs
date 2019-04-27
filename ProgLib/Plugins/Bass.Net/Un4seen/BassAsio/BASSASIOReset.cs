using System;

namespace Un4seen.BassAsio
{
	[Flags]
	public enum BASSASIOReset
	{
		BASS_ASIO_RESET_ENABLE = 1,
		BASS_ASIO_RESET_JOIN = 2,
		BASS_ASIO_RESET_PAUSE = 4,
		BASS_ASIO_RESET_FORMAT = 8,
		BASS_ASIO_RESET_RATE = 16,
		BASS_ASIO_RESET_VOLUME = 32
	}
}
