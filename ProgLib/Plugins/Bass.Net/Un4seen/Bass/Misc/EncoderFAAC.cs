using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderFAAC : BaseEncoder
	{
		public EncoderFAAC(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "faac.exe"));
			}
		}

		public override string ToString()
		{
			return "FAAC Encoder (.m4a)";
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
				if (this.FAAC_UseQualityMode)
				{
					return this.Quality2Kbps(this.FAAC_Quality);
				}
				return this.FAAC_Bitrate;
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
			return string.Format("{0}-{1} kbps, {2} {3}", new object[]
			{
				this.FAAC_UseQualityMode ? "VBR" : "CBR",
				this.EffectiveBitrate,
				this.FAAC_ObjectType,
				this.FAAC_EnableTNS ? "TNS" : ""
			}).Trim();
		}

		public int Quality2Kbps(int q)
		{
			return (int)Math.Round((double)q * (1.2 * Math.Cos((double)q)) + 10.0);
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "faac.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("faac.exe");
			}
			stringBuilder.Append(string.Format(provider, " -P -R {0} {1} -C {2}", new object[]
			{
				base.ChannelSampleRate,
				(base.ChannelBitwidth > 16) ? (base.Force16Bit ? "-B 16 -X" : "-F -B 32") : ("-B " + base.ChannelBitwidth.ToString() + " -X"),
				base.ChannelNumChans
			}));
			if (this.FAAC_UseCustomOptionsOnly)
			{
				if (this.FAAC_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.FAAC_CustomOptions.Trim());
				}
			}
			else
			{
				if (this.FAAC_UseQualityMode)
				{
					stringBuilder.Append(string.Format(" -q {0}", this.FAAC_Quality));
				}
				else
				{
					stringBuilder.Append(string.Format(" -b {0}", this.FAAC_Bitrate));
				}
				if (this.FAAC_Bandwidth > 0)
				{
					stringBuilder.Append(string.Format(" -c {0}", this.FAAC_Bandwidth));
				}
				if (this.FAAC_WrapMP4)
				{
					stringBuilder.Append(" -w");
				}
				if (this.FAAC_WrapMP4 && base.TAGs != null)
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
					if (!string.IsNullOrEmpty(base.TAGs.year))
					{
						stringBuilder.Append(" --year \"" + base.TAGs.year.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.track))
					{
						stringBuilder.Append(" --track \"" + base.TAGs.track.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.disc))
					{
						stringBuilder.Append(" --disc \"" + base.TAGs.disc.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.genre))
					{
						stringBuilder.Append(" --genre \"" + base.TAGs.genre.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.comment))
					{
						stringBuilder.Append(" --comment \"" + base.TAGs.comment.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.composer))
					{
						stringBuilder.Append(" --writer \"" + base.TAGs.composer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
				}
				if (this.FAAC_EnableTNS)
				{
					stringBuilder.Append(" --tns");
				}
				if (this.FAAC_NoMidSide)
				{
					stringBuilder.Append(" --no-midside");
				}
				if (this.FAAC_MpegVersion == 2 || this.FAAC_MpegVersion == 4)
				{
					stringBuilder.Append(string.Format(" --mpeg-vers {0}", this.FAAC_MpegVersion));
				}
				if (this.FAAC_ObjectType == "Main" || this.FAAC_ObjectType == "LTP")
				{
					stringBuilder.Append(string.Format(" --obj-type {0}", this.FAAC_ObjectType));
				}
				if (this.FAAC_BlockType == 1 || this.FAAC_BlockType == 2)
				{
					stringBuilder.Append(string.Format(" --shortctl {0}", this.FAAC_BlockType));
				}
				if (this.FAAC_RawBitstream)
				{
					stringBuilder.Append(" -r");
				}
				if (this.FAAC_CustomOptions != null && this.FAAC_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.FAAC_CustomOptions.Trim());
				}
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
				stringBuilder.Append(" -o \"" + base.OutputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" -o -");
			}
			return stringBuilder.ToString();
		}

		public bool FAAC_UseCustomOptionsOnly;

		public string FAAC_CustomOptions = string.Empty;

		public bool FAAC_UseQualityMode;

		public int FAAC_Quality = 100;

		public int FAAC_Bitrate = 120;

		public int FAAC_Bandwidth = -1;

		public bool FAAC_WrapMP4 = true;

		public bool FAAC_EnableTNS;

		public bool FAAC_NoMidSide;

		public int FAAC_MpegVersion = -1;

		public string FAAC_ObjectType = "LC";

		public int FAAC_BlockType;

		public bool FAAC_RawBitstream;
	}
}
