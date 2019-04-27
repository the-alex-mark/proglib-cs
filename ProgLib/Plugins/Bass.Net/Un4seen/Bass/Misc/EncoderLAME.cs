using System;
using System.Globalization;
using System.IO;
using System.Text;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public class EncoderLAME : BaseEncoder
	{
		public EncoderLAME(int channel) : base(channel)
		{
		}

		public override bool EncoderExists
		{
			get
			{
				return File.Exists(Path.Combine(base.EncoderDirectory, "lame.exe"));
			}
		}

		public override string ToString()
		{
			return "LAME Encoder (.mp3)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return BASSChannelType.BASS_CTYPE_STREAM_MP3;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return ".mp3";
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
				if (!this.LAME_UseVBR)
				{
					return this.LAME_Bitrate;
				}
				if (this.LAME_ABRBitrate > 0)
				{
					return this.LAME_ABRBitrate;
				}
				return (this.LAME_VBRMaxBitrate + this.LAME_Bitrate) / 2;
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
			return string.Format("{0}-{1} kbps, {2} {3}", new object[]
			{
				this.LAME_UseVBR ? "VBR" : "CBR",
				this.EffectiveBitrate,
				this.LAME_Mode,
				this.LAME_UseVBR ? this.LAME_VBRQuality.ToString() : ""
			}).Trim();
		}

		private string BuildEncoderCommandLine()
		{
			CultureInfo provider = new CultureInfo("en-US", false);
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.EncoderDirectory))
			{
				stringBuilder.Append("\"" + Path.Combine(base.EncoderDirectory, "lame.exe") + "\"");
			}
			else
			{
				stringBuilder.Append("lame.exe");
			}
			stringBuilder.Append(string.Format(provider, " -r -s {0:##0.0##} --bitwidth {1}", new object[]
			{
				(float)base.ChannelSampleRate / 1000f,
				(base.ChannelBitwidth > 16) ? (base.Force16Bit ? 16 : 32) : base.ChannelBitwidth
			}));
			if (this.LAME_UseCustomOptionsOnly)
			{
				if (this.LAME_CustomOptions != null)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.LAME_CustomOptions.Trim());
				}
				if (base.OutputFile == null)
				{
					stringBuilder.Append(" --quiet");
				}
			}
			else
			{
				if (this.LAME_Scale != 1f)
				{
					stringBuilder.Append(string.Format(provider, " --scale {0:#0.0####}", new object[]
					{
						this.LAME_Scale
					}));
				}
				if (base.ChannelNumChans == 1)
				{
					this.LAME_Mode = EncoderLAME.LAMEMode.Mono;
				}
				if (base.ChannelNumChans > 1 && this.LAME_Mode == EncoderLAME.LAMEMode.Mono)
				{
					stringBuilder.Append(" -a");
				}
				else
				{
					switch (this.LAME_Mode)
					{
					case EncoderLAME.LAMEMode.Stereo:
						stringBuilder.Append(" -m s");
						break;
					case EncoderLAME.LAMEMode.JointStereo:
						stringBuilder.Append(" -m j");
						break;
					case EncoderLAME.LAMEMode.ForcedJointStereo:
						stringBuilder.Append(" -m f");
						break;
					case EncoderLAME.LAMEMode.Mono:
						stringBuilder.Append(" -m m");
						break;
					case EncoderLAME.LAMEMode.DualMono:
						stringBuilder.Append(" -m d");
						break;
					}
				}
				if (base.OutputFile == null)
				{
					stringBuilder.Append(" --quiet");
				}
				if (this.LAME_PresetName != null && this.LAME_PresetName.Length > 0)
				{
					stringBuilder.Append(string.Format(provider, " --preset {0}", new object[]
					{
						this.LAME_PresetName.Trim()
					}));
				}
				else
				{
					switch (this.LAME_Quality)
					{
					case EncoderLAME.LAMEQuality.Q0:
					case EncoderLAME.LAMEQuality.Q1:
					case EncoderLAME.LAMEQuality.Q2:
					case EncoderLAME.LAMEQuality.Q3:
					case EncoderLAME.LAMEQuality.Q4:
					case EncoderLAME.LAMEQuality.Q5:
					case EncoderLAME.LAMEQuality.Q6:
					case EncoderLAME.LAMEQuality.Q7:
					case EncoderLAME.LAMEQuality.Q8:
					case EncoderLAME.LAMEQuality.Q9:
						stringBuilder.Append(string.Format(provider, " -q {0}", new object[]
						{
							(int)this.LAME_Quality
						}));
						break;
					case EncoderLAME.LAMEQuality.Speed:
						stringBuilder.Append(" -f");
						break;
					case EncoderLAME.LAMEQuality.Quality:
						stringBuilder.Append(" -h");
						break;
					}
					switch (this.LAME_ReplayGain)
					{
					case EncoderLAME.LAMEReplayGain.Fast:
						stringBuilder.Append(" --replaygain-fast");
						break;
					case EncoderLAME.LAMEReplayGain.Accurate:
						stringBuilder.Append(" --replaygain-accurate");
						break;
					case EncoderLAME.LAMEReplayGain.None:
						stringBuilder.Append(" --noreplaygain");
						break;
					}
					if (this.LAME_FreeFormat)
					{
						stringBuilder.Append(" --freeformat");
					}
					if (this.LAME_UseVBR)
					{
						if (this.LAME_ABRBitrate > 0)
						{
							stringBuilder.Append(string.Format(provider, " --abr {0}", new object[]
							{
								this.LAME_ABRBitrate
							}));
						}
						else
						{
							stringBuilder.Append(string.Format(provider, " -V {0}", new object[]
							{
								(int)this.LAME_VBRQuality
							}));
							if (this.LAME_LimitVBR)
							{
								stringBuilder.Append(string.Format(provider, " -b {0}", new object[]
								{
									this.LAME_Bitrate
								}));
								stringBuilder.Append(string.Format(" -B {0}", this.LAME_VBRMaxBitrate));
								if (this.LAME_VBREnforceMinBitrate)
								{
									stringBuilder.Append(" -F");
								}
							}
						}
						if (this.LAME_VBRDisableTag)
						{
							stringBuilder.Append(" -t");
						}
					}
					else
					{
						stringBuilder.Append(string.Format(provider, " -b {0}", new object[]
						{
							this.LAME_Bitrate
						}));
						if (this.LAME_EnforceCBR)
						{
							stringBuilder.Append(" --cbr");
						}
					}
				}
				if (this.LAME_Copyright)
				{
					stringBuilder.Append(" -c");
				}
				if (this.LAME_NonOriginal)
				{
					stringBuilder.Append(" -o");
				}
				if (this.LAME_Protect)
				{
					stringBuilder.Append(" -p");
				}
				if (this.LAME_DisableBitReservoir)
				{
					stringBuilder.Append(" --nores");
				}
				if (this.LAME_EnforceISO)
				{
					stringBuilder.Append(" --strictly-enforce-ISO");
				}
				if (this.LAME_DisableAllFilters)
				{
					stringBuilder.Append(" -k");
				}
				if (this.LAME_PSYuseShortBlocks)
				{
					stringBuilder.Append(" --short");
				}
				if (this.LAME_PSYnoShortBlocks)
				{
					stringBuilder.Append(" --noshort");
				}
				if (this.LAME_PSYallShortBlocks)
				{
					stringBuilder.Append(" --allshort");
				}
				if (this.LAME_PSYnoTemp)
				{
					stringBuilder.Append(" --notemp");
				}
				if (this.LAME_PSYnsSafeJoint)
				{
					stringBuilder.Append(" --nssafejoint");
				}
				switch (this.LAME_ATHControl)
				{
				case EncoderLAME.LAMEATH.Only:
					stringBuilder.Append(" --athonly");
					break;
				case EncoderLAME.LAMEATH.Disable:
					stringBuilder.Append(" --noath");
					break;
				case EncoderLAME.LAMEATH.OnlyShortBlocks:
					stringBuilder.Append(" --athshort");
					break;
				}
				if (this.LAME_TargetSampleRate > 0)
				{
					stringBuilder.Append(string.Format(provider, " --resample {0:##0.0##}", new object[]
					{
						(float)this.LAME_TargetSampleRate / 1000f
					}));
				}
				if (this.LAME_HighPassFreq > 0)
				{
					stringBuilder.Append(string.Format(provider, " --highpass {0}", new object[]
					{
						this.LAME_HighPassFreq
					}));
					if (this.LAME_HighPassFreqWidth > 0)
					{
						stringBuilder.Append(string.Format(provider, " --highpass-width {0}", new object[]
						{
							this.LAME_HighPassFreqWidth
						}));
					}
				}
				if (this.LAME_LowPassFreq > 0)
				{
					stringBuilder.Append(string.Format(provider, " --lowpass {0}", new object[]
					{
						this.LAME_LowPassFreq
					}));
					if (this.LAME_LowPassFreqWidth > 0)
					{
						stringBuilder.Append(string.Format(provider, " --lowpass-width {0}", new object[]
						{
							this.LAME_LowPassFreqWidth
						}));
					}
				}
				switch (this.LAME_NoASM)
				{
				case EncoderLAME.LAMENOASM.NO_MMX:
					stringBuilder.Append(" --noasm mmx");
					break;
				case EncoderLAME.LAMENOASM.NO_3DNOW:
					stringBuilder.Append(" --noasm 3dnow");
					break;
				case EncoderLAME.LAMENOASM.NO_SSE:
					stringBuilder.Append(" --noasm sse");
					break;
				}
				if (base.TAGs != null)
				{
					stringBuilder.Append(" --ignore-tag-errors");
					if (!string.IsNullOrEmpty(base.TAGs.title))
					{
						stringBuilder.Append(" --tt \"" + base.TAGs.title.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.artist))
					{
						stringBuilder.Append(" --ta \"" + base.TAGs.artist.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.album))
					{
						stringBuilder.Append(" --tl \"" + base.TAGs.album.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.year))
					{
						stringBuilder.Append(" --ty \"" + base.TAGs.year.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.comment))
					{
						stringBuilder.Append(" --tc \"" + base.TAGs.comment.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.track))
					{
						stringBuilder.Append(" --tn \"" + base.TAGs.track.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
					if (!string.IsNullOrEmpty(base.TAGs.genre))
					{
						stringBuilder.Append(" --tg \"" + base.TAGs.genre.Replace("\"", "\\\"").Replace("\r", string.Empty).Replace("\n", string.Empty) + "\"");
					}
				}
				if (this.LAME_CustomOptions != null && this.LAME_CustomOptions.Length > 0)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.LAME_CustomOptions.Trim());
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
				stringBuilder.Append(" \"" + base.OutputFile + "\"");
			}
			else
			{
				stringBuilder.Append(" -");
			}
			return stringBuilder.ToString();
		}

		public bool LAME_UseCustomOptionsOnly;

		public string LAME_CustomOptions = string.Empty;

		public EncoderLAME.LAMEMode LAME_Mode;

		public float LAME_Scale = 1f;

		public string LAME_PresetName = string.Empty;

		public EncoderLAME.LAMEQuality LAME_Quality = EncoderLAME.LAMEQuality.Quality;

		public EncoderLAME.LAMEReplayGain LAME_ReplayGain = EncoderLAME.LAMEReplayGain.None;

		public bool LAME_FreeFormat;

		public int LAME_Bitrate = 128;

		public int LAME_TargetSampleRate;

		public bool LAME_EnforceCBR;

		public int LAME_ABRBitrate;

		public bool LAME_UseVBR;

		public bool LAME_LimitVBR;

		public EncoderLAME.LAMEVBRQuality LAME_VBRQuality = EncoderLAME.LAMEVBRQuality.VBR_Q4;

		public int LAME_VBRMaxBitrate = 320;

		public bool LAME_VBRDisableTag;

		public bool LAME_VBREnforceMinBitrate;

		public bool LAME_Copyright;

		public bool LAME_NonOriginal;

		public bool LAME_Protect;

		public bool LAME_DisableBitReservoir;

		public bool LAME_EnforceISO;

		public bool LAME_DisableAllFilters;

		public bool LAME_PSYuseShortBlocks;

		public bool LAME_PSYnoShortBlocks;

		public bool LAME_PSYallShortBlocks;

		public bool LAME_PSYnoTemp;

		public bool LAME_PSYnsSafeJoint;

		public EncoderLAME.LAMENOASM LAME_NoASM;

		public int LAME_HighPassFreq;

		public int LAME_HighPassFreqWidth;

		public int LAME_LowPassFreq;

		public int LAME_LowPassFreqWidth;

		public EncoderLAME.LAMEATH LAME_ATHControl;

		public enum LAMEATH
		{
			Default,
			Only,
			Disable,
			OnlyShortBlocks
		}

		public enum LAMENOASM
		{
			Default,
			NO_MMX,
			NO_3DNOW,
			NO_SSE
		}

		public enum LAMEVBRQuality
		{
			VBR_Q0,
			VBR_Q1,
			VBR_Q2,
			VBR_Q3,
			VBR_Q4,
			VBR_Q5,
			VBR_Q6,
			VBR_Q7,
			VBR_Q8,
			VBR_Q9
		}

		public enum LAMEReplayGain
		{
			Default,
			Fast,
			Accurate,
			None
		}

		public enum LAMEQuality
		{
			Q0,
			Q1,
			Q2,
			Q3,
			Q4,
			Q5,
			Q6,
			Q7,
			Q8,
			Q9,
			None,
			Speed,
			Quality
		}

		public enum LAMEMode
		{
			Default,
			Stereo,
			JointStereo,
			ForcedJointStereo,
			Mono,
			DualMono
		}
	}
}
