using System;

namespace radio42.Multimedia.Midi
{
	public enum MIDITimeType
	{
		TIME_MS = 1,
		TIME_SAMPLES,
		TIME_BYTES = 4,
		TIME_SMPTE = 8,
		TIME_MIDI = 16,
		TIME_TICKS = 32
	}
}
