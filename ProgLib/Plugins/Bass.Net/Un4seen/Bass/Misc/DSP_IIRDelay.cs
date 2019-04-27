using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_IIRDelay : BaseDSP
	{
		public DSP_IIRDelay(float maxDelay)
		{
			this._maxDelay = maxDelay;
		}

		public DSP_IIRDelay(int channel, int priority, float maxDelay) : base(channel, priority, IntPtr.Zero)
		{
			if (maxDelay > 60f)
			{
				maxDelay = 60f;
			}
			else if (maxDelay < 0.001f)
			{
				maxDelay = 0.001f;
			}
			this._maxDelay = maxDelay;
			this.MAX_DELAY = (int)(Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)maxDelay) / (long)(base.ChannelBitwidth / 8));
			this.buffer = new double[this.MAX_DELAY];
		}

		public double WetDry
		{
			get
			{
				return this._wetDry;
			}
			set
			{
				if (value < 0.0)
				{
					this._wetDry = 0.0;
					return;
				}
				if (value > 1.0)
				{
					this._wetDry = 1.0;
					return;
				}
				this._wetDry = value;
			}
		}

		public double Feedback
		{
			get
			{
				return this._feedback;
			}
			set
			{
				if (value < 0.0)
				{
					this._feedback = 0.0;
					return;
				}
				if (value > 1.0)
				{
					this._feedback = 1.0;
					return;
				}
				this._feedback = value;
			}
		}

		public int Delay
		{
			get
			{
				return this._delay;
			}
			set
			{
				if (value < 0)
				{
					this._delay = 0;
					return;
				}
				this._delay = value;
			}
		}

		public float DelaySeconds
		{
			get
			{
				return (float)Bass.BASS_ChannelBytes2Seconds(base.ChannelHandle, (long)(this._delay * (base.ChannelBitwidth / 8)));
			}
			set
			{
				if (value < 0f)
				{
					this._delay = 0;
					return;
				}
				if (value > 60f)
				{
					this._delay = (int)(Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, 60.0) / (long)(base.ChannelBitwidth / 8));
					return;
				}
				this._delay = (int)(Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)value) / (long)(base.ChannelBitwidth / 8));
			}
		}

		public void Preset_Default()
		{
			this.WetDry = 0.5;
			this.Feedback = 0.5;
			this.DelaySeconds = 0.25f;
		}

		public void Preset_Metallic()
		{
			this.WetDry = 0.5;
			this.Feedback = 0.75;
			this.DelaySeconds = 0.004f;
		}

		public void Preset_Echo()
		{
			this.WetDry = 0.42;
			this.Feedback = 0.3;
			this.DelaySeconds = 0.5f;
		}

		public void Reset()
		{
			this.counter = 0;
			for (int i = 0; i < this.MAX_DELAY; i++)
			{
				this.buffer[i] = 0.0;
			}
		}

		public bool UseDithering
		{
			get
			{
				return this._useDithering;
			}
			set
			{
				this._useDithering = value;
			}
		}

		public double DitherFactor
		{
			get
			{
				return this._ditherFactor;
			}
			set
			{
				this._ditherFactor = value;
			}
		}

		public override void OnChannelChanged()
		{
			this.MAX_DELAY = (int)(Bass.BASS_ChannelSeconds2Bytes(base.ChannelHandle, (double)this._maxDelay) / (long)(base.ChannelBitwidth / 8));
			this.buffer = new double[this.MAX_DELAY];
		}

		public unsafe override void DSPCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (base.IsBypassed)
			{
				return;
			}
			if (base.ChannelBitwidth == 16)
			{
				short* ptr = (short*)((void*)buffer);
				for (int i = 0; i < length / 2; i++)
				{
					this._d = (double)ptr[i] * (1.0 - this._wetDry) + this.ProcessSample((double)ptr[i]) * this._wetDry;
					if (this._useDithering)
					{
						this._d = Utils.SampleDither(this._d, this._ditherFactor, 32768.0);
					}
					else
					{
						this._d = Math.Round(this._d);
					}
					if (this._d > 32767.0)
					{
						ptr[i] = short.MaxValue;
					}
					else if (this._d < -32768.0)
					{
						ptr[i] = short.MinValue;
					}
					else
					{
						ptr[i] = (short)this._d;
					}
				}
				return;
			}
			if (base.ChannelBitwidth == 32)
			{
				float* ptr2 = (float*)((void*)buffer);
				for (int j = 0; j < length / 4; j++)
				{
					ptr2[j] = (float)((double)ptr2[j] * (1.0 - this._wetDry) + this.ProcessSample((double)ptr2[j]) * this._wetDry);
				}
				return;
			}
			byte* ptr3 = (byte*)((void*)buffer);
			for (int k = 0; k < length; k++)
			{
				this._d = (double)((int)(ptr3[k] - 128) * 256);
				this._d = this._d * (1.0 - this._wetDry) + this.ProcessSample(this._d) * this._wetDry;
				if (this._useDithering)
				{
					this._d = Utils.SampleDither(this._d, this._ditherFactor, 32768.0);
				}
				else
				{
					this._d = Math.Round(this._d);
				}
				if (this._d > 32767.0)
				{
					ptr3[k] = byte.MaxValue;
				}
				else if (this._d < -32768.0)
				{
					ptr3[k] = 0;
				}
				else
				{
					ptr3[k] = (byte)((int)this._d / 256 + 128);
				}
			}
		}

		private double ProcessSample(double input)
		{
			if (this._delay > 0)
			{
				this.back = this.counter - this._delay;
				if (this.back < 0)
				{
					this.back = this.MAX_DELAY + this.back;
				}
				this.idx0 = this.back;
				this.idx_1 = this.idx0 - base.ChannelInfo.chans;
				this.idx1 = this.idx0 + base.ChannelInfo.chans;
				this.idx2 = this.idx0 + base.ChannelInfo.chans * 2;
				if (this.idx_1 < 0)
				{
					this.idx_1 = this.MAX_DELAY + this.idx_1;
				}
				if (this.idx1 >= this.MAX_DELAY)
				{
					this.idx1 %= this.MAX_DELAY;
				}
				if (this.idx2 >= this.MAX_DELAY)
				{
					this.idx2 %= this.MAX_DELAY;
				}
				this.y_1 = this.buffer[this.idx_1];
				this.y0 = this.buffer[this.idx0];
				this.y1 = this.buffer[this.idx1];
				this.y2 = this.buffer[this.idx2];
				this.c0 = this.y0;
				this.c1 = 0.5 * (this.y1 - this.y_1);
				this.c2 = this.y_1 - 2.5 * this.y0 + 2.0 * this.y1 - 0.5 * this.y2;
				this.c3 = 0.5 * (this.y2 - this.y_1) + 1.5 * (this.y0 - this.y1);
				this.output = ((this.c3 * 0.5 + this.c2) * 0.5 + this.c1) * 0.5 + this.c0;
			}
			else
			{
				this.output = input;
			}
			this.buffer[this.counter] = input * (1.5 - this._feedback) + this.output * this._feedback;
			this.counter++;
			if (this.counter >= this.MAX_DELAY)
			{
				this.counter = 0;
			}
			return this.output;
		}

		public override string ToString()
		{
			return "IIR Delay DSP";
		}

		private int _delay = 4096;

		private double _feedback = 0.5;

		private double _wetDry = 0.7;

		private double _d;

		private bool _useDithering;

		private double _ditherFactor = 0.7;

		private float _maxDelay = 2f;

		private int MAX_DELAY = 88200;

		private double[] buffer;

		private int counter;

		private int back;

		private int idx_1;

		private int idx0;

		private int idx1;

		private int idx2;

		private double y_1;

		private double y0;

		private double y1;

		private double y2;

		private double c0;

		private double c1;

		private double c2;

		private double c3;

		private double output;
	}
}
