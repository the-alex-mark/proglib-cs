using System;

namespace radio42.Multimedia.Midi
{
	[Flags]
	public enum MIDIFlags
	{
		MIDI_CALLBACK_NULL = 0,
		MIDI_CALLBACK_FUNCTION = 196608,
		MIDI_IO_STATUS = 32
	}
}
