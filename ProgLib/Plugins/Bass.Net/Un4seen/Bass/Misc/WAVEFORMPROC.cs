using System;

namespace Un4seen.Bass.Misc
{
	public delegate void WAVEFORMPROC(int framesDone, int framesTotal, TimeSpan elapsedTime, bool finished);
}
