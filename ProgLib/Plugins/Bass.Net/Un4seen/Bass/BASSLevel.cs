using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSLevel
	{
		BASS_LEVEL_ALL = 0,
		BASS_LEVEL_MONO = 1,
		BASS_LEVEL_STEREO = 2,
		BASS_LEVEL_RMS = 4
	}
}
