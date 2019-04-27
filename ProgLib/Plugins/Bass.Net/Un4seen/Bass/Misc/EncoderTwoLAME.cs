using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderTwoLAME : BaseEncoder
	{
		public EncoderTwoLAME(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "twolame.exe"));
			}
		}

		public override string ToString()
		{
			return "TwoLAME Encoder (.mp2)";
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
				return this.TWO_Bitrate;
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
				this.TWO_UseVBR ? "VBR" : "CBR",
				this.EffectiveBitrate,
				this.TWO_Downmix ? "Mono" : "Stereo",
				this.TWO_Mode,
				this.TWO_PsycModel
			}).Trim();
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "twolame.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("twolame.exe");
			}
			if (base.ChannelBitwidth > 24)
			{
				stringBuilder.Append(string.Format(provider, " -r -s {0} --samplefloat -N {1}", new object[]
				{
					base.ChannelSampleRate,
					base.ChannelNumChans
				}));
			}
			else
			{
				stringBuilder.Append(string.Format(provider, " -r -s {0} --samplesize {1} -N {2}", new object[]
				{
					base.ChannelSampleRate,
					(base.ChannelBitwidth > 16) ? (base.Force16Bit ? 16 : 32) : base.ChannelBitwidth,
					base.ChannelNumChans
				}));
			}
			if (this.TWO_UseCustomOptionsOnly)
			{
				if (this.TWO_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.TWO_CustomOptions.Trim());
				}
			}
			else
			{
				if (this.TWO_Scale != 1f)
				{
					stringBuilder.Append(string.Format(provider, " --scale {0:0.0####}", new object[]
					{
						this.TWO_Scale
					}));
				}
				if (base.ChannelNumChans == 1)
				{
					this.TWO_Mode = EncoderTwoLAME.TWOMode.Mono;
				}
				if (base.ChannelNumChans > 1 && this.TWO_Mode == EncoderTwoLAME.TWOMode.Mono)
				{
					stringBuilder.Append(" -m s -a");
				}
				else
				{
					switch (this.TWO_Mode)
					{
					case EncoderTwoLAME.TWOMode.Stereo:
						stringBuilder.Append(" -m s");
						break;
					case EncoderTwoLAME.TWOMode.JointStereo:
						stringBuilder.Append(" -m j");
						break;
					case EncoderTwoLAME.TWOMode.DualChannel:
						stringBuilder.Append(" -m d");
						break;
					case EncoderTwoLAME.TWOMode.Mono:
						stringBuilder.Append(" -m m");
						break;
					}
				}
				stringBuilder.Append(string.Format(provider, " -b {0}", new object[]
				{
					this.TWO_Bitrate
				}));
				if (this.TWO_PsycModel != EncoderTwoLAME.TWOPsycModel.Default)
				{
					stringBuilder.Append(string.Format(provider, " -P {0}", new object[]
					{
						(int)this.TWO_PsycModel
					}));
				}
				if (this.TWO_UseVBR)
				{
					stringBuilder.Append(" -v");
					if (this.TWO_VBRLevel >= -50f && this.TWO_VBRLevel <= 50f)
					{
						stringBuilder.Append(string.Format(provider, " -V {0:#0.0####}", new object[]
						{
							this.TWO_VBRLevel
						}));
					}
					if (this.TWO_MaxBitrate > 0)
					{
						stringBuilder.Append(string.Format(provider, " -B {0}", new object[]
						{
							this.TWO_MaxBitrate
						}));
					}
				}
				if (this.TWO_ATH != 0f)
				{
					stringBuilder.Append(string.Format(provider, " -l {0:#0.0####}", new object[]
					{
						this.TWO_ATH
					}));
				}
				if (this.TWO_Quick > 0)
				{
					stringBuilder.Append(string.Format(provider, " -q {0}", new object[]
					{
						this.TWO_Quick
					}));
				}
				if (this.TWO_Copyright)
				{
					stringBuilder.Append(" -c");
				}
				if (this.TWO_NonOriginal)
				{
					stringBuilder.Append(" -o");
				}
				if (this.TWO_Protect)
				{
					stringBuilder.Append(" -p");
				}
				if (this.TWO_Padding)
				{
					stringBuilder.Append(" -d");
				}
				if (this.TWO_Reserve > 0)
				{
					stringBuilder.Append(string.Format(provider, " -R {0}", new object[]
					{
						this.TWO_Reserve / 8 * 8
					}));
				}
				EncoderTwoLAME.TWODeEmphMode two_DeEmphasis = this.TWO_DeEmphasis;
				if (two_DeEmphasis != EncoderTwoLAME.TWODeEmphMode.CCIT_J17)
				{
					if (two_DeEmphasis == EncoderTwoLAME.TWODeEmphMode.Five)
					{
						stringBuilder.Append(" -e 5");
					}
				}
				else
				{
					stringBuilder.Append(" -e c");
				}
				if (this.TWO_Energy)
				{
					stringBuilder.Append(" -E");
				}
				if (this.TWO_CustomOptions != null && this.TWO_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.TWO_CustomOptions.Trim());
				}
			}
			if (base.OutputFile == null)
			{
				stringBuilder.Append(" --quiet");
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

		public bool TWO_UseCustomOptionsOnly;

		public string TWO_CustomOptions = string.Empty;

		public EncoderTwoLAME.TWOMode TWO_Mode;

		public float TWO_Scale = 1f;

		public bool TWO_Downmix;

		public int TWO_Bitrate = 256;

		public int TWO_MaxBitrate;

		public bool TWO_UseVBR;

		public float TWO_VBRLevel = -100f;

		public EncoderTwoLAME.TWOPsycModel TWO_PsycModel = EncoderTwoLAME.TWOPsycModel.Default;

		public float TWO_ATH;

		public int TWO_Quick;

		public bool TWO_Copyright;

		public bool TWO_NonOriginal;

		public bool TWO_Protect;

		public bool TWO_Padding;

		public int TWO_Reserve;

		public EncoderTwoLAME.TWODeEmphMode TWO_DeEmphasis;

		public bool TWO_Energy;

		public enum TWOMode
		{
			Auto,
			Stereo,
			JointStereo,
			DualChannel,
			Mono
		}

		public enum TWOPsycModel
		{
			Default = -2,
			None,
			Fixed,
			ISO_PAM1,
			ISO_PAM2,
			PAM3,
			PAM4
		}

		public enum TWODeEmphMode
		{
			None,
			CCIT_J17,
			Five
		}
	}
}
