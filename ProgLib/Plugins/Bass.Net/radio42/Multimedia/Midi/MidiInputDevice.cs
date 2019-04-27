using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;

namespace radio42.Multimedia.Midi
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class MidiInputDevice : IDisposable
	{
		public MidiInputDevice(int deviceID)
		{
			this._deviceID = deviceID;
			this._midiInProc = new MIDIINPROC(this.MidiInProc);
			this.InitControllerPairs();
		}

		public MidiInputDevice(int deviceID, MIDIINPROC proc)
		{
			this._deviceID = deviceID;
			if (proc == null)
			{
				this._midiInProc = new MIDIINPROC(this.MidiInProc);
			}
			else
			{
				this._midiInProc = proc;
			}
			this.InitControllerPairs();
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

		~MidiInputDevice()
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

		public int SysExBufferSize
		{
			get
			{
				return this._sysexBufferSize;
			}
			set
			{
				if (value >= 2 && value <= 65536)
				{
					this._sysexBufferSize = value;
				}
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

		public MidiShortMessage ShortMessage
		{
			get
			{
				return this._shortMsg;
			}
		}

		public MidiSysExMessage SysExMessage
		{
			get
			{
				return this._sysexMsg;
			}
		}

		public bool IsOpened
		{
			get
			{
				return this._device != IntPtr.Zero;
			}
		}

		public bool IsStarted
		{
			get
			{
				return this._started;
			}
		}

		public MIDIError LastErrorCode
		{
			get
			{
				return this._lastError;
			}
		}

		public bool AutoPairController
		{
			get
			{
				return this._autoPairController;
			}
			set
			{
				this._autoPairController = value;
			}
		}

		public byte[,] ColtrollerPairMatrix
		{
			get
			{
				return this._controllerPairs;
			}
			set
			{
				if (value != null && value.Rank != 3)
				{
					return;
				}
				this._controllerPairs = value;
			}
		}

		public MIDIMessageType MessageFilter
		{
			get
			{
				return this._messageFilter;
			}
			set
			{
				this._messageFilter = value;
			}
		}

		public bool ProcessErrorMessages
		{
			get
			{
				return this._processErrorMessages;
			}
			set
			{
				this._processErrorMessages = value;
			}
		}

		public static int GetDeviceCount()
		{
			return Midi.MIDI_InGetNumDevs();
		}

		public static string[] GetDeviceDescriptions()
		{
			List<string> list = new List<string>();
			int num = Midi.MIDI_InGetNumDevs();
			MIDI_INCAPS midi_INCAPS = new MIDI_INCAPS();
			for (int i = 0; i < num; i++)
			{
				if (Midi.MIDI_InGetDevCaps(i, midi_INCAPS) == MIDIError.MIDI_OK)
				{
					list.Add(midi_INCAPS.name);
				}
			}
			return list.ToArray();
		}

		public static int[] GetMidiPorts()
		{
			List<int> list = new List<int>();
			int num = Midi.MIDI_InGetNumDevs();
			MIDI_INCAPS caps = new MIDI_INCAPS();
			for (int i = 0; i < num; i++)
			{
				if (Midi.MIDI_InGetDevCaps(i, caps) == MIDIError.MIDI_OK)
				{
					list.Add(i);
				}
			}
			return list.ToArray();
		}

		public static string GetDeviceDescription(int deviceID)
		{
			MIDI_INCAPS midi_INCAPS = new MIDI_INCAPS();
			if (Midi.MIDI_InGetDevCaps(deviceID, midi_INCAPS) == MIDIError.MIDI_OK)
			{
				return midi_INCAPS.name;
			}
			return null;
		}

		public static MIDI_INCAPS GetInfo(int deviceID)
		{
			MIDI_INCAPS midi_INCAPS = new MIDI_INCAPS();
			if (Midi.MIDI_InGetDevCaps(deviceID, midi_INCAPS) == MIDIError.MIDI_OK)
			{
				return midi_INCAPS;
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
			this._lastError = Midi.MIDI_InOpen(ref this._device, this._deviceID, this._midiInProc, new IntPtr(this._deviceID), MIDIFlags.MIDI_IO_STATUS);
			if (this._lastError != MIDIError.MIDI_OK)
			{
				this._device = IntPtr.Zero;
			}
			else
			{
				this._shortMsgOnStack = null;
			}
			return this._device != IntPtr.Zero;
		}

		public bool Close()
		{
			if (this._device == IntPtr.Zero)
			{
				return true;
			}
			this._closing = true;
			bool started = this._started;
			this._lastError = Midi.MIDI_InReset(this._device);
			if (this._lastError == MIDIError.MIDI_OK)
			{
				this._started = false;
				if (started)
				{
					this.RaiseMessageReceived(MidiMessageEventType.Stopped, null);
				}
			}
			this._lastError = Midi.MIDI_InClose(this._device);
			if (this._lastError == MIDIError.MIDI_OK)
			{
				this._device = IntPtr.Zero;
			}
			this._closing = false;
			return this._device == IntPtr.Zero;
		}

		public bool Start()
		{
			if (this._disposing)
			{
				return false;
			}
			if (this._started)
			{
				return true;
			}
			this.AddSysExBuffer();
			this.AddSysExBuffer();
			this._lastError = Midi.MIDI_InStart(this._device);
			if (this._lastError == MIDIError.MIDI_OK)
			{
				this._started = true;
				this.RaiseMessageReceived(MidiMessageEventType.Started, null);
			}
			else
			{
				Midi.MIDI_InReset(this._device);
			}
			return this._started;
		}

		public bool Stop()
		{
			if (this._device == IntPtr.Zero)
			{
				return true;
			}
			if (!this._started)
			{
				return true;
			}
			this._closing = true;
			this._lastError = Midi.MIDI_InStop(this._device);
			if (this._lastError == MIDIError.MIDI_OK)
			{
				this._started = false;
				this.FlushShortMsgStack();
				this.RaiseMessageReceived(MidiMessageEventType.Stopped, null);
			}
			this._closing = false;
			return !this._started;
		}

		public bool AddSysExBuffer()
		{
			if (this._closing)
			{
				return true;
			}
			if (this._device == IntPtr.Zero || this._disposing)
			{
				return false;
			}
			bool result = false;
			try
			{
				MidiSysExMessage midiSysExMessage = new MidiSysExMessage(true, this._device);
				midiSysExMessage.CreateBuffer(this._sysexBufferSize);
				if (midiSysExMessage.Prepare(this._user))
				{
					result = midiSysExMessage.Send();
				}
			}
			catch
			{
			}
			return result;
		}

		private void InitControllerPairs()
		{
			this._controllerPairs = new byte[,]
			{
				{
					0,
					32,
					byte.MaxValue
				},
				{
					1,
					33,
					byte.MaxValue
				},
				{
					2,
					34,
					byte.MaxValue
				},
				{
					3,
					35,
					byte.MaxValue
				},
				{
					4,
					36,
					byte.MaxValue
				},
				{
					5,
					37,
					byte.MaxValue
				},
				{
					6,
					38,
					byte.MaxValue
				},
				{
					7,
					39,
					byte.MaxValue
				},
				{
					8,
					40,
					byte.MaxValue
				},
				{
					9,
					41,
					byte.MaxValue
				},
				{
					10,
					42,
					byte.MaxValue
				},
				{
					11,
					43,
					byte.MaxValue
				},
				{
					12,
					44,
					byte.MaxValue
				},
				{
					13,
					45,
					byte.MaxValue
				},
				{
					14,
					46,
					byte.MaxValue
				},
				{
					15,
					47,
					byte.MaxValue
				},
				{
					16,
					48,
					byte.MaxValue
				},
				{
					17,
					49,
					byte.MaxValue
				},
				{
					18,
					50,
					byte.MaxValue
				},
				{
					19,
					51,
					byte.MaxValue
				},
				{
					20,
					52,
					byte.MaxValue
				},
				{
					21,
					53,
					byte.MaxValue
				},
				{
					22,
					54,
					byte.MaxValue
				},
				{
					23,
					55,
					byte.MaxValue
				},
				{
					24,
					56,
					byte.MaxValue
				},
				{
					25,
					57,
					byte.MaxValue
				},
				{
					26,
					58,
					byte.MaxValue
				},
				{
					27,
					59,
					byte.MaxValue
				},
				{
					28,
					60,
					byte.MaxValue
				},
				{
					29,
					61,
					byte.MaxValue
				},
				{
					30,
					62,
					byte.MaxValue
				},
				{
					31,
					63,
					byte.MaxValue
				},
				{
					99,
					98,
					byte.MaxValue
				},
				{
					101,
					100,
					byte.MaxValue
				}
			};
		}

		private void FlushShortMsgStack()
		{
			if (this._shortMsgOnStack != null)
			{
				this.RaiseMessageReceived(MidiMessageEventType.ShortMessage, this._shortMsgOnStack);
				this._shortMsgOnStack = null;
			}
		}

		public int IsPairedControllerMessage(MidiShortMessage msg)
		{
			if (!this.AutoPairController || msg == null || this._controllerPairs == null || msg.StatusType != MIDIStatus.ControlChange)
			{
				return 0;
			}
			int num = 0;
			byte controller = msg.Controller;
			for (int i = 0; i < this._controllerPairs.GetLength(0); i++)
			{
				byte b = this._controllerPairs[i, 0];
				byte b2 = this._controllerPairs[i, 1];
				if (controller == b || controller == b2)
				{
					num = -1;
					if (msg.PreviousShortMessage != null && msg.PreviousShortMessage.StatusType == MIDIStatus.ControlChange && msg.PreviousShortMessage.Channel == msg.Channel)
					{
						byte controller2 = msg.PreviousShortMessage.Controller;
						if (controller2 == b && controller == b2)
						{
							num = 2;
							msg.SetContinuousController(false, true);
						}
						else if (controller2 == b2 && controller == b)
						{
							num = 1;
							msg.SetContinuousController(true, false);
						}
					}
				}
				if (num != 0)
				{
					break;
				}
			}
			return num;
		}

		private void MidiInProc(IntPtr handle, MIDIMessage msg, IntPtr user, IntPtr param1, IntPtr param2)
		{
			if (msg == MIDIMessage.MIM_OPEN)
			{
				this.RaiseMessageReceived(MidiMessageEventType.Opened, null);
				return;
			}
			if (msg == MIDIMessage.MIM_CLOSE)
			{
				this.FlushShortMsgStack();
				this.RaiseMessageReceived(MidiMessageEventType.Closed, null);
				return;
			}
			if (msg == MIDIMessage.MIM_DATA || msg == MIDIMessage.MIM_MOREDATA)
			{
				this._shortMsg = new MidiShortMessage(param1, param2, this._shortMsg);
				if ((this._shortMsg.MessageType & this.MessageFilter) == MIDIMessageType.Unknown)
				{
					this._pairedResult = this.IsPairedControllerMessage(this._shortMsg);
					if (this._pairedResult == 0)
					{
						this.FlushShortMsgStack();
						this.RaiseMessageReceived(MidiMessageEventType.ShortMessage, this._shortMsg);
						return;
					}
					if (this._pairedResult == -1)
					{
						this._shortMsgOnStack = this._shortMsg;
						return;
					}
					this._shortMsgOnStack = null;
					this.RaiseMessageReceived(MidiMessageEventType.ShortMessage, this._shortMsg);
					return;
				}
			}
			else
			{
				if (msg == MIDIMessage.MIM_LONGDATA)
				{
					this.FlushShortMsgStack();
					this._sysexMsg = new MidiSysExMessage(true, handle, param1, this._sysexMsg);
					if (this._sysexMsg.IsDone && (this._sysexMsg.MessageType & this.MessageFilter) == MIDIMessageType.Unknown)
					{
						this.RaiseMessageReceived(MidiMessageEventType.SystemExclusive, this._sysexMsg);
					}
					this.AddSysExBuffer();
					return;
				}
				if (msg == MIDIMessage.MIM_ERROR)
				{
					this.FlushShortMsgStack();
					if (this.ProcessErrorMessages)
					{
						MidiShortMessage midiShortMessage = new MidiShortMessage(param1, param2);
						if ((midiShortMessage.MessageType & this.MessageFilter) == MIDIMessageType.Unknown)
						{
							this.RaiseMessageReceived(MidiMessageEventType.ShortMessageError, midiShortMessage);
							return;
						}
					}
				}
				else if (msg == MIDIMessage.MIM_LONGERROR)
				{
					this.FlushShortMsgStack();
					MidiSysExMessage midiSysExMessage = new MidiSysExMessage(true, handle, param1);
					if (midiSysExMessage.IsDone && this.ProcessErrorMessages && (midiSysExMessage.MessageType & this.MessageFilter) == MIDIMessageType.Unknown)
					{
						this.RaiseMessageReceived(MidiMessageEventType.SystemExclusiveError, midiSysExMessage);
					}
					this.AddSysExBuffer();
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

		private MidiShortMessage _shortMsg;

		private MidiSysExMessage _sysexMsg;

		private int _sysexBufferSize = 256;

		private MIDIINPROC _midiInProc;

		private IntPtr _user = IntPtr.Zero;

		private bool _started;

		private bool _closing;

		private MIDIError _lastError;

		private bool _autoPairController;

		private byte[,] _controllerPairs;

		private MIDIMessageType _messageFilter;

		private bool _processErrorMessages;

		private MidiShortMessage _shortMsgOnStack;

		private int _pairedResult;
	}
}
