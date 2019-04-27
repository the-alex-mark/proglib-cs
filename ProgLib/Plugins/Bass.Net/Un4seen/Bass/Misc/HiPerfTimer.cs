using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Un4seen.Bass.Misc
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class HiPerfTimer
	{
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		public HiPerfTimer()
		{
			this.startTime = 0L;
			this.stopTime = 0L;
			if (!HiPerfTimer.QueryPerformanceFrequency(out this.freq))
			{
				throw new Win32Exception();
			}
		}

		public void Start()
		{
			Thread.Sleep(0);
			HiPerfTimer.QueryPerformanceCounter(out this.startTime);
		}

		public void Stop()
		{
			HiPerfTimer.QueryPerformanceCounter(out this.stopTime);
		}

		public double Duration
		{
			get
			{
				return (double)(this.stopTime - this.startTime) / (double)this.freq;
			}
		}

		private long startTime;

		private long stopTime;

		private long freq;
	}
}
