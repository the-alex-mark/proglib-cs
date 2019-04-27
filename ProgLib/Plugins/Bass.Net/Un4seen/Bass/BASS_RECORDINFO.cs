using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_RECORDINFO
	{
		public override string ToString()
		{
			return string.Format("Inputs={0}, SingleIn={1}", this.inputs, this.singlein);
		}

		public BASSRecordFormat WaveFormat
		{
			get
			{
				return (BASSRecordFormat)(this.formats & 16777215);
			}
		}

		public int Channels
		{
			get
			{
				return this.formats >> 24;
			}
		}

		public bool SupportsDirectSound
		{
			get
			{
				return (this.flags & BASSRecordInfo.DSCAPS_EMULDRIVER) == BASSRecordInfo.DSCAPS_NONE;
			}
		}

		public bool IsCertified
		{
			get
			{
				return (this.flags & BASSRecordInfo.DSCAPS_CERTIFIED) > BASSRecordInfo.DSCAPS_NONE;
			}
		}

		public BASSRecordInfo flags;

		public int formats;

		public int inputs;

		public bool singlein;

		public int freq;
	}
}
