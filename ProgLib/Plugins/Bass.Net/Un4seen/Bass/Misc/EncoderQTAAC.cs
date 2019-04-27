using System;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Win32;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderQTAAC : BaseEncoder
	{
		public EncoderQTAAC(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "qtaacenc.exe"));
			}
		}

		public override string ToString()
		{
			return "QuickTime AAC Encoder (.m4a)";
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
				return this.QT_Bitrate;
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
			return string.Format("{0}-{1} kbps, {2}", this.QT_Mode, this.EffectiveBitrate, this.QT_HE ? "HE" : "LC");
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "qtaacenc.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("qtaacenc.exe");
			}
			stringBuilder.Append(" --quiet");
			if (this.QT_UseCustomOptionsOnly)
			{
				if (this.QT_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.QT_CustomOptions.Trim());
				}
			}
			else
			{
				switch (this.QT_Mode)
				{
				case EncoderQTAAC.QTMode.CBR:
					stringBuilder.Append(string.Format(provider, " --cbr {0}", new object[]
					{
						this.QT_Bitrate
					}));
					break;
				case EncoderQTAAC.QTMode.ABR:
					stringBuilder.Append(string.Format(provider, " --abr {0}", new object[]
					{
						this.QT_Bitrate
					}));
					break;
				case EncoderQTAAC.QTMode.CVBR:
					stringBuilder.Append(string.Format(provider, " --cvbr {0}", new object[]
					{
						this.QT_Bitrate
					}));
					break;
				case EncoderQTAAC.QTMode.TVBR:
					stringBuilder.Append(string.Format(provider, " --tvbr {0}", new object[]
					{
						this.QT_Quality
					}));
					break;
				}
				if (this.QT_Samplerate < 0)
				{
					stringBuilder.Append(" --samplerate auto");
				}
				else if (this.QT_Samplerate == 0)
				{
					stringBuilder.Append(" --samplerate keep");
				}
				else
				{
					int qt_Samplerate = this.QT_Samplerate;
					if (qt_Samplerate <= 16000)
					{
						if (qt_Samplerate <= 11250)
						{
							if (qt_Samplerate == 8000)
							{
								stringBuilder.Append(" --samplerate 8000");
								goto IL_28A;
							}
							if (qt_Samplerate == 11250)
							{
								stringBuilder.Append(" --samplerate 11250");
								goto IL_28A;
							}
						}
						else
						{
							if (qt_Samplerate == 12000)
							{
								stringBuilder.Append(" --samplerate 12000");
								goto IL_28A;
							}
							if (qt_Samplerate == 16000)
							{
								stringBuilder.Append(" --samplerate 16000");
								goto IL_28A;
							}
						}
					}
					else if (qt_Samplerate <= 24000)
					{
						if (qt_Samplerate == 22050)
						{
							stringBuilder.Append(" --samplerate 22050");
							goto IL_28A;
						}
						if (qt_Samplerate == 24000)
						{
							stringBuilder.Append(" --samplerate 24000");
							goto IL_28A;
						}
					}
					else
					{
						if (qt_Samplerate == 32000)
						{
							stringBuilder.Append(" --samplerate 32000");
							goto IL_28A;
						}
						if (qt_Samplerate == 44100)
						{
							stringBuilder.Append(" --samplerate 44100");
							goto IL_28A;
						}
						if (qt_Samplerate == 48000)
						{
							stringBuilder.Append(" --samplerate 48000");
							goto IL_28A;
						}
					}
					stringBuilder.Append(" --samplerate auto");
				}
				IL_28A:
				switch (this.QT_QualityMode)
				{
				case EncoderQTAAC.QTQuality.Fastest:
					stringBuilder.Append(" --fastest");
					break;
				case EncoderQTAAC.QTQuality.Fast:
					stringBuilder.Append(" --fast");
					break;
				case EncoderQTAAC.QTQuality.Normal:
					stringBuilder.Append(" --normal");
					break;
				case EncoderQTAAC.QTQuality.High:
					stringBuilder.Append(" --high");
					break;
				case EncoderQTAAC.QTQuality.Highest:
					stringBuilder.Append(" --highest");
					break;
				default:
					stringBuilder.Append(" --high");
					break;
				}
				if (this.QT_HE && this.QT_Mode != EncoderQTAAC.QTMode.TVBR)
				{
					stringBuilder.Append(" --he");
				}
				if (base.TAGs != null)
				{
					if (!string.IsNullOrEmpty(base.TAGs.title))
					{
						stringBuilder.Append(" --title \"" + base.TAGs.title.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.artist))
					{
						stringBuilder.Append(" --artist \"" + base.TAGs.artist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.album))
					{
						stringBuilder.Append(" --album \"" + base.TAGs.album.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.albumartist))
					{
						stringBuilder.Append(" --albumartist \"" + base.TAGs.albumartist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.genre))
					{
						stringBuilder.Append(" --genre \"" + base.TAGs.genre.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.year))
					{
						stringBuilder.Append(" --date \"" + base.TAGs.year.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.composer))
					{
						stringBuilder.Append(" --composer \"" + base.TAGs.composer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.grouping))
					{
						stringBuilder.Append(" --grouping \"" + base.TAGs.grouping.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.comment))
					{
						stringBuilder.Append(" --comment \"" + base.TAGs.comment.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.track))
					{
						stringBuilder.Append(" --track \"" + base.TAGs.track.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.disc))
					{
						stringBuilder.Append(" --disc \"" + base.TAGs.disc.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
				}
				if (this.QT_CustomOptions != null && this.QT_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.QT_CustomOptions.Trim());
				}
			}
			if (base.InputFile != null)
			{
				stringBuilder.Append(" \"" + base.InputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" --ignorelength -");
			}
			if (base.OutputFile != null)
			{
				stringBuilder.Append(" \"" + base.OutputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" \"" + Path.ChangeExtension(base.InputFile, this.DefaultOutputExtension) + "\"");
			}
			return stringBuilder.ToString();
		}

		public static bool IsQuickTimeInstalled()
		{
			bool flag = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Apple Computer, Inc.\\QuickTime"))
				{
					if (registryKey != null)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Apple Computer, Inc.\\QuickTime"))
					{
						if (registryKey2 != null)
						{
							flag = true;
						}
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
					using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
					{
						foreach (string name in registryKey3.GetSubKeyNames())
						{
							try
							{
								using (RegistryKey registryKey4 = registryKey3.OpenSubKey(name))
								{
									if (registryKey4 != null && registryKey4.GetValue("DisplayName") as string == "QuickTime")
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

		public bool QT_UseCustomOptionsOnly;

		public string QT_CustomOptions = string.Empty;

		public EncoderQTAAC.QTQuality QT_QualityMode = EncoderQTAAC.QTQuality.High;

		public EncoderQTAAC.QTMode QT_Mode = EncoderQTAAC.QTMode.TVBR;

		public int QT_Bitrate = 128;

		public int QT_Quality = 65;

		public bool QT_HE;

		public int QT_Samplerate;

		public enum QTMode
		{
			CBR,
			ABR,
			CVBR,
			TVBR
		}

		public enum QTQuality
		{
			Fastest,
			Fast,
			Normal,
			High,
			Highest
		}
	}
}
