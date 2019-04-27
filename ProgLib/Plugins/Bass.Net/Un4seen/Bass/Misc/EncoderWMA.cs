using System;
using System.Globalization;
using System.Threading;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Wma;

namespace Un4seen.Bass.Misc
{
	[Serializable]
	public sealed class EncoderWMA : BaseEncoder
	{
		public EncoderWMA(int channel) : base(channel)
		{
			this.WMA_TargetNumChans = base.ChannelNumChans;
			this.WMA_TargetSampleRate = base.ChannelSampleRate;
		}

		public override bool IsActive
		{
			get
			{
				return base.EncoderHandle != 0;
			}
		}

		public override bool IsPaused
		{
			get
			{
				return this._paused;
			}
		}

		public override bool Stop()
		{
			if (this._dspHandle != 0)
			{
				Bass.BASS_ChannelRemoveDSP(base.ChannelHandle, this._dspHandle);
				this._dspHandle = 0;
				this._dspCallback = null;
			}
			if (base.EncoderHandle != 0)
			{
				BassWma.BASS_WMA_EncodeClose(base.EncoderHandle);
				base.EncoderHandle = 0;
				this._wmEncoderProc = null;
				this._encoderProc = null;
			}
			this._byteSend = 0L;
			return true;
		}

		public override bool Pause(bool paused)
		{
			this._paused = paused;
			return true;
		}

		public override bool EncoderExists
		{
			get
			{
				return true;
			}
		}

		public override string ToString()
		{
			return "Windows Media Audio (.wma)";
		}

		public override BASSChannelType EncoderType
		{
			get
			{
				return BASSChannelType.BASS_CTYPE_STREAM_WMA;
			}
		}

		public override string DefaultOutputExtension
		{
			get
			{
				return ".wma";
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
				return string.Empty;
			}
		}

		public override int EffectiveBitrate
		{
			get
			{
				if (this.WMA_UseVBR && this.WMA_VBRQuality == 100)
				{
					return 778;
				}
				return this.WMA_Bitrate;
			}
		}

