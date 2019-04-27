using System;
using System.Security;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BPMCounter
	{
		public BPMCounter(int timerfreq, int samplerate)
		{
			if (timerfreq < 0)
			{
				timerfreq = 50;
			}
			if (samplerate < 1)
			{
				samplerate = 44100;
			}
			this._historyCount = 1000 / timerfreq + 1;
			this.BEAT_RTIME = 1.0 / (double)timerfreq;
			this._beatRelease = 3.0 * this.BEAT_RTIME;
			this._minBPMms = 60000.0 / this._minBPM;
			this._historyBPM = new double[this._historyBPMSize];
			this._historyTapBPM = new double[this._historyBPMSize];
			this._startCounter = 0;
			this._LFfftBin1 = Utils.FFTFrequency2Index(this._LF, 512, samplerate);
			this._MFfftBin0 = Utils.FFTFrequency2Index(this._MF1, 512, samplerate);
			this._MFfftBin1 = Utils.FFTFrequency2Index(this._MF2, 512, samplerate);
			Array.Clear(this._peakEnv, 0, 128);
			Array.Clear(this._beatTrigger, 0, 128);
			Array.Clear(this._prevBeatPulse, 0, 128);
			Array.Clear(this._historyBPM, 0, this._historyBPMSize);
			Array.Clear(this._historyTapBPM, 0, this._historyBPMSize);
			this._beatTriggerC = false;
			this._beatTriggerD = false;
			this._beatTriggerE = false;
			this._prevBeatPulseC = false;
			this._lastBeatTime = DateTime.MinValue;
			this._lastTapBeatTime = DateTime.MinValue;
			this._nextBeatStart = DateTime.MinValue;
			this._nextBeatEnd = DateTime.MinValue;
			this._historyEnergy = new float[128][];
			for (int i = 0; i < 128; i++)
			{
				this._historyEnergy[i] = new float[this._historyCount];
				Array.Clear(this._historyEnergy[i], 0, this._historyCount);
			}
			this.autoresetCounter = 0;
			this.autoresetCounterTap = 0;
			this._errorBPMs = 0;
		}

		public int BPMHistorySize
		{
			get
			{
				return this._historyBPMSize;
			}
			set
			{
				if (this._historyBPMSize == value)
				{
					return;
				}
				if (value < 2)
				{
					return;
				}
				if (value > 50)
				{
					return;
				}
				this._historyBPMSize = value;
				this._historyBPM = new double[this._historyBPMSize];
				this._historyTapBPM = new double[this._historyBPMSize];
				this._startCounter = 0;
				Array.Clear(this._historyBPM, 0, this._historyBPMSize);
				Array.Clear(this._historyTapBPM, 0, this._historyBPMSize);
				this._beatTriggerC = false;
				this._beatTriggerD = false;
				this._beatTriggerE = false;
				this._prevBeatPulseC = false;
				this.autoresetCounter = 0;
				this._errorBPMs = 0;
			}
		}

		public double MinBPM
		{
			get
			{
				return this._minBPM;
			}
			set
			{
				if (value < 30.0 || value > this._maxBPM)
				{
					return;
				}
				this._minBPM = value;
			}
		}

		public double MaxBPM
		{
			get
			{
				return this._maxBPM;
			}
			set
			{
				if (value > 250.0 || value < this._minBPM)
				{
					return;
				}
				this._maxBPM = value;
			}
		}

		public double BPM
		{
			get
			{
				double num = 0.0;
				int num2 = 0;
				foreach (double num3 in this._historyBPM)
				{
					if (num3 > 0.0)
					{
						num += num3;
						num2++;
					}
				}
				return num / (double)num2;
			}
			set
			{
				this.autoresetCounter++;
				if (this.autoresetCounter > 2 * this._historyBPMSize)
				{
					this.autoresetCounter = 0;
					Array.Clear(this._historyBPM, 2, this._historyBPMSize - 2);
				}
				double num;
				for (num = value; num < this._minBPM; num *= 2.0)
				{
				}
				if (this._historyBPM[0] == 0.0)
				{
					while (num > this._maxBPM)
					{
						num /= 2.0;
					}
					this._historyBPM[0] = num;
					return;
				}
				if (num < 0.91 * this._historyBPM[0] || num > 1.11 * this._historyBPM[0])
				{
					this._errorBPMs++;
					if (this._errorBPMs == 3)
					{
						this._errorBPMs = 0;
						this.autoresetCounter = 0;
						Array.Clear(this._historyBPM, 0, this._historyBPMSize);
					}
					if (this.autoresetCounter > 0)
					{
						return;
					}
				}
				else
				{
					this._errorBPMs = 0;
				}
				while (num > this._maxBPM)
				{
					num /= 2.0;
				}
				this.ShiftHistoryBPM();
				this._historyBPM[0] = num;
			}
		}

		public double TappedBPM
		{
			get
			{
				double num = 0.0;
				int num2 = 0;
				foreach (double num3 in this._historyTapBPM)
				{
					if (num3 > 0.0)
					{
						num += num3;
						num2++;
					}
				}
				return num / (double)num2;
			}
			set
			{
				this.autoresetCounterTap++;
				if (this.autoresetCounterTap > 2 * this._historyBPMSize)
				{
					this.autoresetCounterTap = 0;
					Array.Clear(this._historyTapBPM, 2, this._historyBPMSize - 2);
				}
				double num;
				for (num = value; num < this._minBPM; num *= 2.0)
				{
				}
				if (this._historyTapBPM[0] == 0.0)
				{
					while (num > this._maxBPM)
					{
						num /= 2.0;
					}
					this._historyTapBPM[0] = num;
					return;
				}
				if ((num < 0.85 * this._historyTapBPM[0] || num > 1.15 * this._historyTapBPM[0]) && this.autoresetCounterTap > 0)
				{
					this._lastTapBeatTime = DateTime.MinValue;
					this._beatTriggerE = false;
					this.autoresetCounterTap = 0;
					Array.Clear(this._historyTapBPM, 0, this._historyBPMSize);
				}
				while (num > this._maxBPM)
				{
					num /= 2.0;
				}
				this.ShiftHistoryTapBPM();
				this._historyTapBPM[0] = num;
			}
		}

		private double BPMToMilliseconds(double bpm)
		{
			return 60000.0 / bpm;
		}

		private double MillisecondsToBPM(double ms)
		{
			return 60000.0 / ms;
		}

		private float[] HistoryEnergy(int subband)
		{
			return this._historyEnergy[subband];
		}

		private float AverageLocalEnergy(int subband)
		{
			float num = 0f;
			foreach (float num2 in this._historyEnergy[subband])
			{
				num += num2;
			}
			return num / (float)this._historyCount;
		}

		private void ShiftHistoryEnergy(int subband)
		{
			for (int i = this._historyCount - 2; i >= 0; i--)
			{
				this._historyEnergy[subband][i + 1] = this._historyEnergy[subband][i];
			}
		}

		private void ShiftHistoryBPM()
		{
			for (int i = this._historyBPMSize - 2; i >= 0; i--)
			{
				this._historyBPM[i + 1] = this._historyBPM[i];
			}
		}

		private void ShiftHistoryTapBPM()
		{
			for (int i = this._historyBPMSize - 2; i >= 0; i--)
			{
				this._historyTapBPM[i + 1] = this._historyTapBPM[i];
			}
		}

		private float VarianceLocalEnergy(int subband, float avgE)
		{
			float num = 0f;
			foreach (float num2 in this._historyEnergy[subband])
			{
				num += Math.Abs(num2 - avgE);
			}
			return num / (float)this._historyCount;
		}

		public void TapBeat()
		{
			DateTime now = DateTime.Now;
			if (this._beatTriggerE)
			{
				double totalMilliseconds = (now - this._lastTapBeatTime).TotalMilliseconds;
				this.TappedBPM = this.MillisecondsToBPM(totalMilliseconds);
			}
			this._lastTapBeatTime = now;
			this._beatTriggerE = true;
		}

		public void Reset(int samplerate)
		{
			this._lastBeatTime = DateTime.MinValue;
			this._lastTapBeatTime = DateTime.MinValue;
			this._prevPeakSubbandBeatHits = 0.0;
			this._startCounter = 0;
			this._errorBPMs = 0;
			this._minBPMms = 60000.0 / this._minBPM;
			if (samplerate > 0)
			{
				this._LFfftBin1 = Utils.FFTFrequency2Index(this._LF, 512, samplerate);
				this._MFfftBin0 = Utils.FFTFrequency2Index(this._MF1, 512, samplerate);
				this._MFfftBin1 = Utils.FFTFrequency2Index(this._MF2, 512, samplerate);
			}
			Array.Clear(this._peakEnv, 0, 128);
			Array.Clear(this._beatTrigger, 0, 128);
			Array.Clear(this._prevBeatPulse, 0, 128);
			Array.Clear(this._historyBPM, 0, this._historyBPMSize);
			Array.Clear(this._historyTapBPM, 0, this._historyBPMSize);
			this._beatTriggerC = false;
			this._beatTriggerD = false;
			this._beatTriggerE = false;
			this._prevBeatPulseC = false;
			this._nextBeatStart = DateTime.MinValue;
			this._nextBeatEnd = DateTime.MinValue;
			for (int i = 0; i < 128; i++)
			{
				Array.Clear(this._historyEnergy[i], 0, this._historyCount);
			}
			this.autoresetCounter = 0;
		}

		public void SetSamperate(int samplerate)
		{
			if (samplerate > 0)
			{
				this._LFfftBin1 = Utils.FFTFrequency2Index(this._LF, 512, samplerate);
				this._MFfftBin0 = Utils.FFTFrequency2Index(this._MF1, 512, samplerate);
				this._MFfftBin1 = Utils.FFTFrequency2Index(this._MF2, 512, samplerate);
			}
		}

		public bool ProcessAudio(int channel, bool bpmBeatsOnly)
		{
			if (channel == 0)
			{
				return false;
			}
			this.beat = false;
			this.bpmBeat = false;
			this.subbandBeatHits = 0.0;
			if (this._startCounter <= this._historyCount)
			{
				this._startCounter++;
			}
			try
			{
				if (Bass.BASS_ChannelGetData(channel, this._fft, -2147483647) > 0)
				{
					for (int i = 1; i < 128; i++)
					{
						this.Es = this._fft[i];
						if (this._startCounter > this._historyCount)
						{
							this.avgE = this.AverageLocalEnergy(i);
							this.varianceE = this.VarianceLocalEnergy(i, this.avgE);
							this.envIn = (double)this.Es / (double)this.avgE;
							if (this.avgE > 0f)
							{
								if (this.envIn > this._peakEnv[i])
								{
									this._peakEnv[i] = this.envIn;
								}
								else
								{
									this._peakEnv[i] *= this._beatRelease;
									this._peakEnv[i] += (1.0 - this._beatRelease) * this.envIn;
								}
								if (!this._beatTrigger[i])
								{
									if (i <= this._LFfftBin1)
									{
										if (this._peakEnv[i] > 1.7 * (double)((this.avgE + this.varianceE) / this.avgE))
										{
											this._beatTrigger[i] = true;
										}
									}
									else if (i >= this._MFfftBin0 && i <= this._MFfftBin1)
									{
										if (this._peakEnv[i] > 2.4 * (double)((this.avgE + this.varianceE) / this.avgE))
										{
											this._beatTrigger[i] = true;
										}
									}
									else if (this._peakEnv[i] > 2.8 * (double)((this.avgE + this.varianceE) / this.avgE))
									{
										this._beatTrigger[i] = true;
									}
								}
								else if (i <= this._LFfftBin1)
								{
									if (this._peakEnv[i] < 1.4 * (double)((this.avgE + this.varianceE) / this.avgE))
									{
										this._beatTrigger[i] = false;
									}
								}
								else if (i >= this._MFfftBin0 && i <= this._MFfftBin1)
								{
									if (this._peakEnv[i] < 1.1 * (double)((this.avgE + this.varianceE) / this.avgE))
									{
										this._beatTrigger[i] = false;
									}
								}
								else if (this._peakEnv[i] < 1.4 * (double)((this.avgE + this.varianceE) / this.avgE))
								{
									this._beatTrigger[i] = false;
								}
								if (this._beatTrigger[i] && !this._prevBeatPulse[i])
								{
									if (i <= this._LFfftBin1)
									{
										this.subbandBeatHits += 100.0 * (double)(this.avgE / this.varianceE);
									}
									else if (i >= this._MFfftBin0 && i <= this._MFfftBin1)
									{
										this.subbandBeatHits += 10.0 * (double)(this.avgE / this.varianceE);
									}
									else
									{
										this.subbandBeatHits += 2.0 * (double)(this.avgE / this.varianceE);
									}
								}
								this._prevBeatPulse[i] = this._beatTrigger[i];
							}
						}
						else
						{
							this._nextBeatStart = DateTime.Now;
							this._nextBeatEnd = this._nextBeatStart.AddMilliseconds(330.0);
						}
						this.ShiftHistoryEnergy(i);
						this._historyEnergy[i][0] = this.Es;
					}
					if (this.subbandBeatHits > this._peakSubbandBeatHits)
					{
						this._peakSubbandBeatHits = this.subbandBeatHits;
					}
					else
					{
						this._peakSubbandBeatHits = (this._peakSubbandBeatHits + this.subbandBeatHits) / 2.0;
					}
					if (!this._beatTriggerC)
					{
						if (this._peakSubbandBeatHits > 200.0)
						{
							this._beatTriggerC = true;
							this.beatTime = DateTime.Now;
						}
					}
					else if (this._peakSubbandBeatHits < 100.0)
					{
						this._beatTriggerC = false;
					}
					if (this._beatTriggerC && !this._prevBeatPulseC)
					{
						this.beat = true;
					}
					this._prevBeatPulseC = this._beatTriggerC;
					if (this.beat)
					{
						if (this._beatTriggerD)
						{
							this.deltaMS = Math.Round((this.beatTime - this._lastBeatTime).TotalMilliseconds);
							if (this.deltaMS < 333.0 && this._peakSubbandBeatHits < this._prevPeakSubbandBeatHits)
							{
								this.beat = false;
								this.beatTime = this._lastBeatTime;
							}
							else
							{
								this._prevPeakSubbandBeatHits = this._peakSubbandBeatHits;
								if (this.beatTime >= this._nextBeatStart && this.beatTime <= this._nextBeatEnd)
								{
									this.BPM = this.MillisecondsToBPM(this.deltaMS);
									this.bpmBeat = true;
									this._nextBeatStart = this.beatTime.AddMilliseconds((1.0 - this._beatRelease) * this.deltaMS);
									this._nextBeatEnd = this.beatTime.AddMilliseconds((1.0 + this._beatRelease) * this.deltaMS);
								}
								else if (this.beatTime < this._nextBeatStart)
								{
									this._beatTriggerD = false;
								}
							}
							if (this.beatTime > this._nextBeatEnd)
							{
								this._nextBeatStart = this.beatTime.AddMilliseconds((1.0 - this._beatRelease) * this.deltaMS);
								this._nextBeatEnd = this.beatTime.AddMilliseconds((1.0 + this._beatRelease) * this.deltaMS);
							}
						}
						this._lastBeatTime = this.beatTime;
						this._beatTriggerD = true;
					}
				}
			}
			catch
			{
			}
			if (bpmBeatsOnly)
			{
				return this.bpmBeat;
			}
			return this.beat;
		}

		private bool beat;

		private bool bpmBeat;

		private double deltaMS;

		private int _errorBPMs;

		private double _minBPM = 60.0;

		private double _maxBPM = 180.0;

		private DateTime _lastBeatTime = DateTime.MinValue;

		private DateTime _lastTapBeatTime = DateTime.MinValue;

		private double _minBPMms;

		private DateTime _nextBeatStart = DateTime.MinValue;

		private DateTime _nextBeatEnd = DateTime.MinValue;

		private int _startCounter;

		private int _LF = 350;

		private int _MF1 = 1500;

		private int _MF2 = 4000;

		private int _LFfftBin1 = 5;

		private int _MFfftBin0 = 11;

		private int _MFfftBin1 = 52;

		private int _historyCount;

		private float[][] _historyEnergy;

		private int _historyBPMSize = 10;

		private double[] _historyBPM;

		private double[] _historyTapBPM;

		private float[] _fft = new float[256];

		private double BEAT_RTIME = 0.019999999552965164;

		private double _beatRelease = 0.12;

		private double[] _peakEnv = new double[128];

		private bool[] _beatTrigger = new bool[128];

		private bool[] _prevBeatPulse = new bool[128];

		private bool _beatTriggerC;

		private bool _beatTriggerD;

		private bool _beatTriggerE;

		private bool _prevBeatPulseC;

		private double envIn;

		private float Es;

		private float avgE;

		private float varianceE;

		private double subbandBeatHits;

		private double _peakSubbandBeatHits;

		private double _prevPeakSubbandBeatHits;

		private DateTime beatTime;

		private int autoresetCounter;

		private int autoresetCounterTap;
	}
}
