using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_Gain : BaseDSP
	{
		public DSP_Gain()
		{
		}

		public DSP_Gain(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
		}

		public double Gain
		{
			get
			{
				return this._gain;
			}
			set
			{
				if (value < 0.0)
				{
					this._gain = 0.0;
					return;
				}
				if (value > 1024.0)
				{
					this._gain = 1024.0;
					return;
				}
				this._gain = value;
			}
		}

		public double Gain_dBV
		{
			get
			{
				return Utils.LevelToDB(this._gain, 1.0);
			}
			set
			{
				if (value > 60.0)
				{
					this._gain = Utils.DBToLevel(60.0, 1.0);
					return;
				}
				if (value == double.NegativeInfinity)
				{
					this._gain = 0.0;
					return;
				}
				this._gain = Utils.DBToLevel(value, 1.0);
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
			if (base.IsBypassed || this._gain == 1.0)
			{
				return;
			}
			if (base.ChannelBitwidth == 32)
			{
				float* ptr = (float*)((void*)buffer);
				for (int i = 0; i < length / 4; i++)
				{
					ptr[i] = (float)((double)ptr[i] * this._gain);
				}
				return;
			}
			if (base.ChannelBitwidth == 16)
			{
				short* ptr2 = (short*)((void*)buffer);
				for (int j = 0; j < length / 2; j++)
				{
					if (this._useDithering)
					{
						this._d = Utils.SampleDither((double)ptr2[j] * this._gain, this._ditherFactor, 32768.0);
					}
					else
					{
						this._d = Math.Round((double)ptr2[j] * this._gain);
					}
					if (this._d > 32767.0)
					{
						ptr2[j] = short.MaxValue;
					}
					else if (this._d < -32768.0)
					{
						ptr2[j] = short.MinValue;
					}
					else
					{
						ptr2[j] = (short)this._d;
					}
				}
				return;
			}
			byte* ptr3 = (byte*)((void*)buffer);
			for (int k = 0; k < length; k++)
			{
				if (this._useDithering)
				{
					this._d = Utils.SampleDither((double)(ptr3[k] - 128) * this._gain, this._ditherFactor, 128.0);
				}
				else
				{
					this._d = Math.Round((double)(ptr3[k] - 128) * this._gain);
				}
				if (this._d > 127.0)
				{
					ptr3[k] = byte.MaxValue;
				}
				else if (this._d < -128.0)
				{
					ptr3[k] = 0;
				}
				else
				{
					ptr3[k] = (byte)((double)((int)this._d) + 128.0);
				}
			}
		}

		public override string ToString()
		{
			return "Gain DSP";
		}

		private double _gain = 1.0;

		private double _d;

		private bool _useDithering;

		private double _ditherFactor = 0.7;
	}
}
