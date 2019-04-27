using System;
using System.Security;
using System.Text;
using Un4seen.Bass.AddOn.Wma;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class WMAcast : StreamingServer
	{
		public WMAcast(IBaseEncoder encoder) : base(encoder, true)
		{
			if (encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_WMA)
			{
				throw new ArgumentException("Invalid EncoderType (only WMA is supported)!");
			}
			this._wmaEncoder = (encoder as EncoderWMA);
			if (this._wmaEncoder == null)
			{
				throw new ArgumentNullException("Invalid Encoder used, encoder must be of type EncoderWMA!");
			}
			this._wmaEncoder.WMA_UseNetwork = true;
			this._wmaEncoder.InputFile = null;
		}

		public bool UsePublish
		{
			get
			{
				return this._wmaEncoder.WMA_UsePublish;
			}
			set
			{
				this._wmaEncoder.WMA_UsePublish = value;
			}
		}

		public int NetworkPort
		{
			get
			{
				return this._wmaEncoder.WMA_NetworkPort;
			}
			set
			{
				this._wmaEncoder.WMA_NetworkPort = value;
			}
		}

		public int NetworkClients
		{
			get
			{
				return this._wmaEncoder.WMA_NetworkClients;
			}
			set
			{
				this._wmaEncoder.WMA_NetworkClients = value;
			}
		}

		public string PublishUrl
		{
			get
			{
				return this._wmaEncoder.WMA_PublishUrl;
			}
			set
			{
				this._wmaEncoder.WMA_PublishUrl = value;
			}
		}

		public string PublishUsername
		{
			get
			{
				return this._wmaEncoder.WMA_PublishUsername;
			}
			set
			{
				this._wmaEncoder.WMA_PublishUsername = value;
			}
		}

		public string PublishPassword
		{
			get
			{
				return this._wmaEncoder.WMA_PublishPassword;
			}
			set
			{
				this._wmaEncoder.WMA_PublishPassword = value;
			}
		}

		public override bool IsConnected
		{
			get
			{
				return this._wmaEncoder != null && this._wmaEncoder.EncoderHandle != 0 && this._isConnected;
			}
		}

		public override bool Connect()
		{
			if (!this._wmaEncoder.IsActive)
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				base.LastErrorMessage = "Encoder not active or a connection to the server could not be established!";
				BASSError basserror = Bass.BASS_ErrorGetCode();
				if (basserror <= BASSError.BASS_ERROR_ILLPARAM)
				{
					if (basserror != BASSError.BASS_ERROR_FILEOPEN)
					{
						if (basserror == BASSError.BASS_ERROR_ILLPARAM)
						{
							base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
							base.LastErrorMessage = "Illegal parameters have been used!";
						}
					}
					else
					{
						base.LastError = StreamingServer.STREAMINGERROR.Error_CreatingConnection;
						base.LastErrorMessage = "Couldn't connect to the server!";
					}
				}
				else
				{
					if (basserror != BASSError.BASS_ERROR_NOTAVAIL)
					{
						switch (basserror)
						{
						case BASSError.BASS_ERROR_WMA_WM9:
						case BASSError.BASS_ERROR_WMA_CODEC:
							break;
						case BASSError.BASS_ERROR_WMA_DENIED:
							base.LastError = StreamingServer.STREAMINGERROR.Error_Login;
							base.LastErrorMessage = "Access denied - username/password invalid!";
							goto IL_AA;
						default:
							goto IL_AA;
						}
					}
					base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
					base.LastErrorMessage = "WMA codec missing or WMA9 required!";
				}
				IL_AA:
				this._isConnected = false;
				return false;
			}
			if (base.SongTitle != null && base.SongTitle.Length > 0)
			{
				this._wmaEncoder.SetTag("Title", base.SongTitle);
			}
			if (this.StreamAuthor != null && this.StreamAuthor.Length > 0)
			{
				this._wmaEncoder.SetTag("Author", this.StreamAuthor);
			}
			if (this.StreamCopyright != null && this.StreamCopyright.Length > 0)
			{
				this._wmaEncoder.SetTag("Copyright", this.StreamCopyright);
			}
			if (this.StreamDescription != null && this.StreamDescription.Length > 0)
			{
				this._wmaEncoder.SetTag("Description", this.StreamDescription);
			}
			if (this.StreamRating != null && this.StreamRating.Length > 0)
			{
				this._wmaEncoder.SetTag("Rating", this.StreamRating);
			}
			if (this.StreamGenre != null && this.StreamGenre.Length > 0)
			{
				this._wmaEncoder.SetTag("WM/Genre", this.StreamGenre);
			}
			if (this.StreamPublisher != null && this.StreamPublisher.Length > 0)
			{
				this._wmaEncoder.SetTag("WM/Publisher", this.StreamPublisher);
			}
			if (this.StreamUrl != null && this.StreamUrl.Length > 0)
			{
				this._wmaEncoder.SetTag("WM/AuthorURL", this.StreamUrl);
			}
			this._isConnected = true;
			return true;
		}

		public override bool Disconnect()
		{
			if (this._wmaEncoder.IsActive)
			{
				this._wmaEncoder.Stop();
			}
			this._isConnected = false;
			return true;
		}

		public override bool Login()
		{
			return true;
		}

		public override int SendData(IntPtr buffer, int length)
		{
			return length;
		}

		public override bool UpdateTitle(string song, string url)
		{
			base.SongTitle = song;
			return !string.IsNullOrEmpty(base.SongTitle) && this.WMAcastUpdateTitle(song);
		}

		public override int GetListeners(string password)
		{
			if (this.IsConnected)
			{
				return BassWma.BASS_WMA_EncodeGetClients(base.Encoder.EncoderHandle);
			}
			return -1;
		}

		public override string GetStats(string password)
		{
			if (!this.IsConnected)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version=\"1.0\" standalone=\"yes\" ?>\n");
			stringBuilder.Append("<WMACASTSERVER>");
			stringBuilder.Append("<CURRENTLISTENERS>");
			stringBuilder.Append(this.GetListeners(password));
			stringBuilder.Append("</CURRENTLISTENERS>");
			stringBuilder.Append("<MAXLISTENERS>");
			if (this.UsePublish)
			{
				stringBuilder.Append(-1);
			}
			else
			{
				stringBuilder.Append(this.NetworkClients);
			}
			stringBuilder.Append("</MAXLISTENERS>");
			stringBuilder.Append("<SERVERGENRE>");
			stringBuilder.Append(this.StreamGenre);
			stringBuilder.Append("</SERVERGENRE>");
			stringBuilder.Append("<SERVERURL>");
			stringBuilder.Append(this.StreamUrl);
			stringBuilder.Append("</SERVERURL>");
			stringBuilder.Append("<SERVERTITLE>");
			stringBuilder.Append(this.StreamDescription);
			stringBuilder.Append("</SERVERTITLE>");
			stringBuilder.Append("<AUTHOR>");
			stringBuilder.Append(this.StreamAuthor);
			stringBuilder.Append("</AUTHOR>");
			stringBuilder.Append("<COPYRIGHT>");
			stringBuilder.Append(this.StreamCopyright);
			stringBuilder.Append("</COPYRIGHT>");
			stringBuilder.Append("<PUBLISHER>");
			stringBuilder.Append(this.StreamPublisher);
			stringBuilder.Append("</PUBLISHER>");
			stringBuilder.Append("<RATING>");
			stringBuilder.Append(this.StreamRating);
			stringBuilder.Append("</RATING>");
			stringBuilder.Append("<SONGTITLE>");
			stringBuilder.Append(base.SongTitle);
			stringBuilder.Append("</SONGTITLE>");
			stringBuilder.Append("<STREAMSTATUS>");
			stringBuilder.Append(this.IsConnected ? 1 : 0);
			stringBuilder.Append("</STREAMSTATUS>");
			stringBuilder.Append("<BITRATE>");
			stringBuilder.Append(base.Encoder.EffectiveBitrate);
			stringBuilder.Append("</BITRATE>");
			stringBuilder.Append("<CONTENT>");
			stringBuilder.Append("Audio/x-ms-wma");
			stringBuilder.Append("</CONTENT>");
			stringBuilder.Append("<SAMPLERATE>");
			stringBuilder.Append(base.Encoder.ChannelSampleRate);
			stringBuilder.Append("</SAMPLERATE>");
			stringBuilder.Append("<NUMCHANNELS>");
			stringBuilder.Append(base.Encoder.ChannelNumChans);
			stringBuilder.Append("</NUMCHANNELS>");
			stringBuilder.Append("</WMACASTSERVER>");
			return stringBuilder.ToString();
		}

		private bool WMAcastUpdateTitle(string song)
		{
			if (this.IsConnected)
			{
				this._wmaEncoder.SetTag("Title", song);
				return this._wmaEncoder.SetTag("CAPTION", song);
			}
			return false;
		}

		public string StreamAuthor = "";

		public string StreamPublisher = "";

		public string StreamGenre = "";

		public string StreamCopyright = "";

		public string StreamDescription = "";

		public string StreamRating = "";

		public string StreamUrl = "";

		private EncoderWMA _wmaEncoder;

		private bool _isConnected;
	}
}
