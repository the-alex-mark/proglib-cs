using System;
using System.ComponentModel;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public abstract class BaseDSP : IDisposable
	{
		public BaseDSP()
		{
			this._dspProc = new DSPPROC(this.DSPCallback);
			if (Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_FLOATDSP) == 1)
			{
				this._bitwidth = 32;
			}
		}

		public BaseDSP(int channel, int priority, IntPtr user) : this()
		{
			this._channel = channel;
			this._dspPriority = priority;
			this._user = user;
			this.GetChannelInfo(channel);
			this.Start();
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
					this.Stop();
				}
				catch
				{
				}
			}
			this.disposed = true;
		}

		~BaseDSP()
		{
			this.Dispose(false);
		}

		public event EventHandler Notification;

		public int ChannelHandle
		{
			get
			{
				return this._channel;
			}
			set
			{
				if (this._channel == value)
				{
					return;
				}
				this.GetChannelInfo(value);
				if (this._dspHandle != 0)
				{
					this.ReAssign(this._channel, value);
				}
				this._channel = value;
				this.OnChannelChanged();
			}
		}

		public BASS_CHANNELINFO ChannelInfo
		{
			get
			{
				return this._channelInfo;
			}
		}

		public int ChannelBitwidth
		{
			get
			{
				return this._bitwidth;
			}
		}

		public int ChannelSampleRate
		{
			get
			{
				return this._samplerate;
			}
		}

		public int ChannelNumChans
		{
			get
			{
				return this._numchans;
			}
		}

		public int DSPPriority
		{
			get
			{
				return this._dspPriority;
			}
			set
			{
				if (this._dspPriority == value)
				{
					return;
				}
				this._dspPriority = value;
				if (this._dspHandle != 0)
				{
					this.ReAssign(this._channel, this._channel);
				}
			}
		}

		public IntPtr User
		{
			get
			{
				return this._user;
			}
			set
			{
				this._user = value;
			}
		}

		public int DSPHandle
		{
			get
			{
				return this._dspHandle;
			}
		}

		public DSPPROC DSPProc
		{
			get
			{
				return this._dspProc;
			}
		}

		public bool IsBypassed
		{
			get
			{
				return this._bypass;
			}
		}

		public bool IsAssigned
		{
			get
			{
				if (this._dspHandle == 0 || this._channel == 0)
				{
					return false;
				}
				if (Bass.BASS_ChannelFlags(this._channel, BASSFlag.BASS_DEFAULT, BASSFlag.BASS_DEFAULT) == (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_UNICODE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960))
				{
					this._dspHandle = 0;
					this._channel = 0;
					return false;
				}
				return true;
			}
		}

		public bool Start()
		{
			if (this.IsAssigned)
			{
				return true;
			}
			this._dspHandle = Bass.BASS_ChannelSetDSP(this._channel, this._dspProc, this._user, this._dspPriority);
			this.OnStarted();
			return this._dspHandle != 0;
		}

		public bool Stop()
		{
			bool result = Bass.BASS_ChannelRemoveDSP(this._channel, this._dspHandle);
			this._dspHandle = 0;
			this.OnStopped();
			return result;
		}

		public void SetBypass(bool bypass)
		{
			this._bypass = bypass;
			this.OnBypassChanged();
		}

		public virtual void OnChannelChanged()
		{
		}

		public virtual void OnStarted()
		{
		}

		public virtual void OnStopped()
		{
		}

		public virtual void OnBypassChanged()
		{
		}

		public abstract void DSPCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user);

		public void RaiseNotification()
		{
			if (this.Notification != null)
			{
				this.ProcessDelegate(this.Notification, new object[]
				{
					this,
					EventArgs.Empty
				});
			}
		}

		public abstract override string ToString();

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

		private void GetChannelInfo(int channel)
		{
			if (channel == 0)
			{
				return;
			}
			if (!Bass.BASS_ChannelGetInfo(channel, this._channelInfo))
			{
				throw new ArgumentException("Invalid channel: " + Enum.GetName(typeof(BASSError), Bass.BASS_ErrorGetCode()));
			}
			this._samplerate = this._channelInfo.freq;
			this._numchans = this._channelInfo.chans;
			if ((this._channelInfo.flags & BASSFlag.BASS_SAMPLE_MONO) != BASSFlag.BASS_DEFAULT)
			{
				this._numchans = 1;
			}
			this._bitwidth = 16;
			bool flag = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_FLOATDSP) == 1;
			if ((this._channelInfo.flags & BASSFlag.BASS_SAMPLE_FLOAT) > BASSFlag.BASS_DEFAULT || flag)
			{
				this._bitwidth = 32;
				return;
			}
			if ((this._channelInfo.flags & BASSFlag.BASS_SAMPLE_8BITS) != BASSFlag.BASS_DEFAULT)
			{
				this._bitwidth = 8;
			}
		}

		private void ReAssign(int oldChannel, int newChannel)
		{
			Bass.BASS_ChannelRemoveDSP(oldChannel, this._dspHandle);
			this._dspHandle = Bass.BASS_ChannelSetDSP(newChannel, this._dspProc, this._user, this._dspPriority);
		}

		private bool disposed;

		private int _channel;

		private int _bitwidth = 16;

		private int _samplerate = 44100;

		private int _numchans = 2;

		private BASS_CHANNELINFO _channelInfo = new BASS_CHANNELINFO();

		private IntPtr _user = IntPtr.Zero;

		private int _dspPriority;

		private int _dspHandle;

		private DSPPROC _dspProc;

		private bool _bypass;
	}
}
