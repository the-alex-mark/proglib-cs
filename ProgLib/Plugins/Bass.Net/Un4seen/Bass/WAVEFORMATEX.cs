using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class WAVEFORMATEX
	{
		public WAVEFORMATEX()
		{
		}

		public WAVEFORMATEX(WAVEFormatTag format, short channels, int samplesPerSec, short bitsPerSample, short exSize)
		{
			this.wFormatTag = format;
			this.nChannels = channels;
			this.nSamplesPerSec = samplesPerSec;
			this.wBitsPerSample = bitsPerSample;
			this.nBlockAlign = Convert.ToInt16(this.nChannels * (this.wBitsPerSample / 8));
			this.nAvgBytesPerSec = this.nSamplesPerSec * (int)this.nBlockAlign;
			this.cbSize = exSize;
		}

		public override string ToString()
		{
			string text = "Stereo";
			if (this.nChannels == 1)
			{
				text = "Mono";
			}
			else if (this.nChannels == 3)
			{
				text = "2.1";
			}
			else if (this.nChannels == 4)
			{
				text = "Quad";
			}
			else if (this.nChannels == 5)
			{
				text = "4.1";
			}
			else if (this.nChannels == 6)
			{
				text = "5.1";
			}
			else if (this.nChannels == 7)
			{
				text = "6.1";
			}
			else if (this.nChannels > 7)
			{
				text = this.nChannels.ToString() + "chans";
			}
			string text2;
			if (this.wBitsPerSample == 0)
			{
				text2 = string.Format("{0} kbps", this.nAvgBytesPerSec * 8 / 1000);
			}
			else
			{
				text2 = this.wBitsPerSample.ToString() + "-bit";
				if (this.nAvgBytesPerSec > 0)
				{
					text2 += string.Format(", {0} kbps", this.nAvgBytesPerSec * 8 / 1000);
				}
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} {1}, {2:##0.0#}kHz {3}", new object[]
			{
				this.wFormatTag,
				text2,
				(double)this.nSamplesPerSec / 1000.0,
				text
			});
		}

		public WAVEFormatTag wFormatTag = WAVEFormatTag.PCM;

		public short nChannels = 2;

		public int nSamplesPerSec = 44100;

		public int nAvgBytesPerSec = 176400;

		public short nBlockAlign = 4;

		public short wBitsPerSample = 16;

		public short cbSize;
	}
}
