using System;

namespace Un4seen.BassWasapi
{
	[Flags]
	public enum BASSWASAPIInit
	{
		BASS_WASAPI_SHARED = 0,
		BASS_WASAPI_EXCLUSIVE = 1,
		BASS_WASAPI_AUTOFORMAT = 2,
		BASS_WASAPI_BUFFER = 4,
		BASS_WASAPI_EVENT = 16
	}
}
