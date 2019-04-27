using System;
using System.Collections.Generic;

namespace Un4seen.Bass.AddOn.Cd
{
	[Serializable]
	public sealed class BASS_CD_TOC
	{
		public override string ToString()
		{
			return string.Format("{0} tracks ({1} - {2})", this.tracks.Count, this.first, this.last);
		}

		public byte first;

		public byte last;

		public List<BASS_CD_TOC_TRACK> tracks = new List<BASS_CD_TOC_TRACK>();
	}
}
