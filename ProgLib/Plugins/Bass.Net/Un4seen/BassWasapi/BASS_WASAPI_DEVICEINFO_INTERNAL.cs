using System;

namespace Un4seen.BassWasapi
{
	[Serializable]
	internal struct BASS_WASAPI_DEVICEINFO_INTERNAL
	{
		public IntPtr name;

		public IntPtr id;

		public BASSWASAPIDeviceType type;

		public BASSWASAPIDeviceInfo flags;

		public float minperiod;

		public float defperiod;

		public int mixfreq;

		public int mixchans;
	}
}
