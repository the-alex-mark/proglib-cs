using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderNeroAAC : BaseEncoder
	{
		public EncoderNeroAAC(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				if (this.NERO_UseSSE)
				{
					return File.Exists(Path.Combine(base.EncoderDirectory, "neroAacEnc_sse2.exe"));
				}
				return File.Exists(Path.Combine(base.EncoderDirectory, "neroAacEnc.exe"));
			}
		}

		public override string ToString()
		{
			return "Nero Digital Aac Encoder (.m4a)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return BASSChannelType.BASS_CTYPE_STREAM_AAC;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return ".m4a";
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
				return this.BuildEncoderCommandLine();
			}
		}

		public override int EffectiveBitrate
		{
			get
			{
				return this.NERO_Bitrate;
			}
		}

		public override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			BASSEncode bassencode = BASSEncode.BASS_ENCODE_DEFAULT;
			if (base.Force16Bit)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_16BIT;
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
			return string.Format("{0}-{1} kbps, {2}", this.NERO_UseCBR ? "CBR" : "VBR", this.EffectiveBitrate, this.NERO_LC ? "LC" : (this.NERO_HE ? "HE" : (this.NERO_HEv2 ? "HEv2" : "Auto")));
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (this.NERO_UseSSE)
			{
				if (!string.IsNullOrEmpty(base.EncoderDirectory))
				{
					stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "neroAacEnc_sse2.exe") + "\"");
				}
				else
				{
					stringBuilder.Append("neroAacEnc_sse2.exe");
				}
			}
			else if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "neroAacEnc.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("neroAacEnc.exe");
			}
			if (this.NERO_UseCustomOptionsOnly)
			{
				if (this.NERO_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.NERO_CustomOptions.Trim());
				}
			}
			else
			{
				if (this.NERO_UseQualityMode)
				{
					stringBuilder.Append(string.Format(provider, " -q {0:0.0####}", new object[]
					{
						this.NERO_Quality
					}));
				}
				else if (this.NERO_UseCBR)
				{
					stringBuilder.Append(string.Format(provider, " -cbr {0}", new object[]
					{
						this.NERO_Bitrate * 1000
					}));
				}
				else
				{
					stringBuilder.Append(string.Format(provider, " -br {0}", new object[]
					{
						this.NERO_Bitrate * 1000
					}));
				}
				if (base.InputFile != null && this.NERO_2Pass)
				{
					stringBuilder.Append(" -2pass");
					if (this.NERO_2PassPeriod > 0)
					{
						stringBuilder.Append(string.Format(provider, " -2passperiod {0}", new object[]
						{
							this.NERO_2PassPeriod
						}));
					}
				}
				if (this.NERO_LC)
				{
					stringBuilder.Append(" -lc");
				}
				if (this.NERO_HE)
				{
					stringBuilder.Append(" -he");
				}
				if (this.NERO_HEv2)
				{
					stringBuilder.Append(" -hev2");
				}
				if (this.NERO_HintTrack)
				{
					stringBuilder.Append(" -hinttrack");
				}
				if (base.ChannelInfo.ctype == BASSChannelType.BASS_CTYPE_RECORD)
				{
					stringBuilder.Append(" -ignorelength");
				}
				if (this.NERO_CustomOptions != null && this.NERO_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.NERO_CustomOptions.Trim());
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
				stringBuilder.Append(" -of \"" + Path.ChangeExtension(base.InputFile, this.DefaultOutputExtension) + "\"");
			}
			return stringBuilder.ToString();
		}

		public bool NERO_UseSSE;

		public bool NERO_UseCustomOptionsOnly;

		public string NERO_CustomOptions = string.Empty;

		public bool NERO_UseQualityMode;

		public float NERO_Quality = 0.4f;

		public int NERO_Bitrate = 128;

		public bool NERO_UseCBR;

		public bool NERO_2Pass;

		public int NERO_2PassPeriod;

		public bool NERO_LC;

		public bool NERO_HE;

		public bool NERO_HEv2;

		public bool NERO_HintTrack;
	}
}
