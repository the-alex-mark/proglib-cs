using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public class EncoderCMDLN : BaseEncoder
	{
		public EncoderCMDLN(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, this.CMDLN_Executable));
			}
		}

		public override string ToString()
		{
			return "Generic Command-Line Encoder (" + this.DefaultOutputExtension + ")";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return this.CMDLN_EncoderType;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return this.CMDLN_DefaultOutputExtension;
			}
		}

		public override bool SupportsSTDOUT
		{
			get
			{
				return this.CMDLN_SupportsSTDOUT;
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
				return this.CMDLN_Bitrate;
			}
		}

		public override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			BASSEncode bassencode = this.CMDLN_UseNOHEAD ? BASSEncode.BASS_ENCODE_NOHEAD : BASSEncode.BASS_ENCODE_DEFAULT;
			if (base.Force16Bit)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_16BIT;
			}
			else if (this.CMDLN_UseFP_32BIT)
			{
				bassencode |= BASSEncode.BASS_ENCODE_FP_32BIT;
			}
			else if (this.CMDLN_UseFP_24BIT)
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
				this.CMDLN_UseVBR ? "VBR" : "CBR",
				this.EffectiveBitrate,
				this.CMDLN_Option,
				this.CMDLN_Quality,
				this.CMDLN_Mode
			}).Trim();
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, this.CMDLN_Executable) + "\"");
			}
			else
			{
				stringBuilder.Append(this.CMDLN_Executable);
			}
			string text = this.CMDLN_CBRString;
			if (this.CMDLN_UseVBR)
			{
				text = this.CMDLN_VBRString;
			}
			text = text.Replace("${user}", this.CMDLN_UseA ? this.CMDLN_UserA : this.CMDLN_UserB).Replace("${Hz}", base.ChannelSampleRate.ToString(provider)).Replace("${kHz}", ((float)base.ChannelSampleRate / 1000f).ToString("##0.0##", provider)).Replace("${res}", ((base.ChannelBitwidth > 16) ? (base.Force16Bit ? 16 : (this.CMDLN_UseFP_24BIT ? 24 : 32)) : base.ChannelBitwidth).ToString(provider)).Replace("${chan}", base.ChannelNumChans.ToString(provider)).Replace("${mode}", this.CMDLN_Mode).Replace("${quality}", this.CMDLN_Quality).Replace("${option}", this.CMDLN_Option).Replace("${kbps}", this.CMDLN_Bitrate.ToString(provider)).Replace("${bps}", (this.CMDLN_Bitrate * 1000).ToString(provider)).Replace("${input}", (base.InputFile == null) ? this.CMDLN_ParamSTDIN : ("\"" + base.InputFile + "\"")).Replace("${output}", (base.OutputFile == null) ? this.CMDLN_ParamSTDOUT : ("\"" + base.OutputFile + "\"")).Trim();
			stringBuilder.Append(" ");
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		public bool CMDLN_UseNOHEAD = true;

		public bool CMDLN_UseFP_32BIT;

		public bool CMDLN_UseFP_24BIT;

		public string CMDLN_Executable = string.Empty;

		public string CMDLN_ParamSTDOUT = "-";

		public string CMDLN_ParamSTDIN = "-";

		public BASSChannelType CMDLN_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_WAV;

		public string CMDLN_DefaultOutputExtension = ".wav";

		public bool CMDLN_SupportsSTDOUT;

		public bool CMDLN_UseVBR;

		public string CMDLN_CBRString = string.Empty;

		public string CMDLN_VBRString = string.Empty;

		public int CMDLN_Bitrate = 128;

		public string CMDLN_Quality = string.Empty;

		public string CMDLN_Mode = string.Empty;

		public string CMDLN_Option = string.Empty;

		public bool CMDLN_UseA = true;

		public string CMDLN_UserA = string.Empty;

		public string CMDLN_UserB = string.Empty;
	}
}
