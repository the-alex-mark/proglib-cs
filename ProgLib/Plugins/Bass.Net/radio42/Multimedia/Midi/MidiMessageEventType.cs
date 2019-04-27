using System;

namespace radio42.Multimedia.Midi
{
	public enum MidiMessageEventType
	{
		Opened,
		Closed,
		Started,
		Stopped,
		ShortMessage,
		SystemExclusive,
		ShortMessageError,
		SystemExclusiveError,
		SystemExclusiveDone
	}
}
