using System;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderWAV : BaseEncoder
	{
		public EncoderWAV(int channel) : base(channel)
		{
			if (channel != 0)
			{
				this._wavBitsPerSample = base.ChannelBitwidth;
			}
		}

		public override string ToString()
		{
			if (this._wavUseAIFF)
			{
				return "AIFF Encoder (.aif)";
			}
			return "RIFF WAVE (.wav)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return this.WAV_EncoderType;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return this.WAV_DefaultOutputExtension;
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
				return base.ChannelSampleRate * this.WAV_BitsPerSample * base.ChannelNumChans / 1000;
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
			BASSEncode bassencode = BASSEncode.BASS_ENCODE_PCM;
			if (this._wavUseAIFF)
			{
				bassencode = BASSEncode.BASS_ENCODE_AIFF;
			}
			if (this._wavBitsPerSample == 16)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_16BIT;
			}
			else if (this._wavBitsPerSample == 24)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_24BIT;
			}
			else if (this._wavBitsPerSample == 8)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_8BIT;
			}
			else if (this.WAV_Use32BitInteger)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_32BIT;
			}
			if (this.BWF_UseRF64)
			{
				bassencode |= BASSEncode.BASS_ENCODE_RF64;
			}
			if (paused || (base.TAGs != null && !this._wavUseAIFF))
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
			if (this._wavBitsPerSample > 16 || base.ChannelSampleRate > 48000 || base.ChannelNumChans > 2)
			{
				bassencode |= BASSEncode.BASS_ENCODE_WFEXT;
			}
			base.EncoderHandle = BassEnc.BASS_Encode_Start(num, base.OutputFile, bassencode, null, IntPtr.Zero);
			if (base.TAGs != null && !this._wavUseAIFF)
			{
				if (this.BWF_AddBEXT)
				{
					byte[] array = base.TAGs.ConvertToRiffBEXT(true);
					if (array != null && array.Length != 0)
					{
						BassEnc.BASS_Encode_AddChunk(base.EncoderHandle, "bext", array, array.Length);
					}
				}
				if (this.BWF_AddCART)
				{
					byte[] array2 = base.TAGs.ConvertToRiffCART(true);
					if (array2 != null && array2.Length != 0)
					{
						BassEnc.BASS_Encode_AddChunk(base.EncoderHandle, "cart", array2, array2.Length);
					}
				}
				if (this.WAV_AddRiffInfo)
				{
					byte[] array3 = base.TAGs.ConvertToRiffINFO(false);
					if (array3 != null && array3.Length != 0)
					{
						BassEnc.BASS_Encode_AddChunk(base.EncoderHandle, "LIST", array3, array3.Length);
					}
				}
			}
			if (base.TAGs != null && !this._wavUseAIFF)
			{
				BassEnc.BASS_Encode_SetPaused(base.EncoderHandle, paused);
			}
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
			return string.Format("{0} kbps, {1}-bit {2}", this.EffectiveBitrate, this.WAV_BitsPerSample, (this.WAV_BitsPerSample == 32) ? (this.WAV_Use32BitInteger ? "Linear" : "Float") : "").Trim();
		}

		public int WAV_BitsPerSample
		{
			get
			{
				return this._wavBitsPerSample;
			}
			set
			{
				if (value == 8 || value == 16 || value == 24 || value == 32)
				{
					this._wavBitsPerSample = value;
				}
			}
		}

		public bool WAV_Use32BitInteger
		{
			get
			{
				return this._wavUse32BitInteger;
			}
			set
			{
				this._wavUse32BitInteger = value;
			}
		}

		public bool WAV_AddRiffInfo
		{
			get
			{
				return this._wavAddRiffInfo;
			}
			set
			{
				this._wavAddRiffInfo = value;
			}
		}

		public bool WAV_UseAIFF
		{
			get
			{
				return this._wavUseAIFF;
			}
			set
			{
				this._wavUseAIFF = value;
				if (this._wavUseAIFF)
				{
					this.WAV_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_AIFF;
					this.WAV_DefaultOutputExtension = ".aif";
					return;
				}
				this.WAV_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_WAV;
				this.WAV_DefaultOutputExtension = ".wav";
			}
		}

		public bool BWF_UseRF64
		{
			get
			{
				return this._bwfUseRF64;
			}
			set
			{
				this._bwfUseRF64 = value;
			}
		}

		public bool BWF_AddBEXT
		{
			get
			{
				return this._bwfAddBWF;
			}
			set
			{
				this._bwfAddBWF = value;
			}
		}

		public bool BWF_AddCART
		{
			get
			{
				return this._bwfAddCART;
			}
			set
			{
				this._bwfAddCART = value;
			}
		}

		private int _wavBitsPerSample = 16;

		private bool _wavUse32BitInteger;

		private bool _wavAddRiffInfo;

		private bool _wavUseAIFF;

		private bool _bwfUseRF64;

		private bool _bwfAddBWF;

		private bool _bwfAddCART;

		public BASSChannelType WAV_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_WAV;

		public string WAV_DefaultOutputExtension = ".wav";
	}
}
