using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_INFO
	{
		public override string ToString()
		{
			return string.Format("Speakers={0}, MinRate={1}, MaxRate={2}, DX={3}, EAX={4}", new object[]
			{
				this.speakers,
				this.minrate,
				this.maxrate,
				this.dsver,
				this.eax
			});
		}

		public bool SupportsContinuousRate
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_CONTINUOUSRATE) > BASSInfo.DSCAPS_NONE;
			}
		}

		public bool SupportsDirectSound
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_EMULDRIVER) == BASSInfo.DSCAPS_NONE;
			}
		}

		public bool IsCertified
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_CERTIFIED) > BASSInfo.DSCAPS_NONE;
			}
		}

		public bool SupportsMonoSamples
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_SECONDARYMONO) > BASSInfo.DSCAPS_NONE;
			}
		}

		public bool SupportsStereoSamples
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_SECONDARYSTEREO) > BASSInfo.DSCAPS_NONE;
			}
		}

		public bool Supports8BitSamples
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_SECONDARY8BIT) > BASSInfo.DSCAPS_NONE;
			}
		}

		public bool Supports16BitSamples
		{
			get
			{
				return (this.flags & BASSInfo.DSCAPS_SECONDARY16BIT) > BASSInfo.DSCAPS_NONE;
			}
		}

		public BASSInfo flags;

		public int hwsize;

		public int hwfree;

		public int freesam;

		public int free3d;

		public int minrate;

		public int maxrate;

		public bool eax;

		public int minbuf = 500;

		public int dsver;

		public int latency;

		public BASSInit initflags;

		public int speakers;

		public int freq;
	}
}
