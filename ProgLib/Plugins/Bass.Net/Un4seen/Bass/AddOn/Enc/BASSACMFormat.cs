using System;

namespace Un4seen.Bass.AddOn.Enc
{
	[Flags]
	public enum BASSACMFormat
	{
		BASS_ACM_NONE = 0,
		BASS_ACM_DEFAULT = 1,
		BASS_ACM_RATE = 2,
		BASS_ACM_CHANS = 4,
		BASS_ACM_SUGGEST = 8,
		BASS_UNICODE = -2147483648
	}
}
