using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;

namespace radio42.Multimedia.Midi
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class MidiOutputDevice
	{
		public MidiOutputDevice(int deviceID)
		{
			this._deviceID = deviceID;
			this._midiOutProc = new MIDIOUTPROC(this.MidiOutProc);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			this._disposing = true;
			if (!this._disposed && this.IsOpened)
			{
				this.Close();
			}
			this._disposed = true;
		}

		~MidiOutputDevice()
		{
			this.Dispose(false);
		}

		public event MidiMessageEventHandler MessageReceived;

		public bool IsDisposed
		{
			get
			{
				return this._disposing;
			}
		}

		public IntPtr Device
		{
			get
			{
				return this._device;
			}
		}

		public int DeviceID
		{
			get
			{
				return this._deviceID;
			}
		}

		public IntPtr User
		{
			get
			{
				return this._user;
			}
			set
			{
				this._user = value;
			}
		}

		public bool IsOpened
		{
			get
			{
				return this._device != IntPtr.Zero;
			}
		}

		public MIDIError LastErrorCode
		{
			get
			{
				return this._lastError;
			}
		}

		public static int GetDeviceCount()
		{
			return Midi.MIDI_OutGetNumDevs();
		}

		public static string[] GetDeviceDescriptions()
		{
			List<string> list = new List<string>();
			int num = Midi.MIDI_OutGetNumDevs();
			MIDI_OUTCAPS midi_OUTCAPS = new MIDI_OUTCAPS();
			for (int i = 0; i < num; i++)
			{
				if (Midi.MIDI_OutGetDevCaps(i, midi_OUTCAPS) == MIDIError.MIDI_OK)
				{
					list.Add(midi_OUTCAPS.name);
				}
			}
			return list.ToArray();
		}

		public static int[] GetMidiPorts()
		{
			List<int> list = new List<int>();
			int num = Midi.MIDI_OutGetNumDevs();
			MIDI_OUTCAPS midi_OUTCAPS = new MIDI_OUTCAPS();
			for (int i = 0; i < num; i++)
			{
				if (Midi.MIDI_OutGetDevCaps(i, midi_OUTCAPS) == MIDIError.MIDI_OK && midi_OUTCAPS.IsMidiPort)
				{
					list.Add(i);
				}
			}
			return list.ToArray();
		}

		public static string GetDeviceDescription(int deviceID)
		{
			MIDI_OUTCAPS midi_OUTCAPS = new MIDI_OUTCAPS();
			if (Midi.MIDI_OutGetDevCaps(deviceID, midi_OUTCAPS) == MIDIError.MIDI_OK)
			{
				return midi_OUTCAPS.name;
			}
			return null;
		}

		public static MIDI_OUTCAPS GetInfo(int deviceID)
		{
			MIDI_OUTCAPS midi_OUTCAPS = new MIDI_OUTCAPS();
			if (Midi.MIDI_OutGetDevCaps(deviceID, midi_OUTCAPS) == MIDIError.MIDI_OK)
			{
				return midi_OUTCAPS;
			}
			return null;
		}

		public bool Connect(IntPtr handleTo)
		{
			if (this._disposing)
			{
				return false;
			}
			this._lastError = Midi.MIDI_Connect(this._device, handleTo);
			return this._lastError == MIDIError.MIDI_OK;
		}

		public bool Disconnect(IntPtr handleFrom)
		{
			if (this._disposing)
			{
				return false;
			}
			this._lastError = Midi.MIDI_Disconnect(this._device, handleFrom);
			return this._lastError == MIDIError.MIDI_OK;
		}

		public bool Open()
		{
			if (this._disposing)
			{
				return false;
			}
			if (this._device != IntPtr.Zero)
			{
				return true;
			}
			this._lastError = Midi.MIDI_OutOpen(ref this._device, this._deviceID, this._midiOutProc, new IntPtr(this._deviceID));
			if (this._lastError != MIDIError.MIDI_OK)
			{
				this._device = IntPtr.Zero;
			}
			return this._device != IntPtr.Zero;
		}

		public bool Close()
		{
			if (this._device == IntPtr.Zero)
			{
				return true;
			}
			Midi.MIDI_OutReset(this._device);
			this._lastError = Midi.MIDI_OutClose(this._device);
			if (this._lastError == MIDIError.MIDI_OK)
			{
				this._device = IntPtr.Zero;
			}
			return this._device == IntPtr.Zero;
		}

		public bool Send(MidiShortMessage shortMessage)
		{
			if (!this.IsOpened || shortMessage == null)
			{
				return false;
			}
			this._lastError = Midi.MIDI_OutShortMsg(this._device, shortMessage.Message);
			return this._lastError == MIDIError.MIDI_OK;
		}

		public bool Send(int shortMessage)
		{
			if (!this.IsOpened)
			{
				return false;
			}
			this._lastError = Midi.MIDI_OutShortMsg(this._device, shortMessage);
			return this._lastError == MIDIError.MIDI_OK;
		}

		public bool Send(MIDIStatus status, byte channel, byte data1, byte data2)
		{
			if (!this.IsOpened)
			{
				return false;
			}
			MidiShortMessage midiShortMessage = new MidiShortMessage(status, channel, data1, data2, 0L);
			this._lastError = Midi.MIDI_OutShortMsg(this._device, midiShortMessage.Message);
			return this._lastError == MIDIError.MIDI_OK;
		}

		public bool Send(byte status, byte data1, byte data2)
		{
			if (!this.IsOpened)
			{
				return false;
			}
			MidiShortMessage midiShortMessage = new MidiShortMessage(status, data1, data2, 0, 0L);
			this._lastError = Midi.MIDI_OutShortMsg(this._device, midiShortMessage.Message);
			return this._lastError == MIDIError.MIDI_OK;
		}

		public bool Send(MidiSysExMessage sysexMessage)
		{
			if (!this.IsOpened || sysexMessage.IsInput || sysexMessage.Device != this.Device || sysexMessage.IsPrepared)
			{
				return false;
			}
			if (sysexMessage.Prepare(this._user))
			{
				this._lastError = Midi.MIDI_OutLongMsg(this._device, sysexMessage.MessageAsIntPtr);
				return this._lastError == MIDIError.MIDI_OK;
			}
			return false;
		}

		public bool Send(byte[] sysexMessage)
		{
			if (!this.IsOpened || sysexMessage == null)
			{
				return false;
			}
			MidiSysExMessage midiSysExMessage = new MidiSysExMessage(false, this._device);
			if (!midiSysExMessage.CreateBuffer(sysexMessage))
			{
				return false;
			}
			if (midiSysExMessage.Prepare(this._user))
			{
				this._lastError = Midi.MIDI_OutLongMsg(this._device, midiSysExMessage.MessageAsIntPtr);
				return this._lastError == MIDIError.MIDI_OK;
			}
			return false;
		}

		public bool Send(IEnumerable<byte> sysexMessage)
		{
			if (!this.IsOpened || sysexMessage == null)
			{
				return false;
			}
			MidiSysExMessage midiSysExMessage = new MidiSysExMessage(false, this._device);
			if (!midiSysExMessage.CreateBuffer(sysexMessage))
			{
				return false;
			}
			if (midiSysExMessage.Prepare(this._user))
			{
				this._lastError = Midi.MIDI_OutLongMsg(this._device, midiSysExMessage.MessageAsIntPtr);
				return this._lastError == MIDIError.MIDI_OK;
			}
			return false;
		}

		private void MidiOutProc(IntPtr handle, MIDIMessage msg, IntPtr user, IntPtr param1, IntPtr param2)
		{
			if (msg == MIDIMessage.MOM_OPEN)
			{
				this.RaiseMessageReceived(MidiMessageEventType.Opened, null);
				return;
			}
			if (msg == MIDIMessage.MOM_CLOSE)
			{
				this.RaiseMessageReceived(MidiMessageEventType.Closed, null);
				return;
			}
			if (msg == MIDIMessage.MOM_DONE)
			{
				this._sysexMsg = new MidiSysExMessage(false, handle, param1, this._sysexMsg);
				if (this._sysexMsg.IsDone)
				{
					this.RaiseMessageReceived(MidiMessageEventType.SystemExclusiveDone, this._sysexMsg);
				}
			}
		}

		private void RaiseMessageReceived(MidiMessageEventType pEventType, object pData)
		{
			if (this.MessageReceived != null)
			{
				this.ProcessDelegate(this.MessageReceived, new object[]
				{
					this,
					new MidiMessageEventArgs(pEventType, this._deviceID, this._device, pData)
				});
			}
		}

		private void ProcessDelegate(Delegate del, params object[] args)
		{
			if (del == null)
			{
				return;
			}
			foreach (Delegate del2 in del.GetInvocationList())
			{
				this.InvokeDelegate(del2, args);
			}
		}

		private void InvokeDelegate(Delegate del, object[] args)
		{
			ISynchronizeInvoke synchronizeInvoke = del.Target as ISynchronizeInvoke;
			if (synchronizeInvoke != null)
			{
				if (!synchronizeInvoke.InvokeRequired)
				{
					del.DynamicInvoke(args);
					return;
				}
				try
				{
					synchronizeInvoke.BeginInvoke(del, args);
					return;
				}
				catch
				{
					return;
				}
			}
			del.DynamicInvoke(args);
		}

		private bool _disposed;

		private bool _disposing;

		private int _deviceID = -1;

		private IntPtr _device = IntPtr.Zero;

		private MidiSysExMessage _sysexMsg;

		private MIDIOUTPROC _midiOutProc;

		private IntPtr _user = IntPtr.Zero;

		private MIDIError _lastError;
	}
}
