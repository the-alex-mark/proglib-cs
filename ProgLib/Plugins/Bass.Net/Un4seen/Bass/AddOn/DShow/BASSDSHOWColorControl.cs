using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Flags]
	public enum BASSDSHOWColorControl
	{
		BASS_DSHOW_ControlBrightness = 1,
		BASS_DSHOW_ControlContrast = 2,
		BASS_DSHOW_ControlHue = 4,
		BASS_DSHOW_ControlSaturation = 8
	}
}
