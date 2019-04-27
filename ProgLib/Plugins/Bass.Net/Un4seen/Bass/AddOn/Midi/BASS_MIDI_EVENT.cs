using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	public struct BASS_MIDI_EVENT
	{
		public BASS_MIDI_EVENT(BASSMIDIEvent EventType, int Param, int Chan, int Tick, int Pos)
		{
			this.eventtype = EventType;
			this.param = Param;
			this.chan = Chan;
			this.tick = Tick;
			this.pos = Pos;
		}

		public override string ToString()
		{
			return string.Format("Event={0}, Param={1}, Chan={2}, Tick={3} ({4})", new object[]
			{
				this.eventtype,
				this.param,
				this.chan,
				this.tick,
				this.pos
			});
		}

		public BASSMIDIEvent eventtype;

		public int param;

		public int chan;

		public int tick;

		public int pos;
	}
}
