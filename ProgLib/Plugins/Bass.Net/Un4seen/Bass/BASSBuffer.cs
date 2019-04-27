using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BASSBuffer : IDisposable
	{
		public BASSBuffer()
		{
			this.Initialize();
		}

		public BASSBuffer(float seconds, int samplerate, int chans, int bps)
		{
			if (seconds <= 0f)
			{
				seconds = 2f;
			}
			this._samplerate = samplerate;
			if (this._samplerate <= 0)
			{
				this._samplerate = 44100;
			}
			this._chans = chans;
			if (this._chans <= 0)
			{
				this._chans = 2;
			}
			this._bps = bps;
			if (this._bps > 4)
			{
				int bps2 = this._bps;
				if (bps2 != 8)
				{
					if (bps2 == 32)
					{
						this._bps = 4;
					}
					else
					{
						this._bps = 2;
					}
				}
				else
				{
					this._bps = 1;
				}
			}
			if (this._bps <= 0 || this._bps == 3)
			{
				this._bps = 2;
			}
			this._bufferlength = (int)Math.Ceiling((double)seconds * (double)this._samplerate * (double)this._chans * (double)this._bps);
			if (this._bufferlength % this._bps > 0)
			{
				this._bufferlength -= this._bufferlength % this._bps;
			}
			this.Initialize();
		}

		private void Initialize()
		{
			this._buffer = new byte[this._bufferlength];
			this.Clear();
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
			}
			this.disposed = true;
		}

		~BASSBuffer()
		{
			this.Dispose(false);
		}

		public int BufferLength
		{
			get
			{
				return this._bufferlength;
			}
		}

		public int Bps
		{
			get
			{
				return this._bps;
			}
		}

		public int SampleRate
		{
			get
			{
				return this._samplerate;
			}
		}

		public int NumChans
		{
			get
			{
				return this._chans;
			}
		}

		public int Readers
		{
			get
			{
				return this._readers;
			}
			set
			{
				if (value <= 0 || value == this._readers)
				{
					return;
				}
				byte[] buffer = this._buffer;
				lock (buffer)
				{
					int[] array = new int[value];
					for (int i = 0; i < value; i++)
					{
						if (i < this._readers)
						{
							array[i] = this._bufferreadpos[i];
						}
						else
						{
							array[i] = this._bufferreadpos[this._readers - 1];
						}
					}
					this._bufferreadpos = array;
					this._readers = value;
				}
			}
		}

		public void Clear()
		{
			byte[] buffer = this._buffer;
			lock (buffer)
			{
				Array.Clear(this._buffer, 0, this._bufferlength);
				this._bufferwritepos = 0;
				for (int i = 0; i < this._readers; i++)
				{
					this._bufferreadpos[i] = 0;
				}
			}
		}

		public void Resize(float factor)
		{
			if (factor <= 1f)
			{
				return;
			}
			byte[] buffer = this._buffer;
			lock (buffer)
			{
				this._bufferlength = (int)Math.Ceiling((double)factor * (double)this._bufferlength);
				if (this._bufferlength % this._bps > 0)
				{
					this._bufferlength -= this._bufferlength % this._bps;
				}
				byte[] array = new byte[this._bufferlength];
				Array.Clear(array, 0, this._bufferlength);
				Array.Copy(this._buffer, array, this._buffer.Length);
				this._buffer = array;
			}
		}

		public int Space(int reader)
		{
			int num = this._bufferlength;
			byte[] buffer = this._buffer;
			lock (buffer)
			{
				if (reader < 0 || reader >= this._readers)
				{
					for (int i = 0; i < this._readers; i++)
					{
						int num2 = this._bufferlength - (this._bufferwritepos - this._bufferreadpos[reader]);
						if (num2 > this._bufferlength)
						{
							num2 -= this._bufferlength;
						}
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
				else
				{
					num = this._bufferlength - (this._bufferwritepos - this._bufferreadpos[reader]);
					if (num > this._bufferlength)
					{
						num -= this._bufferlength;
					}
				}
			}
			return num;
		}

		public int Count(int reader)
		{
			int num = -1;
			byte[] buffer = this._buffer;
			lock (buffer)
			{
				if (reader < 0 || reader >= this._readers)
				{
					for (int i = 0; i < this._readers; i++)
					{
						int num2 = this._bufferwritepos - this._bufferreadpos[i];
						if (num2 < 0)
						{
							num2 += this._bufferlength;
						}
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
				else
				{
					num = this._bufferwritepos - this._bufferreadpos[reader];
					if (num < 0)
					{
						num += this._bufferlength;
					}
				}
			}
			return num;
		}

		public unsafe int Write(IntPtr buffer, int length)
		{
			byte[] buffer2 = this._buffer;
			int result;
			lock (buffer2)
			{
				if (length > this._bufferlength)
				{
					length = this._bufferlength;
				}
				int num = 0;
				int num2 = this._bufferlength - this._bufferwritepos;
				if (length >= num2)
				{
					Marshal.Copy(buffer, this._buffer, this._bufferwritepos, num2);
					num += num2;
					buffer = new IntPtr((void*)((byte*)buffer.ToPointer() + num2));
					length -= num2;
					this._bufferwritepos = 0;
				}
				Marshal.Copy(buffer, this._buffer, this._bufferwritepos, length);
				num += length;
				this._bufferwritepos += length;
				result = num;
			}
			return result;
		}

		public int Write(byte[] buffer, int length)
		{
			byte[] buffer2 = this._buffer;
			int result;
			lock (buffer2)
			{
				if (length > this._bufferlength)
				{
					length = this._bufferlength;
				}
				int num = 0;
				int num2 = this._bufferlength - this._bufferwritepos;
				if (length >= num2)
				{
					Array.Copy(buffer, num, this._buffer, this._bufferwritepos, num2);
					num += num2;
					length -= num2;
					this._bufferwritepos = 0;
				}
				Array.Copy(buffer, num, this._buffer, this._bufferwritepos, length);
				num += length;
				this._bufferwritepos += length;
				result = num;
			}
			return result;
		}

		public unsafe int Read(IntPtr buffer, int length, int reader)
		{
			byte[] buffer2 = this._buffer;
			int result;
			lock (buffer2)
			{
				int num = 0;
				if (reader < 0 || reader >= this._readers)
				{
					reader = 0;
				}
				int num2 = this._bufferwritepos - this._bufferreadpos[reader];
				if (num2 < 0)
				{
					num2 += this._bufferlength;
				}
				if (length > num2)
				{
					length = num2;
				}
				num2 = this._bufferlength - this._bufferreadpos[reader];
				if (length >= num2)
				{
					Marshal.Copy(this._buffer, this._bufferreadpos[reader], buffer, num2);
					num += num2;
					buffer = new IntPtr((void*)((byte*)buffer.ToPointer() + num2));
					length -= num2;
					this._bufferreadpos[reader] = 0;
				}
				Marshal.Copy(this._buffer, this._bufferreadpos[reader], buffer, length);
				this._bufferreadpos[reader] += length;
				num += length;
				result = num;
			}
			return result;
		}

		public int Read(byte[] buffer, int length, int reader)
		{
			byte[] buffer2 = this._buffer;
			int result;
			lock (buffer2)
			{
				int num = 0;
				if (reader < 0 || reader >= this._readers)
				{
					reader = 0;
				}
				int num2 = this._bufferwritepos - this._bufferreadpos[reader];
				if (num2 < 0)
				{
					num2 += this._bufferlength;
				}
				if (length > num2)
				{
					length = num2;
				}
				num2 = this._bufferlength - this._bufferreadpos[reader];
				if (length >= num2)
				{
					Array.Copy(this._buffer, this._bufferreadpos[reader], buffer, num, num2);
					num += num2;
					length -= num2;
					this._bufferreadpos[reader] = 0;
				}
				Array.Copy(this._buffer, this._bufferreadpos[reader], buffer, num, length);
				this._bufferreadpos[reader] += length;
				num += length;
				result = num;
			}
			return result;
		}

		private bool disposed;

		private int _bufferlength = 352800;

		private int _bps = 2;

		private int _samplerate = 44100;

		private int _chans = 2;

		private byte[] _buffer;

		private int _bufferwritepos;

		private int _readers = 1;

		private int[] _bufferreadpos = new int[1];
	}
}
