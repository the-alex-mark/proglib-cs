using System;

namespace Un4seen.Bass.AddOn.Mix
{
	[Flags]
	public enum BASSMIXEnvelope
	{
		BASS_MIXER_ENV_FREQ = 1,
		BASS_MIXER_ENV_VOL = 2,
		BASS_MIXER_ENV_PAN = 3,
		BASS_MIXER_ENV_LOOP = 65536
	}
}
