using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_BufferStream : BaseDSP
	{
		public DSP_BufferStream()
		{
			this.ConfigBuffer = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_BUFFER);
			this._streamProc = new STREAMPROC(this.BassStreamProc);
		}

		public DSP_BufferStream(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
			this.ConfigBuffer = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_BUFFER);
			this._streamProc = new STREAMPROC(this.BassStreamProc);
		}

		public int BufferStream
		{
			get
			{
				return this._bufferStream;
			}
		}

		public BASSFlag BufferStreamFlags
		{
			get
			{
				return this._bufferStreamFlags;
			}
		}

		public int OutputHandle
		{
			get
			{
				return this._outputHandle;
			}
			set
			{
				this._outputHandle = value;
			}
		}

		public bool IsOutputBuffered
		{
			get
			{
				return this._isOutputBuffered;
			}
			set
			{
				lock (this)
				{
					this._isOutputBuffered = value;
					this._lastPos = (this._isOutputBuffered ? 0 : this._bufferLength);
				}
			}
		}

		public int ConfigBuffer
		{
			get
			{
				return this._configBuffer;
			}
			set
			{
				if (value > 5000)
				{
					this._configBuffer = 5000;
				}
				else if (value < 1)
				{
					this._configBuffer = 1;
				}
				else
				{
					this._configBuffer = value;
				}
				this.OnStopped();
				if ((base.ChannelInfo.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
				{
					this._bufferLength = (int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._configBuffer / 1000.0);
				}
				else if ((base.ChannelInfo.flags & BASSFlag.BASS_SAMPLE_8BITS) != BASSFlag.BASS_DEFAULT && base.ChannelBitwidth == 32)
				{
					this._bufferLength = (int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._configBuffer / 1000.0) * 4;
				}
				else if (base.ChannelBitwidth == 32)
				{
					this._bufferLength = (int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._configBuffer / 1000.0) * 2;
				}
				if (base.IsAssigned)
				{
					this.OnStarted();
				}
			}
		}

		public int ConfigBufferLength
		{
			get
			{
				return this._bufferLength;
			}
		}

		public void ClearBuffer()
		{
			if (this._buffer == null)
			{
				return;
			}
			lock (this)
			{
				Array.Clear(this._buffer, 0, this._bufferLength);
				this._lastPos = (this._isOutputBuffered ? 0 : this._bufferLength);
			}
		}

		public int BufferPosition
		{
			get
			{
				return this._lastPos;
			}
			set
			{
				this._lastPos = value;
			}
		}

		public override void OnChannelChanged()
		{
			this.OnStopped();
			if ((base.ChannelInfo.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
			{
				this._bufferLength = (int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._configBuffer / 1000.0);
			}
			else if ((base.ChannelInfo.flags & BASSFlag.BASS_SAMPLE_8BITS) != BASSFlag.BASS_DEFAULT && base.ChannelBitwidth == 32)
			{
				this._bufferLength = (int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._configBuffer / 1000.0) * 4;
			}
			else if (base.ChannelBitwidth == 32)
			{
				this._bufferLength = (int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._configBuffer / 1000.0) * 2;
			}
			this._bufferStreamFlags = (base.ChannelInfo.flags | BASSFlag.BASS_STREAM_DECODE);
			this._bufferStreamFlags &= ~BASSFlag.BASS_STREAM_AUTOFREE;
			if (base.ChannelBitwidth == 32)
			{
				this._bufferStreamFlags &= ~BASSFlag.BASS_SAMPLE_8BITS;
				this._bufferStreamFlags |= BASSFlag.BASS_SAMPLE_FLOAT;
			}
			else if (base.ChannelBitwidth == 8)
			{
				this._bufferStreamFlags &= ~BASSFlag.BASS_SAMPLE_FLOAT;
				this._bufferStreamFlags |= BASSFlag.BASS_SAMPLE_8BITS;
			}
			else
			{
				this._bufferStreamFlags &= ~BASSFlag.BASS_SAMPLE_FLOAT;
				this._bufferStreamFlags &= ~BASSFlag.BASS_SAMPLE_8BITS;
			}
			if (base.IsAssigned)
			{
				this.OnStarted();
			}
		}

		public override void OnBypassChanged()
		{
			this.ClearBuffer();
		}

		public override void OnStarted()
		{
			this._buffer = new byte[this._bufferLength];
			lock (this)
			{
				this._bufferStream = Bass.BASS_StreamCreate(base.ChannelSampleRate, base.ChannelNumChans, this._bufferStreamFlags, this._streamProc, IntPtr.Zero);
				this._lastPos = (this._isOutputBuffered ? 0 : this._bufferLength);
			}
		}

		public override void OnStopped()
		{
			if (this._bufferStream != 0)
			{
				Bass.BASS_StreamFree(this._bufferStream);
				this._bufferStream = 0;
			}
			this._buffer = null;
		}

		public override void DSPCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (base.IsBypassed)
			{
				return;
			}
			if (length > this._bufferLength)
			{
				length = this._bufferLength;
			}
			lock (this)
			{
				Array.Copy(this._buffer, length, this._buffer, 0, this._bufferLength - length);
				Marshal.Copy(buffer, this._buffer, this._bufferLength - length, length);
				this._lastPos -= length;
				if (this._lastPos < 0)
				{
					this._lastPos = 0;
				}
			}
		}

		private int BassStreamProc(int handle, IntPtr buffer, int length, IntPtr user)
		{
			if (base.IsBypassed)
			{
				return 0;
			}
			DSP_BufferStream obj;
			if (this.OutputHandle != 0)
			{
				int num = Bass.BASS_ChannelGetData(this.OutputHandle, IntPtr.Zero, 0);
				num = (int)Bass.BASS_ChannelSeconds2Bytes(handle, Bass.BASS_ChannelBytes2Seconds(this.OutputHandle, (long)num));
				if (num > this._bufferLength)
				{
					num = this._bufferLength;
				}
				else if (num < 0)
				{
					num = 0;
				}
				if (length > num)
				{
					length = num;
				}
				obj = this;
				lock (obj)
				{
					Marshal.Copy(this._buffer, this._bufferLength - num, buffer, length);
					return length;
				}
			}
			if (this._lastPos + length > this._bufferLength)
			{
				length = this._bufferLength - this._lastPos;
			}
			obj = this;
			lock (obj)
			{
				Marshal.Copy(this._buffer, this._lastPos, buffer, length);
				this._lastPos += length;
				if (this._lastPos > this._bufferLength)
				{
					this._lastPos = this._bufferLength;
				}
			}
			return length;
		}

		public override string ToString()
		{
			return "Buffer Stream DSP";
		}

		private byte[] _buffer;

		private int _configBuffer = 500;

		private int _bufferLength;

		private STREAMPROC _streamProc;

		private int _bufferStream;

		private bool _isOutputBuffered = true;

		private BASSFlag _bufferStreamFlags = BASSFlag.BASS_STREAM_DECODE;

		private int _outputHandle;

		private volatile int _lastPos;
	}
}
