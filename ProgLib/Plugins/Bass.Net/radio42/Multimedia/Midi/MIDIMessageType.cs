using System;

namespace radio42.Multimedia.Midi
{
	[Flags]
	public enum MIDIMessageType : byte
	{
		Unknown = 0,
		Channel = 1,
		SystemCommon = 2,
		SystemRealtime = 4,
		SystemExclusive = 8
	}
}
