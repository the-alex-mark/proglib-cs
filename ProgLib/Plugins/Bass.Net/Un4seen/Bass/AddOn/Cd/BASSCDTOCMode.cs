using System;

namespace Un4seen.Bass.AddOn.Cd
{
	[Flags]
	public enum BASSCDTOCMode
	{
		BASS_CD_TOC_LBA = 0,
		BASS_CD_TOC_TIME = 256,
		BASS_CD_TOC_INDEX = 512
	}
}
