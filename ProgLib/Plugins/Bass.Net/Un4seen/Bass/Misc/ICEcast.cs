using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class ICEcast : StreamingServer
	{
		public ICEcast(IBaseEncoder encoder) : base(encoder)
		{
			if (encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_OGG && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_MP3 && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_AAC && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_OPUS)
			{
				throw new ArgumentException("Invalid EncoderType (only OGG, MP3, AAC,FLAC_OGG and OPUS is supported)!");
			}
		}

		public ICEcast(IBaseEncoder encoder, bool useBASS) : base(encoder, useBASS)
		{
			if (encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_OGG && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_MP3 && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_AAC && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG && encoder.EncoderType != BASSChannelType.BASS_CTYPE_STREAM_OPUS)
			{
				throw new ArgumentException("Invalid EncoderType (only OGG, MP3, AAC, FLAC_OGG and OPUS is supported)!");
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
				this._socket = this.CreateSocket(this.ServerAddress, this.ServerPort);
				return this._socket != null && this._socket.Connected;
			}
			if (this._isConnected)
			{
				return true;
			}
			string content = BassEnc.BASS_ENCODE_TYPE_OGG;
			if (base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_MP3)
			{
				content = BassEnc.BASS_ENCODE_TYPE_MP3;
			}
			else if (base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_AAC)
			{
				content = BassEnc.BASS_ENCODE_TYPE_AAC;
			}
			string pass = this.Password;
			if (!string.IsNullOrEmpty(this.Username) && this.Username.ToLower() != "source")
			{
				pass = this.Username + ":" + this.Password;
			}
			if (BassEnc.BASS_Encode_CastInit(base.Encoder.EncoderHandle, string.Format("{0}:{1}{2}", this.ServerAddress, this.ServerPort, this.MountPoint), pass, content, this.StreamName, this.StreamUrl, this.StreamGenre, this.StreamDescription, (this.Quality == null) ? null : string.Format("ice-bitrate: {0}\r\n", this.Quality), (this.Quality == null) ? base.Encoder.EffectiveBitrate : 0, this.PublicFlag))
			{
				this._myNotifyProc = new ENCODENOTIFYPROC(this.EncoderNotifyProc);
				this._isConnected = BassEnc.BASS_Encode_SetNotify(base.Encoder.EncoderHandle, this._myNotifyProc, IntPtr.Zero);
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
								base.LastErrorMessage = "Encoder not active or invalif Encoder used!";
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
			if (this.ICEcastLogin())
			{
				result = true;
				this._loggedIn = true;
				base.LastError = StreamingServer.STREAMINGERROR.Ok;
				base.LastErrorMessage = string.Empty;
			}
			else
			{
				base.LastError = StreamingServer.STREAMINGERROR.Error_Login;
				base.LastErrorMessage = "Invalid username or password. Server could not be initialized.";
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
			base.SongTitle = song.Trim(new char[1]).Replace('\0', ' ');
			if (!string.IsNullOrEmpty(url))
			{
				url = url.Trim(new char[1]).Replace('\0', ' ');
			}
			if (string.IsNullOrEmpty(base.SongTitle))
			{
				return false;
			}
			if (base.UseBASS && this.IsConnected)
			{
				Encoding encoding;
				if (base.ForceUTF8TitleUpdates || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_OGG || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_OPUS)
				{
					encoding = Encoding.UTF8;
				}
				else
				{
					encoding = Encoding.GetEncoding("latin1");
				}
				return BassEnc.BASS_Encode_CastSetTitle(base.Encoder.EncoderHandle, encoding.GetBytes(base.SongTitle + "\0"), null);
			}
			return this.ICEcastUpdateTitle(base.SongTitle);
		}

		public bool UpdateArtistTitle(string artist, string title)
		{
			base.SongTitle = string.Format("{0} - {1}", artist, title);
			return this.ICEcastUpdateTitle(artist, title);
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
					text = BassEnc.BASS_Encode_CastGetStats(base.Encoder.EncoderHandle, BASSEncodeStats.BASS_ENCODE_STATS_ICE, password);
				}
				else
				{
					text = this.ICEcastGetStats(password, BASSEncodeStats.BASS_ENCODE_STATS_ICE);
				}
				if (text != null)
				{
					int num = text.ToUpper().IndexOf("<LISTENERS>");
					int num2 = text.ToUpper().IndexOf("</LISTENERS>");
					if (num > 0 && num2 > 0)
					{
						num += 11;
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
					result = BassEnc.BASS_Encode_CastGetStats(base.Encoder.EncoderHandle, BASSEncodeStats.BASS_ENCODE_STATS_ICESERV, password);
				}
				else
				{
					result = this.ICEcastGetStats(password, BASSEncodeStats.BASS_ENCODE_STATS_ICESERV);
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
				this.Disconnect();
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

		private bool ICEcastLogin()
		{
			if (this._socket == null)
			{
				return false;
			}
			string text = BassEnc.BASS_ENCODE_TYPE_OGG;
			if (base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_MP3)
			{
				text = BassEnc.BASS_ENCODE_TYPE_MP3;
			}
			else if (base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_AAC)
			{
				text = BassEnc.BASS_ENCODE_TYPE_AAC;
			}
			string text2 = this.Username;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "source";
			}
			string request = string.Format("SOURCE {0} ICE/1.0\r\ncontent-type: {1}\r\nAuthorization: Basic {2}\r\nice-name: {3}\r\nice-url: {4}\r\nice-genre: {5}\r\nice-bitrate: {6}\r\nice-private: {7}\r\nice-public: {8}\r\nice-description: {9}\r\nice-audio-info: ice-samplerate={10};ice-bitrate={11};ice-channels={12}\r\n\r\n", new object[]
			{
				this.MountPoint,
				text,
				this.Base64String(text2 + ":" + this.Password),
				this.StreamName,
				this.StreamUrl,
				this.StreamGenre,
				(this.Quality == null) ? base.Encoder.EffectiveBitrate.ToString() : this.Quality,
				this.PublicFlag ? "0" : "1",
				this.PublicFlag ? "1" : "0",
				this.StreamDescription,
				base.Encoder.ChannelSampleRate,
				base.Encoder.EffectiveBitrate,
				base.Encoder.ChannelNumChans
			});
			string text3 = this.SendAndReceive(this._socket, request);
			return text3 != null && text3.IndexOf("200 OK") >= 0;
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
				byte[] bytes = this._encoding.GetBytes(request);
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
						goto IL_B6;
					}
				}
				result = this._encoding.GetString(array);
				IL_B6:;
			}
			catch
			{
				return null;
			}
			return result;
		}

		private bool ICEcastUpdateTitle(string song)
		{
			bool result = false;
			Socket socket = null;
			try
			{
				string command = string.Format("GET /admin/metadata?mount={0}&mode=updinfo&password={1}&song={2} HTTP/1.0\r\nUser-Agent: {3} (Mozilla Compatible)\r\nAuthorization: Basic {4}\r\nHost: {5}\r\n\r\n", new object[]
				{
					Uri.EscapeDataString(this.MountPoint),
					this.AdminPassword,
					Uri.EscapeDataString(song),
					Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT),
					this.Base64String(this.AdminUsername + ":" + this.AdminPassword),
					this.ServerAddress
				});
				socket = this.CreateSocket(this.ServerAddress, this.ServerPort);
				if (socket != null)
				{
					Encoding encoding;
					if (base.ForceUTF8TitleUpdates || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_OGG || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_OPUS)
					{
						encoding = Encoding.UTF8;
					}
					else
					{
						encoding = Encoding.GetEncoding("latin1");
					}
					if (this.SendCommand(socket, command, encoding))
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
			return result;
		}

		private bool ICEcastUpdateTitle(string artist, string title)
		{
			bool result = false;
			Socket socket = null;
			try
			{
				string command = string.Format("GET /admin/metadata?mount={0}&mode=updinfo&password={1}&artist={2}&title={3} HTTP/1.0\r\nUser-Agent: {4} (Mozilla Compatible)\r\nAuthorization: Basic {5}\r\nHost: {6}\r\n\r\n", new object[]
				{
					Uri.EscapeDataString(this.MountPoint),
					this.AdminPassword,
					Uri.EscapeDataString(artist),
					Uri.EscapeDataString(title),
					Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT),
					this.Base64String(this.AdminUsername + ":" + this.AdminPassword),
					this.ServerAddress
				});
				socket = this.CreateSocket(this.ServerAddress, this.ServerPort);
				if (socket != null)
				{
					Encoding encoding;
					if (base.ForceUTF8TitleUpdates || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_OGG || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG || base.Encoder.EncoderType == BASSChannelType.BASS_CTYPE_STREAM_OPUS)
					{
						encoding = Encoding.UTF8;
					}
					else
					{
						encoding = Encoding.GetEncoding("latin1");
					}
					if (this.SendCommand(socket, command, encoding))
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
			return result;
		}

		private string ICEcastGetStats(string password, BASSEncodeStats type)
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
				string request;
				if (type == BASSEncodeStats.BASS_ENCODE_STATS_ICE)
				{
					request = string.Format("GET /admin/listclients?mount={0} HTTP/1.0\r\nUser-Agent: {1}\r\nAuthorization: Basic {2}\r\n\r\n", Uri.EscapeDataString(this.MountPoint), Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT), this.Base64String(password));
				}
				else
				{
					request = string.Format("GET /admin/stats HTTP/1.0\r\nUser-Agent: {0}\r\nAuthorization: Basic {1}\r\n\r\n", Bass.BASS_GetConfigString(BASSConfig.BASS_CONFIG_NET_AGENT), this.Base64String(password));
				}
				socket = this.CreateSocket(this.ServerAddress, this.ServerPort);
				if (socket != null)
				{
					text = this.SendAndReceive(socket, request, Encoding.UTF8);
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

		public string ServerAddress = "localhost";

		public int ServerPort = 8000;

		public string MountPoint = "/stream.ogg";

		public string Username = string.Empty;

		public string Password = "hackme";

		private string _adminUsername = string.Empty;

		private string _adminPassword = string.Empty;

		public string StreamName = "Your Station Name";

		public string StreamDescription = "Your Station Description";

		public string StreamUrl = "http://www.oddsock.org";

		public string StreamGenre = "Genre1 Genre2";

		public bool PublicFlag = true;

		public string Quality;

		private Socket _socket;

		private bool _isConnected;

		private ENCODENOTIFYPROC _myNotifyProc;

		private bool _loggedIn;

		private byte[] _data;

		private object _lock = false;

		private Encoding _encoding = Encoding.GetEncoding("latin1");

		private int _retrycount = 3;
	}
}
