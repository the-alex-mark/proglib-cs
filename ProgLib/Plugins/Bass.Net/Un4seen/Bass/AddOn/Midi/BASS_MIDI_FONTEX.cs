using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	public struct BASS_MIDI_FONTEX
	{
		public BASS_MIDI_FONTEX(int Font, int SPreset, int SBank, int DPreset, int DBank, int DBanklsb)
		{
			this.font = Font;
			this.spreset = SPreset;
			this.sbank = SBank;
			this.dpreset = DPreset;
			this.dbank = DBank;
			this.dbanklsb = DBanklsb;
		}

		public override string ToString()
		{
			return string.Format("Font={0}, Preset={1}, Bank={2}", this.font, this.spreset, this.sbank);
		}

		public int font;

		public int spreset;

		public int sbank;

		public int dpreset;

		public int dbank;

		public int dbanklsb;
	}
}