		public override bool Start(ENCODEPROC proc, IntPtr user, bool paused)
		{
			if (base.EncoderHandle != 0 || (proc != null && !this.SupportsSTDOUT))
			{
				return false;
			}
			this._paused = paused;
			this._encoderProc = null;
			this._byteSend = 0L;
			BASSWMAEncode basswmaencode = BASSWMAEncode.BASS_WMA_ENCODE_DEFAULT;
			if (this.WMA_ForceStandard)
			{
				basswmaencode |= BASSWMAEncode.BASS_WMA_ENCODE_STANDARD;
			}
			else
			{
				if (this.WMA_UsePro)
				{
					basswmaencode |= BASSWMAEncode.BASS_WMA_ENCODE_PRO;
				}
				if (this.WMA_Use24Bit)
				{
					basswmaencode |= BASSWMAEncode.BASS_WMA_ENCODE_24BIT;
					basswmaencode |= BASSWMAEncode.BASS_WMA_ENCODE_PRO;
				}
			}
			this._channel = base.ChannelHandle;
			this.WMA_TargetNumChans = base.ChannelNumChans;
			this.WMA_TargetSampleRate = base.ChannelSampleRate;
			if (base.InputFile != null)
			{
				this._channel = Bass.BASS_StreamCreateFile(base.InputFile, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | (this.WMA_Use24Bit ? BASSFlag.BASS_SAMPLE_FLOAT : BASSFlag.BASS_DEFAULT));
				if (this._channel == 0)
				{
					return false;
				}
				if (this.WMA_Use24Bit)
				{
					basswmaencode |= BASSWMAEncode.BASS_SAMPLE_FLOAT;
				}
			}
			else if (base.ChannelBitwidth == 32)
			{
				basswmaencode |= BASSWMAEncode.BASS_SAMPLE_FLOAT;
			}
			else if (base.ChannelBitwidth == 8)
			{
				basswmaencode |= BASSWMAEncode.BASS_SAMPLE_8BITS;
			}
			if (this.WMA_UseNetwork)
			{
				basswmaencode |= BASSWMAEncode.BASS_WMA_ENCODE_SCRIPT;
			}
			int bitrate = this.WMA_Bitrate * 1000;
			if (this.WMA_UseVBR && this.WMA_VBRQuality > 0 && this.WMA_VBRQuality <= 100)
			{
				bitrate = this.WMA_VBRQuality;
			}
			if (proc != null && !this.WMA_UseNetwork)
			{
				this._encoderProc = proc;
				this._wmEncoderProc = new WMENCODEPROC(this.EncodingWMAHandler);
			}
			if (base.OutputFile == null)
			{
				if (this.WMA_UseNetwork && !this.WMA_UsePublish)
				{
					if (this.WMA_MultiBitrate != null && this.WMA_MultiBitrate.Length != 0)
					{
						int[] array = new int[this.WMA_MultiBitrate.Length];
						for (int i = 0; i < this.WMA_MultiBitrate.Length; i++)
						{
							array[i] = this.WMA_MultiBitrate[i] * 1000;
						}
						base.EncoderHandle = BassWma.BASS_WMA_EncodeOpenNetworkMulti(this.WMA_TargetSampleRate, this.WMA_TargetNumChans, basswmaencode, array, this.WMA_NetworkPort, this.WMA_NetworkClients);
					}
					else
					{
						if (this.WMA_MultiBitrate != null)
						{
							bitrate = this.WMA_MultiBitrate[0];
						}
						base.EncoderHandle = BassWma.BASS_WMA_EncodeOpenNetwork(this.WMA_TargetSampleRate, this.WMA_TargetNumChans, basswmaencode, bitrate, this.WMA_NetworkPort, this.WMA_NetworkClients);
					}
				}
				else if (this.WMA_UseNetwork && this.WMA_UsePublish)
				{
					if (this.WMA_MultiBitrate != null && this.WMA_MultiBitrate.Length > 1)
					{
						int[] array2 = new int[this.WMA_MultiBitrate.Length];
						for (int j = 0; j < this.WMA_MultiBitrate.Length; j++)
						{
							array2[j] = this.WMA_MultiBitrate[j] * 1000;
						}
						base.EncoderHandle = BassWma.BASS_WMA_EncodeOpenPublishMulti(this.WMA_TargetSampleRate, this.WMA_TargetNumChans, basswmaencode, array2, this.WMA_PublishUrl, this.WMA_PublishUsername, this.WMA_PublishPassword);
					}
					else
					{
						base.EncoderHandle = BassWma.BASS_WMA_EncodeOpenPublish(this.WMA_TargetSampleRate, this.WMA_TargetNumChans, basswmaencode, bitrate, this.WMA_PublishUrl, this.WMA_PublishUsername, this.WMA_PublishPassword);
					}
				}
				else if (proc != null)
				{
					base.EncoderHandle = BassWma.BASS_WMA_EncodeOpen(this.WMA_TargetSampleRate, this.WMA_TargetNumChans, basswmaencode, bitrate, this._wmEncoderProc, user);
				}
			}
			else
			{
				base.EncoderHandle = BassWma.BASS_WMA_EncodeOpenFile(this.WMA_TargetSampleRate, this.WMA_TargetNumChans, basswmaencode, bitrate, base.OutputFile);
				if (base.TAGs != null)
				{
					if (!string.IsNullOrEmpty(base.TAGs.title))
					{
						this.SetTag("Title", base.TAGs.title);
					}
					if (!string.IsNullOrEmpty(base.TAGs.artist))
					{
						this.SetTag("Author", base.TAGs.artist);
					}
					if (!string.IsNullOrEmpty(base.TAGs.album))
					{
						this.SetTag("WM/AlbumTitle", base.TAGs.album);
					}
					if (!string.IsNullOrEmpty(base.TAGs.albumartist))
					{
						this.SetTag("WM/AlbumArtist", base.TAGs.albumartist);
					}
					if (!string.IsNullOrEmpty(base.TAGs.year))
					{
						this.SetTag("WM/Year", base.TAGs.year);
					}
					if (!string.IsNullOrEmpty(base.TAGs.track))
					{
						this.SetTag("WM/TrackNumber", base.TAGs.track);
					}
					if (!string.IsNullOrEmpty(base.TAGs.disc))
					{
						this.SetTag("WM/PartOfSet", base.TAGs.disc);
					}
					if (!string.IsNullOrEmpty(base.TAGs.genre))
					{
						this.SetTag("WM/Genre", base.TAGs.genre);
					}
					if (!string.IsNullOrEmpty(base.TAGs.comment))
					{
						this.SetTag("Description", base.TAGs.comment);
					}
					if (!string.IsNullOrEmpty(base.TAGs.composer))
					{
						this.SetTag("WM/Composer", base.TAGs.composer);
					}
					if (!string.IsNullOrEmpty(base.TAGs.conductor))
					{
						this.SetTag("WM/Conductor", base.TAGs.conductor);
					}
					if (!string.IsNullOrEmpty(base.TAGs.lyricist))
					{
						this.SetTag("WM/Writer", base.TAGs.lyricist);
					}
					if (!string.IsNullOrEmpty(base.TAGs.remixer))
					{
						this.SetTag("WM/ModifiedBy", base.TAGs.remixer);
					}
					if (!string.IsNullOrEmpty(base.TAGs.producer))
					{
						this.SetTag("WM/Producer", base.TAGs.producer);
					}
					if (!string.IsNullOrEmpty(base.TAGs.encodedby))
					{
						this.SetTag("WM/EncodedBy", base.TAGs.encodedby);
					}
					if (!string.IsNullOrEmpty(base.TAGs.copyright))
					{
						this.SetTag("Copyright", base.TAGs.copyright);
					}
					if (!string.IsNullOrEmpty(base.TAGs.publisher))
					{
						this.SetTag("WM/Publisher", base.TAGs.publisher);
					}
					if (!string.IsNullOrEmpty(base.TAGs.bpm))
					{
						this.SetTag("WM/BeatsPerMinute", base.TAGs.bpm);
					}
					if (!string.IsNullOrEmpty(base.TAGs.grouping))
					{
						this.SetTag("WM/ContentGroupDescription", base.TAGs.grouping);
					}
					if (!string.IsNullOrEmpty(base.TAGs.rating))
					{
						this.SetTag("WM/Rating", base.TAGs.rating);
					}
					if (!string.IsNullOrEmpty(base.TAGs.mood))
					{
						this.SetTag("WM/Mood", base.TAGs.mood);
					}
					if (!string.IsNullOrEmpty(base.TAGs.isrc))
					{
						this.SetTag("WM/ISRC", base.TAGs.isrc);
					}
					if (base.TAGs.replaygain_track_peak >= 0f)
					{
						this.SetTag("replaygain_track_peak", base.TAGs.replaygain_track_peak.ToString("R", CultureInfo.InvariantCulture));
					}
					if (base.TAGs.replaygain_track_gain >= -60f && base.TAGs.replaygain_track_gain <= 60f)
					{
						this.SetTag("replaygain_track_gain", base.TAGs.replaygain_track_gain.ToString("R", CultureInfo.InvariantCulture) + " dB");
					}
				}
			}
			if (base.EncoderHandle == 0)
			{
				return false;
			}
			this._dspCallback = new DSPPROC(this.EncodingDSPHandler);
			this._dspHandle = Bass.BASS_ChannelSetDSP(base.ChannelHandle, this._dspCallback, IntPtr.Zero, Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_ENCODE_PRIORITY));
			if (this._dspHandle == 0)
			{
				this.Stop();
				return false;
			}
			if (base.InputFile != null)
			{
				Utils.DecodeAllData(this._channel, true);
			}
			this._channel = 0;
			return base.EncoderHandle != 0;
		}

