using System;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public class EncoderMP3 : EncoderCMDLN
	{
		public EncoderMP3(int channel) : base(channel)
		{
			this.CMDLN_UseNOHEAD = true;
			this.CMDLN_UseFP_32BIT = false;
			this.CMDLN_UseFP_24BIT = true;
			this.CMDLN_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_MP3;
			this.CMDLN_DefaultOutputExtension = ".mp3";
			this.CMDLN_SupportsSTDOUT = true;
			this.CMDLN_Option = "1";
			this.CMDLN_Quality = "2";
			this.CMDLN_UseA = true;
			this.CMDLN_UserA = "";
			this.CMDLN_UserB = "-mono";
			this.CMDLN_Executable = "mp3sEncoder.exe";
			this.CMDLN_CBRString = "${user} -raw -sr ${Hz} -c ${chan} -res ${res} -q ${option} -br ${bps} -if ${input} -of ${output}";
			this.CMDLN_VBRString = "${user} -raw -sr ${Hz} -c ${chan} -res ${res} -q ${option} -m ${quality} -if ${input} -of ${output}";
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

		public override string ToString()
		{
			return "Generic MP3 Encoder (" + this.DefaultOutputExtension + ")";
		}
	}
}
