using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_StereoEnhancer : BaseDSP
	{
		public DSP_StereoEnhancer()
		{
			this._gain = Utils.DBToLevel(-1.0 * this._wideCoeff * this._wetDry, 1.0) + 0.2;
		}

		public DSP_StereoEnhancer(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
			this._gain = Utils.DBToLevel(-1.0 * this._wideCoeff * this._wetDry, 1.0) + 0.2;
		}

		public double WideCoeff
		{
			get
			{
				return this._wideCoeff;
			}
			set
			{
				this._wideCoeff = value;
				this._gain = Utils.DBToLevel(-1.0 * this._wideCoeff * this._wetDry, 1.0) + 0.2;
			}
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
				}
				else if (value > 1.0)
				{
					this._wetDry = 1.0;
				}
				else
				{
					this._wetDry = value;
				}
				this._gain = Utils.DBToLevel(-1.0 * this._wideCoeff * this._wetDry, 1.0) + 0.2;
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
			if (base.ChannelBitwidth == 32)
			{
				float* ptr = (float*)((void*)buffer);
				for (int i = 0; i < length / 4; i += 2)
				{
					this._lCh = (double)ptr[i];
					this._rCh = (double)ptr[i + 1];
					this._mono = (this._lCh + this._rCh) / 2.0;
					this._lCh = ((this._lCh - this._mono) * this._wideCoeff + this._lCh) / 2.0 * this._gain;
					this._rCh = ((this._rCh - this._mono) * this._wideCoeff + this._rCh) / 2.0 * this._gain;
					ptr[i] = (float)(this._lCh * this._wetDry + (double)ptr[i] * (1.0 - this._wetDry));
					ptr[i + 1] = (float)(this._rCh * this._wetDry + (double)ptr[i + 1] * (1.0 - this._wetDry));
				}
				return;
			}
			if (base.ChannelBitwidth == 16)
			{
				short* ptr2 = (short*)((void*)buffer);
				for (int j = 0; j < length / 2; j += 2)
				{
					this._lCh = (double)ptr2[j] / 32768.0;
					this._rCh = (double)ptr2[j + 1] / 32768.0;
					this._mono = (this._lCh + this._rCh) / 2.0;
					if (this._useDithering)
					{
						this._lCh = Utils.SampleDither((((this._lCh - this._mono) * this._wideCoeff + this._lCh) / 2.0 * this._gain + this._lCh * (1.0 - this._wetDry)) * 32768.0, this._ditherFactor, 32768.0);
						this._rCh = Utils.SampleDither((((this._rCh - this._mono) * this._wideCoeff + this._rCh) / 2.0 * this._gain + this._rCh * (1.0 - this._wetDry)) * 32768.0, this._ditherFactor, 32768.0);
					}
					else
					{
						this._lCh = Math.Round((((this._lCh - this._mono) * this._wideCoeff + this._lCh) / 2.0 * this._gain + this._lCh * (1.0 - this._wetDry)) * 32768.0);
						this._rCh = Math.Round((((this._rCh - this._mono) * this._wideCoeff + this._rCh) / 2.0 * this._gain + this._rCh * (1.0 - this._wetDry)) * 32768.0);
					}
					if (this._lCh > 32767.0)
					{
						ptr2[j] = short.MaxValue;
					}
					else if (this._lCh < -32768.0)
					{
						ptr2[j] = short.MinValue;
					}
					else
					{
						ptr2[j] = (short)this._lCh;
					}
					if (this._rCh > 32767.0)
					{
						ptr2[j + 1] = short.MaxValue;
					}
					else if (this._rCh < -32768.0)
					{
						ptr2[j + 1] = short.MinValue;
					}
					else
					{
						ptr2[j + 1] = (short)this._rCh;
					}
				}
				return;
			}
			byte* ptr3 = (byte*)((void*)buffer);
			for (int k = 0; k < length; k += 2)
			{
				this._lCh = (double)(ptr3[k] - 128) / 128.0;
				this._rCh = (double)(ptr3[k + 1] - 128) / 128.0;
				this._mono = (this._lCh + this._rCh) / 2.0;
				if (this._useDithering)
				{
					this._lCh = Utils.SampleDither((((this._lCh - this._mono) * this._wideCoeff + this._lCh) / 2.0 * this._gain + this._lCh * (1.0 - this._wetDry)) * 128.0, this._ditherFactor, 128.0);
					this._rCh = Utils.SampleDither((((this._rCh - this._mono) * this._wideCoeff + this._rCh) / 2.0 * this._gain + this._rCh * (1.0 - this._wetDry)) * 128.0, this._ditherFactor, 128.0);
				}
				else
				{
					this._lCh = Math.Round((((this._lCh - this._mono) * this._wideCoeff + this._lCh) / 2.0 * this._gain + this._lCh * (1.0 - this._wetDry)) * 128.0);
					this._rCh = Math.Round((((this._rCh - this._mono) * this._wideCoeff + this._rCh) / 2.0 * this._gain + this._rCh * (1.0 - this._wetDry)) * 128.0);
				}
				if (this._lCh > 32767.0)
				{
					ptr3[k] = byte.MaxValue;
				}
				else if (this._lCh < -32768.0)
				{
					ptr3[k] = 0;
				}
				else
				{
					ptr3[k] = (byte)((int)this._lCh / 256 + 128);
				}
				if (this._rCh > 32767.0)
				{
					ptr3[k + 1] = byte.MaxValue;
				}
				else if (this._rCh < -32768.0)
				{
					ptr3[k + 1] = 0;
				}
				else
				{
					ptr3[k + 1] = (byte)((int)this._rCh / 256 + 128);
				}
			}
		}

		public override string ToString()
		{
			return "Stereo Enhancer DSP";
		}

		private double _wetDry = 0.5;

		private double _wideCoeff = 2.0;

		private double _gain = 1.0;

		private double _lCh;

		private double _rCh;

		private double _mono;

		private bool _useDithering;

		private double _ditherFactor = 0.7;
	}
}
