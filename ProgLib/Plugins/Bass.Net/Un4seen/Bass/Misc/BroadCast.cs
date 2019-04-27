using System;
using System.ComponentModel;
using System.Security;
using System.Threading;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Tags;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BroadCast
	{
		private BroadCast()
		{
		}

		public BroadCast(IStreamingServer server)
		{
			this._server = server;
			this._autoEncProc = new ENCODEPROC(this.AutoEncodingCallback);
			this._myNotifyProc = new ENCODENOTIFYPROC(this.EncoderNotifyProc);
			this._retryTimerDelegate = new TimerCallback(this.CheckStatus);
		}

		public event BroadCastEventHandler Notification;

		public IStreamingServer Server
		{
			get
			{
				return this._server;
			}
		}

		public bool IsConnected
		{
			get
			{
				return this.Server.IsConnected;
			}
		}

		public bool IsStarted
		{
			get
			{
				return this._isStarted;
			}
		}

		public BroadCast.BROADCASTSTATUS Status
		{
			get
			{
				return this._status;
			}
		}

		public bool AutomaticMode
		{
			get
			{
				return this._automaticMode;
			}
		}

		public bool AutoReconnect
		{
			get
			{
				return this._autoReconnect;
			}
			set
			{
				this._autoReconnect = value;
			}
		}

		public int ReconnectTimeout
		{
			get
			{
				return (int)this._reconnectTimeout.TotalSeconds;
			}
			set
			{
				if (value < 1)
				{
					this._reconnectTimeout = TimeSpan.FromSeconds(1.0);
				}
				else if (value > 86400)
				{
					this._reconnectTimeout = TimeSpan.FromSeconds(86400.0);
				}
				else
				{
					this._reconnectTimeout = TimeSpan.FromSeconds((double)value);
				}
				Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_ENCODE_CAST_TIMEOUT, (int)this._reconnectTimeout.TotalMilliseconds);
				if (!this.Server.UseBASS && this._retryTimer != null)
				{
					this._retryTimer.Change(this._reconnectTimeout, this._reconnectTimeout);
				}
			}
		}

		public bool NotificationSuppressDataSend
		{
			get
			{
				return this._notificationSuppressDataSend;
			}
			set
			{
				this._notificationSuppressDataSend = value;
			}
		}

		public bool NotificationSuppressIsAlive
		{
			get
			{
				return this._notificationSuppressIsAlive;
			}
			set
			{
				this._notificationSuppressIsAlive = value;
			}
		}

		public long TotalBytesSend
		{
			get
			{
				if (this.Server is WMAcast)
				{
					return ((EncoderWMA)((WMAcast)this.Server).Encoder).ByteSend;
				}
				if (this.Server.UseBASS)
				{
					return BassEnc.BASS_Encode_GetCount(this.Server.Encoder.EncoderHandle, BASSEncodeCount.BASS_ENCODE_COUNT_CAST);
				}
				return this._bytesSend;
			}
		}

		public TimeSpan TotalConnectionTime
		{
			get
			{
				return DateTime.Now - this._startTime;
			}
		}

		public bool AutoConnect()
		{
			return this.DoAutoConnect(true);
		}

		public bool Connect()
		{
			return this.DoConnect(true);
		}

		public bool Disconnect()
		{
			return this.Disconnect(false);
		}

		public bool StartEncoder(ENCODEPROC proc, IntPtr user, bool paused)
		{
			bool result = true;
			if (this.Server.Encoder.Start(proc, user, paused))
			{
				this.RaiseNotification(BroadCastEventType.EncoderStarted, this.Server.Encoder);
			}
			else
			{
				this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				this.Server.LastErrorMessage = Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode());
				BASSError basserror = Bass.BASS_ErrorGetCode();
				if (basserror <= BASSError.BASS_ERROR_ILLPARAM)
				{
					if (basserror != BASSError.BASS_ERROR_FILEOPEN)
					{
						if (basserror == BASSError.BASS_ERROR_ILLPARAM)
						{
							this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
							this.Server.LastErrorMessage = "Illegal parameters used to start encoder!";
							goto IL_18C;
						}
					}
					else
					{
						if (this.Server.Encoder is EncoderWMA)
						{
							this.Server.LastError = StreamingServer.STREAMINGERROR.Error_CreatingConnection;
							this.Server.LastErrorMessage = "Couldn't connect to the server. Check URL!";
							goto IL_18C;
						}
						this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
						this.Server.LastErrorMessage = "Couldn't start the encoder. Check that executable exists!";
						goto IL_18C;
					}
				}
				else
				{
					if (basserror == BASSError.BASS_ERROR_NOTAVAIL)
					{
						this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
						this.Server.LastErrorMessage = "Encoder codec missing!";
						goto IL_18C;
					}
					switch (basserror)
					{
					case BASSError.BASS_ERROR_WMA_WM9:
					case BASSError.BASS_ERROR_WMA_CODEC:
						this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
						this.Server.LastErrorMessage = "WMA codec missing or WMA9 required!";
						goto IL_18C;
					case BASSError.BASS_ERROR_WMA_DENIED:
						this.Server.LastError = StreamingServer.STREAMINGERROR.Error_Login;
						this.Server.LastErrorMessage = "Access denied. Check username/password!";
						goto IL_18C;
					}
				}
				this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				this.Server.LastErrorMessage = "Some mystery problem occurred when trying to start the encoder!";
				IL_18C:
				result = false;
				this.RaiseNotification(BroadCastEventType.EncoderStartError, this.Server.Encoder);
			}
			return result;
		}

		public bool StopEncoder()
		{
			this.Server.Encoder.Stop();
			if (this.Server.Encoder.IsActive)
			{
				this.RaiseNotification(BroadCastEventType.EncoderStopError, this.Server.Encoder);
				return false;
			}
			this.RaiseNotification(BroadCastEventType.EncoderStopped, this.Server.Encoder);
			return true;
		}

		public bool SendData(IntPtr buffer, int length)
		{
			if (buffer == IntPtr.Zero || length == 0 || this.Server.UseBASS)
			{
				return true;
			}
			if (!this.IsConnected || this.Server == null)
			{
				this._status = BroadCast.BROADCASTSTATUS.NotConnected;
				return false;
			}
			int num = -1;
			object @lock = this._lock;
			lock (@lock)
			{
				try
				{
					num = this.Server.SendData(buffer, length);
					if (num > 0)
					{
						this.IncrementBytesSend(num);
					}
					if (num < 0)
					{
						this.RaiseNotification(BroadCastEventType.ConnectionLost, DateTime.Now);
					}
					else if (num != length)
					{
						this.RaiseNotification(BroadCastEventType.LessDataSend, length - num);
					}
					else if (!this.NotificationSuppressDataSend)
					{
						this.RaiseNotification(BroadCastEventType.DataSend, num);
					}
				}
				catch (Exception ex)
				{
					this.Server.LastError = StreamingServer.STREAMINGERROR.Error_SendingData;
					this.Server.LastErrorMessage = ex.Message;
					this._status = BroadCast.BROADCASTSTATUS.NotConnected;
					this.Server.Disconnect();
					this.RaiseNotification(BroadCastEventType.ConnectionLost, DateTime.Now);
					return false;
				}
			}
			return num == length;
		}

		public bool UpdateTitle(string song, string url)
		{
			bool flag = this.Server.UpdateTitle(song, url);
			if (flag)
			{
				this.RaiseNotification(BroadCastEventType.TitleUpdated, song);
				return flag;
			}
			this.RaiseNotification(BroadCastEventType.TitleUpdateError, song);
			return flag;
		}

		public bool UpdateTitle(TAG_INFO tag, string url)
		{
			bool flag = this.Server.UpdateTitle(tag, url);
			if (flag)
			{
				this.RaiseNotification(BroadCastEventType.TitleUpdated, tag);
				return flag;
			}
			this.RaiseNotification(BroadCastEventType.TitleUpdateError, tag);
			return flag;
		}

		public int GetListeners(string password)
		{
			return this.Server.GetListeners(password);
		}

		public string GetStats(string password)
		{
			return this.Server.GetStats(password);
		}

		private bool DoAutoConnect(bool startTimer)
		{
			this.Server.LastError = StreamingServer.STREAMINGERROR.Ok;
			this.Server.LastErrorMessage = string.Empty;
			this._automaticMode = true;
			bool flag = false;
			object @lock = this._lock;
			lock (@lock)
			{
				flag = (!this.IsConnected || this.Disconnect());
				if (flag)
				{
					if (this.Server.UseBASS)
					{
						flag = this.StartEncoder(null, IntPtr.Zero, true);
					}
					else
					{
						flag = this.StartEncoder(this._autoEncProc, IntPtr.Zero, true);
					}
					if (flag)
					{
						flag = this.InternalConnect(startTimer);
						if (!flag)
						{
							this.StopEncoder();
							this._status = BroadCast.BROADCASTSTATUS.NotConnected;
						}
						else
						{
							this.Server.Encoder.Pause(false);
						}
					}
					else
					{
						this._status = BroadCast.BROADCASTSTATUS.NotConnected;
					}
				}
				else
				{
					this._status = BroadCast.BROADCASTSTATUS.NotConnected;
				}
				if (this.AutoReconnect)
				{
					if (this._retryTimer != null)
					{
						this._retryTimer.Change(-1, -1);
						this._retryTimer.Dispose();
						this._retryTimer = null;
					}
					if (flag && this.Server.UseBASS)
					{
						if (this.Server is WMAcast)
						{
							((EncoderWMA)this.Server.Encoder).WMA_Notify = this._myNotifyProc;
						}
						else
						{
							Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_ENCODE_CAST_TIMEOUT, (int)this._reconnectTimeout.TotalMilliseconds);
							BassEnc.BASS_Encode_SetNotify(this.Server.Encoder.EncoderHandle, this._myNotifyProc, IntPtr.Zero);
						}
					}
					else if (startTimer)
					{
						this._retryTimer = new Timer(this._retryTimerDelegate, null, this._reconnectTimeout, this._reconnectTimeout);
					}
				}
			}
			this._isStarted = (flag || this.AutoReconnect);
			return flag;
		}

		private bool DoConnect(bool startTimer)
		{
			this.Server.LastError = StreamingServer.STREAMINGERROR.Ok;
			this.Server.LastErrorMessage = string.Empty;
			this._automaticMode = false;
			bool flag = false;
			object @lock = this._lock;
			lock (@lock)
			{
				flag = (!this.IsConnected || this.Disconnect());
				if (flag)
				{
					flag = this.InternalConnect(startTimer);
					if (!flag)
					{
						this.StopEncoder();
						this._status = BroadCast.BROADCASTSTATUS.NotConnected;
					}
				}
				else
				{
					this._status = BroadCast.BROADCASTSTATUS.NotConnected;
				}
				if (this.AutoReconnect)
				{
					if (this._retryTimer != null)
					{
						this._retryTimer.Change(-1, -1);
						this._retryTimer.Dispose();
						this._retryTimer = null;
					}
					if (flag && this.Server.UseBASS)
					{
						if (this.Server is WMAcast)
						{
							((EncoderWMA)this.Server.Encoder).WMA_Notify = this._myNotifyProc;
						}
						else
						{
							Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_ENCODE_CAST_TIMEOUT, (int)this._reconnectTimeout.TotalMilliseconds);
							BassEnc.BASS_Encode_SetNotify(this.Server.Encoder.EncoderHandle, this._myNotifyProc, IntPtr.Zero);
						}
					}
					else if (startTimer)
					{
						this._retryTimer = new Timer(this._retryTimerDelegate, null, this._reconnectTimeout, this._reconnectTimeout);
					}
				}
			}
			this._isStarted = flag;
			return flag;
		}

		private bool InternalConnect(bool initial)
		{
			bool flag = false;
			if (this.Server.Connect())
			{
				if (this.Server.Login())
				{
					this._startTime = DateTime.Now;
					this._bytesSend = 0L;
					flag = true;
					this.Server.UpdateTitle(this.Server.SongTitle, this.Server.SongUrl);
					if (initial)
					{
						this.RaiseNotification(BroadCastEventType.Connected, this._startTime);
					}
					else
					{
						this.RaiseNotification(BroadCastEventType.Reconnected, this._startTime);
					}
				}
				else
				{
					this.RaiseNotification(BroadCastEventType.ConnectionError, DateTime.Now);
				}
			}
			else
			{
				this.RaiseNotification(BroadCastEventType.ConnectionError, DateTime.Now);
			}
			if (flag)
			{
				this._status = BroadCast.BROADCASTSTATUS.Connected;
			}
			else
			{
				this._status = BroadCast.BROADCASTSTATUS.NotConnected;
			}
			return flag;
		}

		private bool Disconnect(bool calledFromReconnectTry)
		{
			if (!calledFromReconnectTry)
			{
				this.Server.LastError = StreamingServer.STREAMINGERROR.Ok;
				this.Server.LastErrorMessage = string.Empty;
			}
			this._isStarted = calledFromReconnectTry;
			bool flag = false;
			object @lock = this._lock;
			lock (@lock)
			{
				flag = this.InternalDisconnect();
				if (flag)
				{
					if (!calledFromReconnectTry)
					{
						this._killAutoReconnectTry = true;
						if (this._autoReconnectTryRunning)
						{
							while (this._autoReconnectTryRunning)
							{
								Thread.Sleep(3);
							}
						}
					}
					if (this._retryTimer != null)
					{
						this._retryTimer.Change(-1, -1);
						this._retryTimer.Dispose();
						this._retryTimer = null;
					}
					if (this.Server.UseBASS)
					{
						if (this.Server is WMAcast)
						{
							((EncoderWMA)this.Server.Encoder).WMA_Notify = null;
						}
						else
						{
							BassEnc.BASS_Encode_SetNotify(this.Server.Encoder.EncoderHandle, null, IntPtr.Zero);
						}
					}
				}
			}
			return flag;
		}

		private bool InternalDisconnect()
		{
			bool result = false;
			if (!this.StopEncoder())
			{
				this.RaiseNotification(BroadCastEventType.DisconnectError, DateTime.Now);
				return false;
			}
			this._status = BroadCast.BROADCASTSTATUS.Unknown;
			if (this.Server.Disconnect())
			{
				this._status = BroadCast.BROADCASTSTATUS.NotConnected;
				this._startTime = DateTime.Now;
				this._bytesSend = 0L;
				this._lastDisconnect = DateTime.Now;
				result = true;
				this.RaiseNotification(BroadCastEventType.Disconnected, this._startTime);
			}
			else
			{
				this.RaiseNotification(BroadCastEventType.DisconnectError, DateTime.Now);
			}
			return result;
		}

		private void EncoderNotifyProc(int handle, BASSEncodeNotify status, IntPtr user)
		{
			if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_CAST_TIMEOUT)
			{
				this.Server.LastError = StreamingServer.STREAMINGERROR.Error_SendingData;
				this.Server.LastErrorMessage = "Data sending timeout!";
			}
			else if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_ENCODER)
			{
				this.Server.LastError = StreamingServer.STREAMINGERROR.Error_EncoderError;
				this.Server.LastErrorMessage = "Encoder died!";
			}
			else if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_CAST)
			{
				this.Server.LastError = StreamingServer.STREAMINGERROR.Error_NotConnected;
				this.Server.LastErrorMessage = "Connection to the server died!";
			}
			else
			{
				if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_FREE)
				{
					this.Server.LastError = StreamingServer.STREAMINGERROR.Ok;
					this.Server.LastErrorMessage = string.Empty;
					return;
				}
				if (status == BASSEncodeNotify.BASS_ENCODE_NOTIFY_QUEUE_FULL)
				{
					this.Server.LastError = StreamingServer.STREAMINGERROR.Warning_LessDataSend;
					this.Server.LastErrorMessage = "Encoding queue is out of space (some data could not be send to the server)!";
					return;
				}
				this.Server.LastError = StreamingServer.STREAMINGERROR.Unknown;
				this.Server.LastErrorMessage = "Unknown encoder status";
				return;
			}
			this.RaiseNotification(BroadCastEventType.ConnectionLost, DateTime.Now);
			if (this._retryTimer != null)
			{
				this._retryTimer.Change(-1, -1);
				this._retryTimer.Dispose();
				this._retryTimer = null;
			}
			if (!this._autoReconnectTryRunning)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.AutoReconnectTry));
			}
		}

		private void AutoEncodingCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (this.IsConnected)
			{
				this.SendData(buffer, length);
			}
		}

		private void CheckStatus(object state)
		{
			if ((bool)this._lock)
			{
				return;
			}
			this._lock = true;
			if (this.IsConnected && this.Server.Encoder.IsActive)
			{
				if (!this.NotificationSuppressIsAlive)
				{
					this.RaiseNotification(BroadCastEventType.IsAlive, DateTime.Now);
				}
				this._lock = false;
				return;
			}
			if (this._retryTimer != null)
			{
				this._retryTimer.Change(-1, -1);
				this._retryTimer.Dispose();
				this._retryTimer = null;
			}
			if (!this._autoReconnectTryRunning)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.AutoReconnectTry));
			}
			this._lock = false;
		}

		private void AutoReconnectTry(object state)
		{
			this.Disconnect(true);
			this._autoReconnectTryRunning = true;
			this._killAutoReconnectTry = false;
			if (this.AutoReconnect)
			{
				Thread.Sleep(this._reconnectTimeout);
			}
			if (this._killAutoReconnectTry)
			{
				this._autoReconnectTryRunning = false;
				return;
			}
			bool flag;
			do
			{
				flag = true;
				if (this.AutoReconnect && !this._killAutoReconnectTry)
				{
					this._lastReconnectTry = DateTime.Now;
					this.RaiseNotification(BroadCastEventType.ReconnectTry, this._lastReconnectTry);
					if (this.AutomaticMode)
					{
						flag = this.DoAutoConnect(false);
					}
					else
					{
						flag = this.DoConnect(false);
					}
					if (!flag)
					{
						this.RaiseNotification(BroadCastEventType.UnsuccessfulReconnectTry, this._lastReconnectTry);
						if (!this.AutomaticMode)
						{
							this.RaiseNotification(BroadCastEventType.EncoderRestartRequired, this.Server.Encoder);
						}
						if (!this._killAutoReconnectTry)
						{
							Thread.Sleep(this._reconnectTimeout);
						}
					}
				}
			}
			while (!flag);
			this._autoReconnectTryRunning = false;
		}

		private void RaiseNotification(BroadCastEventType pEventType, object pData)
		{
			if (this.Notification != null)
			{
				this.ProcessDelegate(this.Notification, new object[]
				{
					this,
					new BroadCastEventArgs(pEventType, pData)
				});
			}
		}

		private void ProcessDelegate(Delegate del, params object[] args)
		{
			if (del == null)
			{
				return;
			}
			foreach (Delegate del2 in del.GetInvocationList())
			{
				this.InvokeDelegate(del2, args);
			}
		}

		private void InvokeDelegate(Delegate del, object[] args)
		{
			ISynchronizeInvoke synchronizeInvoke = del.Target as ISynchronizeInvoke;
			if (synchronizeInvoke != null)
			{
				if (!synchronizeInvoke.InvokeRequired)
				{
					del.DynamicInvoke(args);
					return;
				}
				try
				{
					synchronizeInvoke.BeginInvoke(del, args);
					return;
				}
				catch
				{
					return;
				}
			}
			del.DynamicInvoke(args);
		}

		private void IncrementBytesSend(int sendData)
		{
			try
			{
				this._bytesSend += (long)sendData;
			}
			catch
			{
				this._bytesSend = (long)sendData;
			}
		}

		private IStreamingServer _server;

		private ENCODEPROC _autoEncProc;

		private ENCODENOTIFYPROC _myNotifyProc;

		private BroadCast.BROADCASTSTATUS _status;

		private bool _automaticMode;

		private bool _autoReconnect;

		private bool _isStarted;

		private bool _notificationSuppressDataSend;

		private bool _notificationSuppressIsAlive;

		private TimeSpan _reconnectTimeout = TimeSpan.FromSeconds(5.0);

		private TimerCallback _retryTimerDelegate;

		private Timer _retryTimer;

		private long _bytesSend;

		private DateTime _startTime = DateTime.Now;

		private DateTime _lastDisconnect = DateTime.Now;

		private DateTime _lastReconnectTry = DateTime.MinValue;

		private volatile object _lock = false;

		private volatile bool _autoReconnectTryRunning;

		private volatile bool _killAutoReconnectTry;

		public enum BROADCASTSTATUS
		{
			NotConnected,
			Connected,
			Unknown
		}
	}
}
