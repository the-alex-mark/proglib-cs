using System;
using System.Security;
using Un4seen.Bass.AddOn.Mix;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_StreamCopy : BaseDSP
	{
		public DSP_StreamCopy()
		{
		}

		public DSP_StreamCopy(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
		}

		public int StreamCopy
		{
			get
			{
				return this._streamCopy;
			}
		}

		public BASSFlag StreamCopyFlags
		{
			get
			{
				return this._streamCopyFlags;
			}
			set
			{
				this.OnStopped();
				this._streamCopyFlags = value;
				if (base.IsAssigned)
				{
					this.OnStarted();
				}
			}
		}

		public int StreamCopyDevice
		{
			get
			{
				return this._streamCopyDevice;
			}
			set
			{
				this.OnStopped();
				this._streamCopyDevice = value;
				if (base.IsAssigned)
				{
					this.OnStarted();
				}
			}
		}

		public int OutputLatency
		{
			get
			{
				return this._outputLatency;
			}
			set
			{
				if (value < 0)
				{
					this._outputLatency = 0;
					return;
				}
				this._outputLatency = value;
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
				this.OnStopped();
				this._isOutputBuffered = value;
				if (base.IsAssigned)
				{
					this.OnStarted();
				}
			}
		}

		public int SourceMixerStream
		{
			get
			{
				return this._sourceMixerStream;
			}
			set
			{
				this.OnStopped();
				this._sourceMixerStream = value;
				this._isSourceMixerNonstop = ((Bass.BASS_ChannelFlags(this._sourceMixerStream, BASSFlag.BASS_STREAM_PRESCAN, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_STREAM_PRESCAN) > BASSFlag.BASS_DEFAULT);
				if (base.IsAssigned)
				{
					this.OnStarted();
				}
			}
		}

		public int TargetMixerStream
		{
			get
			{
				return this._targetMixerStream;
			}
			set
			{
				this.OnStopped();
				this._targetMixerStream = value;
				BASSFlag bassflag = Bass.BASS_ChannelFlags(this._targetMixerStream, BASSFlag.BASS_DEFAULT, BASSFlag.BASS_DEFAULT);
				this._isTargetMixerNonstop = ((bassflag & BASSFlag.BASS_STREAM_PRESCAN) > BASSFlag.BASS_DEFAULT);
				this._isTargetMixerImmediate = ((bassflag & BASSFlag.BASS_AAC_FRAME960) > BASSFlag.BASS_DEFAULT);
				if (this._targetMixerStream != 0)
				{
					this._streamCopyFlags |= BASSFlag.BASS_STREAM_DECODE;
				}
				if (base.IsAssigned)
				{
					this.OnStarted();
				}
			}
		}

		public void ReSync()
		{
			if (!base.IsAssigned)
			{
				return;
			}
			if (this._isOutputBuffered && this.TargetMixerStream != 0 && this.SourceMixerStream != 0)
			{
				Bass.BASS_ChannelLock(this.SourceMixerStream, true);
				int num = Bass.BASS_ChannelGetData(this.TargetMixerStream, IntPtr.Zero, 0);
				if (num > 0)
				{
					num = (int)Bass.BASS_ChannelSeconds2Bytes(this._streamCopy, Bass.BASS_ChannelBytes2Seconds(this.TargetMixerStream, (long)num));
					if (!this._isSourceMixerNonstop && this._isTargetMixerNonstop)
					{
						this._streamCopyDelay = num;
						BassMix.BASS_Mixer_ChannelSetPosition(this._streamCopy, 0L);
					}
					else if (!this._isTargetMixerNonstop)
					{
						int num2 = Bass.BASS_ChannelGetData(this.SourceMixerStream, IntPtr.Zero, 0);
						num2 = (int)Bass.BASS_ChannelSeconds2Bytes(this._streamCopy, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num2));
						if (num2 > num)
						{
							byte[] buffer = new byte[num2 - num];
							Bass.BASS_StreamPutData(this._streamCopy, buffer, num2 - num);
						}
					}
				}
				else if (this._isSourceMixerNonstop)
				{
					int num3 = Bass.BASS_ChannelGetData(this.SourceMixerStream, IntPtr.Zero, 0);
					num3 = (int)Bass.BASS_ChannelSeconds2Bytes(this._streamCopy, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num3));
					int num4 = (int)Bass.BASS_ChannelSeconds2Bytes(this._streamCopy, (double)this._outputLatency / 1000.0);
					if (num3 > num4)
					{
						BassMix.BASS_Mixer_ChannelSetPosition(this._streamCopy, 0L);
						byte[] buffer2 = new byte[num3 - num4];
						Bass.BASS_StreamPutData(this._streamCopy, buffer2, num3 - num4);
					}
				}
				if (this._isTargetMixerImmediate && !this._isTargetMixerNonstop)
				{
					Bass.BASS_ChannelUpdate(this.TargetMixerStream, 0);
				}
				Bass.BASS_ChannelLock(this.SourceMixerStream, false);
			}
		}

		public override void OnChannelChanged()
		{
			this.OnStopped();
			if (base.IsAssigned)
			{
				this.OnStarted();
			}
		}

		public override void OnBypassChanged()
		{
			if (base.IsBypassed)
			{
				if (this._isOutputBuffered && !base.ChannelInfo.IsDecodingChannel)
				{
					Bass.BASS_ChannelPause(this._streamCopy);
					Bass.BASS_ChannelSetPosition(this._streamCopy, 0L);
					return;
				}
			}
			else
			{
				int streamCopy = this._streamCopy;
				this._streamCopy = 0;
				if (this.SourceMixerStream == 0)
				{
					Bass.BASS_ChannelLock(base.ChannelHandle, true);
				}
				else
				{
					Bass.BASS_ChannelLock(this.SourceMixerStream, true);
				}
				if (this._isOutputBuffered)
				{
					if (this.SourceMixerStream != 0)
					{
						if (this.TargetMixerStream != 0)
						{
							if (base.ChannelInfo.IsDecodingChannel && (BassMix.BASS_Mixer_ChannelFlags(base.ChannelHandle, BASSFlag.BASS_RECORD_ECHOCANCEL, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_RECORD_ECHOCANCEL) != BASSFlag.BASS_DEFAULT)
							{
								int num = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, IntPtr.Zero, 0);
								int num2 = Bass.BASS_ChannelGetData(this.TargetMixerStream, IntPtr.Zero, 0);
								num2 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, Bass.BASS_ChannelBytes2Seconds(this.TargetMixerStream, (long)num2));
								if (num2 > 0)
								{
									num -= num2;
								}
								if (num > 0)
								{
									int num3 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, (double)this._outputLatency / 1000.0);
									if (num2 > 0)
									{
										num3 = 0;
									}
									byte[] array = new byte[num];
									num = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, array, num);
									if (num > num3)
									{
										if (num3 > 0)
										{
											Array.Copy(array, num3, array, 0, num - num3);
										}
										Bass.BASS_StreamPutData(streamCopy, array, num - num3);
									}
								}
							}
							else
							{
								int num4 = Bass.BASS_ChannelGetData(this.SourceMixerStream, IntPtr.Zero, 0);
								num4 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num4));
								int num5 = Bass.BASS_ChannelGetData(this.TargetMixerStream, IntPtr.Zero, 0);
								num5 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, Bass.BASS_ChannelBytes2Seconds(this.TargetMixerStream, (long)num5));
								num4 -= num5;
								if (num4 > 0)
								{
									int num6 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, (double)this._outputLatency / 1000.0);
									if (num5 > 0)
									{
										num6 = 0;
									}
									byte[] array2 = new byte[num4];
									if (!base.ChannelInfo.IsDecodingChannel)
									{
										num4 = Bass.BASS_ChannelGetData(this.SourceMixerStream, array2, num4);
										num4 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num4));
									}
									if (num4 > num6)
									{
										if (num6 > 0)
										{
											Array.Copy(array2, num6, array2, 0, num4 - num6);
										}
										Bass.BASS_StreamPutData(streamCopy, array2, num4 - num6);
									}
								}
							}
						}
						else if (base.ChannelInfo.IsDecodingChannel && (BassMix.BASS_Mixer_ChannelFlags(base.ChannelHandle, BASSFlag.BASS_RECORD_ECHOCANCEL, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_RECORD_ECHOCANCEL) != BASSFlag.BASS_DEFAULT)
						{
							int num7 = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, IntPtr.Zero, 0);
							if (num7 > 0)
							{
								int num8 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, (double)this._outputLatency / 1000.0);
								byte[] array3 = new byte[num7];
								num7 = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, array3, num7);
								if (num7 > num8)
								{
									if (num8 > 0)
									{
										Array.Copy(array3, num8, array3, 0, num7 - num8);
									}
									Bass.BASS_StreamPutData(streamCopy, array3, num7 - num8);
								}
							}
						}
						else
						{
							int num9 = Bass.BASS_ChannelGetData(this.SourceMixerStream, IntPtr.Zero, 0);
							num9 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num9));
							if (num9 > 0)
							{
								int num10 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, (double)this._outputLatency / 1000.0);
								byte[] array4 = new byte[num9];
								if (!base.ChannelInfo.IsDecodingChannel)
								{
									num9 = Bass.BASS_ChannelGetData(this.SourceMixerStream, array4, num9);
									num9 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num9));
								}
								if (num9 > num10)
								{
									if (num10 > 0)
									{
										Array.Copy(array4, num10, array4, 0, num9 - num10);
									}
									Bass.BASS_StreamPutData(streamCopy, array4, num9 - num10);
								}
							}
						}
					}
					else if (!base.ChannelInfo.IsDecodingChannel)
					{
						int num11 = (int)Bass.BASS_ChannelSeconds2Bytes(streamCopy, (double)this._outputLatency / 1000.0);
						int num12 = Bass.BASS_ChannelGetData(base.ChannelHandle, IntPtr.Zero, 0);
						byte[] array5 = new byte[num12];
						num12 = Bass.BASS_ChannelGetData(base.ChannelHandle, array5, num12);
						if (num12 > num11)
						{
							if (num11 > 0)
							{
								Array.Copy(array5, num11, array5, 0, num12 - num11);
							}
							Bass.BASS_StreamPutData(streamCopy, array5, num12 - num11);
						}
					}
				}
				this._streamCopy = streamCopy;
				if (this.TargetMixerStream != 0 && !this._isTargetMixerImmediate && !this._isTargetMixerNonstop)
				{
					Bass.BASS_ChannelUpdate(this.TargetMixerStream, 0);
				}
				if ((!base.ChannelInfo.IsDecodingChannel || this.SourceMixerStream != 0) && Bass.BASS_ChannelIsActive(base.ChannelHandle) == BASSActive.BASS_ACTIVE_PLAYING)
				{
					Bass.BASS_ChannelPlay(this._streamCopy, false);
				}
				if (this.SourceMixerStream == 0)
				{
					Bass.BASS_ChannelLock(base.ChannelHandle, false);
					return;
				}
				Bass.BASS_ChannelLock(this.SourceMixerStream, false);
			}
		}

		public override void OnStarted()
		{
			int num = Bass.BASS_GetDevice();
			if (num != this._streamCopyDevice)
			{
				Bass.BASS_SetDevice(this._streamCopyDevice);
			}
			if (base.ChannelBitwidth == 32)
			{
				this._streamCopyFlags &= ~BASSFlag.BASS_SAMPLE_8BITS;
				this._streamCopyFlags |= BASSFlag.BASS_SAMPLE_FLOAT;
			}
			else if (base.ChannelBitwidth == 8)
			{
				this._streamCopyFlags &= ~BASSFlag.BASS_SAMPLE_FLOAT;
				this._streamCopyFlags |= BASSFlag.BASS_SAMPLE_8BITS;
			}
			else
			{
				this._streamCopyFlags &= ~BASSFlag.BASS_SAMPLE_FLOAT;
				this._streamCopyFlags &= ~BASSFlag.BASS_SAMPLE_8BITS;
			}
			int num2 = Bass.BASS_StreamCreatePush(base.ChannelSampleRate, base.ChannelNumChans, this._streamCopyFlags, IntPtr.Zero);
			this._streamCopyDelay = 0;
			if (this.SourceMixerStream == 0)
			{
				Bass.BASS_ChannelLock(base.ChannelHandle, true);
			}
			else
			{
				Bass.BASS_ChannelLock(this.SourceMixerStream, true);
			}
			if (this._isOutputBuffered)
			{
				if (this.SourceMixerStream != 0)
				{
					if (this.TargetMixerStream != 0)
					{
						if (base.ChannelInfo.IsDecodingChannel && (BassMix.BASS_Mixer_ChannelFlags(base.ChannelHandle, BASSFlag.BASS_RECORD_ECHOCANCEL, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_RECORD_ECHOCANCEL) != BASSFlag.BASS_DEFAULT)
						{
							int num3 = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, IntPtr.Zero, 0);
							int num4 = Bass.BASS_ChannelGetData(this.TargetMixerStream, IntPtr.Zero, 0);
							num4 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, Bass.BASS_ChannelBytes2Seconds(this.TargetMixerStream, (long)num4));
							if (num4 > 0)
							{
								num3 -= num4;
							}
							if (num3 > 0)
							{
								int num5 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, (double)this._outputLatency / 1000.0);
								if (num4 > 0)
								{
									num5 = 0;
								}
								byte[] array = new byte[num3];
								num3 = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, array, num3);
								if (num3 > num5)
								{
									if (num5 > 0)
									{
										Array.Copy(array, num5, array, 0, num3 - num5);
									}
									Bass.BASS_StreamPutData(num2, array, num3 - num5);
								}
							}
						}
						else
						{
							int num6 = Bass.BASS_ChannelGetData(this.SourceMixerStream, IntPtr.Zero, 0);
							num6 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num6));
							int num7 = Bass.BASS_ChannelGetData(this.TargetMixerStream, IntPtr.Zero, 0);
							num7 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, Bass.BASS_ChannelBytes2Seconds(this.TargetMixerStream, (long)num7));
							num6 -= num7;
							if (num6 > 0)
							{
								int num8 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, (double)this._outputLatency / 1000.0);
								if (num7 > 0)
								{
									num8 = 0;
								}
								byte[] array2 = new byte[num6];
								if (!base.ChannelInfo.IsDecodingChannel)
								{
									num6 = Bass.BASS_ChannelGetData(this.SourceMixerStream, array2, num6);
									num6 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num6));
								}
								if (num6 > num8)
								{
									if (num8 > 0)
									{
										Array.Copy(array2, num8, array2, 0, num6 - num8);
									}
									Bass.BASS_StreamPutData(num2, array2, num6 - num8);
								}
							}
						}
					}
					else if (base.ChannelInfo.IsDecodingChannel && (BassMix.BASS_Mixer_ChannelFlags(base.ChannelHandle, BASSFlag.BASS_RECORD_ECHOCANCEL, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_RECORD_ECHOCANCEL) != BASSFlag.BASS_DEFAULT)
					{
						int num9 = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, IntPtr.Zero, 0);
						if (num9 > 0)
						{
							int num10 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, (double)this._outputLatency / 1000.0);
							byte[] array3 = new byte[num9];
							num9 = BassMix.BASS_Mixer_ChannelGetData(base.ChannelHandle, array3, num9);
							if (num9 > num10)
							{
								if (num10 > 0)
								{
									Array.Copy(array3, num10, array3, 0, num9 - num10);
								}
								Bass.BASS_StreamPutData(num2, array3, num9 - num10);
							}
						}
					}
					else
					{
						int num11 = Bass.BASS_ChannelGetData(this.SourceMixerStream, IntPtr.Zero, 0);
						num11 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num11));
						if (num11 > 0)
						{
							int num12 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, (double)this._outputLatency / 1000.0);
							byte[] array4 = new byte[num11];
							if (!base.ChannelInfo.IsDecodingChannel)
							{
								num11 = Bass.BASS_ChannelGetData(this.SourceMixerStream, array4, num11);
								num11 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, Bass.BASS_ChannelBytes2Seconds(this.SourceMixerStream, (long)num11));
							}
							if (num11 > num12)
							{
								if (num12 > 0)
								{
									Array.Copy(array4, num12, array4, 0, num11 - num12);
								}
								Bass.BASS_StreamPutData(num2, array4, num11 - num12);
							}
						}
					}
				}
				else if (!base.ChannelInfo.IsDecodingChannel)
				{
					int num13 = (int)Bass.BASS_ChannelSeconds2Bytes(num2, (double)this._outputLatency / 1000.0);
					int num14 = Bass.BASS_ChannelGetData(base.ChannelHandle, IntPtr.Zero, 0);
					byte[] array5 = new byte[num14];
					num14 = Bass.BASS_ChannelGetData(base.ChannelHandle, array5, num14);
					if (num14 > num13)
					{
						if (num13 > 0)
						{
							Array.Copy(array5, num13, array5, 0, num14 - num13);
						}
						Bass.BASS_StreamPutData(num2, array5, num14 - num13);
					}
				}
			}
			this._streamCopy = num2;
			if (this.TargetMixerStream != 0 && (this._streamCopyFlags & BASSFlag.BASS_STREAM_DECODE) != BASSFlag.BASS_DEFAULT)
			{
				BassMix.BASS_Mixer_StreamAddChannel(this.TargetMixerStream, this._streamCopy, ((BassMix.BASS_Mixer_ChannelFlags(base.ChannelHandle, BASSFlag.BASS_RECORD_ECHOCANCEL, BASSFlag.BASS_DEFAULT) & BASSFlag.BASS_RECORD_ECHOCANCEL) != BASSFlag.BASS_DEFAULT) ? BASSFlag.BASS_RECORD_ECHOCANCEL : BASSFlag.BASS_DEFAULT);
				if (!this._isTargetMixerImmediate && !this._isTargetMixerNonstop)
				{
					Bass.BASS_ChannelUpdate(this.TargetMixerStream, 0);
				}
			}
			if ((!base.ChannelInfo.IsDecodingChannel || this.SourceMixerStream != 0) && Bass.BASS_ChannelIsActive(base.ChannelHandle) == BASSActive.BASS_ACTIVE_PLAYING)
			{
				Bass.BASS_ChannelPlay(this._streamCopy, false);
			}
			if (this.SourceMixerStream == 0)
			{
				Bass.BASS_ChannelLock(base.ChannelHandle, false);
			}
			else
			{
				Bass.BASS_ChannelLock(this.SourceMixerStream, false);
			}
			if (!base.ChannelInfo.IsDecodingChannel)
			{
				Bass.BASS_ChannelSetLink(base.ChannelHandle, this._streamCopy);
			}
			Bass.BASS_SetDevice(num);
		}

		public override void OnStopped()
		{
			if (this._streamCopy != 0)
			{
				if (this.TargetMixerStream != 0)
				{
					BassMix.BASS_Mixer_ChannelRemove(this._streamCopy);
				}
				Bass.BASS_ChannelRemoveLink(base.ChannelHandle, this._streamCopy);
				Bass.BASS_StreamFree(this._streamCopy);
				this._streamCopy = 0;
			}
		}

		public override void DSPCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (base.IsBypassed || this._streamCopy == 0)
			{
				return;
			}
			if (this._streamCopyDelay > 0)
			{
				this._streamCopyDelay -= length;
				return;
			}
			Bass.BASS_StreamPutData(this._streamCopy, buffer, length);
		}

		public override string ToString()
		{
			return "Stream Copy DSP";
		}

		private int _streamCopy;

		private int _sourceMixerStream;

		private bool _isSourceMixerNonstop;

		private int _targetMixerStream;

		private bool _isTargetMixerNonstop;

		private bool _isTargetMixerImmediate;

		private int _streamCopyDelay;

		private BASSFlag _streamCopyFlags;

		private int _streamCopyDevice = Bass.BASS_GetDevice();

		private bool _isOutputBuffered = true;

		private int _outputLatency;
	}
}
