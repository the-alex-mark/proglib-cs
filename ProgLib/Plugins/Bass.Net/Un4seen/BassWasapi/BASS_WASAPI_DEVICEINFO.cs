using System;

namespace Un4seen.BassWasapi
{
	[Serializable]
	public sealed class BASS_WASAPI_DEVICEINFO
	{
		public override string ToString()
		{
			return this.name;
		}

		public bool IsEnabled
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_ENABLED) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool IsDisabled
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_DISABLED) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool IsUnplugged
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_UNPLUGGED) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool IsDefault
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_DEFAULT) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_INIT) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool IsLoopback
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_LOOPBACK) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool IsInput
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_INPUT) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		public bool SupportsRecording
		{
			get
			{
				return (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_INPUT) != BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN || (this.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_LOOPBACK) > BASSWASAPIDeviceInfo.BASS_DEVICE_UNKNOWN;
			}
		}

		internal BASS_WASAPI_DEVICEINFO_INTERNAL _internal;

		public string name = string.Empty;

		public string id = string.Empty;

		public BASSWASAPIDeviceType type = BASSWASAPIDeviceType.BASS_WASAPI_TYPE_UNKNOWN;

		public BASSWASAPIDeviceInfo flags;

		public float minperiod;

		public float defperiod;

		public int mixfreq;

		public int mixchans;
	}
}
