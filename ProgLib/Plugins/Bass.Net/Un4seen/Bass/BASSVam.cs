using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSVam
	{
		BASS_VAM_HARDWARE = 1,
		BASS_VAM_SOFTWARE = 2,
		BASS_VAM_TERM_TIME = 4,
		BASS_VAM_TERM_DIST = 8,
		BASS_VAM_TERM_PRIO = 16
	}
}
