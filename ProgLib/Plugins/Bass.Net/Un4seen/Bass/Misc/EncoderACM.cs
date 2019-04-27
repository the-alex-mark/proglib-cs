using System;
using System.Runtime.InteropServices;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderACM : BaseEncoder
	{
		public EncoderACM(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return true;
			}
		}

		public override string ToString()
		{
			return "Audio Compression Manager (" + this.DefaultOutputExtension + ")";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return this.ACM_EncoderType;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return this.ACM_DefaultOutputExtension;
			}
		}

		public override bool SupportsSTDOUT
		{
			get
			{
				return true;
			}
		}

		public override string EncoderCommandLine
		{
			get
			{
				return string.Empty;
			}
		}

		public override int EffectiveBitrate
		{
			get
			{
				if (this.ACM_Codec != null && this.ACM_Codec.waveformatex != null)
				{
					return this.ACM_Codec.waveformatex.nAvgBytesPerSec * 8 / 1000;
				}
				return 1411;
			}
		}

		public unsafe override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			if (this.ACM_Codec == null)
			{
				this.ACM_Codec = new ACMFORMAT();
			}
			int num = base.ChannelHandle;
			if (base.InputFile != null)
			{
				num = Bass.BASS_StreamCreateFile(base.InputFile, 0L, 0L, BASSFlag.BASS_STREAM_DECODE);
				if (num == 0)
				{
					return false;
				}
			}
			BASSEncode bassencode = BASSEncode.BASS_ENCODE_DEFAULT;
			if (paused && base.InputFile == null)
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
			fixed (byte* ptr = this.acmCodecToByteArray())
			{
				if (base.OutputFile == null)
				{
					base.EncoderHandle = BassEnc.BASS_Encode_StartACM(num, (IntPtr)((void*)ptr), bassencode, proc, user);
				}
				else
				{
					if (base.OutputFile == null || this.ACM_EncoderType != BASSChannelType.BASS_CTYPE_STREAM_WAV || !this.ACM_WriteWaveHeader)
					{
						bassencode |= BASSEncode.BASS_ENCODE_NOHEAD;
					}
					base.EncoderHandle = BassEnc.BASS_Encode_StartACMFile(num, (IntPtr)((void*)ptr), bassencode, base.OutputFile);
				}
			}
			if (base.InputFile != null)
			{
				Utils.DecodeAllData(num, true);
			}
			return base.EncoderHandle != 0;
		}

		public override string SettingsString()
		{
			if (this.ACM_Codec != null)
			{
				return this.ACM_Codec.ToString();
			}
			return string.Format("{0} kbps", this.EffectiveBitrate);
		}

		private byte[] acmCodecToByteArray()
		{
			byte[] array = new byte[Marshal.SizeOf(this.ACM_Codec) + (int)this.ACM_Codec.waveformatex.cbSize];
			int num = Marshal.SizeOf(this.ACM_Codec);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(this.ACM_Codec, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			Marshal.FreeHGlobal(intPtr);
			for (int i = 0; i < this.ACM_Codec.extension.Length; i++)
			{
				array[18 + i] = this.ACM_Codec.extension[i];
			}
			return array;
		}

		public bool SaveCodec(string acmFile)
		{
			return !string.IsNullOrEmpty(acmFile) && ACMFORMAT.SaveToFile(this.ACM_Codec, acmFile);
		}

		public bool LoadCodec(string acmFile)
		{
			if (!string.IsNullOrEmpty(acmFile))
			{
				ACMFORMAT acmformat = ACMFORMAT.LoadFromFile(acmFile);
				if (acmformat != null)
				{
					this.ACM_Codec = acmformat;
					return true;
				}
			}
			return false;
		}

		public ACMFORMAT ACM_Codec = new ACMFORMAT();

		public bool ACM_WriteWaveHeader = true;

		public BASSChannelType ACM_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_WAV;

		public string ACM_DefaultOutputExtension = ".wav";
	}
}
