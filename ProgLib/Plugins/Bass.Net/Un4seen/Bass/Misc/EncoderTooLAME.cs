using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderTooLAME : BaseEncoder
	{
		public EncoderTooLAME(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "tooLAME.exe"));
			}
		}

		public override string ToString()
		{
			return "tooLAME Encoder (.mp2)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return BASSChannelType.BASS_CTYPE_STREAM_MP2;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return ".mp2";
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
				return this.TOO_Bitrate;
			}
		}

		public override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			BASSEncode bassencode = BASSEncode.BASS_ENCODE_NOHEAD | BASSEncode.BASS_ENCODE_FP_16BIT;
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

		public new bool Force16Bit
		{
			get
			{
				return true;
			}
		}

		public override string SettingsString()
		{
			return string.Format("{0}-{1} kbps, {2} {3} {4}", new object[]
			{
				this.TOO_UseVBR ? "VBR" : "CBR",
				this.EffectiveBitrate,
				this.TOO_Downmix ? "Mono" : "Stereo",
				this.TOO_Mode,
				this.TOO_PsycModel
			}).Trim();
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "tooLAME.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("tooLAME.exe");
			}
			stringBuilder.Append(string.Format(provider, " -s {0:#0.0##}", new object[]
			{
				(float)base.ChannelSampleRate / 1000f
			}));
			if (this.TOO_UseCustomOptionsOnly)
			{
				if (this.TOO_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.TOO_CustomOptions.Trim());
				}
			}
			else
			{
				if (this.TOO_Bitrate > 192)
				{
					this.TOO_Mode = EncoderTooLAME.TOOMode.Stereo;
				}
				if (base.ChannelNumChans == 1)
				{
					this.TOO_Mode = EncoderTooLAME.TOOMode.Mono;
				}
				if (base.ChannelNumChans > 1 && this.TOO_Mode == EncoderTooLAME.TOOMode.Mono)
				{
					stringBuilder.Append(" -m s -a");
				}
				else
				{
					switch (this.TOO_Mode)
					{
					case EncoderTooLAME.TOOMode.Stereo:
						stringBuilder.Append(" -m s");
						break;
					case EncoderTooLAME.TOOMode.JointStereo:
						stringBuilder.Append(" -m j");
						break;
					case EncoderTooLAME.TOOMode.DualChannel:
						stringBuilder.Append(" -m d");
						break;
					case EncoderTooLAME.TOOMode.Mono:
						stringBuilder.Append(" -m m");
						break;
					}
				}
				stringBuilder.Append(string.Format(provider, " -b {0}", new object[]
				{
					this.TOO_Bitrate
				}));
				if (this.TOO_PsycModel != EncoderTooLAME.TOOPsycModel.Default)
				{
					stringBuilder.Append(string.Format(provider, " -P {0}", new object[]
					{
						(int)this.TOO_PsycModel
					}));
				}
				if (this.TOO_UseVBR)
				{
					stringBuilder.Append(string.Format(provider, " -v {0:#0.0####}", new object[]
					{
						this.TOO_VBRLevel
					}));
				}
				if (this.TOO_ATH != 0f)
				{
					stringBuilder.Append(string.Format(provider, " -l {0:#0.0####}", new object[]
					{
						this.TOO_ATH
					}));
				}
				if (this.TOO_Quick > 0)
				{
					stringBuilder.Append(string.Format(provider, " -q {0}", new object[]
					{
						this.TOO_Quick
					}));
				}
				EncoderTooLAME.TOODeEmphMode too_DeEmphasis = this.TOO_DeEmphasis;
				if (too_DeEmphasis != EncoderTooLAME.TOODeEmphMode.CCIT_J17)
				{
					if (too_DeEmphasis == EncoderTooLAME.TOODeEmphMode.Five)
					{
						stringBuilder.Append(" -e 5");
					}
				}
				else
				{
					stringBuilder.Append(" -e c");
				}
				if (this.TOO_Copyright)
				{
					stringBuilder.Append(" -c");
				}
				if (this.TOO_Original)
				{
					stringBuilder.Append(" -o");
				}
				if (this.TOO_Protect)
				{
					stringBuilder.Append(" -e");
				}
				if (this.TOO_Padding)
				{
					stringBuilder.Append(" -r");
				}
				if (this.TOO_DABExtension > 0)
				{
					stringBuilder.Append(string.Format(provider, " -D {0", new object[]
					{
						this.TOO_DABExtension
					}));
				}
				if (this.TOO_CustomOptions != null && this.TOO_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.TOO_CustomOptions.Trim());
				}
			}
			if (base.OutputFile == null)
			{
				stringBuilder.Append(" -t 0");
			}
			if (base.InputFile != null)
			{
				stringBuilder.Append(" \"" + base.InputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" -");
			}
			if (base.OutputFile != null)
			{
				stringBuilder.Append(" \"" + base.OutputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" -");
			}
			return stringBuilder.ToString();
		}

		public bool TOO_UseCustomOptionsOnly;

		public string TOO_CustomOptions = string.Empty;

		public EncoderTooLAME.TOOMode TOO_Mode;

		public bool TOO_Downmix;

		public int TOO_Bitrate = 192;

		public bool TOO_UseVBR;

		public float TOO_VBRLevel = 5f;

		public EncoderTooLAME.TOOPsycModel TOO_PsycModel = EncoderTooLAME.TOOPsycModel.Default;

		public float TOO_ATH;

		public int TOO_Quick;

		public bool TOO_Copyright;

		public bool TOO_Original;

		public bool TOO_Protect;

		public bool TOO_Padding;

		public EncoderTooLAME.TOODeEmphMode TOO_DeEmphasis;

		public int TOO_DABExtension;

		public enum TOOMode
		{
			Auto,
			Stereo,
			JointStereo,
			DualChannel,
			Mono
		}

		public enum TOOPsycModel
		{
			Default = -2,
			None,
			Fixed,
			ISO_PAM1,
			ISO_PAM2,
			PAM3,
			PAM4
		}

		public enum TOODeEmphMode
		{
			None,
			CCIT_J17,
			Five
		}
	}
}
