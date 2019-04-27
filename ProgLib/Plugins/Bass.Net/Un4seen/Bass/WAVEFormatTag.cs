using System;

namespace Un4seen.Bass
{
	public enum WAVEFormatTag : short
	{
		UNKNOWN,
		PCM,
		ADPCM,
		IEEE_FLOAT,
		DOLBY_AC2 = 48,
		GSM610,
		MSNAUDIO,
		MPEG = 80,
		MPEGLAYER3 = 85,
		DOLBY_AC3_SPDIF = 146,
		RAW_AAC1 = 255,
		MSAUDIO1 = 352,
		WMA,
		WMA_PRO,
		WMA_LOSSLESS,
		WMA_SPDIF,
		MPEG_ADTS_AAC = 5632,
		MPEG_RAW_AAC,
		MPEG_LOAS,
		MPEG_HEAAC = 5648,
		EXTENSIBLE = -2
	}
}
