using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Tags;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class SHOUTcast : StreamingServer
	{
		public SHOUTcast(IBaseEncoder encoder) : base(encoder)
		{
			if (encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_MP3 && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_AAC && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_WAV)
			{
				throw new ArgumentException("Invalid EncoderType (only MP3 and AAC is supported)!");
			}
		}

		public SHOUTcast(IBaseEncoder encoder, bool useBASS) : base(encoder, useBASS)
		{
			if (encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_MP3 && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_AAC && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_WAV)
			{
				throw new ArgumentException("Invalid EncoderType (only MP3 and AAC is supported)!");
			}
		}

		public bool UseSHOUTcastv2
		{
			get
			{
				return base.UseBASS && !string.IsNullOrEmpty(this.SID);
			}
			set
			{
				if (value)
				{
					if (base.UseBASS && string.IsNullOrEmpty(this.SID))
					{
						this.SID = "1";
						return;
					}
				}
				else
				{
					this.SID = string.Empty;
				}
			}
		}

		public string AdminUsername
		{
			get
			{
				if (!string.IsNullOrEmpty(this._adminUsername))
				{
					return this._adminUsername;
				}
				if (string.IsNullOrEmpty(this.Username))
				{
					return "admin";
				}
				return this.Username;
			}
			set
			{
				this._adminUsername = value;
			}
		}

		public string AdminPassword
		{
			get
			{
				if (string.IsNullOrEmpty(this._adminPassword))
				{
					return this.Password;
				}
				return this._adminPassword;
			}
			set
			{
				this._adminPassword = value;
			}
		}

		public override bool IsConnected
		{
			get
			{
				if (base.UseBASS)
				{
					return this._isConnected;
				}
				return this._socket != null && this._socket.Connected && this._loggedIn;
			}
		}

		public override bool Connect()
		{
			if (!base.Encoder.IsActive)
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				base.LastErrorMessage = "Encoder not active!";
				return false;
			}
			if (!base.UseBASS)
			{
				if (this._socket != null && this._socket.Connected)
				{
					this._socket.Close();
					this._socket = null;
				}
				this._socket = this.CreateSocket(this.ServerAddress, this.ServerPort + 1);
				return this._socket != null && this._socket.Connected;
			}
			if (this._isConnected)
			{
				return true;
			}
			string content = BassEnc.BASS_ENCODE_TYPE_MP3;
			if (base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_AAC)
			{
				content = BassEnc.BASS_ENCODE_TYPE_AAC;
			}
			string pass = this.Password;
			if (!string.IsNullOrEmpty(this.Username) && this.Username.ToLower() != "source")
			{
				pass = this.Username + ":" + this.Password;
			}
			string text = string.Format("{0}:{1}", this.ServerAddress, this.ServerPort);
			if (this.UseSHOUTcastv2)
			{
				text = text + "," + this.SID;
			}
			if (BassEnc.BASS_Encode_CastInit(base.Encoder.EncoderHandle, text, pass, content, this.StationName, this.Url, this.Genre, null, string.Format("icy-irc:{0}\r\nicy-icq:{1}\r\nicy-aim:{2}\r\n", this.Irc, this.Icq, this.Aim), base.Encoder.EffectiveBitrate, this.PublicFlag))
			{
				this._myNotifyProc = new ENCODENOTIFYPROC(this.EncoderNotifyProc);
				this._isConnected = BassEnc.BASS_Encode_SetNotify(base.Encoder.EncoderHandle, this._myNotifyProc, IntPtr.Zero);
				this.SHOUTcastv2UpdateStationArtwork();
			}
			else
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				base.LastErrorMessage = "Encoder not active or a connction to the server could not be established!";
				BASSError basserror = Bass.BASS_ErrorGetCode();
				if (basserror <= BASSError.BASS_ERROR_HANDLE)
				{
					if (basserror != BASSError.BASS_ERROR_UNKNOWN)
					{
						if (basserror != BASSError.BASS_ERROR_FILEOPEN)
						{
							if (basserror == BASSError.BASS_ERROR_HANDLE)
							{
								base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
								base.LastErrorMessage = "Encoder not active or invalid Encoder used!";
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
						base.LastError = StreamingServer.STREAMINGERROR.Error_CreatingConnection;
						base.LastErrorMessage = "An unknown error occurred!";
					}
				}
				else if (basserror != BASSError.BASS_ERROR_ALREADY)
				{
					if (basserror != BASSError.BASS_ERROR_ILLPARAM)
					{
						if (basserror == BASSError.BASS_ERROR_CAST_DENIED)
						{
							base.LastError = StreamingServer.STREAMINGERROR.Error_Login;
							base.LastErrorMessage = "Username or Password incorrect!";
						}
					}
					else
					{
						base.LastError = StreamingServer.STREAMINGERROR.Error_ResolvingServerAddress;
						base.LastErrorMessage = "Couldn't connect to the server or server doesn't include a port number!";
					}
				}
				else
				{
					base.LastError = StreamingServer.STREAMINGERROR.Error_NotConnected;
					base.LastErrorMessage = "There is already a cast set on the encoder!";
				}
				this._isConnected = false;
			}
			return this._isConnected;
		}

		public override bool Disconnect()
		{
			if (base.UseBASS)
			{
				if (base.Encoder.Stop())
				{
					this._isConnected = false;
				}
				return !this._isConnected;
			}
			bool result = false;
			try
			{
				this._socket.Close();
				base.Encoder.Stop();
			}
			catch
			{
			}
			finally
			{
				if (this._socket != null && this._socket.Connected)
				{
					base.LastError = StreamingServer.STREAMINGERROR.Error_Disconnect;
					base.LastErrorMessage = "Winsock error: " + Convert.ToString(Marshal.GetLastWin32Error());
				}
				else
				{
					result = true;
					this._loggedIn = false;
					this._socket = null;
				}
			}
			return result;
		}

		public override bool Login()
		{
			if (base.UseBASS)
			{
				return true;
			}
			if (this._socket == null)
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_NotConnected;
				base.LastErrorMessage = "Not connected to server.";
				return false;
			}
			bool result = false;
			if (this.SHOUTcastLogin())
			{
				if (this.SHOUTcastInit())
				{
					result = true;
					this._loggedIn = true;
					base.LastError = StreamingServer.STREAMINGERROR.Ok;
					base.LastErrorMessage = string.Empty;
				}
				else
				{
					base.LastError = StreamingServer.STREAMINGERROR.Error_Login;
					base.LastErrorMessage = "Server could not be initialized.";
				}
			}
			else
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_Login;
				base.LastErrorMessage = "Invalid username or password.";
			}
			return result;
		}

		public override int SendData(IntPtr buffer, int length)
		{
			if (buffer == IntPtr.Zero || length == 0 || base.UseBASS)
			{
				return 0;
			}
			int num = -1;
			this._retrycount = 3;
			try
			{
				object @lock = this._lock;
				lock (@lock)
				{
					if (this._data == null || this._data.Length < length)
					{
						this._data = new byte[length];
					}
					Marshal.Copy(buffer, this._data, 0, length);
					num = this._socket.Send(this._data, 0, length, SocketFlags.None);
					while (num < length && this._retrycount > 0)
					{
						this._retrycount--;
						num += this._socket.Send(this._data, num, length - num, SocketFlags.None);
					}
					if (num < 0)
					{
						base.LastError = StreamingServer.STREAMINGERROR.Error_SendingData;
						base.LastErrorMessage = string.Format("{0} bytes not send.", length);
						this.Disconnect();
					}
					else if (num != length)
					{
						base.LastError = StreamingServer.STREAMINGERROR.Warning_LessDataSend;
						base.LastErrorMessage = string.Format("{0} of {1} bytes send.", num, length);
						this.Disconnect();
					}
				}
			}
			catch (Exception ex)
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_SendingData;
				base.LastErrorMessage = ex.Message;
				num = -1;
				this.Disconnect();
			}
			return num;
		}

		public override bool UpdateTitle(string song, string url)
		{
			if (song == null)
			{
				base.SongTitle = string.Empty;
			}
			else
			{
				base.SongTitle = song.Trim(new char[1]).Replace('\0', ' ');
			}
			if (!string.IsNullOrEmpty(url))
			{
				url = url.Trim(new char[1]).Replace('\0', ' ');
			}
			if (string.IsNullOrEmpty(base.SongTitle))
			{
				this.v2SongTitleNext = string.Empty;
			}
			if (!base.UseBASS || !this.IsConnected)
			{
				return this.SHOUTcastUpdateTitle(base.SongTitle, url);
			}
			if (this.UseSHOUTcastv2)
			{
				try
				{
					StringWriterWithEncoding stringWriterWithEncoding = new StringWriterWithEncoding(Encoding.UTF8);
					using (XmlWriter xmlWriter = XmlWriter.Create(stringWriterWithEncoding, new XmlWriterSettings
					{
						Encoding = stringWriterWithEncoding.Encoding,
						IndentChars = string.Empty,
						Indent = false,
						NewLineHandling = NewLineHandling.None,
						NewLineChars = string.Empty,
						CheckCharacters = false
					}))
					{
						xmlWriter.WriteStartElement("metadata");
						xmlWriter.WriteElementString("TIT2", base.SongTitle);
						if (!string.IsNullOrEmpty(this.StationName))
						{
							xmlWriter.WriteElementString("TRSN", this.StationName);
						}
						xmlWriter.WriteElementString("TENC", BassNet.InternalName + " (Broadcast Framework)");
						if (!string.IsNullOrEmpty(url))
						{
							xmlWriter.WriteElementString("WOAF", url);
						}
						if (!string.IsNullOrEmpty(this.Url))
						{
							xmlWriter.WriteElementString("WORS", this.Url);
						}
						if (this.v2SongTitleNext != null)
						{
							xmlWriter.WriteStartElement("extension");
							xmlWriter.WriteStartElement("title");
							xmlWriter.WriteAttributeString("seq", "1");
							xmlWriter.WriteString(base.SongTitle);
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("title");
							xmlWriter.WriteAttributeString("seq", "2");
							xmlWriter.WriteString(this.v2SongTitleNext);
							xmlWriter.WriteEndElement();
							xmlWriter.WriteElementString("soon", this.v2SongTitleNext);
							xmlWriter.WriteEndElement();
						}
						xmlWriter.WriteEndElement();
						xmlWriter.Flush();
					}
					this.v2SongTitleNext = string.Empty;
					return BassEnc.BASS_Encode_CastSendMeta(base.Encoder.EncoderHandle, BASSEncodeMetaDataType.BASS_METADATA_XML_SHOUTCAST, stringWriterWithEncoding.ToString());
				}
				catch
				{
					return false;
				}
			}
			if (base.ForceUTF8TitleUpdates)
			{
				this.v2SongTitleNext = string.Empty;
				bool flag = BassEnc.BASS_Encode_CastSetTitle(base.Encoder.EncoderHandle, Encoding.UTF8.GetBytes(base.SongTitle + "\0"), string.IsNullOrEmpty(url) ? null : Encoding.UTF8.GetBytes(url + "\0"));
				if (!flag)
				{
					flag = this.SHOUTcastUpdateTitle(base.SongTitle, url);
				}
				return flag;
			}
			this.v2SongTitleNext = string.Empty;
			bool flag2 = BassEnc.BASS_Encode_CastSetTitle(base.Encoder.EncoderHandle, this._encoding.GetBytes(base.SongTitle + "\0"), string.IsNullOrEmpty(url) ? null : this._encoding.GetBytes(url + "\0"));
			if (!flag2)
			{
				flag2 = this.SHOUTcastUpdateTitle(base.SongTitle, url);
			}
			return flag2;
		}

		public override bool UpdateTitle(TAG_INFO tag, string url)
		{
			if (tag == null)
			{
				this.v2SongTitleNext = string.Empty;
				return false;
			}
			if (!string.IsNullOrEmpty(url))
			{
				url = url.Trim(new char[1]).Replace('\0', ' ');
			}
			if (!base.UseBASS || !this.IsConnected)
			{
				string text = base.SongTitle;
				if (string.IsNullOrEmpty(text))
				{
					text = tag.ToString();
				}
				return this.SHOUTcastUpdateTitle(text, url);
			}
			if (this.UseSHOUTcastv2)
			{
				try
				{
					StringWriterWithEncoding stringWriterWithEncoding = new StringWriterWithEncoding(Encoding.UTF8);
					using (XmlWriter xmlWriter = XmlWriter.Create(stringWriterWithEncoding, new XmlWriterSettings
					{
						Encoding = stringWriterWithEncoding.Encoding,
						IndentChars = string.Empty,
						Indent = false,
						NewLineHandling = NewLineHandling.None,
						NewLineChars = string.Empty,
						CheckCharacters = false
					}))
					{
						xmlWriter.WriteStartElement("metadata");
						if (this.v2SendSongTitleOnly)
						{
							if (string.IsNullOrEmpty(base.SongTitle))
							{
								xmlWriter.WriteElementString("TIT2", tag.ToString());
							}
							else
							{
								xmlWriter.WriteElementString("TIT2", base.SongTitle);
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(tag.artist))
							{
								xmlWriter.WriteElementString("TIT2", tag.title);
								xmlWriter.WriteElementString("TPE1", tag.artist);
							}
							else
							{
								xmlWriter.WriteElementString("TIT2", tag.ToString());
							}
							if (!string.IsNullOrEmpty(tag.album))
							{
								xmlWriter.WriteElementString("TALB", tag.album);
							}
							if (!string.IsNullOrEmpty(tag.albumartist))
							{
								xmlWriter.WriteElementString("TPE2", tag.albumartist);
							}
							if (!string.IsNullOrEmpty(tag.genre))
							{
								xmlWriter.WriteElementString("TCON", tag.genre);
							}
							if (!string.IsNullOrEmpty(tag.year))
							{
								xmlWriter.WriteElementString("TYER", tag.year);
							}
							if (!string.IsNullOrEmpty(tag.copyright))
							{
								xmlWriter.WriteElementString("TCOP", tag.copyright);
							}
							if (!string.IsNullOrEmpty(tag.publisher))
							{
								xmlWriter.WriteElementString("TPUB", tag.publisher);
							}
							if (!string.IsNullOrEmpty(tag.composer))
							{
								xmlWriter.WriteElementString("TCOM", tag.composer);
							}
							if (!string.IsNullOrEmpty(tag.conductor))
							{
								xmlWriter.WriteElementString("TPE3", tag.conductor);
							}
							if (!string.IsNullOrEmpty(tag.remixer))
							{
								xmlWriter.WriteElementString("TPE4", tag.remixer);
							}
							if (!string.IsNullOrEmpty(tag.lyricist))
							{
								xmlWriter.WriteElementString("TEXT", tag.lyricist);
							}
							if (!string.IsNullOrEmpty(tag.isrc))
							{
								xmlWriter.WriteElementString("TSRC", tag.isrc);
							}
							if (!string.IsNullOrEmpty(tag.producer))
							{
								xmlWriter.WriteStartElement("IPLS");
								xmlWriter.WriteAttributeString("role", "producer");
								xmlWriter.WriteString(tag.producer);
								xmlWriter.WriteEndElement();
							}
							if (!string.IsNullOrEmpty(tag.grouping))
							{
								xmlWriter.WriteElementString("TIT1", tag.grouping);
							}
							if (!string.IsNullOrEmpty(this.StationName))
							{
								xmlWriter.WriteElementString("TRSN", this.StationName);
							}
							xmlWriter.WriteElementString("TENC", BassNet.InternalName + " (Broadcast Framework)");
							if (!string.IsNullOrEmpty(url))
							{
								xmlWriter.WriteElementString("WOAF", url);
							}
							if (!string.IsNullOrEmpty(this.Url))
							{
								xmlWriter.WriteElementString("WORS", this.Url);
							}
						}
						if (this.v2SongTitleNext != null)
						{
							xmlWriter.WriteStartElement("extension");
							xmlWriter.WriteStartElement("title");
							xmlWriter.WriteAttributeString("seq", "1");
							if (string.IsNullOrEmpty(base.SongTitle))
							{
								xmlWriter.WriteString(tag.ToString());
							}
							else
							{
								xmlWriter.WriteString(base.SongTitle);
							}
							xmlWriter.WriteEndElement();
							xmlWriter.WriteStartElement("title");
							xmlWriter.WriteAttributeString("seq", "2");
							xmlWriter.WriteString(this.v2SongTitleNext);
							xmlWriter.WriteEndElement();
							xmlWriter.WriteElementString("soon", this.v2SongTitleNext);
							xmlWriter.WriteEndElement();
						}
						xmlWriter.WriteEndElement();
						xmlWriter.Flush();
					}
					bool result = BassEnc.BASS_Encode_CastSendMeta(base.Encoder.EncoderHandle, BASSEncodeMetaDataType.BASS_METADATA_XML_SHOUTCAST, stringWriterWithEncoding.ToString());
					if (this.v2SendArtwork)
					{
						Task.Factory.StartNew(delegate()
						{
							try
							{
								BASSEncodeMetaDataType bassencodeMetaDataType = (BASSEncodeMetaDataType)0;
								TagPicture tagPicture = null;
								byte[] array = null;
								if (tag.PictureCount > 0)
								{
									tagPicture = tag.PictureGet(0);
								}
								if (tagPicture == null && !string.IsNullOrEmpty(this.v2StreamArtwork))
								{
									tagPicture = new TagPicture(this.v2StreamArtwork, TagPicture.PICTURE_TYPE.Location, this.StationName);
								}
								if (tagPicture != null)
								{
									tagPicture = new TagPicture(tagPicture, 300);
									if (tagPicture.PictureStorage == TagPicture.PICTURE_STORAGE.Internal)
									{
										if (tagPicture.Data.Length <= 523680)
										{
											array = tagPicture.Data;
										}
									}
									else
									{
										try
										{
											using (Stream stream = new FileStream(Encoding.UTF8.GetString(tagPicture.Data), FileMode.Open, FileAccess.Read))
											{
												if (stream.Length <= 523680L)
												{
													byte[] array2 = new byte[stream.Length];
													stream.Read(array2, 0, (int)stream.Length);
													array = array2;
												}
												stream.Close();
											}
										}
										catch
										{
										}
									}
									string mimetype = tagPicture.MIMEType;
									if (!(mimetype == "image/jpeg"))
									{
										if (!(mimetype == "image/bmp"))
										{
											if (!(mimetype == "image/png"))
											{
												if (mimetype == "image/gif")
												{
													bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_ALBUMART_GIF;
												}
											}
											else
											{
												bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_ALBUMART_PNG;
											}
										}
										else
										{
											bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_ALBUMART_BMP;
										}
									}
									else
									{
										bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_ALBUMART_JPG;
									}
								}
								if (bassencodeMetaDataType > (BASSEncodeMetaDataType)0 && array != null)
								{
									BassEnc.BASS_Encode_CastSendMeta(this.Encoder.EncoderHandle, bassencodeMetaDataType, array);
								}
							}
							catch
							{
							}
						});
					}
					this.v2SongTitleNext = string.Empty;
					return result;
				}
				catch
				{
					return false;
				}
			}
			if (base.ForceUTF8TitleUpdates)
			{
				string text2 = base.SongTitle;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = tag.ToString();
				}
				this.v2SongTitleNext = string.Empty;
				bool flag = BassEnc.BASS_Encode_CastSetTitle(base.Encoder.EncoderHandle, Encoding.UTF8.GetBytes(text2 + "\0"), string.IsNullOrEmpty(url) ? null : Encoding.UTF8.GetBytes(url + "\0"));
				if (!flag)
				{
					flag = this.SHOUTcastUpdateTitle(text2, url);
				}
				return flag;
			}
			string text3 = base.SongTitle;
			if (string.IsNullOrEmpty(text3))
			{
				text3 = tag.ToString();
			}
			this.v2SongTitleNext = string.Empty;
			bool flag2 = BassEnc.BASS_Encode_CastSetTitle(base.Encoder.EncoderHandle, this._encoding.GetBytes(text3 + "\0"), string.IsNullOrEmpty(url) ? null : this._encoding.GetBytes(url + "\0"));
			if (!flag2)
			{
				flag2 = this.SHOUTcastUpdateTitle(text3, url);
			}
			return flag2;
		}

		public void UpdateStationArtwork(string stationArtwork)
		{
			this.v2StationArtwork = stationArtwork;
			this.SHOUTcastv2UpdateStationArtwork();
		}

		public override int GetListeners(string password)
		{
			int result = -1;
			try
			{
				if (password == null)
				{
					password = this.AdminUsername + ":" + this.AdminPassword;
				}
				else if (password.IndexOf(':') < 0)
				{
					password = this.AdminUsername + ":" + password;
				}
				string text;
				if (base.UseBASS && this.IsConnected)
				{
					text = BassEnc.BASS_Encode_CastGetStats(base.Encoder.EncoderHandle, BASSEncodeStats.BASS_ENCODE_STATS_SHOUT, password);
				}
				else
				{
					text = this.SHOUTcastGetStats(password);
				}
				if (text != null)
				{
					int num = text.ToUpper().IndexOf("<CURRENTLISTENERS>");
					int num2 = text.ToUpper().IndexOf("</CURRENTLISTENERS>");
					if (num > 0 && num2 > 0)
					{
						num += 18;
						result = int.Parse(text.Substring(num, num2 - num));
					}
				}
			}
			catch
			{
				result = -1;
			}
			return result;
		}

		public override string GetStats(string password)
		{
			string result = null;
			try
			{
				if (password == null)
				{
					password = this.AdminUsername + ":" + this.AdminPassword;
				}
				else if (password.IndexOf(':') < 0)
				{
					password = this.AdminUsername + ":" + password;
				}
				if (base.UseBASS && this.IsConnected)
				{
					result = BassEnc.BASS_Encode_CastGetStats(base.Encoder.EncoderHandle, BASSEncodeStats.BASS_ENCODE_STATS_SHOUT, password);
				}
				else
				{
					result = this.SHOUTcastGetStats(password);
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		private void EncoderNotifyProc(int handle, BASSEncodeNotify status, IntPtr user)
		{
			if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_CAST_TIMEOUT)
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_SendingData;
				base.LastErrorMessage = "Data sending timeout!";
			}
			if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_ENCODER)
			{
				this._isConnected = false;
				base.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				base.LastErrorMessage = "Encoder died!";
				return;
			}
			if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_CAST)
			{
				this._isConnected = false;
				base.LastError = StreamingServer.STREAMINGERROR.Error_SendingData;
				base.LastErrorMessage = "Connection to the server died!";
			}
		}

		private Socket CreateSocket(string serveraddress, int port)
		{
			Socket socket = null;
			try
			{
				if (serveraddress.StartsWith("http://"))
				{
					serveraddress.Substring(7);
				}
				IPAddress[] ipfromHost = StreamingServer.GetIPfromHost(serveraddress);
				if (ipfromHost != null)
				{
					foreach (IPAddress address in ipfromHost)
					{
						try
						{
							IPEndPoint ipendPoint = new IPEndPoint(address, port);
							Socket socket2 = new Socket(ipendPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
							socket2.Connect(ipendPoint);
							if (socket2.Connected)
							{
								socket = socket2;
								socket.SendTimeout = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_ENCODE_CAST_TIMEOUT);
								break;
							}
						}
						catch (Exception ex)
						{
							base.LastError = StreamingServer.STREAMINGERROR.Error_CreatingConnection;
							base.LastErrorMessage = ex.Message;
							socket = null;
						}
					}
				}
			}
			catch (Exception ex2)
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_ResolvingServerAddress;
				base.LastErrorMessage = ex2.Message;
				socket = null;
			}
			return socket;
		}

		private bool SHOUTcastLogin()
		{
			if (this._socket == null)
			{
				return false;
			}
			string text = this.SendAndReceive(this._socket, this.Password);
			return text != null && text.IndexOf("OK2") >= 0;
		}

		private bool SHOUTcastInit()
		{
			if (this._socket == null)
			{
				return false;
			}
			string text = "audio/mpeg";
			if (base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_AAC)
			{
				text = "audio/aacp";
			}
			string command = string.Format("icy-name:{0}\r\nicy-genre:{1}\r\nicy-pub:{2}\r\nicy-br:{3}\r\nicy-url:{4}\r\nicy-irc:{5}\r\nicy-icq:{6}\r\nicy-aim:{7}\r\ncontent-type: {8}\r\nicy-reset: 1\r\n\r\n", new object[]
			{
				this.StationName,
				this.Genre,
				this.PublicFlag ? "1" : "0",
				base.Encoder.EffectiveBitrate,
				this.Url,
				this.Irc,
				this.Icq,
				this.Aim,
				text
			});
			return this.SendCommand(this._socket, command);
		}

		private bool SendCommand(Socket socket, string command)
		{
			return this.SendCommand(socket, command, this._encoding);
		}

		private bool SendCommand(Socket socket, string command, Encoding encoding)
		{
			if (socket == null || command == null)
			{
				return false;
			}
			if (!command.EndsWith("\r\n"))
			{
				command += "\r\n";
			}
			int i = 0;
			try
			{
				byte[] bytes = encoding.GetBytes(command);
				int num = 0;
				int num2 = bytes.Length;
				while (i < num2)
				{
					i = socket.Send(bytes, num, num2 - num, SocketFlags.None);
					num += i;
				}
			}
			catch
			{
				return false;
			}
			return i > 0;
		}

		private string SendAndReceive(Socket socket, string request)
		{
			return this.SendAndReceive(socket, request, this._encoding);
		}

		private string SendAndReceive(Socket socket, string request, Encoding encoding)
		{
			if (socket == null || request == null)
			{
				return null;
			}
			if (!request.EndsWith("\r\n"))
			{
				request += "\r\n";
			}
			string result = null;
			try
			{
				byte[] bytes = encoding.GetBytes(request);
				int i = 0;
				int num = 0;
				int num2 = bytes.Length;
				while (i < num2)
				{
					i = socket.Send(bytes, num, num2 - num, SocketFlags.None);
					num += i;
				}
				result = string.Empty;
				DateTime now = DateTime.Now;
				byte[] array;
				for (;;)
				{
					array = new byte[socket.Available];
					if (socket.Receive(array, 0, array.Length, SocketFlags.None) > 0)
					{
						break;
					}
					Thread.Sleep(100);
					if (DateTime.Now - now > TimeSpan.FromSeconds(5.0))
					{
						goto IL_AC;
					}
				}
				result = encoding.GetString(array);
				IL_AC:;
			}
			catch
			{
				return null;
			}
			return result;
		}

		private bool SHOUTcastUpdateTitle(string song, string url)
		{
			bool result = false;
			Socket socket = null;
			try
			{
				string text = "GET /admin.cgi?";
				if (this.UseSHOUTcastv2)
				{
					text += string.Format("sid={0}&", Uri.EscapeDataString(this.SID));
				}
				if (string.IsNullOrEmpty(url))
				{
					text += string.Format("pass={0}&mode=updinfo&song={1} HTTP/1.0\r\nUser-Agent: {2} (Mozilla Compatible)\r\n\r\n", Uri.EscapeDataString(this.AdminPassword), Uri.EscapeDataString(song), Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT));
				}
				else
				{
					text += string.Format("pass={0}&mode=updinfo&song={1}&Url={2} HTTP/1.0\r\nUser-Agent: {3} (Mozilla Compatible)\r\n\r\n", new object[]
					{
						Uri.EscapeDataString(this.AdminPassword),
						Uri.EscapeDataString(song),
						(url == null) ? "" : Uri.EscapeUriString(url),
						Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT)
					});
				}
				socket = this.CreateSocket(this.ServerAddress, this.ServerPort);
				if (socket != null)
				{
					Encoding encoding = this._encoding;
					if (base.ForceUTF8TitleUpdates || this.UseSHOUTcastv2)
					{
						encoding = Encoding.UTF8;
					}
					if (this.SendCommand(socket, text, encoding))
					{
						result = true;
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
					socket = null;
				}
			}
			this.v2SongTitleNext = string.Empty;
			return result;
		}

		private string SHOUTcastGetStats(string password)
		{
			string text = null;
			Socket socket = null;
			if (password == null)
			{
				password = this.AdminUsername + ":" + this.AdminPassword;
			}
			else if (password.IndexOf(':') < 0)
			{
				password = this.AdminUsername + ":" + password;
			}
			try
			{
				string text2 = "GET /admin.cgi?";
				if (this.UseSHOUTcastv2)
				{
					text2 += string.Format("sid={0}&", Uri.EscapeDataString(this.SID));
				}
				text2 += string.Format("mode=viewxml HTTP/1.0\r\nUser-Agent: {0} (Mozilla Compatible)\r\nAuthorization: Basic {1}\r\n\r\n", Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT), this.Base64String(password));
				socket = this.CreateSocket(this.ServerAddress, this.ServerPort);
				if (socket != null)
				{
					Encoding encoding = this._encoding;
					if (base.ForceUTF8TitleUpdates)
					{
						encoding = Encoding.UTF8;
					}
					text = this.SendAndReceive(socket, text2, encoding);
					if (text != null)
					{
						int num = text.ToUpper().IndexOf("<?XML");
						if (num >= 0)
						{
							text = text.Substring(num);
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
					socket = null;
				}
			}
			return text;
		}

		private string Base64String(string s)
		{
			return Convert.ToBase64String(this._encoding.GetBytes(s));
		}

		private void SHOUTcastv2UpdateStationArtwork()
		{
			if (this.IsConnected && this.UseSHOUTcastv2 && this.v2SendArtwork && !string.IsNullOrEmpty(this.v2StationArtwork))
			{
				Task.Factory.StartNew(delegate()
				{
					try
					{
						BASSEncodeMetaDataType bassencodeMetaDataType = (BASSEncodeMetaDataType)0;
						byte[] array = null;
						TagPicture tagPicture = new TagPicture(this.v2StationArtwork, TagPicture.PICTURE_TYPE.Location, this.StationName);
						if (tagPicture != null)
						{
							tagPicture = new TagPicture(tagPicture, 300);
							if (tagPicture.PictureStorage == TagPicture.PICTURE_STORAGE.Internal)
							{
								if (tagPicture.Data.Length <= 523680)
								{
									array = tagPicture.Data;
								}
							}
							else
							{
								try
								{
									using (Stream stream = new FileStream(Encoding.UTF8.GetString(tagPicture.Data), FileMode.Open, FileAccess.Read))
									{
										if (stream.Length <= 523680L)
										{
											byte[] array2 = new byte[stream.Length];
											stream.Read(array2, 0, (int)stream.Length);
											array = array2;
										}
										stream.Close();
									}
								}
								catch
								{
								}
							}
							string mimetype = tagPicture.MIMEType;
							if (!(mimetype == "image/jpeg"))
							{
								if (!(mimetype == "image/bmp"))
								{
									if (!(mimetype == "image/png"))
									{
										if (mimetype == "image/gif")
										{
											bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_STATIONLOGO_GIF;
										}
									}
									else
									{
										bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_STATIONLOGO_PNG;
									}
								}
								else
								{
									bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_STATIONLOGO_BMP;
								}
							}
							else
							{
								bassencodeMetaDataType = BASSEncodeMetaDataType.BASS_METADATA_BIN_STATIONLOGO_JPG;
							}
						}
						if (bassencodeMetaDataType > (BASSEncodeMetaDataType)0 && array != null)
						{
							BassEnc.BASS_Encode_CastSendMeta(base.Encoder.EncoderHandle, bassencodeMetaDataType, array);
						}
					}
					catch
					{
					}
				});
			}
		}

		public string ServerAddress = "localhost";

		public int ServerPort = 8000;

		public string SID = string.Empty;

		public bool v2SendSongTitleOnly;

		public bool v2SendArtwork;

		public string v2StreamArtwork = string.Empty;

		public string v2StationArtwork = string.Empty;

		public string v2SongTitleNext = string.Empty;

		public string Username = string.Empty;

		public string Password = "changeme";

		private string _adminUsername = string.Empty;

		private string _adminPassword = string.Empty;

		public string StationName = "Your Station Name";

		public string Genre = "Genre1 Genre2";

		public bool PublicFlag = true;

		public string Url = "http://www.shoutcast.com";

		public string Irc = "N/A";

		public string Icq = "N/A";

		public string Aim = "N/A";

		private Socket _socket;

		private bool _isConnected;

		private ENCODENOTIFYPROC _myNotifyProc;

		private bool _loggedIn;

		private byte[] _data;

		private object _lock = false;

		private Encoding _encoding = Encoding.GetEncoding(1252);

		private int _retrycount = 3;
	}
}
