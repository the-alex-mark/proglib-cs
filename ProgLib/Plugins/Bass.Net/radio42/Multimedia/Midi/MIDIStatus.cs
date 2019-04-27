using System;

namespace radio42.Multimedia.Midi
{
	public enum MIDIStatus : byte
	{
		None,
		NoteOff = 128,
		NoteOn = 144,
		Aftertouch = 160,
		ControlChange = 176,
		ProgramChange = 192,
		ChannelPressure = 208,
		PitchBend = 224,
		SystemMsgs = 240,
		MidiTimeCode,
		SongPosition,
		SongSelect,
		TuneRequest = 246,
		EOX,
		Clock,
		Tick,
		Start,
		Continue,
		Stop,
		ActiveSense = 254,
		Reset
	}
}
