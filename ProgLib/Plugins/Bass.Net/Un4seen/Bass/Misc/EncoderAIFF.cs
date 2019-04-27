using System;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderAIFF : BaseEncoder
	{
		public EncoderAIFF(int channel) : base(channel)
		{
			if (channel != 0)
			{
				this._aiffBitsPerSample = base.ChannelBitwidth;
			}
		}

		public override string ToString()
		{
			return "AIFF Encoder (.aif)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return this.AIFF_EncoderType;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return this.AIFF_DefaultOutputExtension;
			}
		}

		public override bool SupportsSTDOUT
		{
			get
			{
				return false;
			}
		}

		public override string EncoderCommandLine
		{
			get
			{
				return base.OutputFile;
			}
		}

		public override int EffectiveBitrate
		{
			get
			{
				return base.ChannelSampleRate * this.AIFF_BitsPerSample * base.ChannelNumChans / 1000;
			}
		}

		public override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			if (base.OutputFile == null)
			{
				return false;
			}
			int num = base.ChannelHandle;
			if (base.InputFile != null)
			{
				num = Bass.BASS_StreamCreateFile(base.InputFile, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
				if (num == 0)
				{
					return false;
				}
			}
			BASSEncode bassencode = BASSEncode.BASS_ENCODE_AIFF;
			if (this._aiffBitsPerSample == 16)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_16BIT;
			}
			else if (this._aiffBitsPerSample == 24)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_24BIT;
			}
			else if (this._aiffBitsPerSample == 8)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_8BIT;
			}
			else if (this.AIFF_Use32BitInteger)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_32BIT;
			}
			if (paused)
			{
				bassencode |= BASSEncode.BASS_ENCODE_PAUSE;
			}
			if (base.NoLimit)
			{
				bassencode |= BASSEncode.BASS_ENCODE_CAST_NOLIMIT;
			}
			if (base.UseAsyncQueue)
			{
				bassencode |= BASSEncode.BASS_ENCODE_QUEUE;
			}
			base.EncoderHandle = BassEnc.BASS_Encode_Start(num, base.OutputFile, bassencode, null, IntPtr.Zero);
			if (base.InputFile != null)
			{
				Utils.DecodeAllData(num, true);
			}
			return base.EncoderHandle != 0;
		}

		public new bool Force16Bit
		{
			get
			{
				return false;
			}
		}

		public override string SettingsString()
		{
			return string.Format("{0} kbps, {1}-bit {2}", this.EffectiveBitrate, this.AIFF_BitsPerSample, (this.AIFF_BitsPerSample == 32) ? (this.AIFF_Use32BitInteger ? "Linear" : "Float") : "").Trim();
		}

		public int AIFF_BitsPerSample
		{
			get
			{
				return this._aiffBitsPerSample;
			}
			set
			{
				if (value == 8 || value == 16 || value == 24 || value == 32)
				{
					this._aiffBitsPerSample = value;
				}
			}
		}

		public bool AIFF_Use32BitInteger
		{
			get
			{
				return this._aiffUse32BitInteger;
			}
			set
			{
				this._aiffUse32BitInteger = value;
			}
		}

		private int _aiffBitsPerSample = 16;

		private bool _aiffUse32BitInteger;

		public BASSChannelType AIFF_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_AIFF;

		public string AIFF_DefaultOutputExtension = ".aif";
	}
}
