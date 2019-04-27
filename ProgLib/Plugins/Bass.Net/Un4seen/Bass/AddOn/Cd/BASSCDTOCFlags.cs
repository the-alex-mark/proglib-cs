using System;

namespace Un4seen.Bass.AddOn.Cd
{
	[Flags]
	public enum BASSCDTOCFlags : byte
	{
		BASS_CD_TOC_CON_NONE = 0,
		BASS_CD_TOC_CON_PRE = 1,
		BASS_CD_TOC_CON_COPY = 2,
		BASS_CD_TOC_CON_DATA = 4
	}
}
