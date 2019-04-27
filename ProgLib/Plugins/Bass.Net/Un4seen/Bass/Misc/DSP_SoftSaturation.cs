using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_SoftSaturation : BaseDSP
	{
		public DSP_SoftSaturation()
		{
		}

		public DSP_SoftSaturation(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
		}

		public double Depth
		{
			get
			{
				return this._depth;
			}
			set
			{
				if (value < 0.0)
				{
					this._depth = 0.0;
					return;
				}
				if (this._depth > 1.0)
				{
					this._depth = 1.0;
					return;
				}
				this._depth = value;
			}
		}

		public double Factor
		{
			get
			{
				return this._factor;
			}
			set
			{
				if (value < 0.0)
				{
					this._factor = 0.0;
					return;
				}
				if (this._factor > 0.99998848714)
				{
					this._factor = 0.99998848714;
					return;
				}
				this._factor = value;
			}
		}

		public double Factor_dBV
		{
			get
			{
				return Utils.LevelToDB(this._factor, 1.0);
			}
			set
			{
				if (value > -0.0001)
				{
					this._factor = 0.99998848714;
					return;
				}
				if (value == double.NegativeInfinity)
				{
					this._factor = 0.0;
					return;
				}
				this._factor = Utils.DBToLevel(value, 1.0);
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
					this._d = Math.Abs((double)ptr[i] / 32768.0);
					this._s = (double)Math.Sign(ptr[i]);
					if (this._d > 1.0)
					{
						this._d = (this._factor + 1.0) / 2.0;
					}
					else if (this._d > this._factor)
					{
						this._d = this._factor + (this._d - this._factor) / (1.0 + Math.Pow((this._d - this._factor) / (1.0 - this._factor), 2.0));
					}
					if (this._depth > 0.0)
					{
						this._d = Math.Tanh(1.4 * ((double)ptr[i] / 32768.0 + this._d * this._s * this._depth));
					}
					else
					{
						this._d *= this._s;
					}
					if (this._useDithering)
					{
						this._d = Utils.SampleDither(this._d * 32768.0, this._ditherFactor, 32768.0);
					}
					else
					{
						this._d = Math.Round(this._d * 32768.0);
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
					this._d = (double)Math.Abs(ptr2[j]);
					this._s = (double)Math.Sign(ptr2[j]);
					if (this._d > 1.0)
					{
						this._d = (this._factor + 1.0) / 2.0;
					}
					else if (this._d > this._factor)
					{
						this._d = this._factor + (this._d - this._factor) / (1.0 + Math.Pow((this._d - this._factor) / (1.0 - this._factor), 2.0));
					}
					if (this._depth > 0.0)
					{
						ptr2[j] = (float)Math.Tanh((double)ptr2[j] + this._d * this._s * this._depth);
					}
					else
					{
						ptr2[j] = (float)(this._d * this._s);
					}
				}
				return;
			}
			byte* ptr3 = (byte*)((void*)buffer);
			for (int k = 0; k < length; k++)
			{
				this._d = Math.Abs((double)ptr3[k] / 255.0);
				if (this._d > 1.0)
				{
					this._d = (this._factor + 1.0) / 2.0;
				}
				else if (this._d > this._factor)
				{
					this._d = this._factor + (this._d - this._factor) / (1.0 + Math.Pow((this._d - this._factor) / (1.0 - this._factor), 2.0));
				}
				if (this._depth > 0.0)
				{
					this._d = Math.Tanh(1.4 * ((double)ptr3[k] / 255.0 + this._d * this._depth));
				}
				if (this._useDithering)
				{
					this._d = Utils.SampleDither(this._d * 255.0, this._ditherFactor, 255.0);
				}
				else
				{
					this._d = Math.Round(this._d * 255.0);
				}
				if (this._d > 255.0)
				{
					ptr3[k] = byte.MaxValue;
				}
				else if (this._d < 0.0)
				{
					ptr3[k] = 0;
				}
				else
				{
					ptr3[k] = (byte)((double)((int)this._d) - 128.0);
				}
			}
		}

		public override string ToString()
		{
			return "Soft Saturation DSP";
		}

		private double _d;

		private double _s = 1.0;

		private double _factor = 0.5;

		private double _depth = 0.5;

		private bool _useDithering;

		private double _ditherFactor = 0.7;
	}
}
