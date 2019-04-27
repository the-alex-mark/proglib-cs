using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass.AddOn.Fx;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class WaveForm
	{
		public WaveForm()
		{
			this.FileName = string.Empty;
		}

		public WaveForm(string fileName)
		{
			this.FileName = fileName;
		}

		public WaveForm(string fileName, WAVEFORMPROC proc, Control win)
		{
			this.FileName = fileName;
			this.NotifyHandler = proc;
			this.WinControl = win;
		}

		private WaveForm(WaveForm clone, bool flat)
		{
			object syncRoot = clone._syncRoot;
			lock (syncRoot)
			{
				this._fileName = clone._fileName;
				this._notifyHandler = clone._notifyHandler;
				this._win = clone._win;
				this._frameResolution = clone._frameResolution;
				this._callbackFrequency = clone._callbackFrequency;
				this._isRendered = clone._isRendered;
				this._renderingInProgress = clone._renderingInProgress;
				this._framesToRender = clone._framesToRender;
				this._framesRendered = clone._framesRendered;
				this._detectBeats = clone._detectBeats;
				this._decodingStream = clone._decodingStream;
				this._renderStartTime = clone._renderStartTime;
				this._recordingNextLength = clone._recordingNextLength;
				this._syncFactor = clone._syncFactor;
				this._channelFactor = clone._channelFactor;
				this._correctionFactor = clone._correctionFactor;
				this._gainFactor = clone._gainFactor;
				if (clone._volumePoints != null)
				{
					this._volumePoints = new List<WaveForm.VolumePoint>(clone._volumePoints.Count);
					foreach (WaveForm.VolumePoint volumePoint in clone._volumePoints)
					{
						this._volumePoints.Add(new WaveForm.VolumePoint(volumePoint.Position, volumePoint.Level));
					}
				}
				if (clone._waveBuffer != null)
				{
					this._waveBuffer = new WaveForm.WaveBuffer();
					this._waveBuffer.bpf = clone._waveBuffer.bpf;
					this._waveBuffer.chans = clone._waveBuffer.chans;
					this._waveBuffer.fileName = clone._waveBuffer.fileName;
					this._waveBuffer.flags = clone._waveBuffer.flags;
					this._waveBuffer.resolution = clone._waveBuffer.resolution;
					if (clone._waveBuffer.marker != null)
					{
						this._waveBuffer.marker = new Dictionary<string, long>(clone._waveBuffer.marker.Count);
						foreach (KeyValuePair<string, long> keyValuePair in clone._waveBuffer.marker)
						{
							this._waveBuffer.marker.Add(keyValuePair.Key, keyValuePair.Value);
						}
					}
					if (flat)
					{
						this._waveBuffer.beats = clone._waveBuffer.beats;
						this._waveBuffer.data = clone._waveBuffer.data;
					}
					else
					{
						if (clone._waveBuffer.beats != null)
						{
							this._waveBuffer.beats = new List<long>(clone._waveBuffer.beats.Count);
							this._waveBuffer.beats.AddRange(clone._waveBuffer.beats);
						}
						if (clone._waveBuffer.data != null)
						{
							this._waveBuffer.data = new WaveForm.WaveBuffer.Level[clone._waveBuffer.data.Length];
							for (int i = 0; i < clone._waveBuffer.data.Length; i++)
							{
								WaveForm.WaveBuffer.Level level = clone._waveBuffer.data[i];
								this._waveBuffer.data[i] = new WaveForm.WaveBuffer.Level(level.left, level.right);
							}
						}
					}
				}
				this._colorBackground = clone._colorBackground;
				this._colorLeft = clone._colorLeft;
				this._colorLeft2 = clone._colorLeft2;
				this._colorLeftEnvelope = clone._colorLeftEnvelope;
				this._drawEnvelope = clone._drawEnvelope;
				this._drawCenterLine = clone._drawCenterLine;
				this._colorMiddleLeft = clone._colorMiddleLeft;
				this._colorMiddleRight = clone._colorMiddleRight;
				this._drawGradient = clone._drawGradient;
				this._colorRight = clone._colorRight;
				this._colorRight2 = clone._colorRight2;
				this._colorRightEnvelope = clone._colorRightEnvelope;
				this._colorVolume = clone._colorVolume;
				this._pixelFormat = clone._pixelFormat;
				this._drawVolume = clone._drawVolume;
				this._volumeCurveZeroLevel = clone._volumeCurveZeroLevel;
				this._colorBeat = clone._colorBeat;
				this._drawBeat = clone._drawBeat;
				this._beatLength = clone._beatLength;
				this._beatWidth = clone._beatWidth;
				this._colorMarker = clone._colorMarker;
				this._markerFont = clone._markerFont;
				this._drawMarker = clone._drawMarker;
				this._markerLength = clone._markerLength;
				this._drawWaveForm = clone._drawWaveForm;
			}
		}

		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (this._renderingInProgress)
				{
					return;
				}
				if (!string.IsNullOrEmpty(value) && value == this._fileName)
				{
					return;
				}
				object syncRoot = this._syncRoot;
				lock (syncRoot)
				{
					this._fileName = value;
					this._isRendered = false;
					this._decodingStream = 0;
					this._framesToRender = 0;
					this._framesRendered = 0;
					if (this._waveBuffer != null)
					{
						this._waveBuffer.data = null;
						if (this._waveBuffer.marker != null)
						{
							this._waveBuffer.marker.Clear();
							this._waveBuffer.marker = null;
						}
						if (this._waveBuffer.beats != null)
						{
							this._waveBuffer.beats.Clear();
							this._waveBuffer.beats = null;
						}
						this._waveBuffer = null;
					}
					if (this._volumePoints != null)
					{
						this._volumePoints.Clear();
						this._volumePoints = null;
					}
				}
			}
		}

		public double FrameResolution
		{
			get
			{
				return this._frameResolution;
			}
			set
			{
				if (value < 0.0010000000474974513)
				{
					this._frameResolution = 0.0010000000474974513;
				}
				else if (value > 5.0)
				{
					this._frameResolution = 5.0;
				}
				else
				{
					this._frameResolution = value;
				}
				if (this._useSimpleScan)
				{
					this._frameResolution = 0.019999999552965164;
				}
			}
		}

		public int CallbackFrequency
		{
			get
			{
				return this._callbackFrequency;
			}
			set
			{
				this._callbackFrequency = value;
			}
		}

		public WAVEFORMPROC NotifyHandler
		{
			get
			{
				return this._notifyHandler;
			}
			set
			{
				this._notifyHandler = value;
			}
		}

		public Control WinControl
		{
			get
			{
				return this._win;
			}
			set
			{
				this._win = value;
			}
		}

		public bool PreScan
		{
			get
			{
				return this._preScan;
			}
			set
			{
				this._preScan = value;
			}
		}

		public bool UseSimpleScan
		{
			get
			{
				return this._useSimpleScan;
			}
			set
			{
				this._useSimpleScan = value;
				if (this._useSimpleScan)
				{
					this._frameResolution = 0.02;
				}
			}
		}

		public bool IsRendered
		{
			get
			{
				return this._isRendered;
			}
		}

		public bool IsRenderingInProgress
		{
			get
			{
				return this._renderingInProgress;
			}
		}

		public int FramesToRender
		{
			get
			{
				return this._framesToRender;
			}
		}

		public int FramesRendered
		{
			get
			{
				return this._framesRendered;
			}
		}

		public bool DetectBeats
		{
			get
			{
				return this._detectBeats;
			}
			set
			{
				this._detectBeats = value;
			}
		}

		public double TempoFactor
		{
			get
			{
				return this._syncFactor / this._channelFactor - 1.0;
			}
			set
			{
				this._syncFactor = this._channelFactor * (1.0 + value);
			}
		}

		public double SyncFactor
		{
			get
			{
				return this._syncFactor;
			}
		}

		public float GainFactor
		{
			get
			{
				return this._gainFactor;
			}
			set
			{
				this._gainFactor = value;
			}
		}

		public WaveForm.WaveBuffer Wave
		{
			get
			{
				return this._waveBuffer;
			}
			set
			{
				if (this._renderingInProgress)
				{
					return;
				}
				if (value != null)
				{
					object syncRoot = this._syncRoot;
					lock (syncRoot)
					{
						this._waveBuffer = value;
						this._isRendered = true;
						this._fileName = this._waveBuffer.fileName;
						this._framesRendered = this._waveBuffer.data.Length;
						this._framesToRender = this._waveBuffer.data.Length;
						this._frameResolution = this._waveBuffer.resolution;
					}
				}
			}
		}

		public Color ColorBackground
		{
			get
			{
				return this._colorBackground;
			}
			set
			{
				this._colorBackground = value;
			}
		}

		public Color ColorLeft
		{
			get
			{
				return this._colorLeft;
			}
			set
			{
				this._colorLeft = value;
			}
		}

		public Color ColorLeft2
		{
			get
			{
				return this._colorLeft2;
			}
			set
			{
				this._colorLeft2 = value;
			}
		}

		public Color ColorLeftEnvelope
		{
			get
			{
				return this._colorLeftEnvelope;
			}
			set
			{
				this._colorLeftEnvelope = value;
			}
		}

		public bool DrawEnvelope
		{
			get
			{
				return this._drawEnvelope;
			}
			set
			{
				this._drawEnvelope = value;
			}
		}

		public bool DrawCenterLine
		{
			get
			{
				return this._drawCenterLine;
			}
			set
			{
				this._drawCenterLine = value;
			}
		}

		public Color ColorMiddleLeft
		{
			get
			{
				return this._colorMiddleLeft;
			}
			set
			{
				this._colorMiddleLeft = value;
			}
		}

		public Color ColorMiddleRight
		{
			get
			{
				return this._colorMiddleRight;
			}
			set
			{
				this._colorMiddleRight = value;
			}
		}

		public bool DrawGradient
		{
			get
			{
				return this._drawGradient;
			}
			set
			{
				this._drawGradient = value;
			}
		}

		public Color ColorRight
		{
			get
			{
				return this._colorRight;
			}
			set
			{
				this._colorRight = value;
			}
		}

		public Color ColorRight2
		{
			get
			{
				return this._colorRight2;
			}
			set
			{
				this._colorRight2 = value;
			}
		}

		public Color ColorRightEnvelope
		{
			get
			{
				return this._colorRightEnvelope;
			}
			set
			{
				this._colorRightEnvelope = value;
			}
		}

		public Color ColorVolume
		{
			get
			{
				return this._colorVolume;
			}
			set
			{
				this._colorVolume = value;
			}
		}

		public PixelFormat PixelFormat
		{
			get
			{
				return this._pixelFormat;
			}
			set
			{
				this._pixelFormat = value;
			}
		}

		public WaveForm.VOLUMEDRAWTYPE DrawVolume
		{
			get
			{
				return this._drawVolume;
			}
			set
			{
				this._drawVolume = value;
			}
		}

		public bool VolumeCurveZeroLevel
		{
			get
			{
				return this._volumeCurveZeroLevel;
			}
			set
			{
				this._volumeCurveZeroLevel = value;
			}
		}

		public Color ColorBeat
		{
			get
			{
				return this._colorBeat;
			}
			set
			{
				this._colorBeat = value;
			}
		}

		public WaveForm.BEATDRAWTYPE DrawBeat
		{
			get
			{
				return this._drawBeat;
			}
			set
			{
				this._drawBeat = value;
			}
		}

		public float BeatLength
		{
			get
			{
				return this._beatLength;
			}
			set
			{
				if (value > 0f && value <= 1f)
				{
					this._beatLength = value / 2f;
				}
			}
		}

		public int BeatWidth
		{
			get
			{
				return this._beatWidth;
			}
			set
			{
				if (value > 0 && value <= 10)
				{
					this._beatWidth = value;
				}
			}
		}

		public Color ColorMarker
		{
			get
			{
				return this._colorMarker;
			}
			set
			{
				this._colorMarker = value;
			}
		}

		public Font MarkerFont
		{
			get
			{
				return this._markerFont;
			}
			set
			{
				this._markerFont = value;
			}
		}

		public WaveForm.MARKERDRAWTYPE DrawMarker
		{
			get
			{
				return this._drawMarker;
			}
			set
			{
				this._drawMarker = value;
			}
		}

		public float MarkerLength
		{
			get
			{
				return this._markerLength;
			}
			set
			{
				if (value > 0f && value <= 1f)
				{
					this._markerLength = value / 2f;
				}
			}
		}

		public WaveForm.WAVEFORMDRAWTYPE DrawWaveForm
		{
			get
			{
				return this._drawWaveForm;
			}
			set
			{
				this._drawWaveForm = value;
			}
		}

		public WaveForm Clone(bool flat)
		{
			if (this.IsRendered && !this.IsRenderingInProgress)
			{
				return new WaveForm(this, flat);
			}
			return null;
		}

		public void Reset()
		{
			this.FileName = string.Empty;
			this._win = null;
			this._notifyHandler = null;
		}

		public bool RenderStart(int decodingStream, bool background)
		{
			return this.RenderStart(decodingStream, background, ThreadPriority.Normal, true);
		}

		public bool RenderStart(int decodingStream, bool background, bool freeStream)
		{
			return this.RenderStart(decodingStream, background, ThreadPriority.Normal, freeStream);
		}

		public bool RenderStart(int decodingStream, bool background, ThreadPriority prio, bool freeStream)
		{
			if (decodingStream == 0)
			{
				return false;
			}
			this._decodingStream = decodingStream;
			Bass.BASS_ChannelSetPosition(this._decodingStream, 0L);
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (!Bass.BASS_ChannelGetInfo(this._decodingStream, bass_CHANNELINFO))
			{
				return false;
			}
			if (bass_CHANNELINFO.chans == 0)
			{
				bass_CHANNELINFO.chans = 2;
			}
			if ((bass_CHANNELINFO.ctype & BASSChannelType.BASS_CTYPE_STREAM) == BASSChannelType.BASS_CTYPE_UNKNOWN && (bass_CHANNELINFO.ctype & BASSChannelType.BASS_CTYPE_MUSIC_MOD) == BASSChannelType.BASS_CTYPE_UNKNOWN)
			{
				return false;
			}
			this._freeStream = freeStream;
			return this.Render(background, prio, bass_CHANNELINFO.flags, bass_CHANNELINFO.chans);
		}

		public bool RenderStart(bool background, BASSFlag flags)
		{
			return this.RenderStart(background, ThreadPriority.Normal, flags, IntPtr.Zero, 0L);
		}

		public bool RenderStart(bool background, ThreadPriority prio, BASSFlag flags)
		{
			return this.RenderStart(background, prio, flags, IntPtr.Zero, 0L);
		}

		public bool RenderStart(bool background, BASSFlag flags, IntPtr memory, long length)
		{
			return this.RenderStart(background, ThreadPriority.Normal, flags, memory, length);
		}

		private bool RenderStart(bool background, ThreadPriority prio, BASSFlag flags, IntPtr memory, long length)
		{
			if (memory == IntPtr.Zero && string.IsNullOrEmpty(this.FileName))
			{
				return false;
			}
			flags |= (BASSFlag.BASS_STREAM_DECODE | (this._preScan ? BASSFlag.BASS_STREAM_PRESCAN : BASSFlag.BASS_DEFAULT));
			this._decodingStream = 0;
			if (memory == IntPtr.Zero)
			{
				this._decodingStream = Bass.BASS_StreamCreateFile(this.FileName, 0L, 0L, flags);
			}
			else
			{
				this._decodingStream = Bass.BASS_StreamCreateFile(memory, 0L, length, flags);
			}
			if (this._decodingStream == 0)
			{
				return false;
			}
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (!Bass.BASS_ChannelGetInfo(this._decodingStream, bass_CHANNELINFO))
			{
				return false;
			}
			int num = bass_CHANNELINFO.chans;
			if (num == 0)
			{
				num = 2;
			}
			if ((bass_CHANNELINFO.ctype & BASSChannelType.BASS_CTYPE_STREAM) == BASSChannelType.BASS_CTYPE_UNKNOWN && (bass_CHANNELINFO.ctype & BASSChannelType.BASS_CTYPE_MUSIC_MOD) == BASSChannelType.BASS_CTYPE_UNKNOWN)
			{
				return false;
			}
			this._freeStream = true;
			return this.Render(background, prio, bass_CHANNELINFO.flags, num);
		}

		public void RenderStop()
		{
			this._killScan = true;
		}

		public bool RenderStartRecording(int recordingStream, int initLength, int nextLength)
		{
			return this.RenderStartRecording(recordingStream, (float)initLength, (float)nextLength);
		}

		public bool RenderStartRecording(int recordingStream, float initLength, float nextLength)
		{
			if (recordingStream == 0 || initLength <= 0f)
			{
				return false;
			}
			if (nextLength < 0f)
			{
				nextLength = 0f;
			}
			if (initLength < 1f)
			{
				initLength = 1f;
			}
			this._decodingStream = recordingStream;
			BASSFlag flags = BASSFlag.BASS_DEFAULT;
			int num = 2;
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (Bass.BASS_ChannelGetInfo(this._decodingStream, bass_CHANNELINFO))
			{
				flags = bass_CHANNELINFO.flags;
				num = bass_CHANNELINFO.chans;
				if (num == 0)
				{
					num = 2;
				}
			}
			this._isRendered = false;
			this._renderingInProgress = true;
			this._framesRendered = 0;
			this._recordingLeftoverBytes = 0;
			this._recordingLeftoverLevel.left = 0;
			this._recordingLeftoverLevel.right = 0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				this._waveBuffer = new WaveForm.WaveBuffer();
				double num2 = (double)Bass.BASS_ChannelSeconds2Bytes(this._decodingStream, (double)initLength);
				double num3 = (double)Bass.BASS_ChannelSeconds2Bytes(this._decodingStream, (double)nextLength);
				this._waveBuffer.chans = num;
				this._waveBuffer.resolution = this.FrameResolution;
				this._waveBuffer.bpf = (int)Bass.BASS_ChannelSeconds2Bytes(this._decodingStream, this._waveBuffer.resolution);
				this._waveBuffer.flags = flags;
				this._framesToRender = (int)Math.Ceiling(num2 / (double)this._waveBuffer.bpf);
				this._recordingNextLength = (int)Math.Ceiling(num3 / (double)this._waveBuffer.bpf);
				this._waveBuffer.data = new WaveForm.WaveBuffer.Level[this._framesToRender];
				this._waveBuffer.fileName = this.FileName;
				this._renderStartTime = DateTime.Now;
			}
			return true;
		}

		public void RenderStopRecording()
		{
			this._renderingInProgress = false;
			this._isRendered = true;
			this._recordingLeftoverBytes = 0;
			this._recordingLeftoverLevel.left = 0;
			this._recordingLeftoverLevel.right = 0;
		}

		public void RenderRecording()
		{
			if (this._waveBuffer == null || this._isRendered)
			{
				return;
			}
			if (Bass.BASS_ChannelIsActive(this._decodingStream) == BASSActive.BASS_ACTIVE_STOPPED)
			{
				this.RenderStopRecording();
				return;
			}
			int num = 0;
			int bps = this._waveBuffer.bps;
			int num2 = this._waveBuffer.bpf / bps;
			int num3 = 0;
			if (this._recordingLeftoverBytes > 0)
			{
				num3 = this._waveBuffer.bpf - this._recordingLeftoverBytes;
			}
			int num4 = Bass.BASS_ChannelGetData(this._decodingStream, IntPtr.Zero, 0);
			if (num4 <= 0)
			{
				return;
			}
			int num5 = num4 / bps;
			if (bps == 2)
			{
				if (this._peakLevelShort == null || this._peakLevelShort.Length < num5)
				{
					this._peakLevelShort = new short[num5];
				}
				num4 = Bass.BASS_ChannelGetData(this._decodingStream, this._peakLevelShort, num4);
			}
			else if (bps == 4)
			{
				if (this._peakLevelFloat == null || this._peakLevelFloat.Length < num5)
				{
					this._peakLevelFloat = new float[num5];
				}
				num4 = Bass.BASS_ChannelGetData(this._decodingStream, this._peakLevelFloat, num4);
			}
			else
			{
				if (this._peakLevelByte == null || this._peakLevelByte.Length < num5)
				{
					this._peakLevelByte = new byte[num5];
				}
				num4 = Bass.BASS_ChannelGetData(this._decodingStream, this._peakLevelByte, num4);
			}
			int num6 = (num4 - num3) / this._waveBuffer.bpf;
			if (this._recordingLeftoverBytes > 0)
			{
				WaveForm.WaveBuffer.Level level;
				if (bps == 2)
				{
					level = WaveForm.GetLevel(this._peakLevelShort, this._waveBuffer.chans, 0, num3 / bps);
				}
				else if (bps == 4)
				{
					level = WaveForm.GetLevel(this._peakLevelFloat, this._waveBuffer.chans, 0, num3 / bps);
				}
				else
				{
					level = WaveForm.GetLevel(this._peakLevelByte, this._waveBuffer.chans, 0, num3 / bps);
				}
				if (this._recordingLeftoverLevel.left == -32768)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left < 0 && this._recordingLeftoverLevel.left < 0 && level.left > this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left > 0 && this._recordingLeftoverLevel.left > 0 && level.left < this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left < 0 && level.left > -this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left > 0 && level.left < -this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				if (this._recordingLeftoverLevel.right == -32768)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right < 0 && this._recordingLeftoverLevel.right < 0 && level.right > this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right > 0 && this._recordingLeftoverLevel.right > 0 && level.right < this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right < 0 && level.right > -this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right > 0 && level.right < -this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				this._waveBuffer.data[this._framesRendered] = level;
				this._framesRendered++;
				this.CheckWaveBufferSize();
				num = num3 / bps;
			}
			for (int i = 0; i < num6; i++)
			{
				WaveForm.WaveBuffer.Level level;
				if (bps == 2)
				{
					level = WaveForm.GetLevel(this._peakLevelShort, this._waveBuffer.chans, num, num2);
				}
				else if (bps == 4)
				{
					level = WaveForm.GetLevel(this._peakLevelFloat, this._waveBuffer.chans, num, num2);
				}
				else
				{
					level = WaveForm.GetLevel(this._peakLevelByte, this._waveBuffer.chans, num, num2);
				}
				num += num2;
				object syncRoot = this._syncRoot;
				lock (syncRoot)
				{
					this._waveBuffer.data[this._framesRendered] = level;
					this._framesRendered++;
				}
				this.CheckWaveBufferSize();
			}
			this._recordingLeftoverBytes = (num4 - num3) % this._waveBuffer.bpf;
			if (this._recordingLeftoverBytes <= 0)
			{
				this._recordingLeftoverLevel.left = 0;
				this._recordingLeftoverLevel.right = 0;
				return;
			}
			if (bps == 2)
			{
				this._recordingLeftoverLevel = WaveForm.GetLevel(this._peakLevelShort, this._waveBuffer.chans, num, this._recordingLeftoverBytes / bps);
				return;
			}
			if (bps == 4)
			{
				this._recordingLeftoverLevel = WaveForm.GetLevel(this._peakLevelFloat, this._waveBuffer.chans, num, this._recordingLeftoverBytes / bps);
				return;
			}
			this._recordingLeftoverLevel = WaveForm.GetLevel(this._peakLevelByte, this._waveBuffer.chans, num, this._recordingLeftoverBytes / bps);
		}

		public void RenderRecording(IntPtr buffer, int length)
		{
			if (this._waveBuffer == null || this._isRendered || buffer == IntPtr.Zero || length <= 0)
			{
				return;
			}
			if (Bass.BASS_ChannelIsActive(this._decodingStream) == BASSActive.BASS_ACTIVE_STOPPED)
			{
				this.RenderStopRecording();
				return;
			}
			int num = 0;
			int bps = this._waveBuffer.bps;
			int num2 = this._waveBuffer.bpf / bps;
			int num3 = 0;
			if (this._recordingLeftoverBytes > 0)
			{
				num3 = this._waveBuffer.bpf - this._recordingLeftoverBytes;
			}
			int num4 = (length - num3) / this._waveBuffer.bpf;
			if (this._recordingLeftoverBytes > 0)
			{
				WaveForm.WaveBuffer.Level level = WaveForm.GetLevel(buffer, this._waveBuffer.chans, bps, 0, num3 / bps);
				if (this._recordingLeftoverLevel.left == -32768)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left < 0 && this._recordingLeftoverLevel.left < 0 && level.left > this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left > 0 && this._recordingLeftoverLevel.left > 0 && level.left < this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left < 0 && level.left > -this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				else if (level.left > 0 && level.left < -this._recordingLeftoverLevel.left)
				{
					level.left = this._recordingLeftoverLevel.left;
				}
				if (this._recordingLeftoverLevel.right == -32768)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right < 0 && this._recordingLeftoverLevel.right < 0 && level.right > this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right > 0 && this._recordingLeftoverLevel.right > 0 && level.right < this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right < 0 && level.right > -this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				else if (level.right > 0 && level.right < -this._recordingLeftoverLevel.right)
				{
					level.right = this._recordingLeftoverLevel.right;
				}
				object syncRoot = this._syncRoot;
				lock (syncRoot)
				{
					this._waveBuffer.data[this._framesRendered] = level;
					this._framesRendered++;
				}
				this.CheckWaveBufferSize();
				num = num3 / bps;
			}
			for (int i = 0; i < num4; i++)
			{
				WaveForm.WaveBuffer.Level level = WaveForm.GetLevel(buffer, this._waveBuffer.chans, bps, num, num2);
				num += num2;
				object syncRoot = this._syncRoot;
				lock (syncRoot)
				{
					this._waveBuffer.data[this._framesRendered] = level;
					this._framesRendered++;
				}
				this.CheckWaveBufferSize();
			}
			this._recordingLeftoverBytes = (length - num3) % this._waveBuffer.bpf;
			if (this._recordingLeftoverBytes > 0)
			{
				this._recordingLeftoverLevel = WaveForm.GetLevel(buffer, this._waveBuffer.chans, bps, num, this._recordingLeftoverBytes / bps);
				return;
			}
			this._recordingLeftoverLevel.left = 0;
			this._recordingLeftoverLevel.right = 0;
		}

		private void CheckWaveBufferSize()
		{
			if (this._framesRendered >= this._framesToRender)
			{
				if (this._recordingNextLength > 0)
				{
					object syncRoot = this._syncRoot;
					lock (syncRoot)
					{
						this._framesToRender += this._recordingNextLength;
						WaveForm.WaveBuffer.Level[] array = new WaveForm.WaveBuffer.Level[this._framesToRender];
						this._waveBuffer.data.CopyTo(array, 0);
						this._waveBuffer.data = array;
						return;
					}
				}
				this._framesRendered = 0;
			}
		}

		public bool SyncPlayback(int channel)
		{
			if (this._waveBuffer == null)
			{
				return false;
			}
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (Bass.BASS_ChannelGetInfo(channel, bass_CHANNELINFO))
			{
				double num = 2.0;
				if (bass_CHANNELINFO.Is32bit)
				{
					num = 4.0;
				}
				else if (bass_CHANNELINFO.Is8bit)
				{
					num = 1.0;
				}
				this._channelFactor = (this._syncFactor = (double)this._waveBuffer.bps / num);
				this._correctionFactor = Bass.BASS_ChannelBytes2Seconds(channel, (long)this._waveBuffer.bpf) / this._syncFactor / this._waveBuffer.resolution;
				return true;
			}
			return false;
		}

		public long Position2Rendering(long bytes)
		{
			long num = (long)((double)bytes * this._syncFactor);
			if (bytes > 0L && num < 0L)
			{
				num = long.MaxValue;
			}
			else if (bytes < 0L && num > 0L)
			{
				num = long.MinValue;
			}
			return num;
		}

		public long Position2Rendering(double seconds)
		{
			long num = -1L;
			if (this._waveBuffer != null)
			{
				num = (long)(seconds * ((double)this._waveBuffer.bpf / this._waveBuffer.resolution) / this._correctionFactor);
				if (seconds > 0.0 && num < 0L)
				{
					num = long.MaxValue;
				}
				else if (seconds < 0.0 && num > 0L)
				{
					num = long.MinValue;
				}
			}
			return num;
		}

		public long Position2Playback(long bytes)
		{
			return (long)((double)bytes / this._syncFactor);
		}

		public long Position2Playback(double seconds)
		{
			long num = -1L;
			if (this._waveBuffer != null)
			{
				num = (long)(seconds * ((double)this._waveBuffer.bpf / this._waveBuffer.resolution) / this._correctionFactor);
				if (seconds > 0.0 && num < 0L)
				{
					num = long.MaxValue;
				}
				else if (seconds < 0.0 && num > 0L)
				{
					num = long.MinValue;
				}
			}
			return num;
		}

		public int Position2Frames(long bytes)
		{
			int num = -1;
			if (this._waveBuffer != null)
			{
				num = (int)((double)bytes * this._syncFactor / (double)this._waveBuffer.bpf);
				if (this._waveBuffer.data != null && num >= this._waveBuffer.data.Length)
				{
					num = this._waveBuffer.data.Length - 1;
				}
			}
			return num;
		}

		public int Position2Frames(double seconds)
		{
			int num = -1;
			if (this._waveBuffer != null)
			{
				num = (int)(seconds / this._waveBuffer.resolution / this._correctionFactor);
				if (seconds > 0.0 && num <= 0)
				{
					num = int.MaxValue;
				}
				else if (seconds < 0.0 && num > 0)
				{
					num = int.MinValue;
				}
				if (this._waveBuffer.data != null && num >= this._waveBuffer.data.Length)
				{
					num = this._waveBuffer.data.Length - 1;
				}
			}
			return num;
		}

		public long Frame2Bytes(int frame)
		{
			double num = -1.0;
			if (this._waveBuffer != null)
			{
				num = (double)((long)frame * (long)this._waveBuffer.bpf) / this._syncFactor;
			}
			return (long)(num + 0.5);
		}

		public double Frame2Seconds(int frame)
		{
			double result = -1.0;
			if (this._waveBuffer != null)
			{
				result = (double)frame * this._waveBuffer.resolution * this._correctionFactor;
			}
			return result;
		}

		private string FindMarker(string name)
		{
			if (this._waveBuffer == null || this._waveBuffer.marker == null)
			{
				return null;
			}
			int num = name.IndexOf('{');
			if (num > 0)
			{
				string value = name.Substring(0, num);
				foreach (string text in this._waveBuffer.marker.Keys)
				{
					if (text.StartsWith(value))
					{
						name = text;
						break;
					}
				}
			}
			return name;
		}

		public bool AddMarker(string name, long position)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && !string.IsNullOrEmpty(name))
				{
					if (this._waveBuffer.marker == null)
					{
						this._waveBuffer.marker = new Dictionary<string, long>();
					}
					if (this._waveBuffer.marker != null)
					{
						position = this.Position2Rendering(position);
						string text = this.FindMarker(name);
						if (this._waveBuffer.marker.ContainsKey(text))
						{
							if (text != name)
							{
								this._waveBuffer.marker.Remove(text);
								this._waveBuffer.marker.Add(name, position);
							}
							else
							{
								this._waveBuffer.marker[name] = position;
							}
							result = true;
						}
						else
						{
							this._waveBuffer.marker.Add(name, position);
							result = true;
						}
					}
				}
			}
			return result;
		}

		public bool AddMarker(string name, double position)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && !string.IsNullOrEmpty(name))
				{
					if (this._waveBuffer.marker == null)
					{
						this._waveBuffer.marker = new Dictionary<string, long>();
					}
					if (this._waveBuffer.marker != null)
					{
						long value = this.Position2Rendering(position);
						name = this.FindMarker(name);
						if (this._waveBuffer.marker.ContainsKey(name))
						{
							this._waveBuffer.marker[name] = value;
							result = true;
						}
						else
						{
							this._waveBuffer.marker.Add(name, value);
							result = true;
						}
					}
				}
			}
			return result;
		}

		public long GetMarker(string name)
		{
			long result = -1L;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && this._waveBuffer.marker != null && !string.IsNullOrEmpty(name))
				{
					try
					{
						name = this.FindMarker(name);
						result = this.Position2Playback(this._waveBuffer.marker[name]);
					}
					catch
					{
						result = -1L;
					}
				}
			}
			return result;
		}

		public double GetMarkerSec(string name)
		{
			double result = -1.0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && this._waveBuffer.marker != null && !string.IsNullOrEmpty(name))
				{
					try
					{
						name = this.FindMarker(name);
						result = (double)this._waveBuffer.marker[name] / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
					}
					catch
					{
						result = -1.0;
					}
				}
			}
			return result;
		}

		public int GetMarkerCount()
		{
			int result = 0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && this._waveBuffer.marker != null)
				{
					result = this._waveBuffer.marker.Count;
				}
			}
			return result;
		}

		public string[] GetMarkers()
		{
			string[] array = null;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && this._waveBuffer.marker != null)
				{
					array = new string[this._waveBuffer.marker.Count];
					this._waveBuffer.marker.Keys.CopyTo(array, 0);
				}
				else
				{
					array = new string[0];
				}
			}
			return array;
		}

		public bool RemoveMarker(string name)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null && this._waveBuffer.marker != null && !string.IsNullOrEmpty(name))
				{
					name = this.FindMarker(name);
					if (this._waveBuffer.marker.ContainsKey(name))
					{
						this._waveBuffer.marker.Remove(name);
						result = true;
					}
				}
			}
			return result;
		}

		public bool ClearAllMarker()
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._waveBuffer != null)
				{
					if (this._waveBuffer.marker != null)
					{
						this._waveBuffer.marker.Clear();
					}
					this._waveBuffer.marker = null;
					result = true;
				}
			}
			return result;
		}

		public int SearchVolumePoint(long position)
		{
			object syncRoot = this._syncRoot;
			int result;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					result = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(position), 1f));
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		public int SearchVolumePoint(double position)
		{
			object syncRoot = this._syncRoot;
			int result;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					result = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(position), 1f));
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		public int SearchVolumePoint(long position, ref WaveForm.VolumePoint prev, ref WaveForm.VolumePoint next)
		{
			int num = 0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					num = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(position), 1f));
					if (num >= 0)
					{
						prev = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num].Position), this._volumePoints[num].Level);
						if (num < this._volumePoints.Count - 1)
						{
							next = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num + 1].Position), this._volumePoints[num + 1].Level);
						}
						else
						{
							next = null;
						}
					}
					else
					{
						int num2 = ~num;
						if (num2 > 0)
						{
							prev = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num2 - 1].Position), this._volumePoints[num2 - 1].Level);
						}
						else
						{
							prev = null;
						}
						if (num2 < this._volumePoints.Count)
						{
							next = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num2].Position), this._volumePoints[num2].Level);
						}
						else
						{
							next = null;
						}
					}
				}
			}
			return num;
		}

		public int SearchVolumePoint(double position, ref WaveForm.VolumePoint prev, ref WaveForm.VolumePoint next)
		{
			int num = 0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					num = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(position), 1f));
					if (num >= 0)
					{
						prev = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num].Position), this._volumePoints[num].Level);
						if (num < this._volumePoints.Count - 1)
						{
							next = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num + 1].Position), this._volumePoints[num + 1].Level);
						}
						else
						{
							next = null;
						}
					}
					else
					{
						int num2 = ~num;
						if (num2 > 0)
						{
							prev = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num2 - 1].Position), this._volumePoints[num2 - 1].Level);
						}
						else
						{
							prev = null;
						}
						if (num2 < this._volumePoints.Count)
						{
							next = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[num2].Position), this._volumePoints[num2].Level);
						}
						else
						{
							next = null;
						}
					}
				}
			}
			return num;
		}

		public bool AddVolumePoint(long position, float level)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints == null)
				{
					this._volumePoints = new List<WaveForm.VolumePoint>();
				}
				if (this._volumePoints != null)
				{
					int num = this.SearchVolumePoint(position);
					if (num >= 0)
					{
						this._volumePoints[num].Level = level;
						result = true;
					}
					else
					{
						position = this.Position2Rendering(position);
						this._volumePoints.Insert(~num, new WaveForm.VolumePoint(position, level));
						result = true;
					}
				}
			}
			return result;
		}

		public bool AddVolumePoint(double position, float level)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints == null)
				{
					this._volumePoints = new List<WaveForm.VolumePoint>();
				}
				if (this._volumePoints != null)
				{
					int num = this.SearchVolumePoint(position);
					if (num >= 0)
					{
						this._volumePoints[num].Level = level;
						result = true;
					}
					else
					{
						long position2 = this.Position2Rendering(position);
						this._volumePoints.Insert(~num, new WaveForm.VolumePoint(position2, level));
						result = true;
					}
				}
			}
			return result;
		}

		public float GetVolumePoint(long position)
		{
			float result = -1f;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					int num = this.SearchVolumePoint(position);
					if (num >= 0)
					{
						result = this._volumePoints[num].Level;
					}
				}
			}
			return result;
		}

		public float GetVolumePoint(double position)
		{
			float result = -1f;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					int num = this.SearchVolumePoint(position);
					if (num >= 0)
					{
						result = this._volumePoints[num].Level;
					}
				}
			}
			return result;
		}

		public WaveForm.VolumePoint GetVolumePoint(int index)
		{
			object syncRoot = this._syncRoot;
			WaveForm.VolumePoint result;
			lock (syncRoot)
			{
				if (this._volumePoints != null && index >= 0 && index < this._volumePoints.Count)
				{
					result = new WaveForm.VolumePoint(this.Position2Playback(this._volumePoints[index].Position), this._volumePoints[index].Level);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		public float GetVolumeLevel(long position, bool reverse, ref long duration, ref float nextLevel)
		{
			float num = 1f;
			nextLevel = num;
			duration = 0L;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					WaveForm.VolumePoint volumePoint = null;
					WaveForm.VolumePoint volumePoint2 = null;
					if (this.SearchVolumePoint(position, ref volumePoint, ref volumePoint2) >= 0)
					{
						if (reverse)
						{
							this.SearchVolumePoint(position - 1L, ref volumePoint, ref volumePoint2);
							if (volumePoint != null)
							{
								nextLevel = volumePoint.Level;
								duration = volumePoint2.Position - volumePoint.Position;
							}
							else
							{
								nextLevel = (this._volumeCurveZeroLevel ? 0f : 1f);
								duration = volumePoint2.Position;
							}
							return volumePoint2.Level;
						}
						if (volumePoint2 != null)
						{
							nextLevel = volumePoint2.Level;
							duration = volumePoint2.Position - volumePoint.Position;
						}
						else
						{
							nextLevel = (this._volumeCurveZeroLevel ? 0f : 1f);
							duration = this.Frame2Bytes(this.FramesRendered - 1) - volumePoint.Position;
						}
						return volumePoint.Level;
					}
					else if (this._volumePoints.Count > 0)
					{
						long num2 = 0L;
						float num3 = this._volumeCurveZeroLevel ? 0f : 1f;
						long num4 = this.Frame2Bytes(this.FramesRendered - 1);
						float num5 = this._volumeCurveZeroLevel ? 0f : 1f;
						if (volumePoint != null)
						{
							num2 = volumePoint.Position;
							num3 = volumePoint.Level;
						}
						if (volumePoint2 != null)
						{
							num4 = volumePoint2.Position;
							num5 = volumePoint2.Level;
						}
						if (reverse)
						{
							duration = position - num2;
							nextLevel = num3;
						}
						else
						{
							duration = num4 - position;
							nextLevel = num5;
						}
						if (num5 > num3)
						{
							num = num3 + (num5 - num3) * ((float)(position - num2) / (float)(num4 - num2));
						}
						else if (num5 < num3)
						{
							num = num5 + (num3 - num5) * ((float)(num4 - position) / (float)(num4 - num2));
						}
						else
						{
							num = num3;
						}
					}
				}
			}
			return num;
		}

		public float GetVolumeLevel(double position, bool reverse, ref double duration, ref float nextLevel)
		{
			float num = 1f;
			nextLevel = num;
			duration = 0.0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					WaveForm.VolumePoint volumePoint = null;
					WaveForm.VolumePoint volumePoint2 = null;
					if (this.SearchVolumePoint(position, ref volumePoint, ref volumePoint2) >= 0)
					{
						if (reverse)
						{
							this.SearchVolumePoint(position - 0.0001, ref volumePoint, ref volumePoint2);
							if (volumePoint != null)
							{
								nextLevel = volumePoint.Level;
								duration = (double)(volumePoint2.Position - volumePoint.Position) / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
							}
							else
							{
								nextLevel = (this._volumeCurveZeroLevel ? 0f : 1f);
								duration = (double)volumePoint2.Position / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
							}
							return volumePoint2.Level;
						}
						if (volumePoint2 != null)
						{
							nextLevel = volumePoint2.Level;
							duration = (double)(volumePoint2.Position - volumePoint.Position) / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
						}
						else
						{
							nextLevel = (this._volumeCurveZeroLevel ? 0f : 1f);
							duration = (double)(this.Frame2Bytes(this.FramesRendered - 1) - volumePoint.Position) / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
						}
						return volumePoint.Level;
					}
					else if (this._volumePoints.Count > 0)
					{
						long num2 = 0L;
						float num3 = this._volumeCurveZeroLevel ? 0f : 1f;
						long num4 = this.Frame2Bytes(this.FramesRendered - 1);
						float num5 = this._volumeCurveZeroLevel ? 0f : 1f;
						if (volumePoint != null)
						{
							num2 = volumePoint.Position;
							num3 = volumePoint.Level;
						}
						if (volumePoint2 != null)
						{
							num4 = volumePoint2.Position;
							num5 = volumePoint2.Level;
						}
						if (reverse)
						{
							duration = (position - (double)num2) / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
							nextLevel = num3;
						}
						else
						{
							duration = ((double)num4 - position) / ((double)this._waveBuffer.bpf / this._waveBuffer.resolution);
							nextLevel = num5;
						}
						if (num5 > num3)
						{
							num = num3 + (num5 - num3) * ((float)(position - (double)num2) / (float)(num4 - num2));
						}
						else if (num5 < num3)
						{
							num = num5 + (num3 - num5) * ((float)((double)num4 - position) / (float)(num4 - num2));
						}
						else
						{
							num = num3;
						}
					}
				}
			}
			return num;
		}

		public int GetVolumePointCount()
		{
			int result = 0;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					result = this._volumePoints.Count;
				}
			}
			return result;
		}

		public bool RemoveVolumePoint(long position)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					int num = this.SearchVolumePoint(position);
					if (num >= 0)
					{
						this._volumePoints.RemoveAt(num);
						result = true;
					}
				}
			}
			return result;
		}

		public bool RemoveVolumePoint(double position)
		{
			bool result = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					int num = this.SearchVolumePoint(position);
					if (num >= 0)
					{
						this._volumePoints.RemoveAt(num);
						result = true;
					}
				}
			}
			return result;
		}

		public void RemoveVolumePointsBetween(long from, long to)
		{
			if (this._volumePoints == null)
			{
				return;
			}
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				int num = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(from), 1f));
				if (num < 0)
				{
					num = ~num;
					if (num >= this._volumePoints.Count)
					{
						return;
					}
				}
				else
				{
					if (num >= this._volumePoints.Count - 1)
					{
						return;
					}
					num++;
				}
				int num2 = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(to), 1f));
				if (num2 < 0)
				{
					num2 = ~num2;
					if (num2 > this._volumePoints.Count)
					{
						num2 = this._volumePoints.Count;
					}
					if (num2 == 0)
					{
						return;
					}
					num2--;
				}
				else
				{
					if (num2 == 0)
					{
						return;
					}
					num2--;
				}
				for (int i = num2; i >= num; i--)
				{
					this._volumePoints.RemoveAt(i);
				}
			}
		}

		public void RemoveVolumePointsBetween(double from, double to)
		{
			if (this._volumePoints == null)
			{
				return;
			}
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				int num = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(from), 1f));
				if (num < 0)
				{
					num = ~num;
					if (num >= this._volumePoints.Count)
					{
						return;
					}
				}
				else
				{
					if (num >= this._volumePoints.Count - 1)
					{
						return;
					}
					num++;
				}
				int num2 = this._volumePoints.BinarySearch(new WaveForm.VolumePoint(this.Position2Rendering(to), 1f));
				if (num2 < 0)
				{
					num2 = ~num2;
					if (num2 > this._volumePoints.Count)
					{
						num2 = this._volumePoints.Count;
					}
					if (num2 == 0)
					{
						return;
					}
					num2--;
				}
				else
				{
					if (num2 == 0)
					{
						return;
					}
					num2--;
				}
				for (int i = num2; i >= num; i--)
				{
					this._volumePoints.RemoveAt(i);
				}
			}
		}

		public bool ClearAllVolumePoints()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._volumePoints != null)
				{
					this._volumePoints.Clear();
				}
				this._volumePoints = null;
			}
			return true;
		}

		public Bitmap CreateBitmap(int width, int height, int frameStart, int frameEnd, bool highQuality)
		{
			if (width <= 1 || height <= 1 || frameStart > frameEnd || this._waveBuffer == null)
			{
				return null;
			}
			Bitmap bitmap = null;
			WaveForm.WAVEFORMDRAWTYPE waveformdrawtype = this._drawWaveForm;
			if (this._waveBuffer.chans == 1 && (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.DualMono))
			{
				waveformdrawtype = WaveForm.WAVEFORMDRAWTYPE.Mono;
			}
			if (this.DrawGradient)
			{
				Graphics graphics = null;
				Pen pen = null;
				Pen pen2 = null;
				Pen pen3 = null;
				Pen pen4 = null;
				Pen pen5 = null;
				Pen pen6 = null;
				Pen pen7 = null;
				Pen pen8 = null;
				Pen pen9 = null;
				Pen pen10 = null;
				Pen pen11 = null;
				try
				{
					float num = (float)height / 2f - 1f;
					float num2 = ((float)height - 1f) / 4f;
					RectangleF rect = Rectangle.Empty;
					RectangleF rect2 = Rectangle.Empty;
					RectangleF rectangleF = Rectangle.Empty;
					RectangleF rectangleF2 = Rectangle.Empty;
					switch (waveformdrawtype)
					{
					case WaveForm.WAVEFORMDRAWTYPE.Stereo:
						rect = new RectangleF(0f, -1f, (float)width, num2 + 1.1f);
						rect2 = new RectangleF(0f, num2 - 1.1f, (float)width, num2 + 1.1f);
						rectangleF = new RectangleF(0f, num2 + num2 - 1.1f, (float)width, num2 + 1.1f);
						rectangleF2 = new RectangleF(0f, num2 + num2 + num2 - 1.1f, (float)width, num2 + 1.1f);
						break;
					case WaveForm.WAVEFORMDRAWTYPE.Mono:
					case WaveForm.WAVEFORMDRAWTYPE.DualMono:
						rectangleF = new RectangleF(0f, -1f, (float)width, num + 1.1f);
						rect = rectangleF;
						rectangleF2 = new RectangleF(0f, num - 1.1f, (float)width, num + 1.1f);
						rect2 = rectangleF2;
						break;
					case WaveForm.WAVEFORMDRAWTYPE.HalfMono:
					case WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped:
						rectangleF2 = new RectangleF(0f, -1f, (float)width, (float)height + 1.1f);
						rectangleF = (rect = (rect2 = rectangleF2));
						break;
					}
					using (Brush brush = new LinearGradientBrush(rect, this.ColorLeft, this.ColorLeft2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 90f : 270f))
					{
						using (Brush brush2 = new LinearGradientBrush(rect2, this.ColorLeft, this.ColorLeft2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 270f : 90f))
						{
							using (Brush brush3 = new LinearGradientBrush(rectangleF, this.ColorRight, this.ColorRight2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 90f : 270f))
							{
								using (Brush brush4 = new LinearGradientBrush(rectangleF2, this.ColorRight, this.ColorRight2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 270f : 90f))
								{
									pen3 = new Pen(brush);
									pen4 = new Pen(brush2);
									pen5 = new Pen(brush3);
									pen6 = new Pen(brush4);
									pen7 = new Pen(this.ColorLeftEnvelope, 1f);
									pen8 = new Pen(this.ColorRightEnvelope, 1f);
									pen9 = new Pen(this.ColorMarker, 1f);
									pen10 = new Pen(this.ColorBeat, (float)this._beatWidth);
									pen11 = new Pen(this.ColorVolume, 1f);
									if ((this._drawVolume & WaveForm.VOLUMEDRAWTYPE.Dotted) != WaveForm.VOLUMEDRAWTYPE.None)
									{
										pen11.DashStyle = DashStyle.Dash;
									}
									if (this.ColorMiddleLeft == Color.Empty)
									{
										pen = pen7;
									}
									else
									{
										pen = new Pen(this.ColorMiddleLeft, 1f);
									}
									if (this.ColorMiddleRight == Color.Empty)
									{
										pen2 = pen8;
									}
									else
									{
										pen2 = new Pen(this.ColorMiddleRight, 1f);
									}
									bitmap = new Bitmap(width, height, this._pixelFormat);
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
										graphics.SmoothingMode = SmoothingMode.Default;
										graphics.CompositingQuality = CompositingQuality.Default;
										graphics.PixelOffsetMode = PixelOffsetMode.Default;
										graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
									}
									this.DrawBitmap(graphics, width, height, frameStart, frameEnd, pen, pen2, pen3, pen4, pen7, pen5, pen6, pen8, pen9, pen10, pen11);
								}
							}
						}
					}
					return bitmap;
				}
				catch
				{
					return null;
				}
				finally
				{
					if (pen3 != null)
					{
						pen3.Dispose();
					}
					if (pen4 != null)
					{
						pen4.Dispose();
					}
					if (pen7 != null)
					{
						pen7.Dispose();
					}
					if (pen5 != null)
					{
						pen5.Dispose();
					}
					if (pen6 != null)
					{
						pen6.Dispose();
					}
					if (pen8 != null)
					{
						pen8.Dispose();
					}
					if (this.ColorMiddleLeft != Color.Empty && pen != null)
					{
						pen.Dispose();
					}
					if (this.ColorMiddleRight != Color.Empty && pen2 != null)
					{
						pen2.Dispose();
					}
					if (pen9 != null)
					{
						pen9.Dispose();
					}
					if (pen10 != null)
					{
						pen10.Dispose();
					}
					if (pen11 != null)
					{
						pen11.Dispose();
					}
					if (graphics != null)
					{
						graphics.Dispose();
					}
				}
			}
			Graphics graphics2 = null;
			Pen pen12 = null;
			Pen pen13 = null;
			Pen pen14 = null;
			Pen pen15 = null;
			Pen pen16 = null;
			Pen pen17 = null;
			Pen pen18 = null;
			Pen pen19 = null;
			Pen pen20 = null;
			try
			{
				int num3 = height / 2;
				using (Brush brush5 = new SolidBrush(this.ColorLeft))
				{
					using (Brush brush6 = new SolidBrush(this.ColorRight))
					{
						pen14 = new Pen(brush5);
						pen15 = new Pen(brush6);
						pen16 = new Pen(this.ColorLeftEnvelope, 1f);
						pen17 = new Pen(this.ColorRightEnvelope, 1f);
						pen18 = new Pen(this.ColorMarker, 1f);
						pen19 = new Pen(this.ColorBeat, (float)this._beatWidth);
						pen20 = new Pen(this.ColorVolume, 1f);
						if ((this._drawVolume & WaveForm.VOLUMEDRAWTYPE.Dotted) != WaveForm.VOLUMEDRAWTYPE.None)
						{
							pen20.DashStyle = DashStyle.Dash;
						}
						if (this.ColorMiddleLeft == Color.Empty)
						{
							pen12 = pen14;
						}
						else
						{
							pen12 = new Pen(this.ColorMiddleLeft, 1f);
						}
						if (this.ColorMiddleRight == Color.Empty)
						{
							pen13 = pen15;
						}
						else
						{
							pen13 = new Pen(this.ColorMiddleRight, 1f);
						}
						bitmap = new Bitmap(width, height, this._pixelFormat);
						graphics2 = Graphics.FromImage(bitmap);
						if (highQuality)
						{
							graphics2.SmoothingMode = SmoothingMode.AntiAlias;
							graphics2.CompositingQuality = CompositingQuality.AssumeLinear;
							graphics2.PixelOffsetMode = PixelOffsetMode.Default;
							graphics2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							graphics2.SmoothingMode = SmoothingMode.Default;
							graphics2.CompositingQuality = CompositingQuality.Default;
							graphics2.PixelOffsetMode = PixelOffsetMode.Default;
							graphics2.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawBitmap(graphics2, width, height, frameStart, frameEnd, pen12, pen13, pen14, pen16, pen15, pen17, pen18, pen19, pen20);
					}
				}
			}
			catch
			{
				bitmap = null;
			}
			finally
			{
				if (pen14 != null)
				{
					pen14.Dispose();
				}
				if (pen16 != null)
				{
					pen16.Dispose();
				}
				if (pen15 != null)
				{
					pen15.Dispose();
				}
				if (pen17 != null)
				{
					pen17.Dispose();
				}
				if (this.ColorMiddleLeft != Color.Empty && pen12 != null)
				{
					pen12.Dispose();
				}
				if (this.ColorMiddleRight != Color.Empty && pen13 != null)
				{
					pen13.Dispose();
				}
				if (pen18 != null)
				{
					pen18.Dispose();
				}
				if (pen19 != null)
				{
					pen19.Dispose();
				}
				if (pen20 != null)
				{
					pen20.Dispose();
				}
				if (graphics2 != null)
				{
					graphics2.Dispose();
				}
			}
			return bitmap;
		}

		public bool CreateBitmap(Graphics g, Rectangle clipRectangle, int frameStart, int frameEnd, bool highQuality)
		{
			if (g == null || clipRectangle.Width <= 1 || clipRectangle.Height <= 1 || frameStart > frameEnd || this._waveBuffer == null)
			{
				return false;
			}
			bool result = true;
			WaveForm.WAVEFORMDRAWTYPE waveformdrawtype = this._drawWaveForm;
			if (this._waveBuffer.chans == 1 && (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.DualMono))
			{
				waveformdrawtype = WaveForm.WAVEFORMDRAWTYPE.Mono;
			}
			if (this.DrawGradient)
			{
				Pen pen = null;
				Pen pen2 = null;
				Pen pen3 = null;
				Pen pen4 = null;
				Pen pen5 = null;
				Pen pen6 = null;
				Pen pen7 = null;
				Pen pen8 = null;
				Pen pen9 = null;
				Pen pen10 = null;
				Pen pen11 = null;
				try
				{
					float num = (float)clipRectangle.Height / 2f - 1f;
					float num2 = ((float)clipRectangle.Height - 1f) / 4f;
					RectangleF rect = Rectangle.Empty;
					RectangleF rect2 = Rectangle.Empty;
					RectangleF rectangleF = Rectangle.Empty;
					RectangleF rectangleF2 = Rectangle.Empty;
					switch (waveformdrawtype)
					{
					case WaveForm.WAVEFORMDRAWTYPE.Stereo:
						rect = new RectangleF(0f, -1f, (float)clipRectangle.Width, num2 + 1.1f);
						rect2 = new RectangleF(0f, num2 - 1.1f, (float)clipRectangle.Width, num2 + 1.1f);
						rectangleF = new RectangleF(0f, num2 + num2 - 1.1f, (float)clipRectangle.Width, num2 + 1.1f);
						rectangleF2 = new RectangleF(0f, num2 + num2 + num2 - 1.1f, (float)clipRectangle.Width, num2 + 1.1f);
						break;
					case WaveForm.WAVEFORMDRAWTYPE.Mono:
					case WaveForm.WAVEFORMDRAWTYPE.DualMono:
						rectangleF = new RectangleF(0f, -1f, (float)clipRectangle.Width, num + 1.1f);
						rect = rectangleF;
						rectangleF2 = new RectangleF(0f, num - 1.1f, (float)clipRectangle.Width, num + 1.1f);
						rect2 = rectangleF2;
						break;
					case WaveForm.WAVEFORMDRAWTYPE.HalfMono:
					case WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped:
						rectangleF2 = new RectangleF(0f, -1f, (float)clipRectangle.Width, (float)clipRectangle.Height + 1.1f);
						rectangleF = (rect = (rect2 = rectangleF2));
						break;
					}
					using (Brush brush = new LinearGradientBrush(rect, this.ColorLeft, this.ColorLeft2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 90f : 270f))
					{
						using (Brush brush2 = new LinearGradientBrush(rect2, this.ColorLeft, this.ColorLeft2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 270f : 90f))
						{
							using (Brush brush3 = new LinearGradientBrush(rectangleF, this.ColorRight, this.ColorRight2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 90f : 270f))
							{
								using (Brush brush4 = new LinearGradientBrush(rectangleF2, this.ColorRight, this.ColorRight2, (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped) ? 270f : 90f))
								{
									pen3 = new Pen(brush);
									pen4 = new Pen(brush2);
									pen5 = new Pen(brush3);
									pen6 = new Pen(brush4);
									pen7 = new Pen(this.ColorLeftEnvelope, 1f);
									pen8 = new Pen(this.ColorRightEnvelope, 1f);
									pen9 = new Pen(this.ColorMarker, 1f);
									pen10 = new Pen(this.ColorBeat, (float)this._beatWidth);
									pen11 = new Pen(this.ColorVolume, 1f);
									if ((this._drawVolume & WaveForm.VOLUMEDRAWTYPE.Dotted) != WaveForm.VOLUMEDRAWTYPE.None)
									{
										pen11.DashStyle = DashStyle.Dash;
									}
									if (this.ColorMiddleLeft == Color.Empty)
									{
										pen = pen7;
									}
									else
									{
										pen = new Pen(this.ColorMiddleLeft, 1f);
									}
									if (this.ColorMiddleRight == Color.Empty)
									{
										pen2 = pen8;
									}
									else
									{
										pen2 = new Pen(this.ColorMiddleRight, 1f);
									}
									if (highQuality)
									{
										g.SmoothingMode = SmoothingMode.AntiAlias;
										g.CompositingQuality = CompositingQuality.AssumeLinear;
										g.PixelOffsetMode = PixelOffsetMode.Default;
										g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
									}
									else
									{
										g.SmoothingMode = SmoothingMode.Default;
										g.CompositingQuality = CompositingQuality.Default;
										g.PixelOffsetMode = PixelOffsetMode.Default;
										g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
									}
									this.DrawBitmap(g, clipRectangle.Width, clipRectangle.Height, frameStart, frameEnd, pen, pen2, pen3, pen4, pen7, pen5, pen6, pen8, pen9, pen10, pen11);
								}
							}
						}
					}
					return result;
				}
				catch
				{
					return false;
				}
				finally
				{
					if (pen3 != null)
					{
						pen3.Dispose();
					}
					if (pen4 != null)
					{
						pen4.Dispose();
					}
					if (pen7 != null)
					{
						pen7.Dispose();
					}
					if (pen5 != null)
					{
						pen5.Dispose();
					}
					if (pen6 != null)
					{
						pen6.Dispose();
					}
					if (pen8 != null)
					{
						pen8.Dispose();
					}
					if (this.ColorMiddleLeft != Color.Empty && pen != null)
					{
						pen.Dispose();
					}
					if (this.ColorMiddleRight != Color.Empty && pen2 != null)
					{
						pen2.Dispose();
					}
					if (pen9 != null)
					{
						pen9.Dispose();
					}
					if (pen10 != null)
					{
						pen10.Dispose();
					}
					if (pen11 != null)
					{
						pen11.Dispose();
					}
				}
			}
			Pen pen12 = null;
			Pen pen13 = null;
			Pen pen14 = null;
			Pen pen15 = null;
			Pen pen16 = null;
			Pen pen17 = null;
			Pen pen18 = null;
			Pen pen19 = null;
			Pen pen20 = null;
			try
			{
				int num3 = clipRectangle.Height / 2;
				using (Brush brush5 = new SolidBrush(this.ColorLeft))
				{
					using (Brush brush6 = new SolidBrush(this.ColorRight))
					{
						pen14 = new Pen(brush5);
						pen15 = new Pen(brush6);
						pen16 = new Pen(this.ColorLeftEnvelope, 1f);
						pen17 = new Pen(this.ColorRightEnvelope, 1f);
						pen18 = new Pen(this.ColorMarker, 1f);
						pen19 = new Pen(this.ColorBeat, (float)this._beatWidth);
						pen20 = new Pen(this.ColorVolume, 1f);
						if ((this._drawVolume & WaveForm.VOLUMEDRAWTYPE.Dotted) != WaveForm.VOLUMEDRAWTYPE.None)
						{
							pen20.DashStyle = DashStyle.Dash;
						}
						if (this.ColorMiddleLeft == Color.Empty)
						{
							pen12 = pen14;
						}
						else
						{
							pen12 = new Pen(this.ColorMiddleLeft, 1f);
						}
						if (this.ColorMiddleRight == Color.Empty)
						{
							pen13 = pen15;
						}
						else
						{
							pen13 = new Pen(this.ColorMiddleRight, 1f);
						}
						if (highQuality)
						{
							g.SmoothingMode = SmoothingMode.AntiAlias;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						}
						else
						{
							g.SmoothingMode = SmoothingMode.Default;
							g.CompositingQuality = CompositingQuality.Default;
							g.PixelOffsetMode = PixelOffsetMode.Default;
							g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
						}
						this.DrawBitmap(g, clipRectangle.Width, clipRectangle.Height, frameStart, frameEnd, pen12, pen13, pen14, pen16, pen15, pen17, pen18, pen19, pen20);
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (pen14 != null)
				{
					pen14.Dispose();
				}
				if (pen16 != null)
				{
					pen16.Dispose();
				}
				if (pen15 != null)
				{
					pen15.Dispose();
				}
				if (pen17 != null)
				{
					pen17.Dispose();
				}
				if (this.ColorMiddleLeft != Color.Empty && pen12 != null)
				{
					pen12.Dispose();
				}
				if (this.ColorMiddleRight != Color.Empty && pen13 != null)
				{
					pen13.Dispose();
				}
				if (pen18 != null)
				{
					pen18.Dispose();
				}
				if (pen19 != null)
				{
					pen19.Dispose();
				}
				if (pen20 != null)
				{
					pen20.Dispose();
				}
			}
			return result;
		}

		public long GetBytePositionFromX(int x, int graphicsWidth, int frameStart, int frameEnd)
		{
			if (this._waveBuffer == null)
			{
				return -1L;
			}
			if (frameEnd >= this._waveBuffer.data.Length)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameEnd < 0)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameStart < 0)
			{
				frameStart = 0;
			}
			if (graphicsWidth == 0)
			{
				graphicsWidth = 1;
			}
			if (x >= graphicsWidth)
			{
				x = graphicsWidth - 1;
			}
			long num = this.Frame2Bytes(frameStart);
			double num2 = (double)(this.Frame2Bytes(frameEnd) - num) / (double)graphicsWidth;
			return (long)((double)x * num2) + num;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double threshold)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, threshold, threshold, -1, -1, -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double thresholdIn, double thresholdOut)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, -1, -1, -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double thresholdIn, double thresholdOut, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, -1, -1, findZeroCrossing ? 1 : -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double threshold, int frameStart, int frameEnd)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, threshold, threshold, frameStart, frameEnd, -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double threshold, int frameStart, int frameEnd, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, threshold, threshold, frameStart, frameEnd, findZeroCrossing ? 1 : -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double thresholdIn, double thresholdOut, int frameStart, int frameEnd)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, frameStart, frameEnd, -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref long startpos, ref long endpos, double thresholdIn, double thresholdOut, int frameStart, int frameEnd, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, frameStart, frameEnd, findZeroCrossing ? 1 : -1);
			startpos = this.Frame2Bytes(frame);
			endpos = this.Frame2Bytes(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double threshold)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, threshold, threshold, -1, -1, -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double thresholdIn, double thresholdOut)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, -1, -1, -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double thresholdIn, double thresholdOut, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, -1, -1, findZeroCrossing ? 1 : -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double threshold, int frameStart, int frameEnd)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, threshold, threshold, frameStart, frameEnd, -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double threshold, int frameStart, int frameEnd, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, threshold, threshold, frameStart, frameEnd, findZeroCrossing ? 1 : -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double thresholdIn, double thresholdOut, int frameStart, int frameEnd)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, frameStart, frameEnd, -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		public bool GetCuePoints(ref double startpos, ref double endpos, double thresholdIn, double thresholdOut, int frameStart, int frameEnd, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return false;
			}
			int frame = 0;
			int frame2 = this._waveBuffer.data.Length - 1;
			this.DetectSilence(ref frame, ref frame2, thresholdIn, thresholdOut, frameStart, frameEnd, findZeroCrossing ? 1 : -1);
			startpos = this.Frame2Seconds(frame);
			endpos = this.Frame2Seconds(frame2);
			return true;
		}

		private void DetectSilence(ref int startpos, ref int endpos, double dBIn, double dBOut, int frameStart, int frameEnd, int findZeroCrossing)
		{
			if (dBIn > 0.0)
			{
				dBIn = 0.0;
			}
			else if (dBIn < -90.0)
			{
				dBIn = -90.0;
			}
			if (dBOut > 0.0)
			{
				dBOut = 0.0;
			}
			else if (dBOut < -90.0)
			{
				dBOut = -90.0;
			}
			if (frameEnd < frameStart)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
				frameStart = 0;
			}
			if (frameEnd >= this._waveBuffer.data.Length)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameEnd < 0)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameStart < 0)
			{
				frameStart = 0;
			}
			int num = frameStart;
			int num2 = frameEnd;
			short num3 = (short)Utils.DBToLevel(dBIn, 32768);
			short num4 = (short)Utils.DBToLevel(dBOut, 32768);
			int i = num;
			while (i < frameEnd && this.PeakLevelOfFrame(i) < num3)
			{
				i++;
			}
			if (i < frameEnd)
			{
				if (findZeroCrossing == 0)
				{
					while (i > frameStart)
					{
						if (this.PeakLevelOfFrame(i) <= num3 / 2)
						{
							break;
						}
						i--;
					}
				}
				else if (findZeroCrossing == 1)
				{
					while (i > frameStart && !this.IsZeroCrossingFrame(i, i + 1))
					{
						i--;
					}
				}
			}
			num = i;
			i = num2;
			while (i > frameStart && this.PeakLevelOfFrame(i) < num4)
			{
				i--;
			}
			if (i > frameStart)
			{
				if (findZeroCrossing == 0)
				{
					while (i < frameEnd)
					{
						if (this.PeakLevelOfFrame(i) <= num4 / 2)
						{
							break;
						}
						i++;
					}
				}
				else if (findZeroCrossing == 1)
				{
					while (i < frameEnd && !this.IsZeroCrossingFrame(i, i - 1))
					{
						i++;
					}
				}
			}
			num2 = i;
			if (num2 <= num)
			{
				num2 = frameEnd;
			}
			startpos = num;
			endpos = num2;
		}

		public float GetNormalizationGain(int frameStart, int frameEnd, ref float peak)
		{
			float result = 1f;
			if (this._waveBuffer == null)
			{
				return result;
			}
			if (frameEnd < frameStart)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
				frameStart = 0;
			}
			if (frameEnd >= this._waveBuffer.data.Length)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameEnd < 0)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameStart < 0)
			{
				frameStart = 0;
			}
			int i = frameStart;
			short num = 0;
			while (i < frameEnd)
			{
				short num2 = this.PeakLevelOfFrame(i);
				if (num2 > num)
				{
					num = num2;
				}
				if (num == 32767)
				{
					break;
				}
				i++;
			}
			if (num < 32767 && num > 0)
			{
				result = 32767f / (float)num;
			}
			peak = (float)num / 32767f;
			return result;
		}

		public long DetectNextLevel(long startpos, double threshold, bool reverse, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return startpos;
			}
			int num = this.Position2Frames(startpos);
			int num2 = this.DetectNextLevel(num, threshold, -1, -1, reverse, findZeroCrossing ? 1 : -1);
			if (num2 == num)
			{
				return startpos;
			}
			return this.Frame2Bytes(num2);
		}

		public double DetectNextLevel(double startpos, double threshold, bool reverse, bool findZeroCrossing)
		{
			if (!this._isRendered || this._waveBuffer == null)
			{
				return startpos;
			}
			int num = this.Position2Frames(startpos);
			int num2 = this.DetectNextLevel(num, threshold, -1, -1, reverse, findZeroCrossing ? 1 : -1);
			if (num2 == num)
			{
				return startpos;
			}
			return this.Frame2Seconds(num2);
		}

		private int DetectNextLevel(int start, double dBvalue, int frameStart, int frameEnd, bool reverse, int findZeroCrossing)
		{
			if (dBvalue > 0.0)
			{
				dBvalue = 0.0;
			}
			else if (dBvalue < -90.0)
			{
				dBvalue = -90.0;
			}
			if (frameEnd < frameStart)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
				frameStart = 0;
			}
			if (frameEnd >= this._waveBuffer.data.Length)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameEnd < 0)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameStart < 0)
			{
				frameStart = 0;
			}
			int i = start;
			short num = (short)Utils.DBToLevel(dBvalue, 32768);
			if (reverse)
			{
				while (i > frameStart && this.PeakLevelOfFrame(i) < num)
				{
					i--;
				}
				if (i > frameStart)
				{
					if (findZeroCrossing == 0)
					{
						while (i < frameEnd)
						{
							if (this.PeakLevelOfFrame(i) <= num / 2)
							{
								break;
							}
							i++;
						}
					}
					else if (findZeroCrossing == 1)
					{
						while (i < frameEnd)
						{
							if (this.IsZeroCrossingFrame(i, i - 1))
							{
								break;
							}
							i++;
						}
					}
				}
				else
				{
					i = start;
				}
			}
			else
			{
				while (i < frameEnd && this.PeakLevelOfFrame(i) < num)
				{
					i++;
				}
				if (i < frameEnd)
				{
					if (findZeroCrossing == 0)
					{
						while (i > frameStart)
						{
							if (this.PeakLevelOfFrame(i) <= num / 2)
							{
								break;
							}
							i--;
						}
					}
					else if (findZeroCrossing == 1)
					{
						while (i > frameStart)
						{
							if (this.IsZeroCrossingFrame(i, i + 1))
							{
								break;
							}
							i--;
						}
					}
				}
				else
				{
					i = start;
				}
			}
			return i;
		}

		public long FindPreviousZeroCrossing(long position)
		{
			if (this._waveBuffer == null || this._waveBuffer.data == null)
			{
				return position;
			}
			int num = this.Position2Frames(position);
			int num2 = 0;
			int num3 = this._waveBuffer.data.Length - 1;
			if (num < num3)
			{
				while (num > num2 && !this.IsZeroCrossingFrame(num, num + 1))
				{
					num--;
				}
			}
			return this.Frame2Bytes(num);
		}

		public long FindNextZeroCrossing(long position)
		{
			if (this._waveBuffer == null || this._waveBuffer.data == null)
			{
				return position;
			}
			int num = this.Position2Frames(position);
			int num2 = 0;
			int num3 = this._waveBuffer.data.Length - 1;
			if (num > num2)
			{
				while (num < num3 && !this.IsZeroCrossingFrame(num, num - 1))
				{
					num++;
				}
			}
			return this.Frame2Bytes(num);
		}

		public short PeakLevelOfFrame(int pos)
		{
			short result;
			try
			{
				short left = this._waveBuffer.data[pos].left;
				short right = this._waveBuffer.data[pos].right;
				if (left == -32768)
				{
					result = short.MaxValue;
				}
				else if (right == -32768)
				{
					result = short.MaxValue;
				}
				else if (left < 0 && right < 0 && left > right)
				{
					result = Convert.ToInt16(-right);
				}
				else if (left > 0 && right > 0 && left < right)
				{
					result = right;
				}
				else if (left < 0 && left > -right)
				{
					result = right;
				}
				else if (left > 0 && left < -right)
				{
					result = right;
				}
				else
				{
					result = left;
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}

		public bool IsZeroCrossingFrame(int pos1, int pos2)
		{
			if (this._waveBuffer == null || pos1 > this._waveBuffer.data.Length - 1 || pos2 > this._waveBuffer.data.Length - 1 || pos1 < 0 || pos2 < 0)
			{
				return false;
			}
			bool result = false;
			try
			{
				WaveForm.WaveBuffer.Level level = this._waveBuffer.data[pos1];
				WaveForm.WaveBuffer.Level level2 = this._waveBuffer.data[pos2];
				if ((level.left >= 0 && level2.left <= 0) || (level.right >= 0 && level2.right <= 0) || (level.left < 0 && level2.left > 0) || (level.right < 0 && level2.right > 0))
				{
					result = true;
				}
			}
			catch
			{
			}
			return result;
		}

		private static WaveForm.WaveBuffer.Level GetLevel(byte[] buffer, int chans, int startIndex, int length)
		{
			WaveForm.WaveBuffer.Level level = default(WaveForm.WaveBuffer.Level);
			if (buffer == null)
			{
				return level;
			}
			int num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				int num2 = (int)(buffer[i] - 128) * 256;
				if (i % 2 == 0)
				{
					if (num2 < 0)
					{
						if (num2 <= -32768)
						{
							level.left = short.MinValue;
						}
						else if (num2 < (int)level.left)
						{
							level.left = (short)num2;
						}
					}
					else if (num2 >= 32767)
					{
						level.left = short.MaxValue;
					}
					else if (num2 > (int)level.left)
					{
						level.left = (short)num2;
					}
				}
				else if (num2 < 0)
				{
					if (num2 <= -32768)
					{
						level.right = short.MinValue;
					}
					else if (num2 < (int)level.right)
					{
						level.right = (short)num2;
					}
				}
				else if (num2 >= 32767)
				{
					level.right = short.MaxValue;
				}
				else if (num2 > (int)level.right)
				{
					level.right = (short)num2;
				}
			}
			if (chans == 1)
			{
				if (level.left == -32768)
				{
					level.right = level.left;
				}
				else if (level.right == -32768)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.right < 0 && level.left > level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.right > 0 && level.left < level.right)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.left > -level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.left < -level.right)
				{
					level.left = level.right;
				}
				else
				{
					level.right = level.left;
				}
			}
			return level;
		}

		private static WaveForm.WaveBuffer.Level GetLevel(short[] buffer, int chans, int startIndex, int length)
		{
			WaveForm.WaveBuffer.Level level = default(WaveForm.WaveBuffer.Level);
			if (buffer == null)
			{
				return level;
			}
			int num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				short num2 = buffer[i];
				if (i % 2 == 0)
				{
					if (num2 < 0)
					{
						if (num2 == -32768)
						{
							level.left = short.MinValue;
						}
						else if (num2 < level.left)
						{
							level.left = num2;
						}
					}
					else if (num2 == 32767)
					{
						level.left = short.MaxValue;
					}
					else if (num2 > level.left)
					{
						level.left = num2;
					}
				}
				else if (num2 < 0)
				{
					if (num2 == -32768)
					{
						level.right = short.MinValue;
					}
					else if (num2 < level.right)
					{
						level.right = num2;
					}
				}
				else if (num2 == 32767)
				{
					level.right = short.MaxValue;
				}
				else if (num2 > level.right)
				{
					level.right = num2;
				}
			}
			if (chans == 1)
			{
				if (level.left == -32768)
				{
					level.right = level.left;
				}
				else if (level.right == -32768)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.right < 0 && level.left > level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.right > 0 && level.left < level.right)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.left > -level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.left < -level.right)
				{
					level.left = level.right;
				}
				else
				{
					level.right = level.left;
				}
			}
			return level;
		}

		private static WaveForm.WaveBuffer.Level GetLevel(float[] buffer, int chans, int startIndex, int length)
		{
			WaveForm.WaveBuffer.Level level = default(WaveForm.WaveBuffer.Level);
			if (buffer == null)
			{
				return level;
			}
			int num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				float num2;
				if (buffer[i] < 0f)
				{
					num2 = (float)((int)(buffer[i] * 32768f - 0.5f));
				}
				else
				{
					num2 = (float)((int)(buffer[i] * 32768f + 0.5f));
				}
				if (i % 2 == 0)
				{
					if (num2 < 0f)
					{
						if (num2 <= -32768f)
						{
							level.left = short.MinValue;
						}
						else if (num2 < (float)level.left)
						{
							level.left = (short)num2;
						}
					}
					else if (num2 >= 32767f)
					{
						level.left = short.MaxValue;
					}
					else if (num2 > (float)level.left)
					{
						level.left = (short)num2;
					}
				}
				else if (num2 < 0f)
				{
					if (num2 <= -32768f)
					{
						level.right = short.MinValue;
					}
					else if (num2 < (float)level.right)
					{
						level.right = (short)num2;
					}
				}
				else if (num2 >= 32767f)
				{
					level.right = short.MaxValue;
				}
				else if (num2 > (float)level.right)
				{
					level.right = (short)num2;
				}
			}
			if (chans == 1)
			{
				if (level.left == -32768)
				{
					level.right = level.left;
				}
				else if (level.right == -32768)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.right < 0 && level.left > level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.right > 0 && level.left < level.right)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.left > -level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.left < -level.right)
				{
					level.left = level.right;
				}
				else
				{
					level.right = level.left;
				}
			}
			return level;
		}

		private unsafe static WaveForm.WaveBuffer.Level GetLevel(IntPtr buffer, int chans, int bps, int startIndex, int length)
		{
			WaveForm.WaveBuffer.Level level = default(WaveForm.WaveBuffer.Level);
			if (buffer == IntPtr.Zero)
			{
				return level;
			}
			if (bps == 16 || bps == 32 || bps == 8)
			{
				bps /= 8;
			}
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			int num = startIndex + length;
			if (bps == 2)
			{
				short* ptr = (short*)((void*)buffer);
				for (int i = startIndex; i < num; i++)
				{
					short num2 = ptr[i];
					if (i % 2 == 0)
					{
						if (num2 < 0)
						{
							if (num2 == -32768)
							{
								level.left = short.MinValue;
							}
							else if (num2 < level.left)
							{
								level.left = num2;
							}
						}
						else if (num2 == 32767)
						{
							level.left = short.MaxValue;
						}
						else if (num2 > level.left)
						{
							level.left = num2;
						}
					}
					else if (num2 < 0)
					{
						if (num2 == -32768)
						{
							level.right = short.MinValue;
						}
						else if (num2 < level.right)
						{
							level.right = num2;
						}
					}
					else if (num2 == 32767)
					{
						level.right = short.MaxValue;
					}
					else if (num2 > level.right)
					{
						level.right = num2;
					}
				}
			}
			else if (bps == 4)
			{
				float* ptr2 = (float*)((void*)buffer);
				for (int j = startIndex; j < num; j++)
				{
					int num3;
					if (ptr2[j] < 0f)
					{
						num3 = (int)(ptr2[j] * 32768f - 0.5f);
					}
					else
					{
						num3 = (int)(ptr2[j] * 32768f + 0.5f);
					}
					if (j % 2 == 0)
					{
						if (num3 < 0)
						{
							if (num3 <= -32768)
							{
								level.left = short.MinValue;
							}
							else if (num3 < (int)level.left)
							{
								level.left = (short)num3;
							}
						}
						else if (num3 >= 32767)
						{
							level.left = short.MaxValue;
						}
						else if (num3 > (int)level.left)
						{
							level.left = (short)num3;
						}
					}
					else if (num3 < 0)
					{
						if (num3 <= -32768)
						{
							level.right = short.MinValue;
						}
						else if (num3 < (int)level.right)
						{
							level.right = (short)num3;
						}
					}
					else if (num3 >= 32767)
					{
						level.right = short.MaxValue;
					}
					else if (num3 > (int)level.right)
					{
						level.right = (short)num3;
					}
				}
			}
			else
			{
				byte* ptr3 = (byte*)((void*)buffer);
				for (int k = startIndex; k < num; k++)
				{
					int num4 = (int)(ptr3[k] - 128) * 256;
					if (k % 2 == 0)
					{
						if (num4 < 0)
						{
							if (num4 <= -32768)
							{
								level.left = short.MinValue;
							}
							else if (num4 < (int)level.left)
							{
								level.left = (short)num4;
							}
						}
						else if (num4 >= 32767)
						{
							level.left = short.MaxValue;
						}
						else if (num4 > (int)level.left)
						{
							level.left = (short)num4;
						}
					}
					else if (num4 < 0)
					{
						if (num4 <= -32768)
						{
							level.right = short.MinValue;
						}
						else if (num4 < (int)level.right)
						{
							level.right = (short)num4;
						}
					}
					else if (num4 >= 32767)
					{
						level.right = short.MaxValue;
					}
					else if (num4 > (int)level.right)
					{
						level.right = (short)num4;
					}
				}
			}
			if (chans == 1)
			{
				if (level.left == -32768)
				{
					level.right = level.left;
				}
				else if (level.right == -32768)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.right < 0 && level.left > level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.right > 0 && level.left < level.right)
				{
					level.left = level.right;
				}
				else if (level.left < 0 && level.left > -level.right)
				{
					level.left = level.right;
				}
				else if (level.left > 0 && level.left < -level.right)
				{
					level.left = level.right;
				}
				else
				{
					level.right = level.left;
				}
			}
			return level;
		}

		private bool Render(bool background, ThreadPriority prio, BASSFlag flags, int chans)
		{
			this._isRendered = false;
			this._waveBuffer = new WaveForm.WaveBuffer();
			double num = (double)Bass.BASS_ChannelGetLength(this._decodingStream);
			if (num < 0.0)
			{
				num = 0.0;
				byte[] buffer = new byte[32768];
				int num2;
				while ((num2 = Bass.BASS_ChannelGetData(this._decodingStream, buffer, 32768)) >= 0)
				{
					num += (double)num2;
				}
				Bass.BASS_ChannelSetPosition(this._decodingStream, 0L);
			}
			this._waveBuffer.chans = chans;
			this._waveBuffer.resolution = this.FrameResolution;
			this._waveBuffer.bpf = (int)Bass.BASS_ChannelSeconds2Bytes(this._decodingStream, this._waveBuffer.resolution);
			this._waveBuffer.flags = flags;
			this._framesToRender = (int)Math.Ceiling(num / (double)this._waveBuffer.bpf);
			this._correctionFactor = Bass.BASS_ChannelBytes2Seconds(this._decodingStream, (long)this._waveBuffer.bpf) / this._waveBuffer.resolution;
			this._waveBuffer.data = new WaveForm.WaveBuffer.Level[this._framesToRender];
			this._waveBuffer.fileName = this.FileName;
			if (this.DetectBeats)
			{
				this._waveBuffer.beats = new List<long>((int)(Bass.BASS_ChannelBytes2Seconds(this._decodingStream, (long)num) / 60.0 * 120.0));
			}
			this._renderStartTime = DateTime.Now;
			bool result;
			if (background)
			{
				if (prio == ThreadPriority.Normal)
				{
					Task.Factory.StartNew(delegate()
					{
						try
						{
							if (this._useSimpleScan)
							{
								this.ScanPeaksSimple();
							}
							else
							{
								this.ScanPeaks();
							}
						}
						catch
						{
						}
					});
				}
				else if (this._useSimpleScan)
				{
					new Thread(new ThreadStart(this.ScanPeaksSimple))
					{
						Priority = prio
					}.Start();
				}
				else
				{
					new Thread(new ThreadStart(this.ScanPeaks))
					{
						Priority = prio
					}.Start();
				}
				result = true;
			}
			else
			{
				if (this._useSimpleScan)
				{
					this.ScanPeaksSimple();
				}
				else
				{
					this.ScanPeaks();
				}
				result = true;
			}
			return result;
		}

		private void ScanPeaks()
		{
			if (this._waveBuffer == null)
			{
				return;
			}
			object obj = null;
			if (this.DetectBeats)
			{
				obj = new BPMBEATPROC(this.BpmBeatCallback);
				BassFx.BASS_FX_BPM_BeatCallbackSet(this._decodingStream, (BPMBEATPROC)obj, IntPtr.Zero);
			}
			this._renderingInProgress = true;
			this._framesRendered = 0;
			WaveForm.WaveBuffer.Level level = default(WaveForm.WaveBuffer.Level);
			int num = 0;
			int bpf = this._waveBuffer.bpf;
			int num2 = (int)Bass.BASS_ChannelSeconds2Bytes(this._decodingStream, 1.0) / this._waveBuffer.bpf * this._waveBuffer.bpf;
			int num3 = num2 / this._waveBuffer.bpf;
			int bps = this._waveBuffer.bps;
			int num4 = num2 / bps;
			int num5 = this._waveBuffer.bpf / bps;
			try
			{
				while (!this._killScan)
				{
					if (Bass.BASS_ChannelIsActive(this._decodingStream) == BASSActive.BASS_ACTIVE_STOPPED)
					{
						num = this._framesToRender;
						this._framesRendered = this._framesToRender;
						this._killScan = true;
					}
					else if (bps == 2)
					{
						if (this._peakLevelShort == null || this._peakLevelShort.Length < num4)
						{
							this._peakLevelShort = new short[num4];
						}
						int num6 = 0;
						num4 = Bass.BASS_ChannelGetData(this._decodingStream, this._peakLevelShort, num2) / bps;
						for (int i = 0; i < num3; i++)
						{
							if (num6 + num5 > num4)
							{
								level = WaveForm.GetLevel(this._peakLevelShort, this._waveBuffer.chans, num6, num4 - num6);
							}
							else
							{
								level = WaveForm.GetLevel(this._peakLevelShort, this._waveBuffer.chans, num6, num5);
							}
							num6 += num5;
							if (num < this._framesToRender)
							{
								this._waveBuffer.data[num] = level;
								num++;
								this._framesRendered = num;
							}
							if (this.CallbackFrequency > 0 && this._framesRendered % this.CallbackFrequency == 0)
							{
								this.InvokeCallback(false);
							}
							if (num6 > num4)
							{
								break;
							}
						}
					}
					else if (bps == 4)
					{
						if (this._peakLevelFloat == null || this._peakLevelFloat.Length < num4)
						{
							this._peakLevelFloat = new float[num4];
						}
						int num6 = 0;
						num4 = Bass.BASS_ChannelGetData(this._decodingStream, this._peakLevelFloat, num2) / bps;
						for (int j = 0; j < num3; j++)
						{
							if (num6 + num5 > num4)
							{
								level = WaveForm.GetLevel(this._peakLevelFloat, this._waveBuffer.chans, num6, num4 - num6);
							}
							else
							{
								level = WaveForm.GetLevel(this._peakLevelFloat, this._waveBuffer.chans, num6, num5);
							}
							num6 += num5;
							if (num < this._framesToRender)
							{
								this._waveBuffer.data[num] = level;
								num++;
								this._framesRendered = num;
							}
							if (this.CallbackFrequency > 0 && this._framesRendered % this.CallbackFrequency == 0)
							{
								this.InvokeCallback(false);
							}
							if (num6 > num4)
							{
								break;
							}
						}
					}
					else
					{
						if (this._peakLevelByte == null || this._peakLevelByte.Length < num4)
						{
							this._peakLevelByte = new byte[num4];
						}
						int num6 = 0;
						num4 = Bass.BASS_ChannelGetData(this._decodingStream, this._peakLevelByte, num2) / bps;
						for (int k = 0; k < num3; k++)
						{
							if (num6 + num5 > num4)
							{
								level = WaveForm.GetLevel(this._peakLevelByte, this._waveBuffer.chans, num6, num4 - num6);
							}
							else
							{
								level = WaveForm.GetLevel(this._peakLevelByte, this._waveBuffer.chans, num6, num5);
							}
							num6 += num5;
							if (num < this._framesToRender)
							{
								this._waveBuffer.data[num] = level;
								num++;
								this._framesRendered = num;
							}
							if (this.CallbackFrequency > 0 && this._framesRendered % this.CallbackFrequency == 0)
							{
								this.InvokeCallback(false);
							}
							if (num6 > num4)
							{
								break;
							}
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				if (obj != null)
				{
					BassFx.BASS_FX_BPM_BeatFree(this._decodingStream);
					obj = null;
				}
			}
			catch
			{
			}
			if (this._freeStream)
			{
				Bass.BASS_StreamFree(this._decodingStream);
			}
			this._isRendered = true;
			this._renderingInProgress = false;
			try
			{
				this.InvokeCallback(true);
			}
			catch
			{
			}
		}

		private void ScanPeaksSimple()
		{
			if (this._waveBuffer == null)
			{
				return;
			}
			object obj = null;
			if (this.DetectBeats)
			{
				obj = new BPMBEATPROC(this.BpmBeatCallback);
				BassFx.BASS_FX_BPM_BeatCallbackSet(this._decodingStream, (BPMBEATPROC)obj, IntPtr.Zero);
			}
			this._renderingInProgress = true;
			this._framesRendered = 0;
			int num = 0;
			try
			{
				while (!this._killScan)
				{
					int dWord = Bass.BASS_ChannelGetLevel(this._decodingStream);
					if (Bass.BASS_ChannelIsActive(this._decodingStream) == BASSActive.BASS_ACTIVE_STOPPED)
					{
						num = this._framesToRender;
						this._framesRendered = this._framesToRender;
						this._killScan = true;
					}
					else
					{
						this._waveBuffer.data[num] = new WaveForm.WaveBuffer.Level(Utils.LowWord(dWord), Utils.HighWord(dWord));
						num++;
						this._framesRendered = num;
						if (this.CallbackFrequency > 0 && this._framesRendered % this.CallbackFrequency == 0)
						{
							this.InvokeCallback(false);
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				if (obj != null)
				{
					BassFx.BASS_FX_BPM_BeatFree(this._decodingStream);
					obj = null;
				}
			}
			catch
			{
			}
			if (this._freeStream)
			{
				Bass.BASS_StreamFree(this._decodingStream);
			}
			this._isRendered = true;
			this._renderingInProgress = false;
			try
			{
				this.InvokeCallback(true);
			}
			catch
			{
			}
		}

		private void BpmBeatCallback(int handle, double beatpos, IntPtr user)
		{
			try
			{
				this._waveBuffer.beats.Add(Bass.BASS_ChannelSeconds2Bytes(this._decodingStream, beatpos));
			}
			catch
			{
			}
		}

		private void InvokeCallback(bool finished)
		{
			if (this.NotifyHandler != null)
			{
				TimeSpan timeSpan = DateTime.Now - this._renderStartTime;
				if (this.WinControl != null)
				{
					this.WinControl.BeginInvoke(this.NotifyHandler, new object[]
					{
						this._framesRendered,
						this._framesToRender,
						timeSpan,
						finished
					});
					return;
				}
				this.NotifyHandler(this._framesRendered, this._framesToRender, timeSpan, finished);
			}
		}

		private void DrawBitmap(Graphics g, int width, int height, int frameStart, int frameEnd, Pen pMiddleL, Pen pMiddleR, Pen pLeft, Pen pLeftEnv, Pen pRight, Pen pRightEnv, Pen pMarker, Pen pBeat, Pen pVolume)
		{
			WaveForm.WAVEFORMDRAWTYPE waveformdrawtype = this._drawWaveForm;
			if (this._waveBuffer.chans == 1 && (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.DualMono))
			{
				waveformdrawtype = WaveForm.WAVEFORMDRAWTYPE.Mono;
			}
			if (frameEnd >= this._waveBuffer.data.Length)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameEnd < 0)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameStart < 0)
			{
				frameStart = 0;
			}
			short num = 0;
			short num2 = 0;
			short num3 = 0;
			short num4 = 0;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = -1f;
			float num8 = -1f;
			double num9 = (double)(frameEnd - frameStart) / (double)width;
			float num10 = (float)height / 2f - 1f;
			float num11 = ((float)height - 2f) / 4f - 1f;
			float num12 = ((float)height - 1f) / 4f;
			float num13 = (float)height - num12 - 1f;
			float num14 = num12;
			float num15 = num13;
			float num16 = num12;
			float num17 = num13;
			if (waveformdrawtype != WaveForm.WAVEFORMDRAWTYPE.Stereo)
			{
				num11 = ((float)height - 3f) / 2f - 1f;
				num13 = (num12 = num10);
				num14 = num12;
				num15 = num13;
				num16 = num12;
				num17 = num13;
			}
			height--;
			if (frameStart > 0)
			{
				num16 = (num14 = num12 - (float)this._waveBuffer.data[frameStart - 1].left * (num11 / 32767f) * this._gainFactor);
				num17 = (num15 = num13 - (float)this._waveBuffer.data[frameStart - 1].right * (num11 / 32767f) * this._gainFactor);
				if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
				{
					num15 = (num14 = (num14 + num15) / 2f);
				}
				else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
				{
					num14 = (num14 + num15) / 2f;
					if (num14 <= num12)
					{
						num15 = (num14 = 2f * num14);
					}
					else
					{
						num15 = (num14 = 2f * (2f * num12 - num14));
					}
				}
			}
			g.Clear(this._colorBackground);
			for (int i = frameStart; i <= frameEnd; i++)
			{
				if (this._waveBuffer.data[i].left > num)
				{
					num = this._waveBuffer.data[i].left;
				}
				if (this._waveBuffer.data[i].right > num2)
				{
					num2 = this._waveBuffer.data[i].right;
				}
				if (this._waveBuffer.data[i].left < num3)
				{
					num3 = this._waveBuffer.data[i].left;
				}
				if (this._waveBuffer.data[i].right < num4)
				{
					num4 = this._waveBuffer.data[i].right;
				}
				num7 = (float)((int)((double)(i - frameStart) / num9 + 0.5));
				if (num7 > num8)
				{
					float num18;
					float num19;
					if (num7 - num8 > 1f)
					{
						if (num3 == -32768)
						{
							num = num3;
						}
						else if (num < 0 && num3 < 0 && num > num3)
						{
							num = num3;
						}
						else if (num > 0 && num3 > 0 && num < num3)
						{
							num = num3;
						}
						else if (num < 0 && num > -num3)
						{
							num = num3;
						}
						else if (num > 0 && num < -num3)
						{
							num = num3;
						}
						if (num4 == -32768)
						{
							num2 = num4;
						}
						else if (num2 < 0 && num4 < 0 && num2 > num4)
						{
							num2 = num4;
						}
						else if (num2 > 0 && num4 > 0 && num2 < num4)
						{
							num2 = num4;
						}
						else if (num2 < 0 && num2 > -num4)
						{
							num2 = num4;
						}
						else if (num2 > 0 && num2 < -num4)
						{
							num2 = num4;
						}
						num5 = num12 - (float)num * (num11 / 32767f) * this._gainFactor;
						num6 = num13 - (float)num2 * (num11 / 32767f) * this._gainFactor;
						num18 = num12 - (float)num3 * (num11 / 32767f) * this._gainFactor;
						num19 = num13 - (float)num4 * (num11 / 32767f) * this._gainFactor;
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							num19 = (num18 = (num18 + num19) / 2f);
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							if (num5 <= num12)
							{
								num6 = (num5 = 2f * num5);
							}
							else
							{
								num6 = (num5 = 2f * (2f * num12 - num5));
							}
							num18 = (num18 + num19) / 2f;
							if (num18 <= num12)
							{
								num19 = (num18 = 2f * num18);
							}
							else
							{
								num19 = (num18 = 2f * (2f * num12 - num18));
							}
						}
						else
						{
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						float num20 = (num5 - num14) / (num7 - num8);
						float num21 = (num6 - num15) / (num7 - num8);
						float num22 = (num18 - num16) / (num7 - num8);
						float num23 = (num19 - num17) / (num7 - num8);
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo)
						{
							int num24 = 0;
							while ((float)num24 < num7 - num8)
							{
								if (num16 + (float)num24 * num22 < num12)
								{
									g.DrawLine(pLeft, num8 + (float)num24, num12, num8 + (float)num24, num14 + (float)num24 * num20);
								}
								else if (num14 + (float)num24 * num20 > num12)
								{
									g.DrawLine(pLeft, num8 + (float)num24, num12, num8 + (float)num24, num16 + (float)num24 * num22);
								}
								else
								{
									g.DrawLine(pLeft, num8 + (float)num24, num16 + (float)num24 * num22, num8 + (float)num24, num14 + (float)num24 * num20);
								}
								if (num17 + (float)num24 * num23 < num13)
								{
									g.DrawLine(pRight, num8 + (float)num24, num13, num8 + (float)num24, num15 + (float)num24 * num21);
								}
								else if (num15 + (float)num24 * num21 > num13)
								{
									g.DrawLine(pRight, num8 + (float)num24, num13, num8 + (float)num24, num17 + (float)num24 * num23);
								}
								else
								{
									g.DrawLine(pRight, num8 + (float)num24, num17 + (float)num24 * num23, num8 + (float)num24, num15 + (float)num24 * num21);
								}
								num24++;
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							int num25 = 0;
							while ((float)num25 < num7 - num8)
							{
								if (num16 + (float)num25 * num22 < num12)
								{
									g.DrawLine(pLeft, num8 + (float)num25, num12, num8 + (float)num25, num14 + (float)num25 * num20);
								}
								else if (num14 + (float)num25 * num20 > num12)
								{
									g.DrawLine(pLeft, num8 + (float)num25, num12, num8 + (float)num25, num16 + (float)num25 * num22);
								}
								else
								{
									g.DrawLine(pLeft, num8 + (float)num25, num16 + (float)num25 * num22, num8 + (float)num25, num14 + (float)num25 * num20);
								}
								num25++;
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono)
						{
							int num26 = 0;
							while ((float)num26 < num7 - num8)
							{
								if (num14 + (float)num26 * num20 < num16 + (float)num26 * num22)
								{
									g.DrawLine(pLeft, num8 + (float)num26, (float)height, num8 + (float)num26, num14 + (float)num26 * num20);
								}
								else
								{
									g.DrawLine(pLeft, num8 + (float)num26, (float)height, num8 + (float)num26, num16 + (float)num26 * num22);
								}
								num26++;
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								}
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							int num27 = 0;
							while ((float)num27 < num7 - num8)
							{
								if (num14 + (float)num27 * num20 < num16 + (float)num27 * num22)
								{
									g.DrawLine(pLeft, num8 + (float)num27, 0f, num8 + (float)num27, (float)height - (num14 + (float)num27 * num20));
								}
								else
								{
									g.DrawLine(pLeft, num8 + (float)num27, 0f, num8 + (float)num27, (float)height - (num16 + (float)num27 * num22));
								}
								num27++;
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num14, num7, (float)height - num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num16, num7, (float)height - num18);
								}
							}
						}
						else
						{
							int num28 = 0;
							while ((float)num28 < num7 - num8)
							{
								if (num16 + (float)num28 * num22 < num12)
								{
									g.DrawLine(pLeft, num8 + (float)num28, num12, num8 + (float)num28, num14 + (float)num28 * num20);
								}
								else if (num14 + (float)num28 * num20 > num12)
								{
									g.DrawLine(pLeft, num8 + (float)num28, num12, num8 + (float)num28, num16 + (float)num28 * num22);
								}
								else
								{
									g.DrawLine(pLeft, num8 + (float)num28, num16 + (float)num28 * num22, num8 + (float)num28, num14 + (float)num28 * num20);
								}
								if (num17 + (float)num28 * num23 < num13)
								{
									g.DrawLine(pRight, num8 + (float)num28, num13, num8 + (float)num28, num15 + (float)num28 * num21);
								}
								else if (num15 + (float)num28 * num21 > num13)
								{
									g.DrawLine(pRight, num8 + (float)num28, num13, num8 + (float)num28, num17 + (float)num28 * num23);
								}
								else
								{
									g.DrawLine(pRight, num8 + (float)num28, num17 + (float)num28 * num23, num8 + (float)num28, num15 + (float)num28 * num21);
								}
								num28++;
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
					}
					else
					{
						num5 = num12 - (float)num * (num11 / 32767f) * this._gainFactor;
						num6 = num13 - (float)num2 * (num11 / 32767f) * this._gainFactor;
						num18 = num12 - (float)num3 * (num11 / 32767f) * this._gainFactor;
						num19 = num13 - (float)num4 * (num11 / 32767f) * this._gainFactor;
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							num19 = (num18 = (num18 + num19) / 2f);
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							if (num5 <= num12)
							{
								num6 = (num5 = 2f * num5);
							}
							else
							{
								num6 = (num5 = 2f * (2f * num12 - num5));
							}
							num18 = (num18 + num19) / 2f;
							if (num18 <= num12)
							{
								num19 = (num18 = 2f * num18);
							}
							else
							{
								num19 = (num18 = 2f * (2f * num12 - num18));
							}
						}
						else
						{
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo)
						{
							if (num18 < num12)
							{
								g.DrawLine(pLeft, num8, num12, num8, num5);
							}
							else if (num5 > num12)
							{
								g.DrawLine(pLeft, num8, num12, num8, num18);
							}
							else
							{
								g.DrawLine(pLeft, num8, num18, num8, num5);
							}
							if (num19 < num13)
							{
								g.DrawLine(pRight, num8, num13, num8, num6);
							}
							else if (num6 > num13)
							{
								g.DrawLine(pRight, num8, num13, num8, num19);
							}
							else
							{
								g.DrawLine(pRight, num8, num19, num8, num6);
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							if (num18 < num12)
							{
								g.DrawLine(pLeft, num8, num12, num8, num5);
							}
							else if (num5 > num12)
							{
								g.DrawLine(pLeft, num8, num12, num8, num18);
							}
							else
							{
								g.DrawLine(pLeft, num8, num18, num8, num5);
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono)
						{
							if (num5 < num18)
							{
								g.DrawLine(pLeft, num8, (float)height, num8, num5);
							}
							else
							{
								g.DrawLine(pLeft, num8, (float)height, num8, num18);
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								}
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							if (num5 < num18)
							{
								g.DrawLine(pLeft, num8, 0f, num8, (float)height - num5);
							}
							else
							{
								g.DrawLine(pLeft, num8, 0f, num8, (float)height - num18);
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num14, num7, (float)height - num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num16, num7, (float)height - num18);
								}
							}
						}
						else
						{
							if (num18 < num12)
							{
								g.DrawLine(pLeft, num8, num12, num8, num5);
							}
							else if (num5 > num12)
							{
								g.DrawLine(pLeft, num8, num12, num8, num18);
							}
							else
							{
								g.DrawLine(pLeft, num8, num18, num8, num5);
							}
							if (num19 < num13)
							{
								g.DrawLine(pRight, num8, num13, num8, num6);
							}
							else if (num6 > num13)
							{
								g.DrawLine(pRight, num8, num13, num8, num19);
							}
							else
							{
								g.DrawLine(pRight, num8, num19, num8, num6);
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
					}
					num8 = num7;
					num = 0;
					num2 = 0;
					num3 = 0;
					num4 = 0;
					num14 = num5;
					num15 = num6;
					num16 = num18;
					num17 = num19;
				}
			}
			if (this._drawCenterLine && waveformdrawtype != WaveForm.WAVEFORMDRAWTYPE.HalfMono && waveformdrawtype != WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
			{
				g.DrawLine(pMiddleL, 0f, num12, (float)width, num12);
				if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo)
				{
					g.DrawLine(pMiddleR, 0f, num13, (float)width, num13);
				}
			}
			long num29 = this.Frame2Bytes(frameStart);
			long num30 = this.Frame2Bytes(frameEnd);
			double num31 = (double)(num30 - num29) / (double)width;
			if (this.DrawBeat != WaveForm.BEATDRAWTYPE.None && this._waveBuffer.beats != null && this._waveBuffer.beats.Count > 0)
			{
				float num32 = (float)(height + 1) * this._beatLength;
				int num33 = this._waveBuffer.beats.BinarySearch((long)frameStart * (long)this._waveBuffer.bpf);
				if (num33 < 0)
				{
					num33 = ~num33;
				}
				for (int j = num33; j < this._waveBuffer.beats.Count; j++)
				{
					long num34 = (long)((double)this._waveBuffer.beats[j] / this._syncFactor);
					if (num34 >= num29 && num34 <= num30)
					{
						num7 = (float)((int)((double)(num34 - num29) / num31 + 0.5));
						if (this.DrawBeat == WaveForm.BEATDRAWTYPE.Middle)
						{
							g.DrawLine(pBeat, num7, num10 - num32, num7, num10 + num32);
						}
						else if (this.DrawBeat == WaveForm.BEATDRAWTYPE.Top)
						{
							g.DrawLine(pBeat, num7, -1f, num7, num32);
						}
						else if (this.DrawBeat == WaveForm.BEATDRAWTYPE.Bottom)
						{
							g.DrawLine(pBeat, num7, (float)height - num32, num7, (float)height);
						}
						else if (this.DrawBeat == WaveForm.BEATDRAWTYPE.TopBottom)
						{
							g.DrawLine(pBeat, num7, -1f, num7, num32);
							g.DrawLine(pBeat, num7, (float)height - num32, num7, (float)height);
						}
					}
					else if (num34 > num30)
					{
						break;
					}
				}
			}
			if (this.DrawMarker != WaveForm.MARKERDRAWTYPE.None && this._waveBuffer.marker != null && this._waveBuffer.marker.Count > 0)
			{
				float num35 = (float)(height + 1) * this._markerLength;
				bool flag = false;
				int num36 = 0;
				long num37 = 0L;
				string a = "Auto";
				List<long> list = null;
				using (Brush brush = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? this._colorBackground : pMarker.Color))
				{
					using (Brush brush2 = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? pMarker.Color : this._colorBackground))
					{
						SizeF sizeF = g.MeasureString("M", this._markerFont);
						num5 = num10 - sizeF.Height / 2f;
						num6 = num5;
						if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NamePositionTop) != WaveForm.MARKERDRAWTYPE.None)
						{
							num5 -= num35 - sizeF.Height / 2f + 1f;
							num6 = num5;
						}
						else if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NamePositionBottom) != WaveForm.MARKERDRAWTYPE.None)
						{
							num5 += num35 - sizeF.Height / 2f + 1f;
							num6 = num5;
						}
						else if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NamePositionMiddle) == WaveForm.MARKERDRAWTYPE.None)
						{
							num5 -= num35 - sizeF.Height / 2f + 1f;
							num6 += num35 - sizeF.Height / 2f + 1f;
							list = new List<long>(this._waveBuffer.marker.Values);
							list.Sort();
							flag = true;
						}
						foreach (string text in this._waveBuffer.marker.Keys)
						{
							num37 = this.Position2Playback(this._waveBuffer.marker[text]);
							num7 = (float)((int)((double)(num37 - num29) / num31 + 0.5));
							if (flag)
							{
								num36 = list.IndexOf(this._waveBuffer.marker[text]) % 2;
							}
							string text2 = text;
							int num38 = text2.IndexOf('{');
							if (num38 > 0)
							{
								text2 = text2.Substring(0, num38);
							}
							Color color = Color.Empty;
							int num39 = text.LastIndexOf("{Color=");
							if (num39 > 0 && text.EndsWith("}"))
							{
								try
								{
									int num40 = text.IndexOf('}', num39);
									string text3 = text.Substring(num39 + 7, num40 - num39 - 7);
									if (text3.StartsWith("0x"))
									{
										color = Color.FromArgb(int.Parse(text3.Substring(2), NumberStyles.HexNumber));
									}
									else
									{
										color = Color.FromName(text3);
									}
								}
								catch
								{
								}
							}
							int num41 = text.LastIndexOf("{Length=");
							if (num41 > 0 && text.EndsWith("}"))
							{
								int num42 = text.IndexOf('}', num41);
								long num43 = 0L;
								if (long.TryParse(text.Substring(num41 + 8, num42 - num41 - 8), out num43) && num43 > 0L)
								{
									long num44 = num37 + this.Position2Playback(num43);
									if (num44 >= num29 && num37 <= num30)
									{
										if (num44 > num30)
										{
											num44 = num30;
										}
										num8 = (float)((int)((double)(num44 - num29) / num31 + 0.5));
										using (Brush brush3 = new SolidBrush(Color.FromArgb(128, (color == Color.Empty) ? pMarker.Color : color)))
										{
											if (num36 == 0)
											{
												g.FillRectangle(brush3, (num37 < num29) ? 0f : num7, num10 - 9f, num8 - ((num37 < num29) ? 0f : num7), 8f);
											}
											else
											{
												g.FillRectangle(brush3, (num37 < num29) ? 0f : num7, num10 + 1f, num8 - ((num37 < num29) ? 0f : num7), 8f);
											}
										}
									}
								}
							}
							int num45 = text.LastIndexOf("{Align=");
							if (num45 > 0 && text.EndsWith("}"))
							{
								int num46 = text.IndexOf('}', num45);
								a = text.Substring(num45 + 7, num46 - num45 - 7);
							}
							if (num37 >= num29 && num37 <= num30)
							{
								if (color != Color.Empty)
								{
									using (Pen pen = new Pen(color))
									{
										using (Brush brush4 = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? this._colorBackground : color))
										{
											using (Brush brush5 = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? color : this._colorBackground))
											{
												if ((this._drawMarker & WaveForm.MARKERDRAWTYPE.Line) > WaveForm.MARKERDRAWTYPE.None)
												{
													g.DrawLine(pen, num7, num10 - num35, num7, num10 + num35);
												}
												if ((this._drawMarker & WaveForm.MARKERDRAWTYPE.Name) > WaveForm.MARKERDRAWTYPE.None)
												{
													sizeF = g.MeasureString(text2, this._markerFont);
													if (!(a == "Right"))
													{
														if (!(a == "Center"))
														{
															if (a == "Auto")
															{
																if (num7 + sizeF.Width > (float)width)
																{
																	num7 -= sizeF.Width;
																}
															}
														}
														else
														{
															num7 -= sizeF.Width / 2f;
														}
													}
													else
													{
														num7 -= sizeF.Width;
													}
													if ((this._drawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFilled) != WaveForm.MARKERDRAWTYPE.None || (this._drawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None)
													{
														if (num36 == 0)
														{
															g.FillRectangle(brush5, num7, num5, sizeF.Width, sizeF.Height);
															g.DrawRectangle(pen, num7, num5, sizeF.Width, sizeF.Height);
														}
														else
														{
															g.FillRectangle(brush5, num7, num6, sizeF.Width, sizeF.Height);
															g.DrawRectangle(pen, num7, num6, sizeF.Width, sizeF.Height);
														}
													}
													else
													{
														num7 -= 3f;
													}
													if (num36 == 0)
													{
														g.DrawString(text2, this._markerFont, brush4, num7, num5);
													}
													else
													{
														g.DrawString(text2, this._markerFont, brush4, num7, num6);
													}
												}
												continue;
											}
										}
									}
								}
								if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.Line) > WaveForm.MARKERDRAWTYPE.None)
								{
									g.DrawLine(pMarker, num7, num10 - num35, num7, num10 + num35);
								}
								if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.Name) > WaveForm.MARKERDRAWTYPE.None)
								{
									sizeF = g.MeasureString(text2, this._markerFont);
									if (!(a == "Right"))
									{
										if (!(a == "Center"))
										{
											if (a == "Auto")
											{
												if (num7 + sizeF.Width > (float)width)
												{
													num7 -= sizeF.Width;
												}
											}
										}
										else
										{
											num7 -= sizeF.Width / 2f;
										}
									}
									else
									{
										num7 -= sizeF.Width;
									}
									if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFilled) != WaveForm.MARKERDRAWTYPE.None || (this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None)
									{
										if (num36 == 0)
										{
											g.FillRectangle(brush2, num7, num5, sizeF.Width, sizeF.Height);
											g.DrawRectangle(pMarker, num7, num5, sizeF.Width, sizeF.Height);
										}
										else
										{
											g.FillRectangle(brush2, num7, num6, sizeF.Width, sizeF.Height);
											g.DrawRectangle(pMarker, num7, num6, sizeF.Width, sizeF.Height);
										}
									}
									else
									{
										num7 -= 3f;
									}
									if (num36 == 0)
									{
										g.DrawString(text2, this._markerFont, brush, num7, num5);
									}
									else
									{
										g.DrawString(text2, this._markerFont, brush, num7, num6);
									}
								}
							}
						}
					}
				}
			}
			if (this._drawVolume != WaveForm.VOLUMEDRAWTYPE.None && this._volumePoints != null && this._volumePoints.Count > 0)
			{
				long num47 = 0L;
				num5 = this.GetVolumeLevel(num29, false, ref num47, ref num8);
				num6 = this.GetVolumeLevel(num30, false, ref num47, ref num8);
				int num48 = this.SearchVolumePoint(num29);
				if (num48 < 0)
				{
					num48 = ~num48;
				}
				num7 = (num8 = 0f);
				num16 = (float)height - (float)height * num5;
				float num18;
				for (int k = num48; k < this._volumePoints.Count; k++)
				{
					WaveForm.VolumePoint volumePoint = this._volumePoints[k];
					num47 = this.Position2Playback(volumePoint.Position);
					if (num47 >= num29 && num47 <= num30)
					{
						num8 = (float)((int)((double)(num47 - num29) / num31 + 0.5));
						num18 = (float)height - (float)height * volumePoint.Level;
						g.DrawLine(pVolume, num7, num16, num8, num18);
						if ((this._drawVolume & WaveForm.VOLUMEDRAWTYPE.NoPoints) == WaveForm.VOLUMEDRAWTYPE.None)
						{
							g.DrawRectangle(pVolume, num8 - 1f, num18 - 1f, 3f, 3f);
							g.DrawRectangle(pVolume, num8 - 3f, num18 - 3f, 7f, 7f);
						}
						num7 = num8;
						num16 = num18;
					}
					else if (num47 > num30)
					{
						break;
					}
				}
				num8 = (float)width;
				num18 = (float)height - (float)height * num6;
				g.DrawLine(pVolume, num7, num16, num8, num18);
				return;
			}
			if (this._drawVolume != WaveForm.VOLUMEDRAWTYPE.None)
			{
				float num18 = this.VolumeCurveZeroLevel ? ((float)height) : 0f;
				g.DrawLine(pVolume, 0f, num18, (float)width, num18);
			}
		}

		private void DrawBitmap(Graphics g, int width, int height, int frameStart, int frameEnd, Pen pMiddleL, Pen pMiddleR, Pen pLeftUpper, Pen pLeftLower, Pen pLeftEnv, Pen pRightUpper, Pen pRightLower, Pen pRightEnv, Pen pMarker, Pen pBeat, Pen pVolume)
		{
			WaveForm.WAVEFORMDRAWTYPE waveformdrawtype = this._drawWaveForm;
			if (this._waveBuffer.chans == 1 && (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.DualMono))
			{
				waveformdrawtype = WaveForm.WAVEFORMDRAWTYPE.Mono;
			}
			if (frameEnd >= this._waveBuffer.data.Length)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameEnd < 0)
			{
				frameEnd = this._waveBuffer.data.Length - 1;
			}
			if (frameStart < 0)
			{
				frameStart = 0;
			}
			short num = 0;
			short num2 = 0;
			short num3 = 0;
			short num4 = 0;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = -1f;
			float num8 = -1f;
			double num9 = (double)(frameEnd - frameStart) / (double)width;
			float num10 = (float)height / 2f - 1f;
			float num11 = ((float)height - 2f) / 4f - 1f;
			float num12 = ((float)height - 1f) / 4f;
			float num13 = (float)height - num12 - 1f;
			float num14 = num12;
			float num15 = num13;
			float num16 = num12;
			float num17 = num13;
			if (waveformdrawtype != WaveForm.WAVEFORMDRAWTYPE.Stereo)
			{
				num11 = ((float)height - 3f) / 2f - 1f;
				num13 = (num12 = num10);
				num14 = num12;
				num15 = num13;
				num16 = num12;
				num17 = num13;
			}
			height--;
			if (frameStart > 0)
			{
				num16 = (num14 = num12 - (float)this._waveBuffer.data[frameStart - 1].left * (num11 / 32767f) * this._gainFactor);
				num17 = (num15 = num13 - (float)this._waveBuffer.data[frameStart - 1].right * (num11 / 32767f) * this._gainFactor);
				if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
				{
					num15 = (num14 = (num14 + num15) / 2f);
				}
				else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
				{
					num14 = (num14 + num15) / 2f;
					if (num14 <= num12)
					{
						num15 = (num14 = 2f * num14);
					}
					else
					{
						num15 = (num14 = 2f * (2f * num12 - num14));
					}
				}
			}
			g.Clear(this._colorBackground);
			for (int i = frameStart; i <= frameEnd; i++)
			{
				if (this._waveBuffer.data[i].left > num)
				{
					num = this._waveBuffer.data[i].left;
				}
				if (this._waveBuffer.data[i].right > num2)
				{
					num2 = this._waveBuffer.data[i].right;
				}
				if (this._waveBuffer.data[i].left < num3)
				{
					num3 = this._waveBuffer.data[i].left;
				}
				if (this._waveBuffer.data[i].right < num4)
				{
					num4 = this._waveBuffer.data[i].right;
				}
				num7 = (float)((int)((double)(i - frameStart) / num9 + 0.5));
				if (num7 > num8)
				{
					float num18;
					float num19;
					if (num7 - num8 > 1f)
					{
						if (num3 == -32768)
						{
							num = num3;
						}
						else if (num < 0 && num3 < 0 && num > num3)
						{
							num = num3;
						}
						else if (num > 0 && num3 > 0 && num < num3)
						{
							num = num3;
						}
						else if (num < 0 && num > -num3)
						{
							num = num3;
						}
						else if (num > 0 && num < -num3)
						{
							num = num3;
						}
						if (num4 == -32768)
						{
							num2 = num4;
						}
						else if (num2 < 0 && num4 < 0 && num2 > num4)
						{
							num2 = num4;
						}
						else if (num2 > 0 && num4 > 0 && num2 < num4)
						{
							num2 = num4;
						}
						else if (num2 < 0 && num2 > -num4)
						{
							num2 = num4;
						}
						else if (num2 > 0 && num2 < -num4)
						{
							num2 = num4;
						}
						num5 = num12 - (float)num * (num11 / 32767f) * this._gainFactor;
						num6 = num13 - (float)num2 * (num11 / 32767f) * this._gainFactor;
						num18 = num12 - (float)num3 * (num11 / 32767f) * this._gainFactor;
						num19 = num13 - (float)num4 * (num11 / 32767f) * this._gainFactor;
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							num19 = (num18 = (num18 + num19) / 2f);
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							if (num5 <= num12)
							{
								num6 = (num5 = 2f * num5);
							}
							else
							{
								num6 = (num5 = 2f * (2f * num12 - num5));
							}
							num18 = (num18 + num19) / 2f;
							if (num18 <= num12)
							{
								num19 = (num18 = 2f * num18);
							}
							else
							{
								num19 = (num18 = 2f * (2f * num12 - num18));
							}
						}
						else
						{
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						float num20 = (num5 - num14) / (num7 - num8);
						float num21 = (num6 - num15) / (num7 - num8);
						float num22 = (num18 - num16) / (num7 - num8);
						float num23 = (num19 - num17) / (num7 - num8);
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo)
						{
							int num24 = 0;
							while ((float)num24 < num7 - num8)
							{
								float y = num14 + (float)num24 * num20;
								g.DrawLine(pLeftUpper, num8 + (float)num24, num12, num8 + (float)num24, y);
								y = num16 + (float)num24 * num22;
								g.DrawLine(pLeftLower, num8 + (float)num24, num12, num8 + (float)num24, y);
								y = num15 + (float)num24 * num21;
								g.DrawLine(pRightUpper, num8 + (float)num24, num13, num8 + (float)num24, y);
								y = num17 + (float)num24 * num23;
								g.DrawLine(pRightLower, num8 + (float)num24, num13, num8 + (float)num24, y);
								num24++;
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							int num25 = 0;
							while ((float)num25 < num7 - num8)
							{
								float y = num14 + (float)num25 * num20;
								g.DrawLine(pLeftUpper, num8 + (float)num25, num12, num8 + (float)num25, y);
								y = num16 + (float)num25 * num22;
								g.DrawLine(pLeftLower, num8 + (float)num25, num12, num8 + (float)num25, y);
								num25++;
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono)
						{
							int num26 = 0;
							while ((float)num26 < num7 - num8)
							{
								if (num14 + (float)num26 * num20 < num16 + (float)num26 * num22)
								{
									g.DrawLine(pLeftUpper, num8 + (float)num26, (float)height, num8 + (float)num26, num14 + (float)num26 * num20);
								}
								else
								{
									g.DrawLine(pLeftUpper, num8 + (float)num26, (float)height, num8 + (float)num26, num16 + (float)num26 * num22);
								}
								num26++;
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								}
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							int num27 = 0;
							while ((float)num27 < num7 - num8)
							{
								if (num14 + (float)num27 * num20 < num16 + (float)num27 * num22)
								{
									g.DrawLine(pLeftUpper, num8 + (float)num27, 0f, num8 + (float)num27, (float)height - (num14 + (float)num27 * num20));
								}
								else
								{
									g.DrawLine(pLeftUpper, num8 + (float)num27, 0f, num8 + (float)num27, (float)height - (num16 + (float)num27 * num22));
								}
								num27++;
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num14, num7, (float)height - num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num16, num7, (float)height - num18);
								}
							}
						}
						else
						{
							int num28 = 0;
							while ((float)num28 < num7 - num8)
							{
								float y = num14 + (float)num28 * num20;
								g.DrawLine(pLeftUpper, num8 + (float)num28, num12, num8 + (float)num28, y);
								y = num16 + (float)num28 * num22;
								g.DrawLine(pLeftLower, num8 + (float)num28, num12, num8 + (float)num28, y);
								y = num15 + (float)num28 * num21;
								g.DrawLine(pRightUpper, num8 + (float)num28, num13, num8 + (float)num28, y);
								y = num17 + (float)num28 * num23;
								g.DrawLine(pRightLower, num8 + (float)num28, num13, num8 + (float)num28, y);
								num28++;
							}
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
					}
					else
					{
						num5 = num12 - (float)num * (num11 / 32767f) * this._gainFactor;
						num6 = num13 - (float)num2 * (num11 / 32767f) * this._gainFactor;
						num18 = num12 - (float)num3 * (num11 / 32767f) * this._gainFactor;
						num19 = num13 - (float)num4 * (num11 / 32767f) * this._gainFactor;
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							num19 = (num18 = (num18 + num19) / 2f);
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono || waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							num6 = (num5 = (num5 + num6) / 2f);
							if (num5 <= num12)
							{
								num6 = (num5 = 2f * num5);
							}
							else
							{
								num6 = (num5 = 2f * (2f * num12 - num5));
							}
							num18 = (num18 + num19) / 2f;
							if (num18 <= num12)
							{
								num19 = (num18 = 2f * num18);
							}
							else
							{
								num19 = (num18 = 2f * (2f * num12 - num18));
							}
						}
						else
						{
							if (num5 > num12)
							{
								num5 = num12;
							}
							if (num18 < num12)
							{
								num18 = num12;
							}
							if (num6 > num13)
							{
								num6 = num13;
							}
							if (num19 < num13)
							{
								num19 = num13;
							}
						}
						if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo)
						{
							g.DrawLine(pLeftUpper, num8, num12, num8, num5);
							g.DrawLine(pLeftLower, num8, num12, num8, num18);
							g.DrawLine(pRightUpper, num8, num13, num8, num6);
							g.DrawLine(pRightLower, num8, num13, num8, num19);
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Mono)
						{
							g.DrawLine(pLeftUpper, num8, num12, num8, num5);
							g.DrawLine(pLeftLower, num8, num12, num8, num18);
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMono)
						{
							if (num5 < num18)
							{
								g.DrawLine(pLeftUpper, num8, (float)height, num8, num5);
							}
							else
							{
								g.DrawLine(pLeftUpper, num8, (float)height, num8, num18);
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								}
							}
						}
						else if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
						{
							if (num5 < num18)
							{
								g.DrawLine(pLeftUpper, num8, 0f, num8, (float)height - num5);
							}
							else
							{
								g.DrawLine(pLeftUpper, num8, 0f, num8, (float)height - num18);
							}
							if (this._drawEnvelope)
							{
								if (num5 < num18)
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num14, num7, (float)height - num5);
								}
								else
								{
									g.DrawLine(pLeftEnv, num8, (float)height - num16, num7, (float)height - num18);
								}
							}
						}
						else
						{
							g.DrawLine(pLeftUpper, num8, num12, num8, num5);
							g.DrawLine(pLeftLower, num8, num12, num8, num18);
							g.DrawLine(pRightUpper, num8, num13, num8, num6);
							g.DrawLine(pRightLower, num8, num13, num8, num19);
							if (this._drawEnvelope)
							{
								g.DrawLine(pLeftEnv, num8, num14, num7, num5);
								g.DrawLine(pLeftEnv, num8, num16, num7, num18);
								g.DrawLine(pRightEnv, num8, num15, num7, num6);
								g.DrawLine(pRightEnv, num8, num17, num7, num19);
							}
						}
					}
					num8 = num7;
					num = 0;
					num2 = 0;
					num3 = 0;
					num4 = 0;
					num14 = num5;
					num15 = num6;
					num16 = num18;
					num17 = num19;
				}
			}
			if (this._drawCenterLine && waveformdrawtype != WaveForm.WAVEFORMDRAWTYPE.HalfMono && waveformdrawtype != WaveForm.WAVEFORMDRAWTYPE.HalfMonoFlipped)
			{
				g.DrawLine(pMiddleL, 0f, num12, (float)width, num12);
				if (waveformdrawtype == WaveForm.WAVEFORMDRAWTYPE.Stereo)
				{
					g.DrawLine(pMiddleR, 0f, num13, (float)width, num13);
				}
			}
			long num29 = this.Frame2Bytes(frameStart);
			long num30 = this.Frame2Bytes(frameEnd);
			double num31 = (double)(num30 - num29) / (double)width;
			if (this.DrawBeat != WaveForm.BEATDRAWTYPE.None && this._waveBuffer.beats != null && this._waveBuffer.beats.Count > 0)
			{
				float num32 = (float)(height + 1) * this._beatLength;
				int num33 = this._waveBuffer.beats.BinarySearch((long)frameStart * (long)this._waveBuffer.bpf);
				if (num33 < 0)
				{
					num33 = ~num33;
				}
				for (int j = num33; j < this._waveBuffer.beats.Count; j++)
				{
					long num34 = (long)((double)this._waveBuffer.beats[j] / this._syncFactor);
					if (num34 >= num29 && num34 <= num30)
					{
						num7 = (float)((int)((double)(num34 - num29) / num31 + 0.5));
						if (this.DrawBeat == WaveForm.BEATDRAWTYPE.Middle)
						{
							g.DrawLine(pBeat, num7, num10 - num32, num7, num10 + num32);
						}
						else if (this.DrawBeat == WaveForm.BEATDRAWTYPE.Top)
						{
							g.DrawLine(pBeat, num7, -1f, num7, num32);
						}
						else if (this.DrawBeat == WaveForm.BEATDRAWTYPE.Bottom)
						{
							g.DrawLine(pBeat, num7, (float)height - num32, num7, (float)height);
						}
						else if (this.DrawBeat == WaveForm.BEATDRAWTYPE.TopBottom)
						{
							g.DrawLine(pBeat, num7, -1f, num7, num32);
							g.DrawLine(pBeat, num7, (float)height - num32, num7, (float)height);
						}
					}
					else if (num34 > num30)
					{
						break;
					}
				}
			}
			if (this.DrawMarker != WaveForm.MARKERDRAWTYPE.None && this._waveBuffer.marker != null && this._waveBuffer.marker.Count > 0)
			{
				float num35 = (float)(height + 1) * this._markerLength;
				bool flag = false;
				int num36 = 0;
				long num37 = 0L;
				string a = "Auto";
				List<long> list = null;
				using (Brush brush = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? this._colorBackground : pMarker.Color))
				{
					using (Brush brush2 = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? pMarker.Color : this._colorBackground))
					{
						SizeF sizeF = g.MeasureString("M", this._markerFont);
						num5 = num10 - sizeF.Height / 2f;
						num6 = num5;
						if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NamePositionTop) != WaveForm.MARKERDRAWTYPE.None)
						{
							num5 -= num35 - sizeF.Height / 2f + 1f;
							num6 = num5;
						}
						else if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NamePositionBottom) != WaveForm.MARKERDRAWTYPE.None)
						{
							num5 += num35 - sizeF.Height / 2f + 1f;
							num6 = num5;
						}
						else if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NamePositionMiddle) == WaveForm.MARKERDRAWTYPE.None)
						{
							num5 -= num35 - sizeF.Height / 2f + 1f;
							num6 += num35 - sizeF.Height / 2f + 1f;
							list = new List<long>(this._waveBuffer.marker.Values);
							list.Sort();
							flag = true;
						}
						foreach (string text in this._waveBuffer.marker.Keys)
						{
							num37 = this.Position2Playback(this._waveBuffer.marker[text]);
							num7 = (float)((int)((double)(num37 - num29) / num31 + 0.5));
							if (flag)
							{
								num36 = list.IndexOf(this._waveBuffer.marker[text]) % 2;
							}
							string text2 = text;
							int num38 = text2.IndexOf('{');
							if (num38 > 0)
							{
								text2 = text2.Substring(0, num38);
							}
							Color color = Color.Empty;
							int num39 = text.LastIndexOf("{Color=");
							if (num39 > 0 && text.EndsWith("}"))
							{
								try
								{
									int num40 = text.IndexOf('}', num39);
									string text3 = text.Substring(num39 + 7, num40 - num39 - 7);
									if (text3.StartsWith("0x"))
									{
										color = Color.FromArgb(int.Parse(text3.Substring(2), NumberStyles.HexNumber));
									}
									else
									{
										color = Color.FromName(text3);
									}
								}
								catch
								{
								}
							}
							int num41 = text.LastIndexOf("{Length=");
							if (num41 > 0 && text.EndsWith("}"))
							{
								int num42 = text.IndexOf('}', num41);
								long num43 = 0L;
								if (long.TryParse(text.Substring(num41 + 8, num42 - num41 - 8), out num43) && num43 > 0L)
								{
									long num44 = num37 + this.Position2Playback(num43);
									if (num44 >= num29 && num37 <= num30)
									{
										if (num44 > num30)
										{
											num44 = num30;
										}
										num8 = (float)((int)((double)(num44 - num29) / num31 + 0.5));
										using (Brush brush3 = new SolidBrush(Color.FromArgb(128, (color == Color.Empty) ? pMarker.Color : color)))
										{
											if (num36 == 0)
											{
												g.FillRectangle(brush3, (num37 < num29) ? 0f : num7, num10 - 8f, num8 - ((num37 < num29) ? 0f : num7), 8f);
											}
											else
											{
												g.FillRectangle(brush3, (num37 < num29) ? 0f : num7, num10 + 1f, num8 - ((num37 < num29) ? 0f : num7), 8f);
											}
										}
									}
								}
							}
							int num45 = text.LastIndexOf("{Align=");
							if (num45 > 0 && text.EndsWith("}"))
							{
								int num46 = text.IndexOf('}', num45);
								a = text.Substring(num45 + 7, num46 - num45 - 7);
							}
							if (num37 >= num29 && num37 <= num30)
							{
								if (color != Color.Empty)
								{
									using (Pen pen = new Pen(color))
									{
										using (Brush brush4 = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? this._colorBackground : color))
										{
											using (Brush brush5 = new SolidBrush(((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None) ? color : this._colorBackground))
											{
												if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.Line) > WaveForm.MARKERDRAWTYPE.None)
												{
													g.DrawLine(pen, num7, num10 - num35, num7, num10 + num35);
												}
												if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.Name) > WaveForm.MARKERDRAWTYPE.None)
												{
													sizeF = g.MeasureString(text2, this._markerFont);
													if (!(a == "Right"))
													{
														if (!(a == "Center"))
														{
															if (a == "Auto")
															{
																if (num7 + sizeF.Width > (float)width)
																{
																	num7 -= sizeF.Width;
																}
															}
														}
														else
														{
															num7 -= sizeF.Width / 2f;
														}
													}
													else
													{
														num7 -= sizeF.Width;
													}
													if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFilled) != WaveForm.MARKERDRAWTYPE.None || (this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None)
													{
														if (num36 == 0)
														{
															g.FillRectangle(brush5, num7, num5, sizeF.Width, sizeF.Height);
															g.DrawRectangle(pen, num7, num5, sizeF.Width, sizeF.Height);
														}
														else
														{
															g.FillRectangle(brush5, num7, num6, sizeF.Width, sizeF.Height);
															g.DrawRectangle(pen, num7, num6, sizeF.Width, sizeF.Height);
														}
													}
													else
													{
														num7 -= 3f;
													}
													if (num36 == 0)
													{
														g.DrawString(text2, this._markerFont, brush4, num7, num5);
													}
													else
													{
														g.DrawString(text2, this._markerFont, brush4, num7, num6);
													}
												}
												continue;
											}
										}
									}
								}
								if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.Line) > WaveForm.MARKERDRAWTYPE.None)
								{
									g.DrawLine(pMarker, num7, num10 - num35, num7, num10 + num35);
								}
								if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.Name) > WaveForm.MARKERDRAWTYPE.None)
								{
									sizeF = g.MeasureString(text2, this._markerFont);
									if (!(a == "Right"))
									{
										if (!(a == "Center"))
										{
											if (a == "Auto")
											{
												if (num7 + sizeF.Width > (float)width)
												{
													num7 -= sizeF.Width;
												}
											}
										}
										else
										{
											num7 -= sizeF.Width / 2f;
										}
									}
									else
									{
										num7 -= sizeF.Width;
									}
									if ((this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFilled) != WaveForm.MARKERDRAWTYPE.None || (this.DrawMarker & WaveForm.MARKERDRAWTYPE.NameBoxFillInverted) != WaveForm.MARKERDRAWTYPE.None)
									{
										if (num36 == 0)
										{
											g.FillRectangle(brush2, num7, num5, sizeF.Width, sizeF.Height);
											g.DrawRectangle(pMarker, num7, num5, sizeF.Width, sizeF.Height);
										}
										else
										{
											g.FillRectangle(brush2, num7, num6, sizeF.Width, sizeF.Height);
											g.DrawRectangle(pMarker, num7, num6, sizeF.Width, sizeF.Height);
										}
									}
									else
									{
										num7 -= 3f;
									}
									if (num36 == 0)
									{
										g.DrawString(text2, this._markerFont, brush, num7, num5);
									}
									else
									{
										g.DrawString(text2, this._markerFont, brush, num7, num6);
									}
								}
							}
						}
					}
				}
			}
			if (this._drawVolume != WaveForm.VOLUMEDRAWTYPE.None && this._volumePoints != null && this._volumePoints.Count > 0)
			{
				long num47 = 0L;
				num5 = this.GetVolumeLevel(num29, false, ref num47, ref num8);
				num6 = this.GetVolumeLevel(num30, false, ref num47, ref num8);
				int num48 = this.SearchVolumePoint(num29);
				if (num48 < 0)
				{
					num48 = ~num48;
				}
				num7 = (num8 = 0f);
				num16 = (float)height - (float)height * num5;
				float num18;
				for (int k = num48; k < this._volumePoints.Count; k++)
				{
					WaveForm.VolumePoint volumePoint = this._volumePoints[k];
					num47 = this.Position2Playback(volumePoint.Position);
					if (num47 >= num29 && num47 <= num30)
					{
						num8 = (float)((int)((double)(num47 - num29) / num31 + 0.5));
						num18 = (float)height - (float)height * volumePoint.Level;
						g.DrawLine(pVolume, num7, num16, num8, num18);
						if ((this._drawVolume & WaveForm.VOLUMEDRAWTYPE.NoPoints) == WaveForm.VOLUMEDRAWTYPE.None)
						{
							g.DrawRectangle(pVolume, num8 - 1f, num18 - 1f, 3f, 3f);
							g.DrawRectangle(pVolume, num8 - 3f, num18 - 3f, 7f, 7f);
						}
						num7 = num8;
						num16 = num18;
					}
					else if (num47 > num30)
					{
						break;
					}
				}
				num8 = (float)width;
				num18 = (float)height - (float)height * num6;
				g.DrawLine(pVolume, num7, num16, num8, num18);
				return;
			}
			if (this._drawVolume != WaveForm.VOLUMEDRAWTYPE.None)
			{
				float num18 = this.VolumeCurveZeroLevel ? ((float)height) : 0f;
				g.DrawLine(pVolume, 0f, num18, (float)width, num18);
			}
		}

		public bool WaveFormSaveToFile(string fileName)
		{
			return this.WaveFormSaveToFile(fileName, false);
		}

		public bool WaveFormSaveToFile(string fileName, bool binary)
		{
			if (!this.IsRendered)
			{
				return false;
			}
			bool result = false;
			Stream stream = null;
			try
			{
				stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
				this.SerializeWaveForm(stream, binary);
				result = true;
			}
			catch
			{
			}
			finally
			{
				if (stream != null)
				{
					stream.Flush();
					stream.Close();
				}
			}
			return result;
		}

		public byte[] WaveFormSaveToMemory()
		{
			return this.WaveFormSaveToMemory(false);
		}

		public byte[] WaveFormSaveToMemory(bool binary)
		{
			if (!this.IsRendered)
			{
				return null;
			}
			byte[] array = null;
			Stream stream = null;
			try
			{
				stream = new MemoryStream();
				this.SerializeWaveForm(stream, binary);
				array = new byte[stream.Length];
				stream.Position = 0L;
				stream.Read(array, 0, array.Length);
			}
			catch
			{
			}
			finally
			{
				if (stream != null)
				{
					stream.Flush();
					stream.Close();
				}
			}
			return array;
		}

		private void SerializeWaveForm(Stream stream, bool binary)
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (binary)
				{
					BinaryWriter binaryWriter = new BinaryWriter(stream);
					binaryWriter.Write(1);
					binaryWriter.Write((this._waveBuffer.fileName == null) ? string.Empty : this._waveBuffer.fileName);
					binaryWriter.Write(this._waveBuffer.bpf);
					binaryWriter.Write(this._waveBuffer.chans);
					binaryWriter.Write((int)this._waveBuffer.flags);
					binaryWriter.Write(this._waveBuffer.resolution);
					if (this._waveBuffer.beats != null)
					{
						binaryWriter.Write(this._waveBuffer.beats.Count);
						using (List<long>.Enumerator enumerator = this._waveBuffer.beats.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								long value = enumerator.Current;
								binaryWriter.Write(value);
							}
							goto IL_F5;
						}
					}
					binaryWriter.Write(0);
					IL_F5:
					if (this._waveBuffer.marker != null)
					{
						binaryWriter.Write(this._waveBuffer.marker.Count);
						using (Dictionary<string, long>.Enumerator enumerator2 = this._waveBuffer.marker.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								KeyValuePair<string, long> keyValuePair = enumerator2.Current;
								binaryWriter.Write(keyValuePair.Key);
								binaryWriter.Write(keyValuePair.Value);
							}
							goto IL_16F;
						}
					}
					binaryWriter.Write(0);
					IL_16F:
					if (this._waveBuffer.data != null)
					{
						binaryWriter.Write(this._waveBuffer.data.Length);
						foreach (WaveForm.WaveBuffer.Level level in this._waveBuffer.data)
						{
							binaryWriter.Write(level.left);
							binaryWriter.Write(level.right);
						}
					}
					else
					{
						binaryWriter.Write(0);
					}
				}
				else
				{
					new BinaryFormatter().Serialize(stream, this._waveBuffer);
				}
			}
		}

		public bool WaveFormLoadFromFile(string fileName)
		{
			return this.WaveFormLoadFromFile(fileName, false);
		}

		public bool WaveFormLoadFromFile(string fileName, bool binary)
		{
			bool result = true;
			Stream stream = null;
			try
			{
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				this.DeserializeWaveForm(stream, binary);
				if (this._waveBuffer != null)
				{
					this._isRendered = true;
					this._fileName = this._waveBuffer.fileName;
					this._framesRendered = this._waveBuffer.data.Length;
					this._framesToRender = this._waveBuffer.data.Length;
					this._frameResolution = this._waveBuffer.resolution;
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return result;
		}

		public bool WaveFormLoadFromMemory(byte[] data)
		{
			return this.WaveFormLoadFromMemory(data, false);
		}

		public bool WaveFormLoadFromMemory(byte[] data, bool binary)
		{
			bool result = true;
			Stream stream = null;
			try
			{
				stream = new MemoryStream(data);
				this.DeserializeWaveForm(stream, binary);
				if (this._waveBuffer != null)
				{
					this._isRendered = true;
					this._fileName = this._waveBuffer.fileName;
					this._framesRendered = this._waveBuffer.data.Length;
					this._framesToRender = this._waveBuffer.data.Length;
					this._frameResolution = this._waveBuffer.resolution;
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return result;
		}

		private void DeserializeWaveForm(Stream stream, bool binary)
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (binary)
				{
					BinaryReader binaryReader = new BinaryReader(stream);
					binaryReader.ReadInt32();
					this._waveBuffer = new WaveForm.WaveBuffer();
					this._waveBuffer.fileName = binaryReader.ReadString();
					this._waveBuffer.bpf = binaryReader.ReadInt32();
					this._waveBuffer.chans = binaryReader.ReadInt32();
					this._waveBuffer.flags = (BASSFlag)binaryReader.ReadInt32();
					this._waveBuffer.resolution = binaryReader.ReadDouble();
					int num = binaryReader.ReadInt32();
					if (num > 0)
					{
						this._waveBuffer.beats = new List<long>(num);
						for (int i = 0; i < num; i++)
						{
							this._waveBuffer.beats.Add(binaryReader.ReadInt64());
						}
					}
					int num2 = binaryReader.ReadInt32();
					if (num2 > 0)
					{
						this._waveBuffer.marker = new Dictionary<string, long>(num2);
						for (int j = 0; j < num2; j++)
						{
							string key = binaryReader.ReadString();
							long value = binaryReader.ReadInt64();
							this._waveBuffer.marker[key] = value;
						}
					}
					int num3 = binaryReader.ReadInt32();
					if (num3 > 0)
					{
						this._waveBuffer.data = new WaveForm.WaveBuffer.Level[num3];
						for (int k = 0; k < num3; k++)
						{
							short levelL = binaryReader.ReadInt16();
							short levelR = binaryReader.ReadInt16();
							this._waveBuffer.data[k] = new WaveForm.WaveBuffer.Level(levelL, levelR);
						}
					}
				}
				else
				{
					IFormatter formatter = new BinaryFormatter();
					bool flag2 = false;
					while (!flag2)
					{
						try
						{
							this._waveBuffer = (formatter.Deserialize(stream) as WaveForm.WaveBuffer);
							flag2 = true;
						}
						catch
						{
							flag2 = true;
						}
					}
				}
			}
		}

		private object _syncRoot = new object();

		private string _fileName = string.Empty;

		private double _frameResolution = 0.01;

		private int _callbackFrequency = 250;

		private WAVEFORMPROC _notifyHandler;

		private Control _win;

		private bool _preScan = true;

		private bool _useSimpleScan;

		private bool _isRendered;

		private int _framesToRender;

		private int _framesRendered;

		private bool _detectBeats;

		private float _gainFactor = 1f;

		private Color _colorBackground = SystemColors.Control;

		private Color _colorLeft = Color.Gainsboro;

		private Color _colorLeft2 = Color.White;

		private Color _colorLeftEnvelope = Color.Gray;

		private bool _drawEnvelope = true;

		private bool _drawCenterLine = true;

		private Color _colorMiddleLeft = Color.Empty;

		private Color _colorMiddleRight = Color.Empty;

		private bool _drawGradient;

		private Color _colorRight = Color.LightGray;

		private Color _colorRight2 = Color.White;

		private Color _colorRightEnvelope = Color.DimGray;

		private Color _colorVolume = Color.IndianRed;

		private PixelFormat _pixelFormat = PixelFormat.Format32bppArgb;

		private WaveForm.VOLUMEDRAWTYPE _drawVolume;

		private bool _volumeCurveZeroLevel;

		private Color _colorBeat = Color.CornflowerBlue;

		private WaveForm.BEATDRAWTYPE _drawBeat;

		private float _beatLength = 0.05f;

		private int _beatWidth = 1;

		private Color _colorMarker = Color.DarkBlue;

		private Font _markerFont = new Font("Arial", 7.5f, FontStyle.Regular);

		private WaveForm.MARKERDRAWTYPE _drawMarker;

		private float _markerLength = 0.1f;

		private WaveForm.WAVEFORMDRAWTYPE _drawWaveForm;

		private int _decodingStream;

		private DateTime _renderStartTime;

		private bool _killScan;

		private volatile bool _renderingInProgress;

		private WaveForm.WaveBuffer _waveBuffer;

		private float[] _peakLevelFloat;

		private short[] _peakLevelShort;

		private byte[] _peakLevelByte;

		private int _recordingNextLength = 10;

		private double _channelFactor = 1.0;

		private double _syncFactor = 1.0;

		private double _correctionFactor = 1.0;

		private List<WaveForm.VolumePoint> _volumePoints;

		private int _recordingLeftoverBytes;

		private WaveForm.WaveBuffer.Level _recordingLeftoverLevel;

		private bool _freeStream = true;

		[Flags]
		public enum MARKERDRAWTYPE
		{
			None = 0,
			Line = 1,
			Name = 2,
			NamePositionAlternate = 0,
			NamePositionTop = 4,
			NamePositionBottom = 8,
			NamePositionMiddle = 16,
			NameBoxFilled = 256,
			NameBoxFillInverted = 512
		}

		[Flags]
		public enum VOLUMEDRAWTYPE
		{
			None = 0,
			Solid = 1,
			Dotted = 2,
			NoPoints = 4
		}

		[Flags]
		public enum BEATDRAWTYPE
		{
			None = 0,
			Top = 1,
			Bottom = 2,
			Middle = 3,
			TopBottom = 4
		}

		public enum WAVEFORMDRAWTYPE
		{
			Stereo,
			Mono,
			DualMono,
			HalfMono,
			HalfMonoFlipped
		}

		[Serializable]
		public class WaveBuffer
		{
			internal WaveBuffer()
			{
			}

			public int bps
			{
				get
				{
					if ((this.flags & BASSFlag.BASS_SAMPLE_FLOAT) != BASSFlag.BASS_DEFAULT)
					{
						return 4;
					}
					if ((this.flags & BASSFlag.BASS_SAMPLE_8BITS) != BASSFlag.BASS_DEFAULT)
					{
						return 1;
					}
					return 2;
				}
			}

			internal string fileName = string.Empty;

			public BASSFlag flags;

			public int chans = 2;

			public int bpf;

			public double resolution = 0.02;

			public WaveForm.WaveBuffer.Level[] data;

			public Dictionary<string, long> marker;

			public List<long> beats;

			[Serializable]
			public struct Level
			{
				public Level(short levelL, short levelR)
				{
					this.left = levelL;
					this.right = levelR;
				}

				public short left;

				public short right;
			}
		}

		public class VolumePoint : IComparable
		{
			public VolumePoint(long position, float level)
			{
				this.Position = position;
				this.Level = level;
			}

			public override string ToString()
			{
				return string.Format("Position: {0}, Level: {1}", this.Position, this.Level);
			}

			public int CompareTo(object obj)
			{
				if (obj is WaveForm.VolumePoint)
				{
					WaveForm.VolumePoint volumePoint = (WaveForm.VolumePoint)obj;
					return this.Position.CompareTo(volumePoint.Position);
				}
				throw new ArgumentException("object is not a VolumePoint");
			}

			public long Position;

			public float Level = 1f;
		}
	}
}
