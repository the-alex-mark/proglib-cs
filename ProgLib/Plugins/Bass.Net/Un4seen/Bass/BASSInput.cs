using System;

namespace Un4seen.Bass
{
	[Flags]
	public enum BASSInput
	{
		BASS_INPUT_NONE = 0,
		BASS_INPUT_OFF = 65536,
		BASS_INPUT_ON = 131072
	}
}
