using System;

namespace radio42.Multimedia.Midi
{
	public enum MIDIDevice : short
	{
		MOD_UNKNOWN,
		MOD_MIDIPORT,
		MOD_SYNTH,
		MOD_SQSYNTH,
		MOD_FMSYNTH,
		MOD_MAPPER,
		MOD_WAVETABLE,
		MOD_SWSYNTH
	}
}
