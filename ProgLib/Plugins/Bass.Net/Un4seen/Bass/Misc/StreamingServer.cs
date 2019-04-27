using System;
using System.Net;
using System.Security;
using Un4seen.Bass.AddOn.Tags;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public abstract class StreamingServer : IStreamingServer, IDisposable
	{
		public StreamingServer(IBaseEncoder encoder)
		{
			this._encoder = encoder;
			this._useBASS = true;
			if (this._encoder == null)
			{
				throw new ArgumentNullException("encoder", "No encoder specified!");
			}
			if (!this._encoder.EncoderExists)
			{
				throw new ArgumentException("Encoder does NOT exist!");
			}
		}

		public StreamingServer(IBaseEncoder encoder, bool useBASS)
		{
			this._encoder = encoder;
			this._useBASS = useBASS;
			if (this._encoder == null)
			{
				throw new ArgumentNullException("encoder", "No encoder specified!");
			}
			if (!this._encoder.EncoderExists)
			{
				throw new ArgumentException("Encoder does NOT exist!");
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					this.Disconnect();
					if (this.Encoder != null && this.Encoder.IsActive)
					{
						this.Encoder.Stop();
					}
				}
				catch
				{
				}
			}
			this.disposed = true;
		}

		~StreamingServer()
		{
			this.Dispose(false);
		}

		protected static IPAddress[] GetIPfromHost(string hostname)
		{
			IPAddress ipaddress = null;
			if (IPAddress.TryParse(hostname, out ipaddress))
			{
				return new IPAddress[]
				{
					ipaddress
				};
			}
			IPAddress[] result = null;
			try
			{
				IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
				if (hostEntry != null && hostEntry.AddressList != null && hostEntry.AddressList.Length != 0)
				{
					result = hostEntry.AddressList;
				}
				else
				{
					result = Dns.GetHostAddresses(hostname);
				}
			}
			catch
			{
			}
			return result;
		}

		public bool UseBASS
		{
			get
			{
				return this._useBASS;
			}
		}

		public string SongTitle
		{
			get
			{
				return this._songTitle;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this._songTitle = value;
			}
		}

		public string SongUrl
		{
			get
			{
				return this._songUrl;
			}
			set
			{
				this._songUrl = value;
			}
		}

		public bool ForceUTF8TitleUpdates
		{
			get
			{
				return this._forceUTF8TitleUpdates;
			}
			set
			{
				this._forceUTF8TitleUpdates = value;
			}
		}

		public abstract bool IsConnected { get; }

		public IBaseEncoder Encoder
		{
			get
			{
				return this._encoder;
			}
		}

		public StreamingServer.STREAMINGERROR LastError
		{
			get
			{
				return this._lastError;
			}
			set
			{
				this._lastError = value;
			}
		}

		public string LastErrorMessage
		{
			get
			{
				return this._lastErrorMsg;
			}
			set
			{
				this._lastErrorMsg = value;
			}
		}

		public abstract bool Connect();

		public abstract bool Disconnect();

		public abstract bool Login();

		public abstract int SendData(IntPtr buffer, int length);

		public abstract bool UpdateTitle(string song, string url);

		public virtual bool UpdateTitle(TAG_INFO tag, string url)
		{
			return this.UpdateTitle(tag.ToString(), url);
		}

		public virtual int GetListeners(string password)
		{
			return -1;
		}

		public virtual string GetStats(string password)
		{
			return null;
		}

		private bool disposed;

		private IBaseEncoder _encoder;

		private StreamingServer.STREAMINGERROR _lastError;

		private string _lastErrorMsg = string.Empty;

		private string _songTitle = "radio42";

		private string _songUrl;

		private bool _useBASS = true;

		private bool _forceUTF8TitleUpdates;

		public enum STREAMINGERROR
		{
			Ok,
			Error_ResolvingServerAddress = 100,
			Error_CreatingConnection,
			Error_SendingData,
			Error_EncoderError,
			Error_Login,
			Error_Disconnect,
			Error_NotConnected,
			Warning_LessDataSend = 201,
			Unknown = -1
		}
	}
}
