using System;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_DEVICEINFO
	{
		public BASSDeviceInfo status
		{
			get
			{
				return this.flags & (BASSDeviceInfo)16777215;
			}
		}

		public BASSDeviceInfo type
		{
			get
			{
				return this.flags & BASSDeviceInfo.BASS_DEVICE_TYPE_MASK;
			}
		}

		public override string ToString()
		{
			return this.name;
		}

		public bool IsEnabled
		{
			get
			{
				return (this.flags & BASSDeviceInfo.BASS_DEVICE_ENABLED) > BASSDeviceInfo.BASS_DEVICE_NONE;
			}
		}

		public bool IsDefault
		{
			get
			{
				return (this.flags & BASSDeviceInfo.BASS_DEVICE_DEFAULT) > BASSDeviceInfo.BASS_DEVICE_NONE;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return (this.flags & BASSDeviceInfo.BASS_DEVICE_INIT) > BASSDeviceInfo.BASS_DEVICE_NONE;
			}
		}

		internal BASS_DEVICEINFO_INTERNAL _internal;

		public string name = string.Empty;

		public string driver = string.Empty;

		public string id;

		public BASSDeviceInfo flags;
	}
}
