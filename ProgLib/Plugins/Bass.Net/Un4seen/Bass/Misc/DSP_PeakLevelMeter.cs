using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class DSP_PeakLevelMeter : BaseDSP
	{
		public DSP_PeakLevelMeter()
		{
		}

		public DSP_PeakLevelMeter(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
			this.UpdateTime = this._updateTime;
		}

		public int LevelL
		{
			get
			{
				return (int)Math.Round(this._levelL);
			}
		}

		public int PeakHoldLevelL
		{
			get
			{
				return (int)Math.Round(this._peakHoldLevelL);
			}
		}

		public double LevelL_dBV
		{
			get
			{
				return Utils.LevelToDB(this._levelL, 32768.0);
			}
		}

		public double PeakHoldLevelL_dBV
		{
			get
			{
				return Utils.LevelToDB(this._peakHoldLevelL, 32768.0);
			}
		}

		public int LevelR
		{
			get
			{
				return (int)Math.Round(this._levelR);
			}
		}

		public int PeakHoldLevelR
		{
			get
			{
				return (int)Math.Round(this._peakHoldLevelR);
			}
		}

		public double LevelR_dBV
		{
			get
			{
				return Utils.LevelToDB(this._levelR, 32768.0);
			}
		}

		public double PeakHoldLevelR_dBV
		{
			get
			{
				return Utils.LevelToDB(this._peakHoldLevelR, 32768.0);
			}
		}

		public bool CalcRMS
		{
			get
			{
				return this._calcRMS;
			}
			set
			{
				this._calcRMS = value;
				if (!this._calcRMS)
				{
					this._rms = 0.0;
					this._avg = 0.0;
				}
			}
		}

		public double AVG
		{
			get
			{
				return this._avg;
			}
		}

		public double AVG_dBV
		{
			get
			{
				return Utils.LevelToDB(this._avg, 32768.0);
			}
		}

		public double RMS
		{
			get
			{
				return this._rms;
			}
		}

		public double RMS_dBV
		{
			get
			{
				return Utils.LevelToDB(this._rms, 32768.0);
			}
		}

		public float UpdateTime
		{
			get
			{
				return (float)Bass.BASS_ChannelBytes2Seconds(base.ChannelHandle, this._updateLength);
			}
			set
			{
				if (value <= 0f)
				{
					this._updateTime = 0f;
					this._updateLength = 0L;
				}
				else
				{
					if (value > 60f)
					{
						this._updateTime = 60f;
					}
					else
					{
						this._updateTime = value;
					}
					this._updateLength = Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._updateTime);
					if (this._updateLength <= 0L)
					{
						this._updateLength = 0L;
					}
				}
				this._refreshLength = 0L;
			}
		}

		public override void OnChannelChanged()
		{
			this.ResetPeakHold();
			this._updateLength = (long)((int)Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._updateTime));
		}

		public void ResetPeakHold()
		{
			this._peakHoldLevelL = 0.0;
			this._peakHoldLevelR = 0.0;
		}

		public unsafe override void DSPCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (base.IsBypassed)
			{
				return;
			}
			if (this._refreshLength == 0L)
			{
				this._squSum = 0.0;
				this._avgSum = 0.0;
				this._levelL = 0.0;
				this._levelR = 0.0;
			}
			if (base.ChannelBitwidth == 32)
			{
				this._length = length / 4;
				float* ptr = (float*)((void*)buffer);
				for (int i = 0; i < this._length; i++)
				{
					this._dummy = Math.Round(Math.Abs((double)ptr[i]) * 32768.0);
					if (i % 2 == 0)
					{
						if (this._dummy > this._levelL)
						{
							this._levelL = this._dummy;
						}
					}
					else if (this._dummy > this._levelR)
					{
						this._levelR = this._dummy;
					}
					if (this.CalcRMS)
					{
						this._avgSum += this._dummy;
						this._squSum += this._dummy * this._dummy;
					}
				}
			}
			else if (base.ChannelBitwidth == 16)
			{
				this._length = length / 2;
				short* ptr2 = (short*)((void*)buffer);
				for (int j = 0; j < this._length; j++)
				{
					this._dummy = Math.Abs((double)ptr2[j]);
					if (j % 2 == 0)
					{
						if (this._dummy > this._levelL)
						{
							this._levelL = this._dummy;
						}
					}
					else if (this._dummy > this._levelR)
					{
						this._levelR = this._dummy;
					}
					if (this.CalcRMS)
					{
						this._avgSum += this._dummy;
						this._squSum += this._dummy * this._dummy;
					}
				}
			}
			else
			{
				this._length = length;
				byte* ptr3 = (byte*)((void*)buffer);
				for (int k = 0; k < this._length; k++)
				{
					this._dummy = (double)((int)(ptr3[k] - 128) * 256);
					if (k % 2 == 0)
					{
						if (this._dummy > this._levelL)
						{
							this._levelL = this._dummy;
						}
					}
					else if (this._dummy > this._levelR)
					{
						this._levelR = this._dummy;
					}
					if (this.CalcRMS)
					{
						this._avgSum += this._dummy;
						this._squSum += this._dummy * this._dummy;
					}
				}
			}
			if (base.ChannelNumChans == 1)
			{
				this._levelR = (this._levelL = Math.Max(this._levelL, this._levelR));
			}
			this._refreshLength += (long)length;
			if (this._refreshLength >= this._updateLength)
			{
				if (this.CalcRMS && this._refreshLength > 0L)
				{
					this._avg = this._avgSum / (double)this._refreshLength;
					this._rms = Math.Sqrt(((double)this._refreshLength * this._squSum - this._avgSum * this._avgSum) / ((double)this._refreshLength * ((double)this._refreshLength - 1.0)));
				}
				if (this._levelL > this._peakHoldLevelL)
				{
					this._peakHoldLevelL = this._levelL;
				}
				if (this._levelR > this._peakHoldLevelR)
				{
					this._peakHoldLevelR = this._levelR;
				}
				this._refreshLength = 0L;
				base.RaiseNotification();
			}
		}

		public override string ToString()
		{
			return "Peak Level Meter DSP";
		}

		private double _levelL;

		private double _levelR;

		private double _peakHoldLevelL;

		private double _peakHoldLevelR;

		private int _length;

		private double _dummy;

		private float _updateTime = 0.1f;

		private long _updateLength = 35280L;

		private long _refreshLength;

		private bool _calcRMS;

		private double _rms;

		private double _avg;

		private double _squSum;

		private double _avgSum;
	}
}
