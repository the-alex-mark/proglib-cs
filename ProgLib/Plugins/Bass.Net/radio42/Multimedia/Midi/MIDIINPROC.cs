using System;

namespace radio42.Multimedia.Midi
{
	public delegate void MIDIINPROC(IntPtr handle, MIDIMessage msg, IntPtr instance, IntPtr param1, IntPtr param2);
}