		public new bool Force16Bit
		{
			get
			{
				return false;
			}
		}

		public override string SettingsString()
		{
			string text = this.EffectiveBitrate.ToString();
			if (this.WMA_MultiBitrate != null && this.WMA_MultiBitrate.Length != 0)
			{
				text = Convert.ToString(this.WMA_MultiBitrate[0], CultureInfo.InvariantCulture);
				for (int i = 1; i < this.WMA_MultiBitrate.Length; i++)
				{
					text = text + "+" + Convert.ToString(this.WMA_MultiBitrate[i], CultureInfo.InvariantCulture);
				}
			}
			if (this.WMA_UseVBR && this.WMA_VBRQuality == 100)
			{
				return string.Format("Lossless-{0} kbps, {1}-bit {2}", text, this.WMA_Use24Bit ? 24 : 16, this.WMA_UsePro ? "Pro" : "").Trim();
			}
			return string.Format("{0}-{1} kbps, {2}-bit {3} {4}", new object[]
			{
				this.WMA_UseVBR ? "VBR" : "CBR",
				text,
				this.WMA_Use24Bit ? 24 : 16,
				this.WMA_UsePro ? "Pro" : "",
				this.WMA_UseVBR ? (this.WMA_VBRQuality.ToString() + "%") : ""
			}).Trim();
		}

