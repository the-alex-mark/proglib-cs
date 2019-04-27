using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public class EncoderMP3S : BaseEncoder
	{
		public EncoderMP3S(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "mp3sEncoder.exe"));
			}
		}

		public override string ToString()
		{
			return "Fraunhofer IIS MP3 Surround Encoder (.mp3)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return BASSChannelType.BASS_CTYPE_STREAM_MP3;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return ".mp3";
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
				return this.BuildEncoderCommandLine();
			}
		}

		public override int EffectiveBitrate
		{
			get
			{
				return this.MP3S_Bitrate;
			}
		}

		public override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			BASSEncode bassencode = BASSEncode.BASS_ENCODE_NOHEAD;
			if (base.Force16Bit)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_16BIT;
			}
			else
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_24BIT;
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
			base.EncoderHandle = BassEnc.BASS_Encode_Start(base.ChannelHandle, this.EncoderCommandLine, bassencode, proc, user);
			return base.EncoderHandle != 0;
		}

		public override string SettingsString()
		{
			return string.Format("{0}-{1} kbps, {2} {3} {4}", new object[]
			{
				this.MP3S_UseVBR ? "VBR" : "CBR",
				this.EffectiveBitrate,
				this.MP3S_Mono ? "Mono" : "Stereo",
				this.MP3S_Quality ? "HQ" : "Fast",
				this.MP3S_UseVBR ? this.MP3S_VBRQuality.ToString() : ""
			}).Trim();
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "mp3sEncoder.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("mp3sEncoder.exe");
			}
			stringBuilder.Append(string.Format(provider, " -raw -sr {0} -c {1} -res {2}", new object[]
			{
				base.ChannelSampleRate,
				base.ChannelNumChans,
				(base.ChannelBitwidth > 16) ? (base.Force16Bit ? 16 : 24) : base.ChannelBitwidth
			}));
			if (this.MP3S_UseCustomOptionsOnly)
			{
				if (this.MP3S_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.MP3S_CustomOptions.Trim());
				}
			}
			else
			{
				if (base.ChannelNumChans > 1 && this.MP3S_Mono)
				{
					stringBuilder.Append(" -mono");
				}
				if (this.MP3S_Quality)
				{
					stringBuilder.Append(" -q 1");
				}
				if (this.MP3S_UseVBR)
				{
					stringBuilder.Append(string.Format(provider, " -m {0}", new object[]
					{
						(int)this.MP3S_VBRQuality
					}));
				}
				else
				{
					stringBuilder.Append(string.Format(provider, " -br {0}", new object[]
					{
						this.MP3S_Bitrate * 1000
					}));
				}
				if (this.MP3S_UseVBR && this.MP3S_WriteVBRHeader)
				{
					stringBuilder.Append(" -vbri");
				}
				if (this.MP3S_CustomOptions != null && this.MP3S_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.MP3S_CustomOptions.Trim());
				}
			}
			if (base.InputFile != null)
			{
				stringBuilder.Append(" -if \"" + base.InputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" -if -");
			}
			if (base.OutputFile != null)
			{
				stringBuilder.Append(" -of \"" + base.OutputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" -of -");
			}
			return stringBuilder.ToString();
		}

		public bool MP3S_UseCustomOptionsOnly;

		public string MP3S_CustomOptions = string.Empty;

		public bool MP3S_Mono;

		public bool MP3S_Quality;

		public int MP3S_Bitrate = 128;

		public bool MP3S_UseVBR;

		public EncoderMP3S.MP3SVBRQuality MP3S_VBRQuality = EncoderMP3S.MP3SVBRQuality.High;

		public bool MP3S_WriteVBRHeader;

		public enum MP3SVBRQuality
		{
			Highest = 1,
			High,
			Intermediate,
			Medium,
			Low
		}
	}
}
