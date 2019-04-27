using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Security;
using System.Windows.Forms;
using Un4seen.Bass.AddOn.Mix;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class Visuals
	{
		public bool ChannelIsMixerSource
		{
			get
			{
				return this._channelIsMixerSource;
			}
			set
			{
				this._channelIsMixerSource = value;
			}
		}

		public int ScaleFactorLinear
		{
			get
			{
				return this._scaleFactorLinear;
			}
			set
			{
				this._scaleFactorLinear = value;
			}
		}

		public float ScaleFactorLinearBoost
		{
			get
			{
				return this._scaleFactorLinearBoost;
			}
			set
			{
				this._scaleFactorLinearBoost = value;
			}
		}

		public int ScaleFactorSqr
		{
			get
			{
				return this._scaleFactorSqr;
			}
			set
			{
				this._scaleFactorSqr = value;
			}
		}

		public float ScaleFactorSqrBoost
		{
			get
			{
				return this._scaleFactorSqrBoost;
			}
			set
			{
				this._scaleFactorSqrBoost = value;
			}
		}

		public int MaxFFTData
		{
			get
			{
				return this._maxFFTData;
			}
		}

		public int MaxFFTSampleIndex
		{
			get
			{
				return this._maxFFTSampleIndex;
			}
		}

		public BASSData MaxFFT
		{
			get
			{
				return this._maxFFT;
			}
			set
			{
				switch (value)
				{
				case BASSData.BASS_DATA_FFT512:
					this._maxFFTData = 1024;
					this._maxFFT = value;
					this._maxFFTSampleIndex = 255;
					break;
				case BASSData.BASS_DATA_FFT1024:
					this._maxFFTData = 1024;
					this._maxFFT = value;
					this._maxFFTSampleIndex = 511;
					break;
				case BASSData.BASS_DATA_FFT2048:
					this._maxFFTData = 2048;
					this._maxFFT = value;
					this._maxFFTSampleIndex = 1023;
					break;
				case BASSData.BASS_DATA_FFT4096:
					this._maxFFTData = 4096;
					this._maxFFT = value;
					this._maxFFTSampleIndex = 2047;
					break;
				case BASSData.BASS_DATA_FFT8192:
					this._maxFFTData = 8192;
					this._maxFFT = value;
					this._maxFFTSampleIndex = 4095;
					break;
				default:
					this._maxFFTData = 4096;
					this._maxFFT = BASSData.BASS_DATA_FFT4096;
					this._maxFFTSampleIndex = 2047;
					break;
				}
				if (this._maxFrequencySpectrum > this._maxFFTSampleIndex)
				{
					this._maxFrequencySpectrum = this._maxFFTSampleIndex;
				}
			}
		}

		public int MaxFrequencySpectrum
		{
			get
			{
				return this._maxFrequencySpectrum;
			}
			set
			{
				if (value > this.MaxFFTSampleIndex)
				{
					this._maxFrequencySpectrum = this.MaxFFTSampleIndex;
				}
				if (value < 1)
				{
					this._maxFrequencySpectrum = 1;
					return;
				}
				this._maxFrequencySpectrum = value;
			}
		}

		public int GetFrequencyFromPosX(int x, int samplerate)
		{
			int num = (int)Math.Round((double)(x + 1) * this.spp);
			if (num > this.MaxFFTSampleIndex)
			{
				num = this.MaxFFTSampleIndex;
			}
			if (num < 1)
			{
				num = 1;
			}
			return Utils.FFTIndex2Frequency(num, this.MaxFFTData, samplerate);
		}

		public float GetAmplitudeFromPosY(int y, int height)
		{
			y = height - y;
			float result;
			if (this.ylinear)
			{
				result = (float)y / ((float)this.ScaleFactorLinear * (float)height);
			}
			else
			{
				result = (float)Math.Pow((double)((float)y / ((float)this.ScaleFactorSqr * (float)height)), 2.0);
			}
			return result;
		}

		public Bitmap CreateSpectrum(int channel, int width, int height, Color color1, Color color2, Color background, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, 1, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.AssumeLinear;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrum(graphics, width, height, pen, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrum(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (g == null || channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1)
			{
				return false;
			}
			Pen pen = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, 1, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrum(g, clipRectangle.Width, clipRectangle.Height, pen, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateSpectrumLine(int channel, int width, int height, Color color1, Color color2, Color background, int linewidth, int distance, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || linewidth < 1 || distance < 0)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.AssumeLinear;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumLine(graphics, width, height, pen, linewidth, distance, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumLine(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, int linewidth, int distance, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (g == null || channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || linewidth < 1 || distance < 0)
			{
				return false;
			}
			Pen pen = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumLine(g, clipRectangle.Width, clipRectangle.Height, pen, linewidth, distance, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateSpectrumEllipse(int channel, int width, int height, Color color1, Color color2, Color background, int linewidth, int distance, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || linewidth < 1 || distance < 0)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.GammaCorrected;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumEllipse(graphics, width, height, pen, 2 * distance, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumEllipse(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, int linewidth, int distance, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (g == null || channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || linewidth < 1 || distance < 0)
			{
				return false;
			}
			Pen pen = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.GammaCorrected;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumEllipse(g, clipRectangle.Width, clipRectangle.Height, pen, 2 * distance, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateSpectrumDot(int channel, int width, int height, Color color1, Color color2, Color background, int dotwidth, int distance, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || dotwidth < 1 || distance < 0)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, dotwidth, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)dotwidth);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.AssumeLinear;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumDot(graphics, width, height, pen, dotwidth, distance, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumDot(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, int dotwidth, int distance, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || dotwidth < 1 || distance < 0)
			{
				return false;
			}
			Pen pen = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, dotwidth, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)dotwidth);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumDot(g, clipRectangle.Width, clipRectangle.Height, pen, dotwidth, distance, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateSpectrumLinePeak(int channel, int width, int height, Color color1, Color color2, Color color3, Color background, int linewidth, int peakwidth, int distance, int peakdelay, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || linewidth < 1 || distance < 0 || peakdelay < 0)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			Pen pen2 = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						pen2 = new Pen(color3, (float)peakwidth);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.AssumeLinear;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumLinePeak(graphics, width, height, pen, pen2, linewidth, distance, peakdelay, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumLinePeak(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color color3, Color background, int linewidth, int peakwidth, int distance, int peakdelay, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (g == null || channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || linewidth < 1 || distance < 0 || peakdelay < 0)
			{
				return false;
			}
			Pen pen = null;
			Pen pen2 = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						pen2 = new Pen(color3, (float)peakwidth);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumLinePeak(g, clipRectangle.Width, clipRectangle.Height, pen, pen2, linewidth, distance, peakdelay, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
			}
			return result;
		}

		public void ClearPeaks()
		{
			for (int i = 0; i < this._lastPeak.Length; i++)
			{
				this._lastPeak[i] = 0f;
			}
		}

		public Bitmap CreateSpectrumWave(int channel, int width, int height, Color color1, Color color2, Color background, int linewidth, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || linewidth < 1)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.AssumeLinear;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumWave(graphics, width, height, pen, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumWave(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, int linewidth, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || linewidth < 1)
			{
				return false;
			}
			Pen pen = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, linewidth, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, (float)linewidth);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumWave(g, clipRectangle.Width, clipRectangle.Height, pen, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateWaveForm(int channel, int width, int height, Color color1, Color color2, Color color3, Color background, int linewidth, bool fullSpectrum, bool mono, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || linewidth < 1)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			Pen pen2 = null;
			Pen pen3 = null;
			try
			{
				if (this.GetFFTData(channel, this.MaxFFTData * 2 + 1073741824) > 0)
				{
					pen = new Pen(color1, (float)linewidth);
					pen2 = new Pen(color2, (float)linewidth);
					pen3 = new Pen(color3, 1f);
					bitmap = new Bitmap(width, height);
					graphics = Graphics.FromImage(bitmap);
					if (highQuality)
					{
						graphics.SmoothingMode = SmoothingMode.AntiAlias;
						graphics.CompositingQuality = CompositingQuality.AssumeLinear;
						graphics.PixelOffsetMode = PixelOffsetMode.Default;
						graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					}
					else
					{
						graphics.SmoothingMode = SmoothingMode.HighSpeed;
						graphics.CompositingQuality = CompositingQuality.HighSpeed;
						graphics.PixelOffsetMode = PixelOffsetMode.None;
						graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
					}
					this.DrawWaveForm(graphics, width, height, pen, pen2, pen3, background, fullSpectrum, mono);
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
				if (pen3 != null)
				{
					pen3.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateWaveForm(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color color3, Color background, int linewidth, bool fullSpectrum, bool mono, bool highQuality)
		{
			if (channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || linewidth < 1)
			{
				return false;
			}
			Pen pen = null;
			Pen pen2 = null;
			Pen pen3 = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, this.MaxFFTData * 2 + 1073741824) > 0)
				{
					pen = new Pen(color1, (float)linewidth);
					pen2 = new Pen(color2, (float)linewidth);
					pen3 = new Pen(color3, 1f);
					if (highQuality)
					{
						g.SmoothingMode = SmoothingMode.AntiAlias;
						g.CompositingQuality = CompositingQuality.AssumeLinear;
						g.PixelOffsetMode = PixelOffsetMode.Default;
						g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					}
					else
					{
						g.SmoothingMode = SmoothingMode.HighSpeed;
						g.CompositingQuality = CompositingQuality.HighSpeed;
						g.PixelOffsetMode = PixelOffsetMode.None;
						g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
					}
					this.DrawWaveForm(g, clipRectangle.Width, clipRectangle.Height, pen, pen2, pen3, background, fullSpectrum, mono);
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
				if (pen3 != null)
				{
					pen3.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateSpectrumBean(int channel, int width, int height, Color color1, Color color2, Color background, int beanwidth, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || beanwidth < 1)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			Pen pen = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, beanwidth, height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, 2f);
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.GammaCorrected;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumBean(graphics, width, height, pen, beanwidth, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumBean(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, int beanwidth, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || beanwidth < 1)
			{
				return false;
			}
			Pen pen = null;
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, beanwidth, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						pen = new Pen(brush, 2f);
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.GammaCorrected;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumBean(g, clipRectangle.Width, clipRectangle.Height, pen, beanwidth, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
			}
			return result;
		}

		public Bitmap CreateSpectrumText(int channel, int width, int height, Color color1, Color color2, Color background, string text, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || width <= 1 || height <= 1 || text == null)
			{
				return null;
			}
			Bitmap bitmap = null;
			Graphics graphics = null;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, width, height), color2, color1, LinearGradientMode.Vertical))
					{
						bitmap = new Bitmap(width, height);
						graphics = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics.SmoothingMode = SmoothingMode.AntiAlias;
							graphics.CompositingQuality = CompositingQuality.GammaCorrected;
							graphics.PixelOffsetMode = PixelOffsetMode.Default;
							graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics.SmoothingMode = SmoothingMode.HighSpeed;
							graphics.CompositingQuality = CompositingQuality.HighSpeed;
							graphics.PixelOffsetMode = PixelOffsetMode.None;
							graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumText(graphics, width, height, brush, text, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateSpectrumText(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, Color background, string text, bool linear, bool fullSpectrum, bool highQuality)
		{
			if (channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || text == null)
			{
				return false;
			}
			bool result = true;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					using (Brush brush = new LinearGradientBrush(new Rectangle(0, 0, clipRectangle.Height, clipRectangle.Height), color2, color1, LinearGradientMode.Vertical))
					{
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.GammaCorrected;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.HighSpeed;
							g.CompositingQuality = CompositingQuality.HighSpeed;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawSpectrumText(g, clipRectangle.Width, clipRectangle.Height, brush, text, background, linear, fullSpectrum);
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public float DetectFrequency(int channel, int freq1, int freq2, bool linear)
		{
			if (freq1 < 1 || freq2 < 1 || freq1 > freq2 || channel == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 1;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
					if (Bass.BASS_ChannelGetInfo(channel, bass_CHANNELINFO))
					{
						int num3 = Utils.FFTFrequency2Index(freq1, this.MaxFFTData, bass_CHANNELINFO.freq);
						int num4 = Utils.FFTFrequency2Index(freq2, this.MaxFFTData, bass_CHANNELINFO.freq);
						num2 = num4 - num3 + 1;
						for (int i = num3; i <= num4; i++)
						{
							if (linear)
							{
								num += this._fft[i] * (float)this.ScaleFactorLinear;
							}
							else
							{
								num += (float)Math.Sqrt((double)this._fft[i]) * (float)this.ScaleFactorSqr;
							}
						}
					}
				}
			}
			catch
			{
				return 0f;
			}
			return num / (float)num2;
		}

		public int DetectPeakFrequency(int channel, out float energy)
		{
			energy = 0f;
			if (channel == 0)
			{
				return 0;
			}
			int result = 0;
			int index = 0;
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
					if (Bass.BASS_ChannelGetInfo(channel, bass_CHANNELINFO))
					{
						for (int i = 1; i <= this.MaxFFTSampleIndex; i++)
						{
							if (this._fft[i] > energy)
							{
								energy = this._fft[i];
								index = i;
							}
						}
						result = Utils.FFTIndex2Frequency(index, this.MaxFFTData, bass_CHANNELINFO.freq);
					}
				}
			}
			catch
			{
				return 0;
			}
			return result;
		}

		public bool CreateSpectrum3DVoicePrint(int channel, Graphics g, Rectangle clipRectangle, Color color1, Color color2, int pos, bool linear, bool fullSpectrum)
		{
			if (channel == 0 || clipRectangle.Width <= 1 || clipRectangle.Height <= 1)
			{
				return false;
			}
			if (pos > clipRectangle.Width - 1)
			{
				pos = 0;
			}
			bool result = true;
			Pen pen = new Pen(color1, 1f);
			Pen pen2 = new Pen(color2, 1f);
			try
			{
				if (this.GetFFTData(channel, (int)this.MaxFFT) > 0)
				{
					float num = 0f;
					int num2 = 0;
					double num3 = (double)this.MaxFrequencySpectrum / (double)clipRectangle.Height;
					int num4 = this.MaxFrequencySpectrum + 1;
					if (!fullSpectrum)
					{
						num3 = 1.0;
						num4 = clipRectangle.Height + 1;
						if (num4 > this.MaxFFTSampleIndex + 1)
						{
							num4 = this.MaxFFTSampleIndex + 1;
						}
					}
					for (int i = 1; i < num4; i++)
					{
						if (linear)
						{
							int num5 = (int)(this._fft[i] * (float)this.ScaleFactorLinear * 65535f);
							if (num2 < num5)
							{
								num2 = num5;
							}
							if (num2 < 0)
							{
								num2 = 0;
							}
						}
						else
						{
							int num5 = (int)(Math.Sqrt((double)this._fft[i]) * (double)this.ScaleFactorSqr * 65535.0);
							if (num2 < num5)
							{
								num2 = num5;
							}
							if (num2 < 0)
							{
								num2 = 0;
							}
						}
						pen.Color = Color.FromArgb((int)color1.R + ((num2 & 16777215) >> 16) & 255, (num2 & 65535) >> 8, num2 & 255);
						float num6 = (float)Math.Round((double)i / num3) - 1f;
						if (num6 > num)
						{
							g.DrawLine(pen, (float)pos, num, (float)pos, num + 1f);
							num = num6;
							num2 = 0;
						}
					}
					if (pos < clipRectangle.Width - 1)
					{
						g.DrawLine(pen2, (float)pos + 1f, 0f, (float)pos + 1f, (float)clipRectangle.Height);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
			}
			return result;
		}

		private int GetFFTData(int channel, int length)
		{
			if (this._channelIsMixerSource)
			{
				return BassMix.BASS_Mixer_ChannelGetData(channel, this._fft, length);
			}
			return Bass.BASS_ChannelGetData(channel, this._fft, length);
		}

		private void DrawSpectrum(Graphics g, int width, int height, Pen p, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num3 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num3 = width + 1;
				if (num3 > this.MaxFFTSampleIndex + 1)
				{
					num3 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num4 = 0;
			float num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num3; i++)
			{
				float num6;
				if (linear)
				{
					num6 = this._fft[i] * (float)this.ScaleFactorLinear * num5 * (float)height;
				}
				else
				{
					num6 = (float)Math.Sqrt((double)(this._fft[i] * num5)) * (float)this.ScaleFactorSqr * (float)height;
				}
				num2 += num6;
				float num7 = (float)Math.Round((double)i / this.spp) - 1f;
				if (num7 > num)
				{
					num2 /= num7 - num;
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawLine(p, num, (float)height - 1f, num, (float)height - 1f - num2);
					num = num7;
					num2 = 0f;
					num4++;
					num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawSpectrumLine(Graphics g, int width, int height, Pen p, int linewidth, int distance, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num3 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num3 = width + 1;
				if (num3 > this.MaxFFTSampleIndex + 1)
				{
					num3 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num4 = 0;
			float num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num3; i++)
			{
				float num6;
				if (linear)
				{
					num6 = this._fft[i] * (float)this.ScaleFactorLinear * num5 * (float)height;
				}
				else
				{
					num6 = (float)Math.Sqrt((double)(this._fft[i] * num5)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num6;
				float num7 = (float)Math.Round((double)i / this.spp) - 1f;
				if ((int)num7 % (distance + linewidth) == 0 && num7 > num)
				{
					num2 /= (float)(distance + linewidth);
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawLine(p, num + (float)(linewidth / 2) + 1f, (float)height - 1f, num + (float)(linewidth / 2) + 1f, (float)height - 1f - num2);
					num = num7;
					num2 = 0f;
					num4++;
					num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawSpectrumEllipse(Graphics g, int width, int height, Pen p, int distance, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num3 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num3 = width + 1;
				if (num3 > this.MaxFFTSampleIndex + 1)
				{
					num3 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num4 = 0;
			float num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num3; i++)
			{
				float num6;
				if (linear)
				{
					num6 = this._fft[i] * (float)this.ScaleFactorLinear * num5 * (float)height;
				}
				else
				{
					num6 = (float)Math.Sqrt((double)(this._fft[i] * num5)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num6;
				float num7 = (float)Math.Round((double)i / this.spp) - 1f;
				if ((int)num7 % distance == 0 && num7 > num)
				{
					num2 /= (float)distance;
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawEllipse(p, num, (float)height - 1f - num2, num + (float)distance, (float)height - 1f);
					num = num7;
					num2 = 0f;
					num4++;
					num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawSpectrumDot(Graphics g, int width, int height, Pen p, int dotwidth, int distance, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num3 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num3 = width + 1;
				if (num3 > this.MaxFFTSampleIndex + 1)
				{
					num3 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num4 = 0;
			float num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num3; i++)
			{
				float num6;
				if (linear)
				{
					num6 = this._fft[i] * (float)this.ScaleFactorLinear * num5 * (float)height;
				}
				else
				{
					num6 = (float)Math.Sqrt((double)(this._fft[i] * num5)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num6;
				float num7 = (float)Math.Round((double)i / this.spp) - 1f;
				if ((int)num7 % (distance + dotwidth) == 0 && num7 > num)
				{
					num2 /= (float)(distance + dotwidth);
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawLine(p, num + (float)distance, (float)height - (float)dotwidth - num2, num + (float)distance, (float)height - num2);
					num = num7;
					num2 = 0f;
					num4++;
					num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawSpectrumLinePeak(Graphics g, int width, int height, Pen p1, Pen p2, int linewidth, int distance, int peakdelay, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num3 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num3 = width + 1;
				if (num3 > this.MaxFFTSampleIndex + 1)
				{
					num3 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num4 = 0;
			float num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num3; i++)
			{
				float num6;
				if (linear)
				{
					num6 = this._fft[i] * (float)this.ScaleFactorLinear * num5 * (float)height;
				}
				else
				{
					num6 = (float)Math.Sqrt((double)(this._fft[i] * num5)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num6;
				float num7 = (float)Math.Round((double)i / this.spp) - 1f;
				if ((int)num7 % (distance + linewidth) == 0 && num7 > num)
				{
					num2 /= (float)(distance + linewidth);
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					float num8 = num2;
					if (this._lastPeak[(int)num7] < num8)
					{
						this._lastPeak[(int)num7] = num8;
					}
					else
					{
						this._lastPeak[(int)num7] = (num8 + (float)peakdelay * this._lastPeak[(int)num7]) / (float)(peakdelay + 1);
					}
					g.DrawLine(p1, num + (float)(linewidth / 2) + 1f, (float)height - 1f, num + (float)(linewidth / 2) + 1f, (float)height - 1f - num2);
					float num9 = (float)((p2.Width > 1f) ? 0 : -1);
					g.DrawLine(p2, num + 1f, (float)height - p2.Width / 2f - this._lastPeak[(int)num7], num + (float)linewidth + num9 + 1f, (float)height - p2.Width / 2f - this._lastPeak[(int)num7]);
					num = num7;
					num2 = 0f;
					num4++;
					num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawSpectrumWave(Graphics g, int width, int height, Pen p, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num4 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num4 = width + 1;
				if (num4 > this.MaxFFTSampleIndex + 1)
				{
					num4 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num5 = 0;
			float num6 = 1f + (float)num5 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num4; i++)
			{
				float num7;
				if (linear)
				{
					num7 = this._fft[i] * (float)this.ScaleFactorLinear * num6 * (float)height;
				}
				else
				{
					num7 = (float)Math.Sqrt((double)(this._fft[i] * num6)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num7;
				float num8 = (float)Math.Round((double)i / this.spp) - 1f;
				if (num8 > num)
				{
					num2 /= num8 - num;
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawLine(p, num8, (float)height - num2, num, (float)height - num3);
					num3 = num2;
					num = num8;
					num2 = 0f;
					num5++;
					num6 = 1f + (float)num5 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawWaveForm(Graphics g, int width, int height, Pen pL, Pen pR, Pen pM, Color background, bool fullSpectrum, bool mono)
		{
			g.Clear(background);
			this.ylinear = true;
			float num = (float)height / 2f - 1f;
			float num2 = -1f;
			float num3 = -1f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			g.DrawLine(pM, 0f, num, (float)width, num);
			this.spp = (double)(this.MaxFrequencySpectrum + 1) / (double)width;
			int num8 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num8 = width + 1;
				if (num8 > this.MaxFFTSampleIndex + 1)
				{
					num8 = this.MaxFFTSampleIndex + 1;
				}
			}
			for (int i = 0; i < num8; i++)
			{
				if (mono)
				{
					float num9 = this._fft[i] * num;
					float num10 = this._fft[i + 1] * num;
					num9 = (num9 + num10) / 2f;
					if (Math.Abs(num6) < Math.Abs(num9))
					{
						num6 = num9;
					}
					float num11 = (float)Math.Round((double)i / this.spp) - 1f;
					if (num2 == -1f)
					{
						num7 = num9;
					}
					if (num11 > num2)
					{
						g.DrawLine(pL, num11, num - num6, num2, num - num7);
						num7 = num6;
						num2 = num11;
						num6 = 0f;
					}
					i++;
				}
				else if (i % 2 == 0)
				{
					float num9 = this._fft[i] * num;
					if (Math.Abs(num6) < Math.Abs(num9))
					{
						num6 = num9;
					}
					float num11 = (float)Math.Round((double)i / this.spp) - 1f;
					if (num2 == -1f)
					{
						num7 = num9;
					}
					if (num11 > num2)
					{
						g.DrawLine(pL, num11, num - num6, num2, num - num7);
						num7 = num6;
						num2 = num11;
						num6 = 0f;
					}
				}
				else
				{
					float num10 = this._fft[i] * num;
					if (Math.Abs(num4) < Math.Abs(num10))
					{
						num4 = num10;
					}
					float num12 = (float)Math.Round((double)(i - 1) / this.spp) - 1f;
					if (num3 == -1f)
					{
						num5 = num10;
					}
					if (num12 > num3)
					{
						g.DrawLine(pR, num12, num - num4, num3, num - num5);
						num5 = num4;
						num3 = num12;
						num4 = 0f;
					}
				}
			}
		}

		private void DrawSpectrumBean(Graphics g, int width, int height, Pen p, int beanwidth, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num3 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num3 = width + 1;
				if (num3 > this.MaxFFTSampleIndex + 1)
				{
					num3 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num4 = 0;
			float num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num3; i++)
			{
				float num6;
				if (linear)
				{
					num6 = this._fft[i] * (float)this.ScaleFactorLinear * num5 * (float)height;
				}
				else
				{
					num6 = (float)Math.Sqrt((double)(this._fft[i] * num5)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num6;
				float num7 = (float)Math.Round((double)i / this.spp) - 1f;
				if ((int)num7 % (beanwidth + 1) == 0 && num7 > num)
				{
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawEllipse(p, num + 1f, (float)height - num2, (float)beanwidth, (float)(2 * beanwidth));
					num = num7;
					num2 = 0f;
					num4++;
					num5 = 1f + (float)num4 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private void DrawSpectrumText(Graphics g, int width, int height, Brush b, string text, Color background, bool linear, bool fullSpectrum)
		{
			g.Clear(background);
			this.ylinear = linear;
			float num = 0f;
			float num2 = 0f;
			int num3 = 0;
			this.spp = (double)this.MaxFrequencySpectrum / (double)width;
			int num4 = this.MaxFrequencySpectrum + 1;
			if (!fullSpectrum)
			{
				this.spp = 1.0;
				num4 = width + 1;
				if (num4 > this.MaxFFTSampleIndex + 1)
				{
					num4 = this.MaxFFTSampleIndex + 1;
				}
			}
			int num5 = 0;
			float num6 = 1f + (float)num5 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
			for (int i = 1; i < num4; i++)
			{
				float num7;
				if (linear)
				{
					num7 = this._fft[i] * (float)this.ScaleFactorLinear * num6 * (float)height;
				}
				else
				{
					num7 = (float)Math.Sqrt((double)(this._fft[i] * num6)) * (float)this.ScaleFactorSqr * (float)height - 4f;
				}
				num2 += num7;
				float num8 = (float)Math.Round((double)i / this.spp) - 1f;
				if ((int)num8 % 6 == 0 && num8 > num)
				{
					num2 /= 6f;
					if (num2 > (float)height)
					{
						num2 = (float)height;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					g.DrawString(text[num3].ToString(), SystemInformation.MenuFont, b, num + 1f, (float)height - 15f - num2, StringFormat.GenericDefault);
					num = num8;
					num2 = 0f;
					num3++;
					if (num3 >= text.Length)
					{
						num3 = 0;
					}
					num5++;
					num6 = 1f + (float)num5 * (linear ? this.ScaleFactorLinearBoost : this.ScaleFactorSqrBoost);
				}
			}
		}

		private float[] _fft = new float[2048];

		private float[] _lastPeak = new float[2048];

		private bool _channelIsMixerSource;

		private int _scaleFactorLinear = 9;

		private float _scaleFactorLinearBoost = 0.05f;

		private int _scaleFactorSqr = 4;

		private float _scaleFactorSqrBoost = 0.005f;

		private int _maxFFTData = 4096;

		private int _maxFFTSampleIndex = 2047;

		private BASSData _maxFFT = BASSData.BASS_DATA_FFT4096;

		private int _maxFrequencySpectrum = 2047;

		private double spp = 1.0;

		private bool ylinear;
	}
}
