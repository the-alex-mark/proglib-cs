using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class DSP_Mono : BaseDSP
	{
		public DSP_Mono()
		{
		}

		public DSP_Mono(int channel, int priority) : base(channel, priority, IntPtr.Zero)
		{
		}

		public bool Invert
		{
			get
			{
				return this._invert;
			}
			set
			{
				this._invert = value;
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
					if (this._invert)
					{
						ptr[i] = (float)((double)(0f - ptr[i] + ptr[i + 1]) / 2.0);
					}
					else
					{
						ptr[i] = (float)((double)(ptr[i] + ptr[i + 1]) / 2.0);
					}
					ptr[i + 1] = ptr[i];
				}
				return;
			}
			if (base.ChannelBitwidth == 16)
			{
				short* ptr2 = (short*)((void*)buffer);
				for (int j = 0; j < length / 2; j += 2)
				{
					if (this._useDithering)
					{
						if (this._invert)
						{
							this._mono = Utils.SampleDither((0.0 - (double)ptr2[j] + (double)ptr2[j + 1]) / 2.0, this._ditherFactor, 32768.0);
						}
						else
						{
							this._mono = Utils.SampleDither(((double)ptr2[j] + (double)ptr2[j + 1]) / 2.0, this._ditherFactor, 32768.0);
						}
					}
					else if (this._invert)
					{
						this._mono = Math.Round((double)(0 - ptr2[j] + ptr2[j + 1]) / 2.0);
					}
					else
					{
						this._mono = Math.Round((double)(ptr2[j] + ptr2[j + 1]) / 2.0);
					}
					if (this._mono > 32767.0)
					{
						ptr2[j] = short.MaxValue;
					}
					else if (this._mono < -32768.0)
					{
						ptr2[j] = short.MinValue;
					}
					else
					{
						ptr2[j] = (short)this._mono;
					}
					ptr2[j + 1] = ptr2[j];
				}
				return;
			}
			byte* ptr3 = (byte*)((void*)buffer);
			for (int k = 0; k < length; k += 2)
			{
				if (this._useDithering)
				{
					if (this._invert)
					{
						this._mono = Utils.SampleDither((0.0 - (double)(ptr3[k] - 128) + (double)(ptr3[k + 1] - 128)) / 2.0, this._ditherFactor, 128.0);
					}
					else
					{
						this._mono = Utils.SampleDither(((double)(ptr3[k] - 128) + (double)(ptr3[k + 1] - 128)) / 2.0, this._ditherFactor, 128.0);
					}
				}
				else
				{
					this._mono = Math.Round(((double)(ptr3[k] - 128) + (double)(ptr3[k + 1] - 128)) / 2.0);
				}
				if (this._mono > 127.0)
				{
					ptr3[k] = byte.MaxValue;
				}
				else if (this._mono < -128.0)
				{
					ptr3[k] = 0;
				}
				else
				{
					ptr3[k] = (byte)((int)this._mono + 128);
				}
				ptr3[k + 1] = ptr3[k];
			}
		}

		public override string ToString()
		{
			return "Stereo to Mono DSP";
		}

		private double _mono;

		private bool _invert;

		private bool _useDithering;

		private double _ditherFactor = 0.7;
	}
}