		private void EncodingWMAHandler(int handle, BASSWMAEncodeCallback type, IntPtr buffer, int length, IntPtr user)
		{
			if (type == BASSWMAEncodeCallback.BASS_WMA_ENCODE_DATA)
			{
				this._encoderProc(handle, this._channel, buffer, length, user);
				return;
			}
			if (type == BASSWMAEncodeCallback.BASS_WMA_ENCODE_HEAD)
			{
				this._encoderProc(handle, this._channel, buffer, -1 * length, user);
			}
		}

		private void EncodingDSPHandler(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (this._paused)
			{
				return;
			}
			if (base.EncoderHandle != 0)
			{
				if (!BassWma.BASS_WMA_EncodeWrite(base.EncoderHandle, buffer, length))
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.DoNotify));
				}
				try
				{
					this._byteSend += (long)length;
				}
				catch
				{
					this._byteSend = (long)length;
				}
			}
		}

		private void DoNotify(object state)
		{
			this.Stop();
			if (this._notify != null)
			{
				this._notify(base.EncoderHandle, BASSEncodeNotify.BASS_ENCODE_NOTIFY_ENCODER, IntPtr.Zero);
			}
		}

		public bool SetTag(string tag, string value)
		{
			return base.EncoderHandle != 0 && BassWma.BASS_WMA_EncodeSetTag(base.EncoderHandle, tag, value);
		}

		internal long ByteSend
		{
			get
			{
				return this._byteSend;
			}
		}

		public ENCODENOTIFYPROC WMA_Notify
		{
			get
			{
				return this._notify;
			}
			set
			{
				this._notify = value;
			}
		}

		private bool _paused;

		private WMENCODEPROC _wmEncoderProc;

		private ENCODEPROC _encoderProc;

		private ENCODENOTIFYPROC _notify;

		private int _channel;

		private long _byteSend;

		private DSPPROC _dspCallback;

		private int _dspHandle;

		public int WMA_Bitrate = 128;

		public bool WMA_Use24Bit;

		public bool WMA_UsePro;

		public bool WMA_ForceStandard;

		public bool WMA_UseVBR;

		public int WMA_VBRQuality = 75;

		private int WMA_TargetSampleRate = 44100;

		private int WMA_TargetNumChans = 2;

		public bool WMA_UseNetwork;

		public bool WMA_UsePublish;

		public int[] WMA_MultiBitrate;

		public int WMA_NetworkPort = 8080;

		public int WMA_NetworkClients = 1;

		public string WMA_PublishUrl = string.Empty;

		public string WMA_PublishUsername = string.Empty;

		public string WMA_PublishPassword = string.Empty;
	}
}
