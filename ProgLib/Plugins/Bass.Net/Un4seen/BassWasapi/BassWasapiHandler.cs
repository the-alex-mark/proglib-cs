using System;
using System.Security;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Un4seen.BassWasapi
{
    [SuppressUnmanagedCodeSecurity]
	public class BassWasapiHandler : IDisposable
	{
		public BassWasapiHandler(int device, bool exclusive, int freq, int chans, float buffer, float period)
		{
			this._device = device;
			this._exclusive = exclusive;
			if (!BassWasapi.BASS_WASAPI_GetDeviceInfo(device, this._wasapiDeviceInfo))
			{
				throw new ArgumentException("Invalid device: " + Enum.GetName(typeof(BASSError), Bass.Bass.BASS_ErrorGetCode()));
			}
			if (exclusive)
			{
				this._numchans = chans;
			}
			else
			{
				this._numchans = this._wasapiDeviceInfo.mixchans;
			}
			if (exclusive)
			{
				this._samplerate = freq;
			}
			else
			{
				this._samplerate = this._wasapiDeviceInfo.mixfreq;
			}
			this._updatePeriod = period;
			if (buffer == 0f)
			{
				this._bufferLength = ((this._updatePeriod == 0f) ? this._wasapiDeviceInfo.defperiod : this._updatePeriod) * 4f;
			}
			else
			{
				this._bufferLength = buffer;
			}
			if (this.IsInput)
			{
				this.UseInput = true;
				return;
			}
			this._internalMixer = BassMix.BASS_Mixer_StreamCreate(this._samplerate, this._numchans, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_AAC_FRAME960);
			if (this._internalMixer == 0)
			{
				throw new NotSupportedException("Internal Mixer: " + Enum.GetName(typeof(BASSError), Bass.Bass.BASS_ErrorGetCode()));
			}
		}

		public BassWasapiHandler(int device, bool exclusive, bool eventSystem, int freq, int chans, float buffer, float period)
		{
			this._device = device;
			this._exclusive = exclusive;
			this._eventSystem = eventSystem;
			if (!BassWasapi.BASS_WASAPI_GetDeviceInfo(device, this._wasapiDeviceInfo))
			{
				throw new ArgumentException("Invalid device: " + Enum.GetName(typeof(BASSError), Bass.Bass.BASS_ErrorGetCode()));
			}
			if (exclusive)
			{
				this._numchans = chans;
			}
			else
			{
				this._numchans = this._wasapiDeviceInfo.mixchans;
			}
			if (exclusive)
			{
				this._samplerate = freq;
			}
			else
			{
				this._samplerate = this._wasapiDeviceInfo.mixfreq;
			}
			this._updatePeriod = period;
			if (buffer == 0f)
			{
				this._bufferLength = ((this._updatePeriod == 0f) ? this._wasapiDeviceInfo.defperiod : this._updatePeriod) * 4f;
			}
			else
			{
				this._bufferLength = buffer;
			}
			if (this.IsInput)
			{
				this.UseInput = true;
				return;
			}
			this._internalMixer = BassMix.BASS_Mixer_StreamCreate(this._samplerate, this._numchans, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_AAC_FRAME960);
			if (this._internalMixer == 0)
			{
				throw new NotSupportedException("Internal Mixer: " + Enum.GetName(typeof(BASSError), Bass.Bass.BASS_ErrorGetCode()));
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
				if (disposing)
				{
					int num = BassWasapi.BASS_WASAPI_GetDevice();
					if (num != this._device)
					{
						BassWasapi.BASS_WASAPI_SetDevice(this._device);
					}
					this.RemoveFullDuplex();
					BassWasapi.BASS_WASAPI_Stop(true);
					Bass.Bass.BASS_StreamFree(this._internalMixer);
					this._internalMixer = 0;
					BassWasapi.BASS_WASAPI_Free();
					BassWasapi.BASS_WASAPI_SetDevice(num);
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

		~BassWasapiHandler()
		{
			this.Dispose(false);
		}

		public WASAPIPROC InternalWasapiProc
		{
			get
			{
				return this._internalWasapiProc;
			}
		}

		public bool IsInput
		{
			get
			{
				return this._wasapiDeviceInfo.SupportsRecording;
			}
		}

		public bool Exclusive
		{
			get
			{
				return this._exclusive;
			}
		}

		public bool EventSystem
		{
			get
			{
				return this._eventSystem;
			}
		}

		public int Device
		{
			get
			{
				return this._device;
			}
		}

		public double SampleRate
		{
			get
			{
				return (double)this._samplerate;
			}
		}

		public int NumChans
		{
			get
			{
				return this._numchans;
			}
		}

		public float BufferLength
		{
			get
			{
				return this._bufferLength;
			}
		}

		public float UpdatePeriod
		{
			get
			{
				return this._updatePeriod;
			}
		}

		public int InternalMixer
		{
			get
			{
				return this._internalMixer;
			}
		}

		public int OutputChannel
		{
			get
			{
				return this._outputChannel;
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
				this.SetVolume(this._volume, this._pan);
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
				this.SetVolume(this._volume, this._pan);
			}
		}

		public float SessionVolume
		{
			get
			{
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				float result = BassWasapi.BASS_WASAPI_GetVolume(BASSWASAPIVolume.BASS_WASAPI_VOL_SESSION);
				BassWasapi.BASS_WASAPI_SetDevice(num);
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
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				BassWasapi.BASS_WASAPI_SetVolume(BASSWASAPIVolume.BASS_WASAPI_VOL_SESSION, value);
				BassWasapi.BASS_WASAPI_SetDevice(num);
			}
		}

		public bool SessionMute
		{
			get
			{
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				bool result = BassWasapi.BASS_WASAPI_GetMute(BASSWASAPIVolume.BASS_WASAPI_VOL_SESSION);
				BassWasapi.BASS_WASAPI_SetDevice(num);
				return result;
			}
			set
			{
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				BassWasapi.BASS_WASAPI_SetMute(BASSWASAPIVolume.BASS_WASAPI_VOL_SESSION, value);
				BassWasapi.BASS_WASAPI_SetDevice(num);
			}
		}

		public float DeviceVolume
		{
			get
			{
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				float result = BassWasapi.BASS_WASAPI_GetVolume(BASSWASAPIVolume.BASS_WASAPI_CURVE_LINEAR);
				BassWasapi.BASS_WASAPI_SetDevice(num);
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
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				BassWasapi.BASS_WASAPI_SetVolume(BASSWASAPIVolume.BASS_WASAPI_CURVE_LINEAR, value);
				BassWasapi.BASS_WASAPI_SetDevice(num);
			}
		}

		public bool DeviceMute
		{
			get
			{
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				bool result = BassWasapi.BASS_WASAPI_GetMute(BASSWASAPIVolume.BASS_WASAPI_CURVE_DB);
				BassWasapi.BASS_WASAPI_SetDevice(num);
				return result;
			}
			set
			{
				int num = BassWasapi.BASS_WASAPI_GetDevice();
				if (num != this._device)
				{
					BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				BassWasapi.BASS_WASAPI_SetMute(BASSWASAPIVolume.BASS_WASAPI_CURVE_DB, value);
				BassWasapi.BASS_WASAPI_SetDevice(num);
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
					this._inputChannel = Bass.Bass.BASS_StreamCreateDummy(this._samplerate, this._numchans, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE, IntPtr.Zero);
				}
			}
		}

		public bool IsInputFullDuplex
		{
			get
			{
				return this.IsInput && this._fullDuplex;
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

		public bool Init(bool buffered = false)
		{
			BASSWASAPIInit basswasapiinit = BASSWASAPIInit.BASS_WASAPI_SHARED;
			if (this._exclusive)
			{
				basswasapiinit |= BASSWASAPIInit.BASS_WASAPI_EXCLUSIVE;
			}
			if (this._eventSystem)
			{
				basswasapiinit |= BASSWASAPIInit.BASS_WASAPI_EVENT;
			}
			if (buffered)
			{
				basswasapiinit |= BASSWASAPIInit.BASS_WASAPI_BUFFER;
			}
			if (this.IsInput)
			{
				this._internalWasapiProc = new WASAPIPROC(this.WasapiInputCallback);
				return BassWasapi.BASS_WASAPI_Init(this._device, this._samplerate, this._numchans, basswasapiinit, this._bufferLength, this._updatePeriod, this._internalWasapiProc, IntPtr.Zero);
			}
			this._internalWasapiProc = new WASAPIPROC(this.WasapiOutputCallback);
			return BassWasapi.BASS_WASAPI_Init(this._device, this._samplerate, this._numchans, basswasapiinit, this._bufferLength, this._updatePeriod, this._internalWasapiProc, IntPtr.Zero);
		}

		public bool Start()
		{
			bool flag = true;
			int num = BassWasapi.BASS_WASAPI_GetDevice();
			if (num != this._device)
			{
				flag &= BassWasapi.BASS_WASAPI_SetDevice(this._device);
			}
			if (flag && !BassWasapi.BASS_WASAPI_IsStarted())
			{
				flag &= BassWasapi.BASS_WASAPI_Start();
			}
			BassWasapi.BASS_WASAPI_SetDevice(num);
			return flag;
		}

		public bool Stop()
		{
			bool flag = true;
			int num = BassWasapi.BASS_WASAPI_GetDevice();
			if (num != this._device)
			{
				flag &= BassWasapi.BASS_WASAPI_SetDevice(this._device);
			}
			if (flag && BassWasapi.BASS_WASAPI_IsStarted())
			{
				flag &= BassWasapi.BASS_WASAPI_Stop(true);
			}
			BassWasapi.BASS_WASAPI_SetDevice(num);
			return flag;
		}

		public bool Pause(bool pause)
		{
			bool flag = true;
			int num = BassWasapi.BASS_WASAPI_GetDevice();
			if (pause)
			{
				if (num != this._device)
				{
					flag &= BassWasapi.BASS_WASAPI_SetDevice(this._device);
				}
				if (flag)
				{
					flag &= BassWasapi.BASS_WASAPI_Stop(false);
				}
			}
			else if (flag)
			{
				flag &= BassWasapi.BASS_WASAPI_SetDevice(this._device);
				if (flag)
				{
					flag &= BassWasapi.BASS_WASAPI_Start();
				}
			}
			BassWasapi.BASS_WASAPI_SetDevice(num);
			return flag;
		}

		public bool AddOutputSource(int channel, BASSFlag flags)
		{
			BASS_CHANNELINFO bass_CHANNELINFO = Bass.Bass.BASS_ChannelGetInfo(channel);
			if (bass_CHANNELINFO == null)
			{
				return false;
			}
			if (!bass_CHANNELINFO.IsDecodingChannel && bass_CHANNELINFO.ctype != BASSChannelType.BASS_CTYPE_RECORD)
			{
				return false;
			}
			if (flags < BASSFlag.BASS_SPEAKER_FRONT)
			{
				flags |= BASSFlag.BASS_WV_STEREO;
			}
			return BassMix.BASS_Mixer_StreamAddChannel(this._internalMixer, channel, flags);
		}

		public bool SetFullDuplex(int bassDevice, BASSFlag flags, bool buffered)
		{
			if (!this.IsInput)
			{
				return false;
			}
			bool flag = true;
			int num = Bass.Bass.BASS_GetDevice();
			if (num != bassDevice)
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
				flags |= BASSFlag.BASS_SAMPLE_FLOAT;
				this._outputChannel = Bass.Bass.BASS_StreamCreatePush(this._samplerate, this._numchans, flags, IntPtr.Zero);
				if (this._outputChannel != 0)
				{
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
					this._fullDuplex = true;
				}
			}
			Bass.Bass.BASS_SetDevice(num);
			return flag;
		}

		public bool RemoveFullDuplex()
		{
			if (!this.IsInputFullDuplex)
			{
				return false;
			}
			this._fullDuplex = false;
			Bass.Bass.BASS_StreamFree(this._outputChannel);
			this._outputChannel = 0;
			this.BypassFullDuplex = false;
			return true;
		}

		public unsafe virtual int WasapiOutputCallback(IntPtr buffer, int length, IntPtr user)
		{
			if (this._internalMixer == 0)
			{
				return 0;
			}
			int num = Bass.Bass.BASS_ChannelGetData(this._internalMixer, buffer, length);
			if (num <= 0)
			{
				num = 0;
				if (!this._mixerStalled)
				{
					this._mixerStalled = true;
					this.RaiseNotification(BassWasapiHandlerSyncType.SourceStalled, this._internalMixer);
				}
			}
			else if (this._mixerStalled)
			{
				this._mixerStalled = false;
				this.RaiseNotification(BassWasapiHandlerSyncType.SourceResumed, this._internalMixer);
			}
			if (num > 0 && (this._volL != 1f || this._volR != 1f))
			{
				float* ptr = (float*)((void*)buffer);
				for (int i = 0; i < length / 4; i++)
				{
					if (i % 2 == 0)
					{
						ptr[i] = ptr[i] * this._volL;
					}
					else
					{
						ptr[i] = ptr[i] * this._volR;
					}
				}
			}
			return num;
		}

		public unsafe virtual int WasapiInputCallback(IntPtr buffer, int length, IntPtr user)
		{
			if (length > 0 && (this._volL != 1f || this._volR != 1f))
			{
				float* ptr = (float*)((void*)buffer);
				for (int i = 0; i < length / 4; i++)
				{
					if (i % 2 == 0)
					{
						ptr[i] = ptr[i] * this._volL;
					}
					else
					{
						ptr[i] = ptr[i] * this._volR;
					}
				}
			}
			if (this._inputChannel != 0)
			{
				Bass.Bass.BASS_ChannelGetData(this._inputChannel, buffer, length);
			}
			if (this._fullDuplex && !this._bypassFullDuplex)
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
			return 1;
		}

		private void SetVolume(float volume, float pan)
		{
			this._volL = volume;
			this._volR = volume;
			if (this._numchans > 1)
			{
				if (pan < 0f)
				{
					this._volR = (1f + pan) * volume;
					return;
				}
				if (pan > 0f)
				{
					this._volL = (1f - pan) * volume;
				}
			}
		}

		private void RaiseNotification(BassWasapiHandlerSyncType syncType, int data)
		{
			if (this.Notification != null)
			{
				this.Notification(this, new BassWasapiHandlerEventArgs(syncType, data));
			}
		}

		public event BassWasapiHandler.BassWasapiHandlerEventHandler Notification;

		private bool disposed;

		private BASS_WASAPI_DEVICEINFO _wasapiDeviceInfo = new BASS_WASAPI_DEVICEINFO();

		private WASAPIPROC _internalWasapiProc;

		private bool _mixerStalled;

		private volatile float _volL = 1f;

		private volatile float _volR = 1f;

		private int _bassSamplesNeeded = -1;

		private bool _bypassFullDuplex;

		private bool _exclusive = true;

		private bool _eventSystem;

		private int _device = -1;

		private int _samplerate = 48000;

		private int _numchans = 2;

		private float _bufferLength;

		private float _updatePeriod;

		private int _internalMixer;

		private int _outputChannel;

		private float _volume = 1f;

		private float _pan;

		private int _inputChannel;

		private bool _useInputChannel;

		private volatile bool _fullDuplex;

		public delegate void BassWasapiHandlerEventHandler(object sender, BassWasapiHandlerEventArgs e);
	}
}
