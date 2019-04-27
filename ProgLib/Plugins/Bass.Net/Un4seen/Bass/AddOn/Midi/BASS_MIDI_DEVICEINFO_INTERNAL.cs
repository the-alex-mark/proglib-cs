using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	internal struct BASS_MIDI_DEVICEINFO_INTERNAL
	{
		public IntPtr name;

		public int id;

		public BASSDeviceInfo flags;
	}
}
