using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	public struct BASS_MIDI_FONT
	{
		public BASS_MIDI_FONT(int Font, int Preset, int Bank)
		{
			this.font = Font;
			this.preset = Preset;
			this.bank = Bank;
		}

		public override string ToString()
		{
			return string.Format("Font={0}, Preset={1}, Bank={2}", this.font, this.preset, this.bank);
		}

		public int font;

		public int preset;

		public int bank;
	}
}
