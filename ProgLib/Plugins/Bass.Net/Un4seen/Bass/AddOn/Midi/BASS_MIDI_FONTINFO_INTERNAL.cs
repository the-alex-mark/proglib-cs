using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	internal struct BASS_MIDI_FONTINFO_INTERNAL
	{
		public IntPtr name;

		public IntPtr copyright;

		public IntPtr comment;

		public int presets;

		public int samsize;

		public int samload;

		public int samtype;
	}
}
