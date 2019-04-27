using System;
using System.Runtime.InteropServices;

namespace radio42.Multimedia.Midi
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class MIDI_OUTCAPS
	{
		public override string ToString()
		{
			return this.name;
		}

		public bool SupportsCache
		{
			get
			{
				return (this.support & 4) != 0;
			}
		}

		public bool SupportsLRVolume
		{
			get
			{
				return (this.support & 2) != 0;
			}
		}

		public bool SupportsVolume
		{
			get
			{
				return (this.support & 1) != 0;
			}
		}

		public bool SupportsStream
		{
			get
			{
				return (this.support & 8) != 0;
			}
		}

		public bool IsMidiPort
		{
			get
			{
				return this.technology == MIDIDevice.MOD_MIDIPORT;
			}
		}

		public bool IsMidiMapper
		{
			get
			{
				return this.technology == MIDIDevice.MOD_MAPPER;
			}
		}

		public short mid;

		public short pid;

		public int driverVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string name = string.Empty;

		public MIDIDevice technology;

		public short voices;

		public short notes;

		public short channelMask;

		public int support;
	}
}
