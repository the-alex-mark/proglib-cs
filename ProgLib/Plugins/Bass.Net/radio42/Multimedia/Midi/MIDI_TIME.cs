using System;
using System.Runtime.InteropServices;

namespace radio42.Multimedia.Midi
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MIDI_TIME
	{
		[FieldOffset(0)]
		public MIDITimeType type;

		[FieldOffset(4)]
		public int ms;

		[FieldOffset(4)]
		public int sample;

		[FieldOffset(4)]
		public int cb;

		[FieldOffset(4)]
		public int ticks;

		[FieldOffset(4)]
		public byte hour;

		[FieldOffset(5)]
		public byte min;

		[FieldOffset(6)]
		public byte sec;

		[FieldOffset(7)]
		public byte frame;

		[FieldOffset(8)]
		public byte fps;

		[FieldOffset(9)]
		private byte dummy;

		[FieldOffset(10)]
		private byte pad1;

		[FieldOffset(11)]
		private byte pad2;

		[FieldOffset(4)]
		public int songptrpos;
	}
}
