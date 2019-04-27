using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Cd
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_CD_TOC_TRACK
	{
		public byte hour
		{
			get
			{
				return (byte)(this.lba >> 24 & 15);
			}
		}

		public byte minute
		{
			get
			{
				return (byte)(this.lba >> 16 & 15);
			}
		}

		public byte second
		{
			get
			{
				return (byte)(this.lba >> 8 & 15);
			}
		}

		public byte frame
		{
			get
			{
				return (byte)(this.lba & 15);
			}
		}

		public byte ADR
		{
			get
			{
				return (byte)(this.adrcon >> 4 & 15);
			}
		}

		public BASSCDTOCFlags Control
		{
			get
			{
				return (BASSCDTOCFlags)(this.adrcon & 15);
			}
		}

		public override string ToString()
		{
			if (this.track == 170)
			{
				return string.Format("Lead-Out: adr={0}, con={1}, start={2} [{3:00}:{4:00}:{5:00}]", new object[]
				{
					this.ADR,
					this.Control,
					this.lba,
					this.minute,
					this.second,
					this.frame
				});
			}
			return string.Format("Track {0}: adr={1}, con={2}, start={3} [{4:00}:{5:00}:{6:00}]", new object[]
			{
				this.track,
				this.ADR,
				this.Control,
				this.lba,
				this.minute,
				this.second,
				this.frame
			});
		}

		public byte res1;

		public byte adrcon;

		public byte track;

		public byte res2;

		public int lba;
	}
}
