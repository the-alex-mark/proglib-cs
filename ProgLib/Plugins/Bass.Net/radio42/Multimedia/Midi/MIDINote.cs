using System;

namespace radio42.Multimedia.Midi
{
	public enum MIDINote : byte
	{
		C,
		CSharp,
		D,
		DSharp,
		E,
		F,
		FSharp,
		G,
		GSharp,
		A,
		ASharp,
		B,
		NOTE_LOW_KEYBOARD88 = 21,
		NOTE_LOW_KEYBOARD76 = 28,
		NOTE_LOW_KEYBOARD61 = 36,
		NOTE_MIDDLE_C = 60,
		NOTE_STANDARDPITCH = 69,
		NOTE_HIGH_KEYBOARD61 = 96,
		NOTE_HIGH_KEYBOARD76 = 103,
		NOTE_HIGH_KEYBOARD88 = 108,
		NOTE_HIGHEST = 127
	}
}
