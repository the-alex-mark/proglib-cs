using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	public sealed class BASS_MIDI_DEVICEINFO
	{
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

		public bool IsInitialized
		{
			get
			{
				return (this.flags & BASSDeviceInfo.BASS_DEVICE_INIT) > BASSDeviceInfo.BASS_DEVICE_NONE;
			}
		}

		internal BASS_MIDI_DEVICEINFO_INTERNAL _internal;

		public string name = string.Empty;

		public int id;

		public BASSDeviceInfo flags;
	}
}
