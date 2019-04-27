using System;
using System.Runtime.InteropServices;
using System.Security;
using Un4seen.Bass;

namespace Un4seen.BassAsio
{
	[SuppressUnmanagedCodeSecurity]
	public class BassAsioHandler : IDisposable
	{
		public BassAsioHandler()
		{
		}

		public BassAsioHandler(bool input, int asioDevice, int asioChannel, int asioNumChans, BASSASIOFormat asioFormat, double asioSamplerate)
		{
			this._input = input;
			this._device = asioDevice;
			this._channel = asioChannel;
			this._numchans = asioNumChans;
			this._format = asioFormat;
			BassAsio.BASS_ASIO_Init(asioDevice, BassAsioHandler.UseDedicatedThreads ? BASSASIOInit.BASS_ASIO_THREAD : BASSASIOInit.BASS_ASIO_DEFAULT);
			BassAsio.BASS_ASIO_SetDevice(asioDevice);
			if (asioSamplerate <= 0.0)
			{
				this._samplerate = BassAsio.BASS_ASIO_GetRate();
			}
			else
			{
				BassAsio.BASS_ASIO_SetRate(asioSamplerate);
				this._samplerate = BassAsio.BASS_ASIO_GetRate();
			}
			if (this._input)
			{
				this._internalAsioProc = new ASIOPROC(this.AsioInputCallback);
				this.UseInput = true;
			}
			else
			{
				this._internalAsioProc = new ASIOPROC(this.AsioOutputCallback);
			}
			bool flag = BassAsio.BASS_ASIO_IsStarted();
			BASSASIOActive bassasioactive = BassAsio.BASS_ASIO_ChannelIsActive(this._input, this._channel);
			if (flag && bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
			{
				BassAsio.BASS_ASIO_Stop();
			}
			this.EnableAndJoin(this._input, this._channel, this._numchans, this._internalAsioProc, this._format);
			if (flag)
			{
				BassAsio.BASS_ASIO_Start(0, 0);
			}
		}

		public BassAsioHandler(int asioDevice, int asioChannel, int outputChannel)
		{
			this._input = false;
			this._device = asioDevice;
			this._channel = asioChannel;
			this._outputChannel = outputChannel;
			BassAsio.BASS_ASIO_Init(asioDevice, BassAsioHandler.UseDedicatedThreads ? BASSASIOInit.BASS_ASIO_THREAD : BASSASIOInit.BASS_ASIO_DEFAULT);
			BassAsio.BASS_ASIO_SetDevice(asioDevice);
			this.GetChannelInfo(outputChannel);
			this._numchans = this._outputChannelInfo.chans;
			this._format = BASSASIOFormat.BASS_ASIO_FORMAT_16BIT;
			if ((this._outputChannelInfo.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
			{
				this._format = BASSASIOFormat.BASS_ASIO_FORMAT_FLOAT;
			}
			this._samplerate = (double)this._outputChannelInfo.freq;
			BassAsio.BASS_ASIO_SetRate(this._samplerate);
			this._internalAsioProc = new ASIOPROC(this.AsioOutputCallback);
			bool flag = BassAsio.BASS_ASIO_IsStarted();
			BASSASIOActive bassasioactive = BassAsio.BASS_ASIO_ChannelIsActive(this._input, this._channel);
			if (flag && bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
			{
				BassAsio.BASS_ASIO_Stop();
			}
			this.EnableAndJoin(this._input, this._channel, this._numchans, this._internalAsioProc, this._format);
			if (flag)
			{
				BassAsio.BASS_ASIO_Start(0, 0);
			}
		}

		private bool EnableAndJoin(bool input, int channel, int numchans, ASIOPROC proc, BASSASIOFormat format)
		{
			bool flag = true;
			if (BassAsio.BASS_ASIO_ChannelIsActive(input, channel) == BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
			{
				flag &= BassAsio.BASS_ASIO_ChannelEnable(input, channel, proc, IntPtr.Zero);
				for (int i = 1; i < numchans; i++)
				{
					BassAsio.BASS_ASIO_ChannelJoin(input, channel + i, channel);
				}
			}
			else
			{
				flag &= BassAsio.BASS_ASIO_ChannelEnable(input, channel, proc, IntPtr.Zero);
			}
			flag &= BassAsio.BASS_ASIO_ChannelSetFormat(input, channel, format);
			return flag & BassAsio.BASS_ASIO_ChannelSetRate(input, channel, this._samplerate);
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
				if (disposing)
				{
					int num = BassAsio.BASS_ASIO_GetDevice();
					if (num != this._device)
					{
						BassAsio.BASS_ASIO_SetDevice(this._device);
					}
					bool flag = BassAsio.BASS_ASIO_Stop();
					this.RemoveFullDuplex(true);
					this.RemoveMirror();
					BassAsio.BASS_ASIO_ChannelEnable(this._input, this._channel, null, IntPtr.Zero);
					BassAsio.BASS_ASIO_ChannelReset(this._input, this._channel, BASSASIOReset.BASS_ASIO_RESET_ENABLE | BASSASIOReset.BASS_ASIO_RESET_FORMAT | BASSASIOReset.BASS_ASIO_RESET_RATE | BASSASIOReset.BASS_ASIO_RESET_VOLUME);
					for (int i = 1; i < this._numchans; i++)
					{
						BassAsio.BASS_ASIO_ChannelReset(this._input, this._channel + i, BASSASIOReset.BASS_ASIO_RESET_JOIN);
					}
					if (flag)
					{
						BassAsio.BASS_ASIO_Start(0, 0);
					}
					BassAsio.BASS_ASIO_SetDevice(num);
				}
				if (this._outputChannel != 0)
				{
                    Bass.Bass.BASS_StreamFree(this._outputChannel);
					this._outputChannel = 0;
				}
				if (this._inputChannel != 0)
				{
                    Bass.Bass.BASS_StreamFree(this._inputChannel);
					this._inputChannel = 0;
				}
			}
			this.disposed = true;
		}

		~BassAsioHandler()
		{
			this.Dispose(false);
		}

		public float DeviceVolume
		{
			get
			{
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				float result = BassAsio.BASS_ASIO_ChannelGetVolume(this._input, -1);
				BassAsio.BASS_ASIO_SetDevice(num);
				return result;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				else if (value > 1f)
				{
					value = 1f;
				}
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				BassAsio.BASS_ASIO_ChannelSetVolume(this._input, -1, value);
				BassAsio.BASS_ASIO_SetDevice(num);
			}
		}

		public ASIOPROC InternalAsioProc
		{
			get
			{
				return this._internalAsioProc;
			}
		}

		public float Volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				if (this._volume == value)
				{
					return;
				}
				if (value < 0f)
				{
					this._volume = 0f;
				}
				else
				{
					this._volume = value;
				}
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				this.SetVolume(this._volume, this._pan);
				BassAsio.BASS_ASIO_SetDevice(num);
			}
		}

		public float VolumeMirror
		{
			get
			{
				return this._volumeMirror;
			}
			set
			{
				if (this._volumeMirror == value)
				{
					return;
				}
				if (value < 0f)
				{
					this._volumeMirror = 0f;
				}
				else
				{
					this._volumeMirror = value;
				}
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				this.SetVolumeMirror(this._volumeMirror, this._panMirror);
				BassAsio.BASS_ASIO_SetDevice(num);
			}
		}

		public float Pan
		{
			get
			{
				return this._pan;
			}
			set
			{
				if (this._pan == value)
				{
					return;
				}
				if (value < -1f)
				{
					this._pan = -1f;
				}
				else if (value > 1f)
				{
					this._pan = 1f;
				}
				else
				{
					this._pan = value;
				}
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				this.SetVolume(this._volume, this._pan);
				BassAsio.BASS_ASIO_SetDevice(num);
			}
		}

		public float PanMirror
		{
			get
			{
				return this._panMirror;
			}
			set
			{
				if (this._panMirror == value)
				{
					return;
				}
				if (value < -1f)
				{
					this._panMirror = -1f;
				}
				else if (value > 1f)
				{
					this._panMirror = 1f;
				}
				else
				{
					this._panMirror = value;
				}
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				this.SetVolumeMirror(this._volumeMirror, this._panMirror);
				BassAsio.BASS_ASIO_SetDevice(num);
			}
		}

		public bool IsInput
		{
			get
			{
				return this._input;
			}
		}

		public int Device
		{
			get
			{
				return this._device;
			}
		}

		public int Channel
		{
			get
			{
				return this._channel;
			}
		}

		public bool IsInputFullDuplex
		{
			get
			{
				return this._input && this._fullDuplex;
			}
		}

		public int FullDuplexDevice
		{
			get
			{
				return this._fullDuplexDevice;
			}
		}

		public int FullDuplexChannel
		{
			get
			{
				return this._fullDuplexChannel;
			}
		}

		public bool BypassFullDuplex
		{
			get
			{
				return this._bypassFullDuplex;
			}
			set
			{
				this._bypassFullDuplex = value;
			}
		}

		public bool IsMirrored
		{
			get
			{
				return this._mirrorChannel != -1;
			}
		}

		public int MirrorChannel
		{
			get
			{
				return this._mirrorChannel;
			}
		}

		public double SampleRate
		{
			get
			{
				return this._samplerate;
			}
			set
			{
				if (this._samplerate == value)
				{
					return;
				}
				if (value > 0.0)
				{
					this._samplerate = value;
				}
				BassAsio.BASS_ASIO_ChannelSetRate(this._input, this._channel, this._samplerate);
			}
		}

		public int ChannelNumChans
		{
			get
			{
				return this._numchans;
			}
		}

		public BASSASIOFormat Format
		{
			get
			{
				return this._format;
			}
			set
			{
				this._format = value;
				BassAsio.BASS_ASIO_ChannelSetFormat(this._input, this._channel, this._format);
			}
		}

		public int OutputChannel
		{
			get
			{
				return this._outputChannel;
			}
			set
			{
				this._outputChannel = value;
			}
		}

		public bool IsResampling
		{
			get
			{
				bool flag = false;
				int num = BassAsio.BASS_ASIO_GetDevice();
				if (num != this._device)
				{
					BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				double num2 = BassAsio.BASS_ASIO_ChannelGetRate(this._input, this._channel);
				if (num2 != 0.0)
				{
					flag = (BassAsio.BASS_ASIO_GetRate() != num2);
				}
				if (!flag && this.IsInputFullDuplex)
				{
					if (this._device != this._fullDuplexDevice)
					{
						BassAsio.BASS_ASIO_SetDevice(this._fullDuplexDevice);
					}
					double num3 = BassAsio.BASS_ASIO_ChannelGetRate(false, this._fullDuplexChannel);
					if (num3 != 0.0)
					{
						double num4 = BassAsio.BASS_ASIO_GetRate();
						flag &= (num4 != num3);
					}
				}
				BassAsio.BASS_ASIO_SetDevice(num);
				return flag;
			}
		}

		public int InputChannel
		{
			get
			{
				return this._inputChannel;
			}
		}

		public bool UseInput
		{
			get
			{
				return this._useInputChannel;
			}
			set
			{
				if (!this.IsInput)
				{
					this._useInputChannel = false;
					return;
				}
				this._useInputChannel = value;
				if (this._inputChannel != 0)
				{
                    Bass.Bass.BASS_StreamFree(this._inputChannel);
					this._inputChannel = 0;
				}
				if (this._useInputChannel)
				{
					int num = BassAsio.BASS_ASIO_GetDevice();
					if (num != this._device)
					{
						BassAsio.BASS_ASIO_SetDevice(this._device);
					}
					this._inputChannel = Bass.Bass.BASS_StreamCreateDummy((int)this._samplerate, this._numchans, ((this._format == BASSASIOFormat.BASS_ASIO_FORMAT_FLOAT) ? BASSFlag.BASS_SAMPLE_FLOAT : BASSFlag.BASS_DEFAULT) | BASSFlag.BASS_STREAM_DECODE, IntPtr.Zero);
					BassAsio.BASS_ASIO_SetDevice(num);
				}
			}
		}

		public bool Start(int buflen, int threads)
		{
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (flag && !BassAsio.BASS_ASIO_IsStarted())
			{
				flag &= BassAsio.BASS_ASIO_Start(buflen, threads);
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool Stop()
		{
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (flag && BassAsio.BASS_ASIO_IsStarted())
			{
				flag &= BassAsio.BASS_ASIO_Stop();
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool StartFullDuplex(int buflen)
		{
			if (this._fullDuplexDevice < 0)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._fullDuplexDevice)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._fullDuplexDevice);
			}
			if (flag && !BassAsio.BASS_ASIO_IsStarted())
			{
				flag &= BassAsio.BASS_ASIO_Start(buflen, 0);
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool StopFullDuplex()
		{
			if (this._fullDuplexDevice < 0)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._fullDuplexDevice)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._fullDuplexDevice);
			}
			if (flag && BassAsio.BASS_ASIO_IsStarted())
			{
				flag &= BassAsio.BASS_ASIO_Stop();
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool Pause(bool pause)
		{
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (pause)
			{
				if (num != this._device)
				{
					flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
				}
				if (flag)
				{
					flag &= BassAsio.BASS_ASIO_ChannelPause(this._input, this._channel);
				}
				if (flag && this.IsInputFullDuplex)
				{
					if (this._fullDuplexChannel != -1)
					{
						if (this._device != this._fullDuplexDevice)
						{
							BassAsio.BASS_ASIO_SetDevice(this._fullDuplexDevice);
						}
						flag &= BassAsio.BASS_ASIO_ChannelPause(false, this._fullDuplexChannel);
					}
					else
					{
						Bass.Bass.BASS_ChannelPause(this._outputChannel);
						Bass.Bass.BASS_ChannelSetPosition(this._outputChannel, 0L);
					}
				}
			}
			else
			{
				if (this.IsInputFullDuplex)
				{
					if (this._fullDuplexChannel != -1)
					{
						if (num != this._fullDuplexDevice)
						{
							flag &= BassAsio.BASS_ASIO_SetDevice(this._fullDuplexDevice);
						}
						if (flag)
						{
							flag &= BassAsio.BASS_ASIO_ChannelReset(false, this._fullDuplexChannel, BASSASIOReset.BASS_ASIO_RESET_PAUSE);
						}
					}
					else
					{
						Bass.Bass.BASS_ChannelPlay(this._outputChannel, false);
					}
				}
				if (flag)
				{
					flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
					if (flag)
					{
						flag &= BassAsio.BASS_ASIO_ChannelReset(this._input, this._channel, BASSASIOReset.BASS_ASIO_RESET_PAUSE);
					}
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool PauseMirror(bool pause)
		{
			if (!this.IsMirrored)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (pause)
			{
				if (flag)
				{
					for (int i = 0; i < this._numchans; i++)
					{
						flag &= BassAsio.BASS_ASIO_ChannelPause(false, this._mirrorChannel + i);
					}
				}
			}
			else if (flag)
			{
				for (int j = 0; j < this._numchans; j++)
				{
					flag &= BassAsio.BASS_ASIO_ChannelReset(false, this._mirrorChannel + j, BASSASIOReset.BASS_ASIO_RESET_PAUSE);
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool AssignOutputChannel(int outputChannel)
		{
			if (this._input)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (flag)
			{
				BASSASIOActive bassasioactive = BassAsio.BASS_ASIO_ChannelIsActive(this._input, this._channel);
				if (bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_ENABLED)
				{
					BassAsio.BASS_ASIO_ChannelPause(this._input, this._channel);
				}
				this._outputChannel = outputChannel;
				if (this._outputChannel == 0)
				{
					this._samplerate = BassAsio.BASS_ASIO_GetRate();
					this._numchans = 1;
					this._format = BASSASIOFormat.BASS_ASIO_FORMAT_16BIT;
					this._internalAsioProc = new ASIOPROC(this.AsioOutputCallback);
				}
				else
				{
					this.GetChannelInfo(this._outputChannel);
					this._numchans = this._outputChannelInfo.chans;
					this._format = BASSASIOFormat.BASS_ASIO_FORMAT_16BIT;
					if ((this._outputChannelInfo.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
					{
						this._format = BASSASIOFormat.BASS_ASIO_FORMAT_FLOAT;
					}
					this._samplerate = (double)this._outputChannelInfo.freq;
				}
				BassAsio.BASS_ASIO_SetRate(this._samplerate);
				if (bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
				{
					this._internalAsioProc = new ASIOPROC(this.AsioOutputCallback);
				}
				flag &= this.EnableAndJoin(this._input, this._channel, this._numchans, this._internalAsioProc, this._format);
				if (bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_ENABLED)
				{
					BassAsio.BASS_ASIO_ChannelReset(this._input, this._channel, BASSASIOReset.BASS_ASIO_RESET_PAUSE);
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool SetFullDuplex(int asioDevice, int asioChannel)
		{
			if (!this._input)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (flag)
			{
				if (this._outputChannel != 0)
				{
					Bass.Bass.BASS_StreamFree(this._outputChannel);
				}
				BASSFlag bassflag = BASSFlag.BASS_STREAM_DECODE;
				if (this._format == BASSASIOFormat.BASS_ASIO_FORMAT_FLOAT)
				{
					bassflag |= BASSFlag.BASS_SAMPLE_FLOAT;
				}
				this._outputChannel = Bass.Bass.BASS_StreamCreateDummy((int)BassAsio.BASS_ASIO_GetRate(), this._numchans, bassflag, IntPtr.Zero);
				BASS_ASIO_INFO bass_ASIO_INFO = BassAsio.BASS_ASIO_GetInfo();
				this._fullDuplexBuffer = new byte[bass_ASIO_INFO.bufmax * this._numchans * 4];
				this._internalAsioProc = new ASIOPROC(this.AsioToAsioFullDuplexCallback);
				flag &= this.EnableAndJoin(this._input, this._channel, this._numchans, this._internalAsioProc, this._format);
				if (flag)
				{
					if (this._device != asioDevice)
					{
						flag &= BassAsio.BASS_ASIO_SetDevice(asioDevice);
					}
					if (flag)
					{
						BassAsio.BASS_ASIO_SetRate(this._samplerate);
						bool flag2 = BassAsio.BASS_ASIO_IsStarted();
						BASSASIOActive bassasioactive = BassAsio.BASS_ASIO_ChannelIsActive(false, asioChannel);
						if (flag2 && bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
						{
							BassAsio.BASS_ASIO_Stop();
						}
						flag &= this.EnableAndJoin(false, asioChannel, this._numchans, this._internalAsioProc, this._format);
						if (flag)
						{
							this._fullDuplexDevice = asioDevice;
							this._fullDuplexChannel = asioChannel;
							this._fullDuplex = true;
						}
						if (flag2)
						{
							BassAsio.BASS_ASIO_Start(0, 0);
						}
					}
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool SetFullDuplex(int bassDevice, BASSFlag flags, bool buffered)
		{
			if (!this._input)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			int num2 = Bass.Bass.BASS_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (num2 != bassDevice)
			{
				flag &= Bass.Bass.BASS_SetDevice(bassDevice);
			}
			if (flag)
			{
				if (this._outputChannel != 0)
				{
					Bass.Bass.BASS_StreamFree(this._outputChannel);
				}
				flags &= ~BASSFlag.BASS_SAMPLE_8BITS;
				if (this._format == BASSASIOFormat.BASS_ASIO_FORMAT_FLOAT)
				{
					flags |= BASSFlag.BASS_SAMPLE_FLOAT;
				}
				else if (this._format == BASSASIOFormat.BASS_ASIO_FORMAT_16BIT)
				{
					flags &= ~BASSFlag.BASS_SAMPLE_FLOAT;
				}
				this._outputChannel = Bass.Bass.BASS_StreamCreatePush((int)BassAsio.BASS_ASIO_GetRate(), this._numchans, flags, IntPtr.Zero);
				if (this._outputChannel != 0)
				{
					Bass.Bass.BASS_ChannelGetInfo(this._outputChannel, this._outputChannelInfo);
					if (buffered)
					{
						this._bassSamplesNeeded = (int)Bass.Bass.BASS_ChannelSeconds2Bytes(this._outputChannel, (double)Bass.Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD) / 500.0);
					}
					else
					{
						this._bassSamplesNeeded = -1;
					}
					if (!buffered && (flags & BASSFlag.BASS_STREAM_DECODE) == BASSFlag.BASS_DEFAULT)
					{
						Bass.Bass.BASS_ChannelPlay(this._outputChannel, false);
					}
					this._internalAsioProc = new ASIOPROC(this.AsioToBassFullDuplexCallback);
					flag &= this.EnableAndJoin(this._input, this._channel, this._numchans, this._internalAsioProc, this._format);
					if (flag)
					{
						this._fullDuplexDevice = bassDevice;
						this._fullDuplexChannel = -1;
						this._fullDuplex = true;
					}
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			Bass.Bass.BASS_SetDevice(num2);
			return flag;
		}

		public bool RemoveFullDuplex(bool disableOutput)
		{
			if (!this.IsInputFullDuplex)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (this._fullDuplexChannel != -1 && num != this._fullDuplexDevice)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._fullDuplexDevice);
			}
			if (flag)
			{
				bool flag2 = false;
				if (this._fullDuplexChannel != -1)
				{
					flag2 = BassAsio.BASS_ASIO_IsStarted();
					BASSASIOActive bassasioactive = BassAsio.BASS_ASIO_ChannelIsActive(false, this._fullDuplexChannel);
					if (flag2 && disableOutput && bassasioactive != BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
					{
						BassAsio.BASS_ASIO_Stop();
						flag &= BassAsio.BASS_ASIO_ChannelEnable(false, this._fullDuplexChannel, null, IntPtr.Zero);
						if (flag)
						{
							BassAsio.BASS_ASIO_ChannelReset(false, this._fullDuplexChannel, BASSASIOReset.BASS_ASIO_RESET_ENABLE | BASSASIOReset.BASS_ASIO_RESET_FORMAT | BASSASIOReset.BASS_ASIO_RESET_RATE | BASSASIOReset.BASS_ASIO_RESET_VOLUME);
							for (int i = 1; i < this._numchans; i++)
							{
								BassAsio.BASS_ASIO_ChannelReset(false, this._fullDuplexChannel + i, BASSASIOReset.BASS_ASIO_RESET_JOIN);
							}
						}
					}
				}
				if (flag)
				{
					this._fullDuplexChannel = -1;
					this._fullDuplexDevice = -1;
					this._fullDuplex = false;
				}
				if (flag2)
				{
					BassAsio.BASS_ASIO_Start(0, 0);
				}
				if (flag)
				{
					if (this._fullDuplexDevice != this._device)
					{
						flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
					}
					if (flag)
					{
						this._internalAsioProc = new ASIOPROC(this.AsioInputCallback);
						flag &= this.EnableAndJoin(this._input, this._channel, this._numchans, this._internalAsioProc, this._format);
					}
                    Bass.Bass.BASS_StreamFree(this._outputChannel);
					this._outputChannel = 0;
					this._fullDuplexBuffer = null;
					this.BypassFullDuplex = false;
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool SetMirror(int asioChannel)
		{
			if (!this._input && this._channel == asioChannel)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (flag)
			{
				bool flag2 = BassAsio.BASS_ASIO_IsStarted();
				BASSASIOActive bassasioactive = BassAsio.BASS_ASIO_ChannelIsActive(false, asioChannel);
				if (flag2)
				{
					BassAsio.BASS_ASIO_Stop();
				}
				for (int i = 0; i < this._numchans; i++)
				{
					flag &= BassAsio.BASS_ASIO_ChannelEnableMirror(asioChannel + i, this._input, this._channel + i);
				}
				if (flag)
				{
					this._mirrorChannel = asioChannel;
				}
				else if (bassasioactive == BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
				{
					for (int j = 0; j < this._numchans; j++)
					{
						BassAsio.BASS_ASIO_ChannelEnable(false, asioChannel + j, null, IntPtr.Zero);
						BassAsio.BASS_ASIO_ChannelReset(false, asioChannel + j, BASSASIOReset.BASS_ASIO_RESET_ENABLE | BASSASIOReset.BASS_ASIO_RESET_FORMAT | BASSASIOReset.BASS_ASIO_RESET_RATE | BASSASIOReset.BASS_ASIO_RESET_VOLUME);
					}
				}
				if (flag2)
				{
					BassAsio.BASS_ASIO_Start(0, 0);
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public bool RemoveMirror()
		{
			if (!this.IsMirrored)
			{
				return false;
			}
			bool flag = true;
			int num = BassAsio.BASS_ASIO_GetDevice();
			if (num != this._device)
			{
				flag &= BassAsio.BASS_ASIO_SetDevice(this._device);
			}
			if (flag)
			{
				bool flag2 = BassAsio.BASS_ASIO_IsStarted();
				if (BassAsio.BASS_ASIO_ChannelIsActive(false, this._mirrorChannel) != BASSASIOActive.BASS_ASIO_ACTIVE_DISABLED)
				{
					BassAsio.BASS_ASIO_Stop();
					for (int i = 0; i < this._numchans; i++)
					{
						flag &= BassAsio.BASS_ASIO_ChannelEnable(false, this._mirrorChannel + i, null, IntPtr.Zero);
						BassAsio.BASS_ASIO_ChannelReset(false, this._mirrorChannel + i, BASSASIOReset.BASS_ASIO_RESET_ENABLE | BASSASIOReset.BASS_ASIO_RESET_FORMAT | BASSASIOReset.BASS_ASIO_RESET_RATE | BASSASIOReset.BASS_ASIO_RESET_VOLUME);
					}
				}
				if (flag)
				{
					this._mirrorChannel = -1;
				}
				if (flag2)
				{
					BassAsio.BASS_ASIO_Start(0, 0);
				}
			}
			BassAsio.BASS_ASIO_SetDevice(num);
			return flag;
		}

		public virtual int AsioOutputCallback(bool input, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (input || this._outputChannel == 0)
			{
				return 0;
			}
			int num = Bass.Bass.BASS_ChannelGetData(this._outputChannel, buffer, length);
			if (num < length && !this._sourceStalled)
			{
				this.RaiseNotification(BassAsioHandlerSyncType.BufferUnderrun, this._outputChannel);
			}
			if (num <= 0)
			{
				num = 0;
				if (!this._sourceStalled)
				{
					this._sourceStalled = true;
					this.RaiseNotification(BassAsioHandlerSyncType.SourceStalled, this._outputChannel);
				}
			}
			else if (this._sourceStalled)
			{
				this._sourceStalled = false;
				this.RaiseNotification(BassAsioHandlerSyncType.SourceResumed, this._outputChannel);
			}
			return num;
		}

		public virtual int AsioInputCallback(bool input, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (!input)
			{
				return 0;
			}
			if (this._inputChannel != 0)
			{
				Bass.Bass.BASS_ChannelGetData(this._inputChannel, buffer, length);
			}
			return 0;
		}

		public virtual int AsioToAsioFullDuplexCallback(bool input, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (input)
			{
				if (this._fullDuplexBuffer.Length < length)
				{
					this._fullDuplexBuffer = new byte[length];
				}
				if (this._inputChannel != 0)
				{
					Bass.Bass.BASS_ChannelGetData(this._inputChannel, buffer, length);
				}
				Marshal.Copy(buffer, this._fullDuplexBuffer, 0, length);
				return 0;
			}
			if (!this._bypassFullDuplex)
			{
				if (length > this._fullDuplexBuffer.Length)
				{
					length = this._fullDuplexBuffer.Length;
				}
				Marshal.Copy(this._fullDuplexBuffer, 0, buffer, length);
				Bass.Bass.BASS_ChannelGetData(this._outputChannel, buffer, length);
				return length;
			}
			return 0;
		}

		public virtual int AsioToBassFullDuplexCallback(bool input, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (!input)
			{
				return 0;
			}
			if (this._inputChannel != 0)
			{
				Bass.Bass.BASS_ChannelGetData(this._inputChannel, buffer, length);
			}
			if (!this._bypassFullDuplex)
			{
				Bass.Bass.BASS_ChannelLock(this._outputChannel, true);
				int num = Bass.Bass.BASS_StreamPutData(this._outputChannel, buffer, length);
				if (num > 1536000)
				{
					Bass.Bass.BASS_ChannelGetData(this._outputChannel, buffer, num - 1536000);
				}
				Bass.Bass.BASS_ChannelLock(this._outputChannel, false);
				if (this._bassSamplesNeeded > 0 && Bass.Bass.BASS_StreamPutData(this._outputChannel, IntPtr.Zero, 0) >= this._bassSamplesNeeded)
				{
					Bass.Bass.BASS_ChannelPlay(this._outputChannel, false);
					this._bassSamplesNeeded = -1;
				}
			}
			return 0;
		}

		private void GetChannelInfo(int channel)
		{
			if (channel == 0)
			{
				throw new ArgumentException("Invalid channel: must be a valid BASS channel!");
			}
			if (!Bass.Bass.BASS_ChannelGetInfo(channel, this._outputChannelInfo))
			{
				throw new ArgumentException("Invalid channel: " + Enum.GetName(typeof(BASSError), Bass.Bass.BASS_ErrorGetCode()));
			}
			if (!this._outputChannelInfo.IsDecodingChannel && this._outputChannelInfo.ctype != BASSChannelType.BASS_CTYPE_RECORD)
			{
				throw new ArgumentException("Invalid channel: must be a decoding or recording channel!");
			}
		}

		private void SetVolume(float volume, float pan)
		{
			float num = volume;
			float num2 = volume;
			if (this.ChannelNumChans > 1)
			{
				if (pan < 0f)
				{
					num2 = (1f + pan) * volume;
				}
				else if (pan > 0f)
				{
					num = (1f - pan) * volume;
				}
			}
			for (int i = 0; i < this.ChannelNumChans; i++)
			{
				float volume2;
				if (i % 2 == 0)
				{
					volume2 = num;
				}
				else
				{
					volume2 = num2;
				}
				BassAsio.BASS_ASIO_ChannelSetVolume(this._input, i + this.Channel, volume2);
			}
		}

		private void SetVolumeMirror(float volume, float pan)
		{
			float num = volume;
			float num2 = volume;
			if (this.ChannelNumChans > 1)
			{
				if (pan < 0f)
				{
					num2 = (1f + pan) * volume;
				}
				else if (pan > 0f)
				{
					num = (1f - pan) * volume;
				}
			}
			for (int i = 0; i < this.ChannelNumChans; i++)
			{
				float volume2;
				if (i % 2 == 0)
				{
					volume2 = num;
				}
				else
				{
					volume2 = num2;
				}
				BassAsio.BASS_ASIO_ChannelSetVolume(false, i + this.MirrorChannel, volume2);
			}
		}

		private void RaiseNotification(BassAsioHandlerSyncType syncType, int data)
		{
			if (this.Notification != null)
			{
				this.Notification(this, new BassAsioHandlerEventArgs(syncType, data));
			}
		}

		public event BassAsioHandler.BassAsioHandlerEventHandler Notification;

		public static bool UseDedicatedThreads = true;

		private bool disposed;

		private bool _input;

		private bool _fullDuplex;

		private int _device = -1;

		private int _channel = -1;

		private int _fullDuplexDevice = -1;

		private int _fullDuplexChannel = -1;

		private int _mirrorChannel = -1;

		private double _samplerate = 48000.0;

		private int _numchans = 2;

		private BASSASIOFormat _format = BASSASIOFormat.BASS_ASIO_FORMAT_FLOAT;

		private int _outputChannel;

		private BASS_CHANNELINFO _outputChannelInfo = new BASS_CHANNELINFO();

		private bool _useInputChannel;

		private int _inputChannel;

		private byte[] _fullDuplexBuffer;

		private int _bassSamplesNeeded = -1;

		private bool _bypassFullDuplex;

		private ASIOPROC _internalAsioProc;

		private bool _sourceStalled;

		private float _volume = 1f;

		private float _pan;

		private float _volumeMirror = 1f;

		private float _panMirror;

		public delegate void BassAsioHandlerEventHandler(object sender, BassAsioHandlerEventArgs e);
	}
}
