using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	internal struct MIDI_MARK_INTERNAL
	{
		public int track;

		public int pos;

		public IntPtr text;
	}
}
