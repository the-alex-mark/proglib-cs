using System;

namespace radio42.Multimedia.Midi
{
	[Flags]
	public enum MIDIHeader
	{
		MHDR_NONE = 0,
		MHDR_DONE = 1,
		MHDR_PREPARED = 2,
		MHDR_INQUEUE = 4,
		MHDR_ISSTRM = 8
	}
}
