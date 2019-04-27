using System;
using System.IO;
using System.Security;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Tags;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public abstract class BaseEncoder : IBaseEncoder, IDisposable
	{
		public BaseEncoder(int channel)
		{
			this.InitChannel(channel);
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

		~BaseEncoder()
		{
			this.Dispose(false);
		}

		private void InitChannel(int channel)
		{
			this._channel = channel;
			if (this._channel != 0)
			{
				if (!Bass.BASS_ChannelGetInfo(channel, this._channelInfo))
				{
					this._channelInfo.chans = 2;
					this._channelInfo.ctype = BASSChannelType.BASS_CTYPE_UNKNOWN;
					this._channelInfo.freq = 44100;
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
					return;
				}
			}
			else
			{
				this._channel = 0;
				this._bitwidth = 16;
				this._samplerate = 44100;
				this._numchans = 2;
				this._channelInfo = new BASS_CHANNELINFO();
			}
		}

		public int ChannelHandle
		{
			get
			{
				return this._channel;
			}
			set
			{
				if (value != 0 && value == BassEnc.BASS_Encode_GetChannel(this._enc))
				{
					return;
				}
				if (this.IsActive && value != 0)
				{
					if (BassEnc.BASS_Encode_SetChannel(this._enc, value))
					{
						this.InitChannel(value);
						return;
					}
				}
				else
				{
					this.InitChannel(value);
				}
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

		public abstract bool SupportsSTDOUT { get; }

		public abstract BASSChannelType EncoderType { get; }

		public abstract string DefaultOutputExtension { get; }

		public int EncoderHandle
		{
			get
			{
				return this._enc;
			}
			set
			{
				this._enc = value;
			}
		}

		public virtual bool IsActive
		{
			get
			{
				return this.EncoderHandle != 0 && BassEnc.BASS_Encode_IsActive(this.EncoderHandle) > BASSActive.BASS_ACTIVE_STOPPED;
			}
		}

		public virtual bool IsPaused
		{
			get
			{
				return this.EncoderHandle != 0 && BassEnc.BASS_Encode_IsActive(this.EncoderHandle) == BASSActive.BASS_ACTIVE_PAUSED;
			}
		}

		public string EncoderDirectory
		{
			get
			{
				return this._encDir;
			}
			set
			{
				this._encDir = value;
			}
		}

		public abstract string EncoderCommandLine { get; }

		public virtual bool EncoderExists
		{
			get
			{
				return true;
			}
		}

		public string InputFile
		{
			get
			{
				return this._inputFile;
			}
			set
			{
				this._inputFile = value;
			}
		}

		public bool Force16Bit
		{
			get
			{
				return this._force16Bit;
			}
			set
			{
				this._force16Bit = value;
			}
		}

		public bool NoLimit
		{
			get
			{
				return this._noLimit;
			}
			set
			{
				this._noLimit = value;
			}
		}

		public bool UseAsyncQueue
		{
			get
			{
				return this._useAsyncQueue;
			}
			set
			{
				this._useAsyncQueue = value;
			}
		}

		public string OutputFile
		{
			get
			{
				return this._outputFile;
			}
			set
			{
				this._outputFile = value;
			}
		}

		public virtual int EffectiveBitrate
		{
			get
			{
				return 128;
			}
		}

		public TAG_INFO TAGs
		{
			get
			{
				return this._tagInfo;
			}
			set
			{
				this._tagInfo = value;
			}
		}

		public abstract bool Start(ENCODEPROC proc, IntPtr user, bool paused);

		public virtual bool Stop()
		{
			if (this.EncoderHandle == 0)
			{
				return true;
			}
			if (this._useAsyncQueue)
			{
				if (BassEnc.BASS_Encode_StopEx(this.EncoderHandle, false))
				{
					this.EncoderHandle = 0;
					return true;
				}
				if (BassEnc.BASS_Encode_Stop(this.EncoderHandle))
				{
					this.EncoderHandle = 0;
					return true;
				}
			}
			else if (BassEnc.BASS_Encode_Stop(this.EncoderHandle))
			{
				this.EncoderHandle = 0;
				return true;
			}
			return false;
		}

		public virtual bool Stop(bool queue)
		{
			if (this.EncoderHandle == 0)
			{
				return true;
			}
			if (!this._useAsyncQueue)
			{
				return this.Stop();
			}
			if (BassEnc.BASS_Encode_StopEx(this.EncoderHandle, queue))
			{
				this.EncoderHandle = 0;
				return true;
			}
			return false;
		}

		public virtual bool Pause(bool paused)
		{
			return this.EncoderHandle != 0 && BassEnc.BASS_Encode_SetPaused(this.EncoderHandle, paused);
		}

		public virtual string SettingsString()
		{
			return string.Format("{0} kbps", this.EffectiveBitrate);
		}

		public static bool EncodeFile(string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput)
		{
			return BaseEncoder.EncodeFileInternal(inputFile, outputFile, encoder, proc, overwriteOutput, deleteInput, false, -1L, -1L);
		}

		public static bool EncodeFile(BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput)
		{
			return BaseEncoder.EncodeFileInternal(encoder.InputFile, encoder.OutputFile, encoder, proc, overwriteOutput, deleteInput, false, -1L, -1L);
		}

		public static bool EncodeFile(string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags)
		{
			return BaseEncoder.EncodeFileInternal(inputFile, outputFile, encoder, proc, overwriteOutput, deleteInput, updateTags, -1L, -1L);
		}

		public static bool EncodeFile(BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags)
		{
			return BaseEncoder.EncodeFileInternal(encoder.InputFile, encoder.OutputFile, encoder, proc, overwriteOutput, deleteInput, updateTags, -1L, -1L);
		}

		public static bool EncodeFile(string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags, long fromPos, long toPos)
		{
			return BaseEncoder.EncodeFileInternal(inputFile, outputFile, encoder, proc, overwriteOutput, deleteInput, updateTags, fromPos, toPos);
		}

		public static bool EncodeFile(BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags, long fromPos, long toPos)
		{
			return BaseEncoder.EncodeFileInternal(encoder.InputFile, encoder.OutputFile, encoder, proc, overwriteOutput, deleteInput, updateTags, fromPos, toPos);
		}

		public static bool EncodeFile(string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags, double fromPos, double toPos)
		{
			return BaseEncoder.EncodeFileInternal(inputFile, outputFile, encoder, proc, overwriteOutput, deleteInput, updateTags, fromPos, toPos);
		}

		public static bool EncodeFile(BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags, double fromPos, double toPos)
		{
			return BaseEncoder.EncodeFileInternal(encoder.InputFile, encoder.OutputFile, encoder, proc, overwriteOutput, deleteInput, updateTags, fromPos, toPos);
		}

		private static bool EncodeFileInternal(string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags, long fromPos, long toPos)
		{
			if (encoder == null || (inputFile == null && encoder.ChannelHandle == 0))
			{
				return false;
			}
			if (toPos <= fromPos)
			{
				fromPos = -1L;
				toPos = -1L;
			}
			bool flag = false;
			string inputFile2 = encoder.InputFile;
			string outputFile2 = encoder.OutputFile;
			int channelHandle = encoder.ChannelHandle;
			int num = 0;
			try
			{
				if (inputFile == null)
				{
					if (encoder.ChannelHandle == 0 || encoder.ChannelInfo == null || string.IsNullOrEmpty(encoder.ChannelInfo.filename))
					{
						return false;
					}
					num = encoder.ChannelHandle;
					inputFile = encoder.ChannelInfo.filename;
				}
				else
				{
					num = Bass.BASS_StreamCreateFile(inputFile, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | ((fromPos < 0L && toPos < 0L) ? BASSFlag.BASS_DEFAULT : BASSFlag.BASS_STREAM_PRESCAN));
				}
				flag = BaseEncoder.EncodeFileInternal2(num, inputFile, outputFile, encoder, proc, overwriteOutput, updateTags, fromPos, toPos);
			}
			catch
			{
				flag = false;
			}
			finally
			{
				if (num != 0)
				{
					Bass.BASS_StreamFree(num);
				}
				encoder.InputFile = inputFile2;
				encoder.OutputFile = outputFile2;
				encoder.ChannelHandle = channelHandle;
				if (flag && deleteInput)
				{
					try
					{
						File.Delete(inputFile);
					}
					catch
					{
					}
				}
			}
			return flag;
		}

		private static bool EncodeFileInternal(string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool deleteInput, bool updateTags, double fromPos, double toPos)
		{
			if (encoder == null || (inputFile == null && encoder.ChannelHandle == 0))
			{
				return false;
			}
			if (toPos <= fromPos)
			{
				fromPos = -1.0;
				toPos = -1.0;
			}
			bool flag = false;
			string inputFile2 = encoder.InputFile;
			string outputFile2 = encoder.OutputFile;
			int channelHandle = encoder.ChannelHandle;
			int num = 0;
			try
			{
				if (inputFile == null)
				{
					if (encoder.ChannelHandle == 0 || encoder.ChannelInfo == null || string.IsNullOrEmpty(encoder.ChannelInfo.filename))
					{
						return false;
					}
					num = encoder.ChannelHandle;
					inputFile = encoder.ChannelInfo.filename;
				}
				else
				{
					num = Bass.BASS_StreamCreateFile(inputFile, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | ((fromPos < 0.0 && toPos < 0.0) ? BASSFlag.BASS_DEFAULT : BASSFlag.BASS_STREAM_PRESCAN));
				}
				flag = BaseEncoder.EncodeFileInternal2(num, inputFile, outputFile, encoder, proc, overwriteOutput, updateTags, Bass.BASS_ChannelSeconds2Bytes(num, fromPos), Bass.BASS_ChannelSeconds2Bytes(num, toPos));
			}
			catch
			{
				flag = false;
			}
			finally
			{
				if (num != 0)
				{
					Bass.BASS_StreamFree(num);
				}
				encoder.InputFile = inputFile2;
				encoder.OutputFile = outputFile2;
				encoder.ChannelHandle = channelHandle;
				if (flag && deleteInput)
				{
					try
					{
						File.Delete(inputFile);
					}
					catch
					{
					}
				}
			}
			return flag;
		}

		private static bool EncodeFileInternal2(int stream, string inputFile, string outputFile, BaseEncoder encoder, BaseEncoder.ENCODEFILEPROC proc, bool overwriteOutput, bool updateTags, long fromPos, long toPos)
		{
			bool result = false;
			if (stream != 0)
			{
				long num = 0L;
				long num2 = Bass.BASS_ChannelGetLength(stream);
				if (toPos > 0L && toPos > num2)
				{
					toPos = num2;
				}
				if (updateTags)
				{
					TAG_INFO tags = new TAG_INFO(inputFile);
					if (BassTags.BASS_TAG_GetFromFile(stream, tags))
					{
						encoder.TAGs = tags;
					}
				}
				if (num2 > 0L)
				{
					if (outputFile == null)
					{
						outputFile = Path.ChangeExtension(inputFile, encoder.DefaultOutputExtension);
						while (outputFile.Equals(inputFile))
						{
							outputFile += encoder.DefaultOutputExtension;
						}
					}
					if (File.Exists(outputFile))
					{
						if (!overwriteOutput)
						{
							throw new IOException(string.Format("The output file {0} already exists!", outputFile));
						}
						File.Delete(outputFile);
					}
					encoder.InputFile = null;
					encoder.OutputFile = outputFile;
					encoder.ChannelHandle = stream;
					if (toPos > 0L)
					{
						num2 = toPos;
					}
					if (fromPos > 0L)
					{
						Bass.BASS_ChannelSetPosition(stream, fromPos);
						num2 -= fromPos;
					}
					int num3 = 262144;
					long num4 = toPos - fromPos;
					bool wma_UseNetwork = false;
					if (encoder is EncoderWMA)
					{
						wma_UseNetwork = ((EncoderWMA)encoder).WMA_UseNetwork;
						((EncoderWMA)encoder).WMA_UseNetwork = false;
					}
					if (encoder.Start(null, IntPtr.Zero, false))
					{
						byte[] buffer = new byte[num3];
						while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
						{
							if (fromPos < 0L && toPos < 0L)
							{
								num += (long)Bass.BASS_ChannelGetData(stream, buffer, num3);
								if (proc != null)
								{
									proc(num2, num);
								}
							}
							else
							{
								int num5;
								if (num4 < (long)num3)
								{
									num5 = Bass.BASS_ChannelGetData(stream, buffer, (int)num4);
								}
								else
								{
									num5 = Bass.BASS_ChannelGetData(stream, buffer, num3);
								}
								num += (long)num5;
								num4 -= (long)num5;
								if (proc != null)
								{
									proc(num2, num);
								}
								if (num4 <= 0L)
								{
									break;
								}
							}
						}
						if (encoder.Stop())
						{
							result = true;
						}
					}
					if (encoder is EncoderWMA)
					{
						((EncoderWMA)encoder).WMA_UseNetwork = wma_UseNetwork;
					}
				}
			}
			return result;
		}

		private bool disposed;

		private int _channel;

		private int _bitwidth = 16;

		private int _samplerate = 44100;

		private int _numchans = 2;

		private int _enc;

		private BASS_CHANNELINFO _channelInfo = new BASS_CHANNELINFO();

		private string _encDir = string.Empty;

		private string _inputFile;

		private bool _force16Bit;

		private bool _noLimit;

		private bool _useAsyncQueue;

		private string _outputFile;

		private TAG_INFO _tagInfo;

		public enum BITRATE
		{
			kbps_6 = 6,
			kbps_8 = 8,
			kbps_10 = 10,
			kbps_12 = 12,
			kbps_16 = 16,
			kbps_20 = 20,
			kbps_22 = 22,
			kbps_24 = 24,
			kbps_32 = 32,
			kbps_40 = 40,
			kbps_48 = 48,
			kbps_56 = 56,
			kbps_64 = 64,
			kbps_80 = 80,
			kbps_96 = 96,
			kbps_112 = 112,
			kbps_128 = 128,
			kbps_144 = 144,
			kbps_160 = 160,
			kbps_192 = 192,
			kbps_224 = 224,
			kbps_256 = 256,
			kbps_320 = 320
		}

		public enum SAMPLERATE
		{
			Hz_8000 = 8000,
			Hz_11025 = 11025,
			Hz_16000 = 16000,
			Hz_22050 = 22050,
			Hz_32000 = 32000,
			Hz_44100 = 44100,
			Hz_48000 = 48000,
			Hz_96000 = 96000,
			Hz_192000 = 192000
		}

		public delegate void ENCODEFILEPROC(long bytesTotal, long bytesDone);
	}
}
