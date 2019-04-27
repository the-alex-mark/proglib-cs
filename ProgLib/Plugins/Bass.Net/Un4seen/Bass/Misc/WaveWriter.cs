using System;
using System.IO;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class WaveWriter : IDisposable
	{
		public WaveWriter(string fileName, int stream, bool rewrite)
		{
			this.FileName = fileName;
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (Bass.BASS_ChannelGetInfo(stream, bass_CHANNELINFO))
			{
				if ((bass_CHANNELINFO.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
				{
					this.BitsPerSample = 32;
				}
				else if ((bass_CHANNELINFO.flags & BASSFlag.BASS_SAMPLE_8BITS) != BASSFlag.BASS_DEFAULT)
				{
					this.BitsPerSample = 8;
				}
				else
				{
					this.BitsPerSample = 16;
				}
				this.OrigResolution = this.BitsPerSample;
				this._numChannels = bass_CHANNELINFO.chans;
				this._sampleRate = bass_CHANNELINFO.freq;
				this.Initialize(rewrite);
				return;
			}
			throw new ArgumentException("Could not retrieve channel information!");
		}

		public WaveWriter(string fileName, int stream, int bitsPerSample, bool rewrite)
		{
			this.FileName = fileName;
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (Bass.BASS_ChannelGetInfo(stream, bass_CHANNELINFO))
			{
				this.BitsPerSample = bitsPerSample;
				if ((bass_CHANNELINFO.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
				{
					this.OrigResolution = 32;
				}
				else if ((bass_CHANNELINFO.flags & BASSFlag.BASS_SAMPLE_8BITS) != BASSFlag.BASS_DEFAULT)
				{
					this.OrigResolution = 8;
				}
				else
				{
					this.OrigResolution = 16;
				}
				this._numChannels = bass_CHANNELINFO.chans;
				this._sampleRate = bass_CHANNELINFO.freq;
				this.Initialize(rewrite);
				return;
			}
			throw new ArgumentException("Could not retrieve channel information!");
		}

		public WaveWriter(string fileName, int numChannels, int sampleRate, int bitsPerSample, bool rewrite)
		{
			this.FileName = fileName;
			this.BitsPerSample = bitsPerSample;
			if (bitsPerSample > 16)
			{
				this.OrigResolution = 32;
			}
			else
			{
				this.OrigResolution = this.BitsPerSample;
			}
			this._numChannels = numChannels;
			this._sampleRate = sampleRate;
			this.Initialize(rewrite);
		}

		private void Initialize(bool rewrite)
		{
			this._dataWritten = 0u;
			if (File.Exists(this.FileName) && !rewrite)
			{
				throw new IOException(string.Format("The file {0} already exists!", this.FileName));
			}
			this._fs = new FileStream(this.FileName, FileMode.Create);
			this._bs = new BufferedStream(this._fs, 4096 * this.BitsPerSample);
			this._bw = new BinaryWriter(this._bs);
			this.WriteWaveHeader();
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
				this.Close();
			}
			this.disposed = true;
		}

		~WaveWriter()
		{
			this.Dispose(false);
		}

		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this._fileName = value;
			}
		}

		public int NumChannels
		{
			get
			{
				return this._numChannels;
			}
		}

		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		public int BitsPerSample
		{
			get
			{
				return this._bitsPerSample;
			}
			set
			{
				if (value == 8 || value == 16 || value == 24 || value == 32)
				{
					this._bitsPerSample = value;
				}
			}
		}

		public int OrigResolution
		{
			get
			{
				return this._origResolution;
			}
			set
			{
				if (value == 8 || value == 16 || value == 32)
				{
					this._origResolution = value;
				}
			}
		}

		private void WriteWaveHeader()
		{
			try
			{
				this._bw.Write(new byte[]
				{
					82,
					73,
					70,
					70
				});
				this._bw.Write(0u);
				this._bw.Write(new byte[]
				{
					87,
					65,
					86,
					69
				});
				uint num = 0u;
				if (this.NumChannels > 2)
				{
					num = 22u;
					this._waveHeaderSize += num;
					this._waveHeaderFormatSize += num;
					this._offsetDataSize += num;
					this._offsetSampleSize += num;
				}
				this._bw.Write(new byte[]
				{
					102,
					109,
					116,
					32
				});
				this._bw.Write(this._waveHeaderFormatSize);
				if (this.NumChannels > 2)
				{
					this._bw.Write(65534);
				}
				else if (this.BitsPerSample > 24)
				{
					this._bw.Write(3);
				}
				else
				{
					this._bw.Write(1);
				}
				int num2 = this.NumChannels * (this.BitsPerSample / 8);
				this._bw.Write((ushort)this.NumChannels);
				this._bw.Write((uint)this.SampleRate);
				this._bw.Write((uint)(this.SampleRate * num2));
				this._bw.Write((ushort)num2);
				this._bw.Write((ushort)this.BitsPerSample);
				this._bw.Write((ushort)num);
				if (this.NumChannels > 2)
				{
					this._bw.Write((ushort)this.BitsPerSample);
					int value;
					switch (this.NumChannels)
					{
					case 3:
						value = 7;
						break;
					case 4:
						value = 51;
						break;
					case 5:
						value = 55;
						break;
					case 6:
						value = 63;
						break;
					case 7:
						value = 319;
						break;
					case 8:
						value = 255;
						break;
					case 9:
						value = 511;
						break;
					default:
						value = int.MinValue;
						break;
					}
					this._bw.Write((uint)value);
					Guid empty = Guid.Empty;
					if (this.BitsPerSample > 24)
					{
						empty = new Guid("00000003-0000-0010-8000-00aa00389b71");
					}
					else
					{
						empty = new Guid("00000001-0000-0010-8000-00aa00389b71");
					}
					this._bw.Write(empty.ToByteArray());
				}
				if (this.NumChannels > 2 || this.BitsPerSample > 16)
				{
					this._bw.Write(new byte[]
					{
						102,
						97,
						99,
						116
					});
					this._bw.Write(4u);
					this._bw.Write(0u);
					this._waveHeaderSize += 12u;
					this._offsetDataSize += 12u;
				}
				this._bw.Write(new byte[]
				{
					100,
					97,
					116,
					97
				});
				this._bw.Write(0u);
			}
			catch (Exception innerException)
			{
				throw new IOException("The Wave Header could not be written.", innerException);
			}
		}

		private void WriteSample(float sample)
		{
			if (this.BitsPerSample == 32)
			{
				this._bw.Write(sample);
				this._dataWritten += 4u;
				return;
			}
			if (this.BitsPerSample == 24)
			{
				this._bw.Write(Utils.SampleTo24Bit(sample));
				this._dataWritten += 3u;
				return;
			}
			if (this.BitsPerSample == 16)
			{
				this._bw.Write(Utils.SampleTo16Bit(sample));
				this._dataWritten += 2u;
				return;
			}
			this._bw.Write(Utils.SampleTo8Bit(sample));
			this._dataWritten += 1u;
		}

		private void WriteSample(short sample)
		{
			if (this.BitsPerSample == 32)
			{
				this._bw.Write(Utils.SampleTo32Bit(sample));
				this._dataWritten += 4u;
				return;
			}
			if (this.BitsPerSample == 24)
			{
				this._bw.Write(Utils.SampleTo24Bit(sample));
				this._dataWritten += 3u;
				return;
			}
			if (this.BitsPerSample == 16)
			{
				this._bw.Write(sample);
				this._dataWritten += 2u;
				return;
			}
			this._bw.Write(Utils.SampleTo8Bit(sample));
			this._dataWritten += 1u;
		}

		private void WriteSample(byte sample)
		{
			if (this.BitsPerSample == 32)
			{
				this._bw.Write(Utils.SampleTo32Bit(sample));
				this._dataWritten += 4u;
				return;
			}
			if (this.BitsPerSample == 24)
			{
				this._bw.Write(Utils.SampleTo24Bit(sample));
				this._dataWritten += 3u;
				return;
			}
			if (this.BitsPerSample == 16)
			{
				this._bw.Write(Utils.SampleTo16Bit(sample));
				this._dataWritten += 2u;
				return;
			}
			this._bw.Write(sample);
			this._dataWritten += 1u;
		}

		private void WriteSampleNoConvert(byte sample)
		{
			this._bw.Write(sample);
			this._dataWritten += 1u;
		}

		public unsafe void Write(IntPtr buffer, int length)
		{
			if (this.OrigResolution == 32)
			{
				float* ptr = (float*)((void*)buffer);
				for (int i = 0; i < length / 4; i++)
				{
					this.WriteSample(ptr[i]);
				}
				return;
			}
			if (this.OrigResolution == 8)
			{
				byte* ptr2 = (byte*)((void*)buffer);
				for (int j = 0; j < length; j++)
				{
					this.WriteSample(ptr2[j]);
				}
				return;
			}
			short* ptr3 = (short*)((void*)buffer);
			for (int k = 0; k < length / 2; k++)
			{
				this.WriteSample(ptr3[k]);
			}
		}

		public void Write(float[] buffer, int length)
		{
			for (int i = 0; i < length / 4; i++)
			{
				this.WriteSample(buffer[i]);
			}
		}

		public void Write(short[] buffer, int length)
		{
			for (int i = 0; i < length / 2; i++)
			{
				this.WriteSample(buffer[i]);
			}
		}

		public void Write(byte[] buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this.WriteSample(buffer[i]);
			}
		}

		public void WriteNoConvert(byte[] buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this.WriteSampleNoConvert(buffer[i]);
			}
		}

		public void Close()
		{
			if (this._bs != null && this._bw != null && this._fs != null)
			{
				this._bs.Seek(4L, SeekOrigin.Begin);
				this._bw.Write(this._dataWritten + this._waveHeaderSize);
				if (this.NumChannels > 2 || this.BitsPerSample > 16)
				{
					int num = this.NumChannels * (this.BitsPerSample / 8);
					this._bs.Seek((long)((ulong)this._offsetSampleSize), SeekOrigin.Begin);
					this._bw.Write((uint)((ulong)this._dataWritten / (ulong)((long)num)));
				}
				this._bs.Seek((long)((ulong)this._offsetDataSize), SeekOrigin.Begin);
				this._bw.Write(this._dataWritten);
				this._bw.Flush();
				this._bw.Close();
				this._bw = null;
				this._bs.Close();
				this._bs = null;
				this._fs.Close();
				this._fs = null;
			}
		}

		private bool disposed;

		private const string Guid_KSDATAFORMAT_SUBTYPE_PCM = "00000001-0000-0010-8000-00aa00389b71";

		private const string Guid_KSDATAFORMAT_SUBTYPE_IEEE_FLOAT = "00000003-0000-0010-8000-00aa00389b71";

		private uint _waveHeaderSize = 38u;

		private uint _waveHeaderFormatSize = 18u;

		private FileStream _fs;

		private BinaryWriter _bw;

		private BufferedStream _bs;

		private uint _dataWritten;

		private uint _offsetDataSize = 42u;

		private uint _offsetSampleSize = 46u;

		private string _fileName = string.Empty;

		private int _numChannels = 2;

		private int _sampleRate = 44100;

		private int _bitsPerSample = 16;

		private int _origResolution = 16;

		private enum WaveFormat
		{
			WAVE_FORMAT_UNKNOWN,
			WAVE_FORMAT_PCM,
			WAVE_FORMAT_IEEE_FLOAT = 3,
			WAVE_FORMAT_EXTENSIBLE = 65534
		}

		[Flags]
		private enum WaveSpeakers
		{
			SPEAKER_FRONT_LEFT = 1,
			SPEAKER_FRONT_RIGHT = 2,
			SPEAKER_FRONT_CENTER = 4,
			SPEAKER_LOW_FREQUENCY = 8,
			SPEAKER_BACK_LEFT = 16,
			SPEAKER_BACK_RIGHT = 32,
			SPEAKER_FRONT_LEFT_OF_CENTER = 64,
			SPEAKER_FRONT_RIGHT_OF_CENTER = 128,
			SPEAKER_BACK_CENTER = 256,
			SPEAKER_SIDE_LEFT = 512,
			SPEAKER_SIDE_RIGHT = 1024,
			SPEAKER_TOP_CENTER = 2048,
			SPEAKER_TOP_FRONT_LEFT = 4096,
			SPEAKER_TOP_FRONT_CENTER = 8192,
			SPEAKER_TOP_FRONT_RIGHT = 16384,
			SPEAKER_TOP_BACK_LEFT = 32768,
			SPEAKER_TOP_BACK_CENTER = 65536,
			SPEAKER_TOP_BACK_RIGHT = 131072,
			SPEAKER_ALL = -2147483648
		}
	}
}
