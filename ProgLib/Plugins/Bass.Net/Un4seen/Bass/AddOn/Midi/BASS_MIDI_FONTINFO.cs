using System;

namespace Un4seen.Bass.AddOn.Midi
{
	[Serializable]
	public sealed class BASS_MIDI_FONTINFO
	{
		public override string ToString()
		{
			return string.Format("Name={0}, Copyright={1}, Comment={2}", (this.name == null) ? string.Empty : this.name, (this.copyright == null) ? string.Empty : this.copyright, (this.comment == null) ? string.Empty : this.comment);
		}

		internal BASS_MIDI_FONTINFO_INTERNAL _internal;

		public string name = string.Empty;

		public string copyright = string.Empty;

		public string comment = string.Empty;

		public int presets;

		public int samsize;

		public int samload;

		public int samtype = -1;
	}
}
