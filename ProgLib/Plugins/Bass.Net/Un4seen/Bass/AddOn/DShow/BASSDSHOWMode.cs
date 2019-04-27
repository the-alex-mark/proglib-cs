using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Flags]
	public enum BASSDSHOWMode
	{
		BASS_DSHOW_POS_SEC = 0,
		BASS_DSHOW_POS_FRAMES = 1,
		BASS_DSHOW_POS_MILISEC = 2,
		BASS_DSHOW_POS_REFTIME = 3
	}
}
