using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderFLAC : BaseEncoder
	{
		public EncoderFLAC(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "flac.exe"));
			}
		}

		public override string ToString()
		{
			if (this.FLAC_UseOgg)
			{
				return "FLAC Encoder (.ogg)";
			}
			return "FLAC Encoder (.flac)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				if (this.FLAC_UseOgg)
				{
					return BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG;
				}
				return BASSChannelType.BASS_CTYPE_STREAM_FLAC;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				if (this.FLAC_UseOgg)
				{
					return ".ogg";
				}
				return ".flac";
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
				return base.ChannelSampleRate * 16 * base.ChannelNumChans / 1000;
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
			else
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
			return string.Format("{0} kbps, Comp.{1}", this.EffectiveBitrate, this.FLAC_CompressionLevel);
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "flac.exe") + "\" -f");
			}
			else
			{
				stringBuilder.Append("flac.exe -f");
			}
			stringBuilder.Append(string.Format(provider, " --force-raw-format --endian=little --sample-rate={0} --channels={1} --bps={2} --sign=signed", new object[]
			{
				base.ChannelSampleRate,
				base.ChannelNumChans,
				(base.ChannelBitwidth > 16) ? (base.Force16Bit ? 16 : 24) : base.ChannelBitwidth
			}));
			if (this.FLAC_UseCustomOptionsOnly)
			{
				if (this.FLAC_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.FLAC_CustomOptions.Trim());
				}
			}
			else
			{
				if (this.FLAC_UseOgg)
				{
					stringBuilder.Append(" --ogg");
				}
				if (this.FLAC_Verify)
				{
					stringBuilder.Append(" -V");
				}
				if (this.FLAC_Lax)
				{
					stringBuilder.Append(" --lax");
				}
				if (this.FLAC_ReplayGain)
				{
					stringBuilder.Append(" --replay-gain");
				}
				if (this.FLAC_NoPadding)
				{
					stringBuilder.Append(" --no-padding");
				}
				else if (this.FLAC_Padding > 0)
				{
					stringBuilder.Append(string.Format(" -P {0}", this.FLAC_Padding));
				}
				if (this.FLAC_Blocksize > 0)
				{
					stringBuilder.Append(string.Format(" -b {0}", this.FLAC_Blocksize));
				}
				stringBuilder.Append(string.Format(" -{0}", this.FLAC_CompressionLevel));
				if (base.TAGs != null)
				{
					if (!string.IsNullOrEmpty(base.TAGs.title))
					{
						stringBuilder.Append(" -T \"TITLE=" + base.TAGs.title.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.artist))
					{
						stringBuilder.Append(" -T \"ARTIST=" + base.TAGs.artist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.album))
					{
						stringBuilder.Append(" -T \"ALBUM=" + base.TAGs.album.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.albumartist))
					{
						stringBuilder.Append(" -T \"ALBUMARTIST=" + base.TAGs.albumartist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.year))
					{
						stringBuilder.Append(" -T \"DATE=" + base.TAGs.year.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.track))
					{
						stringBuilder.Append(" -T \"TRACKNUMBER=" + base.TAGs.track.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.disc))
					{
						stringBuilder.Append(" -T \"DISCNUMBER=" + base.TAGs.disc.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.genre))
					{
						stringBuilder.Append(" -T \"GENRE=" + base.TAGs.genre.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.comment))
					{
						stringBuilder.Append(" -T \"COMMENT=" + base.TAGs.comment.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.composer))
					{
						stringBuilder.Append(" -T \"COMPOSER=" + base.TAGs.composer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.conductor))
					{
						stringBuilder.Append(" -T \"CONDUCTOR=" + base.TAGs.conductor.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.lyricist))
					{
						stringBuilder.Append(" -T \"LYRICIST=" + base.TAGs.lyricist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.remixer))
					{
						stringBuilder.Append(" -T \"REMIXER=" + base.TAGs.remixer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.producer))
					{
						stringBuilder.Append(" -T \"PRODUCER=" + base.TAGs.producer.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.encodedby))
					{
						stringBuilder.Append(" -T \"ENCODEDBY=" + base.TAGs.encodedby.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.copyright))
					{
						stringBuilder.Append(" -T \"COPYRIGHT=" + base.TAGs.copyright.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.publisher))
					{
						stringBuilder.Append(" -T \"LABEL=" + base.TAGs.publisher.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.bpm))
					{
						stringBuilder.Append(" -T \"BPM=" + base.TAGs.bpm.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.grouping))
					{
						stringBuilder.Append(" -T \"GROUPING=" + base.TAGs.grouping.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.mood))
					{
						stringBuilder.Append(" -T \"MOOD=" + base.TAGs.mood.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.rating))
					{
						stringBuilder.Append(" -T \"RATING=" + base.TAGs.rating.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.isrc))
					{
						stringBuilder.Append(" -T \"ISRC=" + base.TAGs.isrc.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (base.TAGs.replaygain_track_peak >= 0f)
					{
						stringBuilder.Append(" -T \"replaygain_track_peak=" + base.TAGs.replaygain_track_peak.ToString("R", CultureInfo.InvariantCulture) + "\"");
					}
					if (base.TAGs.replaygain_track_gain >= -60f && base.TAGs.replaygain_track_gain <= 60f)
					{
						stringBuilder.Append(" -T \"replaygain_track_gain=" + base.TAGs.replaygain_track_gain.ToString("R", CultureInfo.InvariantCulture) + " dB\"");
					}
				}
				if (this.FLAC_CustomOptions != null && this.FLAC_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.FLAC_CustomOptions.Trim());
				}
			}
			if (base.OutputFile == null)
			{
				stringBuilder.Append(" --totally-silent");
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
				stringBuilder.Append(" -c");
			}
			return stringBuilder.ToString();
		}

		public bool FLAC_UseCustomOptionsOnly;

		public string FLAC_CustomOptions = string.Empty;

		public bool FLAC_UseOgg;

		public bool FLAC_Verify;

		public bool FLAC_Lax;

		public bool FLAC_ReplayGain;

		public int FLAC_Padding;

		public bool FLAC_NoPadding;

		public int FLAC_Blocksize;

		public int FLAC_CompressionLevel = 5;
	}
}
