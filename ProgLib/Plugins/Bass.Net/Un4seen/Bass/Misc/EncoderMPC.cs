using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderMPC : BaseEncoder
	{
		public EncoderMPC(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return this.MPC_UseVersion7 ? File.Exists(Path.Combine(base.EncoderDirectory, "mppenc.exe")) : File.Exists(Path.Combine(base.EncoderDirectory, "mpcenc.exe"));
			}
		}

		public override string ToString()
		{
			return "MusePack Encoder (.mpc)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return BASSChannelType.BASS_CTYPE_STREAM_MPC;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return ".mpc";
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
				int result = 180;
				if (this.MPC_Quality != 5)
				{
					switch (this.MPC_Quality)
					{
					case 0:
						result = 20;
						break;
					case 1:
						result = 30;
						break;
					case 2:
						result = 60;
						break;
					case 3:
						result = 90;
						break;
					case 4:
						result = 130;
						break;
					case 5:
						result = 180;
						break;
					case 6:
						result = 210;
						break;
					case 7:
						result = 240;
						break;
					case 8:
						result = 270;
						break;
					case 9:
						result = 300;
						break;
					case 10:
						result = 350;
						break;
					default:
						result = 180;
						break;
					}
				}
				else if (this.MPC_Preset != EncoderMPC.MPCPreset.standard)
				{
					switch (this.MPC_Preset)
					{
					case EncoderMPC.MPCPreset.telephone:
						return 60;
					case EncoderMPC.MPCPreset.thumb:
						return 90;
					case EncoderMPC.MPCPreset.radio:
						return 130;
					case EncoderMPC.MPCPreset.xtreme:
						return 210;
					case EncoderMPC.MPCPreset.insane:
						return 240;
					case EncoderMPC.MPCPreset.braindead:
						return 270;
					}
					result = 180;
				}
				return result;
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
				bassencode = BASSEncode.BASS_ENCODE_FP_32BIT;
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
			base.EncoderHandle = BassEnc.BASS_Encode_Start(base.ChannelHandle, this.EncoderCommandLine, bassencode, null, IntPtr.Zero);
			return base.EncoderHandle != 0;
		}

		public override string SettingsString()
		{
			return string.Format("{0} kbps, Quality {1}", this.EffectiveBitrate, this.MPC_Quality);
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (this.MPC_UseVersion7)
			{
				if (!string.IsNullOrEmpty(base.EncoderDirectory))
				{
					stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "mppenc.exe") + "\" --overwrite");
				}
				else
				{
					stringBuilder.Append("mppenc.exe --overwrite");
				}
			}
			else if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "mpcenc.exe") + "\" --overwrite");
			}
			else
			{
				stringBuilder.Append("mpcenc.exe --overwrite");
			}
			if (this.MPC_UseCustomOptionsOnly)
			{
				if (this.MPC_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.MPC_CustomOptions.Trim());
				}
			}
			else
			{
				if (this.MPC_Preset != EncoderMPC.MPCPreset.standard)
				{
					stringBuilder.Append(" --" + this.MPC_Preset.ToString());
				}
				if (this.MPC_Scale != 1f)
				{
					stringBuilder.Append(string.Format(provider, " --scale {0:0.0####}", new object[]
					{
						this.MPC_Scale
					}));
				}
				if (this.MPC_Quality != 5)
				{
					stringBuilder.Append(string.Format(provider, " --quality {0}", new object[]
					{
						this.MPC_Quality
					}));
				}
				if (this.MPC_NMT != 6.5f)
				{
					stringBuilder.Append(string.Format(provider, " --nmt {0:0.0####}", new object[]
					{
						this.MPC_NMT
					}));
				}
				if (this.MPC_TMN != 18f)
				{
					stringBuilder.Append(string.Format(provider, " --tmn {0:0.0####}", new object[]
					{
						this.MPC_TMN
					}));
				}
				if (this.MPC_PNS != 0f)
				{
					stringBuilder.Append(string.Format(provider, " --pns {0:0.0####}", new object[]
					{
						this.MPC_PNS
					}));
				}
				if (base.TAGs != null)
				{
					if (!string.IsNullOrEmpty(base.TAGs.title))
					{
						stringBuilder.Append(" --tag \"Title=" + base.TAGs.title.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.artist))
					{
						stringBuilder.Append(" --tag \"Artist=" + base.TAGs.artist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.album))
					{
						stringBuilder.Append(" --tag \"Album=" + base.TAGs.album.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.albumartist))
					{
						stringBuilder.Append(" --tag \"Album Artist=" + base.TAGs.albumartist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.year))
					{
						stringBuilder.Append(" --tag \"Year=" + base.TAGs.year.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.track))
					{
						stringBuilder.Append(" --tag \"Track=" + base.TAGs.track.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.disc))
					{
						stringBuilder.Append(" --tag \"Disc=" + base.TAGs.disc.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.genre))
					{
						stringBuilder.Append(" --tag \"Genre=" + base.TAGs.genre.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.comment))
					{
						stringBuilder.Append(" --tag \"Comment=" + base.TAGs.comment.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.composer))
					{
						stringBuilder.Append(" --tag \"Composer=" + base.TAGs.composer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.conductor))
					{
						stringBuilder.Append(" --tag \"Conductor=" + base.TAGs.conductor.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.lyricist))
					{
						stringBuilder.Append(" --tag \"Lyricist=" + base.TAGs.lyricist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.remixer))
					{
						stringBuilder.Append(" --tag \"MixArtist=" + base.TAGs.remixer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.producer))
					{
						stringBuilder.Append(" --tag \"Producer=" + base.TAGs.producer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.encodedby))
					{
						stringBuilder.Append(" --tag \"EncodedBy=" + base.TAGs.encodedby.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.copyright))
					{
						stringBuilder.Append(" --tag \"Copyright=" + base.TAGs.copyright.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.publisher))
					{
						stringBuilder.Append(" --tag \"Label=" + base.TAGs.publisher.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.bpm))
					{
						stringBuilder.Append(" --tag \"BPM=" + base.TAGs.bpm.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.grouping))
					{
						stringBuilder.Append(" --tag \"Grouping=" + base.TAGs.grouping.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.mood))
					{
						stringBuilder.Append(" --tag \"Mood=" + base.TAGs.mood.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.rating))
					{
						stringBuilder.Append(" --tag \"Rating=" + base.TAGs.rating.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.isrc))
					{
						stringBuilder.Append(" --tag \"ISRC=" + base.TAGs.isrc.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (base.TAGs.replaygain_track_peak >= 0f)
					{
						stringBuilder.Append(" --tag \"replaygain_track_peak=" + base.TAGs.replaygain_track_peak.ToString("R", CultureInfo.InvariantCulture) + "\"");
					}
					if (base.TAGs.replaygain_track_gain >= -60f && base.TAGs.replaygain_track_gain <= 60f)
					{
						stringBuilder.Append(" --tag \"replaygain_track_gain=" + base.TAGs.replaygain_track_gain.ToString("R", CultureInfo.InvariantCulture) + " dB\"");
					}
				}
				if (this.MPC_CustomOptions != null && this.MPC_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.MPC_CustomOptions.Trim());
				}
			}
			if (base.OutputFile == null)
			{
				stringBuilder.Append(" --silent");
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

		public bool MPC_UseVersion7;

		public bool MPC_UseCustomOptionsOnly;

		public string MPC_CustomOptions = string.Empty;

		public EncoderMPC.MPCPreset MPC_Preset = EncoderMPC.MPCPreset.standard;

		public float MPC_Scale = 1f;

		public int MPC_Quality = 5;

		public float MPC_NMT = 6.5f;

		public float MPC_TMN = 18f;

		public float MPC_PNS;

		public enum MPCPreset
		{
			telephone,
			thumb,
			radio,
			standard,
			xtreme,
			insane,
			braindead
		}
	}
}
