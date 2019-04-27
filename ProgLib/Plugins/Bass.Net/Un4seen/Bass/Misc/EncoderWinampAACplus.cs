using System;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Win32;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderWinampAACplus : BaseEncoder
	{
		public EncoderWinampAACplus(int channel) : base(channel)
		{
			if (base.ChannelBitwidth < 16)
			{
				throw new ArgumentException("8-bit channels are not supported by the Winamp AACPlus encoder!");
			}
		}

		public override bool EncoderExists
		{
			get
			{
				if (this.AACPlus_UseMp4Output)
				{
					return File.Exists(Path.Combine(base.EncoderDirectory, "enc_aacPlus.exe")) && File.Exists(Path.Combine(base.EncoderDirectory, "enc_aacPlus.dll")) && File.Exists(Path.Combine(base.EncoderDirectory, "libmp4v2.dll"));
				}
				return File.Exists(Path.Combine(base.EncoderDirectory, "enc_aacPlus.exe")) && File.Exists(Path.Combine(base.EncoderDirectory, "enc_aacPlus.dll"));
			}
		}

		public override string ToString()
		{
			return "Winamp AACplus v2 Encoder (" + this.DefaultOutputExtension + ")";
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
				if (this.AACPlus_UseMp4Output)
				{
					return ".m4a";
				}
				return ".aac";
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
				return this.AACPlus_Bitrate;
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
			return string.Format("CBR-{0} kbps, {1} {2}", this.EffectiveBitrate, this.AACPlus_High ? "HE-AAC+" : (this.AACPlus_LC ? "LC-AAC" : "HE-AAC"), this.AACPlus_EnableParametricStereo ? "Parametric" : (this.AACPlus_IndependedStereo ? "Independent" : (this.AACPlus_Mono ? "Mono" : "Stereo")));
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "enc_aacPlus.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("enc_aacPlus.exe");
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
			stringBuilder.Append(string.Format(provider, " --br {0}", new object[]
			{
				this.AACPlus_Bitrate * 1000
			}));
			if (this.AACPlus_EnableParametricStereo)
			{
				stringBuilder.Append(" --ps");
			}
			if (this.AACPlus_IndependedStereo)
			{
				stringBuilder.Append(" --is");
			}
			if (this.AACPlus_PreferDualChannel)
			{
				stringBuilder.Append(" --dc");
			}
			if (this.AACPlus_Mono)
			{
				stringBuilder.Append(" --mono");
			}
			if (this.AACPlus_Speech)
			{
				stringBuilder.Append(" --speech");
			}
			if (this.AACPlus_PNS)
			{
				stringBuilder.Append(" --pns");
			}
			if (this.AACPlus_Mpeg2Aac)
			{
				stringBuilder.Append(" --mpeg2aac");
			}
			if (this.AACPlus_Mpeg4Aac)
			{
				stringBuilder.Append(" --mpeg4aac");
			}
			if (this.AACPlus_LC)
			{
				stringBuilder.Append(" --lc");
			}
			if (this.AACPlus_HE && this.AACPlus_Bitrate <= 128)
			{
				stringBuilder.Append(" --he");
			}
			if (this.AACPlus_High && this.AACPlus_Bitrate <= 256 && base.ChannelNumChans <= 2)
			{
				stringBuilder.Append(" --high");
			}
			if (base.OutputFile == null)
			{
				stringBuilder.Append(" --silent");
			}
			if (base.InputFile == null)
			{
				stringBuilder.Append(string.Format(provider, " --rawpcm {0} {1} {2}", new object[]
				{
					base.ChannelSampleRate,
					base.ChannelNumChans,
					16
				}));
			}
			return stringBuilder.ToString();
		}

		public static bool IsWinampInstalled()
		{
			bool flag = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Nullsoft\\Winamp"))
				{
					if (registryKey != null)
					{
						flag = true;
					}
				}
			}
			catch
			{
			}
			if (!flag)
			{
				try
				{
					using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
					{
						foreach (string name in registryKey2.GetSubKeyNames())
						{
							try
							{
								using (RegistryKey registryKey3 = registryKey2.OpenSubKey(name))
								{
									if (registryKey3 != null && registryKey3.GetValue("DisplayName") as string == "Winamp")
									{
										flag = true;
										break;
									}
								}
							}
							catch
							{
							}
						}
					}
				}
				catch
				{
				}
			}
			return flag;
		}

		public int AACPlus_Bitrate = 128;

		public bool AACPlus_EnableParametricStereo;

		public bool AACPlus_IndependedStereo;

		public bool AACPlus_PreferDualChannel;

		public bool AACPlus_Mono;

		public bool AACPlus_Mpeg2Aac;

		public bool AACPlus_Mpeg4Aac;

		public bool AACPlus_LC;

		public bool AACPlus_HE;

		public bool AACPlus_High;

		public bool AACPlus_Speech;

		public bool AACPlus_PNS;

		public bool AACPlus_UseMp4Output;
	}
}
