using System;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Win32;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderFHGAACplus : BaseEncoder
	{
		public EncoderFHGAACplus(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				if (this.AACPlus_UseADTS)
				{
					return File.Exists(Path.Combine(base.EncoderDirectory, "fhgaacenc.exe")) && File.Exists(Path.Combine(base.EncoderDirectory, "libsndfile-1.dll")) && File.Exists(Path.Combine(base.EncoderDirectory, "nsutil.dll")) && File.Exists(Path.Combine(base.EncoderDirectory, "enc_fhgaac.dll"));
				}
				return File.Exists(Path.Combine(base.EncoderDirectory, "fhgaacenc.exe")) && File.Exists(Path.Combine(base.EncoderDirectory, "enc_fhgaac.dll")) && File.Exists(Path.Combine(base.EncoderDirectory, "libsndfile-1.dll")) && File.Exists(Path.Combine(base.EncoderDirectory, "nsutil.dll")) && File.Exists(Path.Combine(base.EncoderDirectory, "libmp4v2.dll"));
			}
		}

		public override string ToString()
		{
			return "FHG Winamp AACplus v2 Encoder (" + this.DefaultOutputExtension + ")";
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
				if (this.AACPlus_UseADTS)
				{
					return ".aac";
				}
				return ".m4a";
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
				return this.AACPlus_ConstantBitrate;
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
			else
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
			base.EncoderHandle = BassEnc.BASS_Encode_Start(base.ChannelHandle, this.EncoderCommandLine, bassencode, proc, user);
			return base.EncoderHandle != 0;
		}

		public override string SettingsString()
		{
			if (this.AACPlus_VariableBitrate > 0)
			{
				return string.Format("VBR-{0}, {1} {2}", this.AACPlus_VariableBitrate, this.AACPlus_Profile, this.AACPlus_UseADTS ? "ADTS" : "MPEG-4");
			}
			return string.Format("CBR-{0} kbps, {1} {2}", this.AACPlus_ConstantBitrate, this.AACPlus_Profile, this.AACPlus_UseADTS ? "ADTS" : "MPEG-4");
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "fhgaacenc.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("fhgaacenc.exe");
			}
			if (this.AACPlus_VariableBitrate > 0)
			{
				stringBuilder.Append(string.Format(provider, " --vbr {0}", new object[]
				{
					this.AACPlus_VariableBitrate
				}));
			}
			else
			{
				stringBuilder.Append(string.Format(provider, " --cbr {0}", new object[]
				{
					this.AACPlus_ConstantBitrate
				}));
				if (this.AACPlus_Profile != EncoderFHGAACplus.FHGProfile.Auto)
				{
					stringBuilder.Append(string.Format(provider, " --profile {0}", new object[]
					{
						this.AACPlus_Profile
					}));
				}
			}
			if (this.AACPlus_UseADTS || base.OutputFile == null)
			{
				stringBuilder.Append(" --adts");
			}
			if (base.InputFile == null)
			{
				stringBuilder.Append(" --ignorelength");
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

		public static bool IsWinampInstalled()
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Winamp"))
				{
					if (registryKey != null)
					{
						return true;
					}
				}
			}
			catch
			{
			}
			try
			{
				using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Nullsoft\\Winamp"))
				{
					if (registryKey2 != null)
					{
						return true;
					}
				}
			}
			catch
			{
			}
			try
			{
				using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Nullsoft\\Winamp"))
				{
					if (registryKey3 != null)
					{
						return true;
					}
				}
			}
			catch
			{
			}
			try
			{
				using (RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
				{
					foreach (string name in registryKey4.GetSubKeyNames())
					{
						try
						{
							using (RegistryKey registryKey5 = registryKey4.OpenSubKey(name))
							{
								if (registryKey5 != null && registryKey5.GetValue("DisplayName") as string == "Winamp")
								{
									return true;
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
			return false;
		}

		public int AACPlus_ConstantBitrate = 128;

		public int AACPlus_VariableBitrate;

		public EncoderFHGAACplus.FHGProfile AACPlus_Profile;

		public bool AACPlus_UseADTS;

		public enum FHGProfile
		{
			Auto,
			LC,
			HE,
			HEv2
		}
	}
}
