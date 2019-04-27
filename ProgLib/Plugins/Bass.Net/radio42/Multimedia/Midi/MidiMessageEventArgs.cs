using System;

namespace radio42.Multimedia.Midi
{
	[Serializable]
	public class MidiMessageEventArgs : EventArgs
	{
		public MidiMessageEventArgs(MidiMessageEventType pEventType, int pDeviceID, IntPtr pDevice, object pMessage)
		{
			this._eventType = pEventType;
			this._deviceID = pDeviceID;
			this._device = pDevice;
			this._message = pMessage;
		}

		public MidiMessageEventType EventType
		{
			get
			{
				return this._eventType;
			}
		}

		public int DeviceID
		{
			get
			{
				return this._deviceID;
			}
		}

		public IntPtr Device
		{
			get
			{
				return this._device;
			}
		}

		public object Message
		{
			get
			{
				return this._message;
			}
		}

		public MidiShortMessage ShortMessage
		{
			get
			{
				if (this.EventType == MidiMessageEventType.ShortMessage || this.EventType == MidiMessageEventType.ShortMessageError)
				{
					return this._message as MidiShortMessage;
				}
				return null;
			}
		}

		public bool IsShortMessage
		{
			get
			{
				return this.EventType == MidiMessageEventType.ShortMessage || this.EventType == MidiMessageEventType.ShortMessageError;
			}
		}

		public MidiSysExMessage SysExMessage
		{
			get
			{
				if (this.EventType == MidiMessageEventType.SystemExclusive || this.EventType == MidiMessageEventType.SystemExclusiveError || this.EventType == MidiMessageEventType.SystemExclusiveDone)
				{
					return this._message as MidiSysExMessage;
				}
				return null;
			}
		}

		public bool IsSysExMessage
		{
			get
			{
				return this.EventType == MidiMessageEventType.SystemExclusive || this.EventType == MidiMessageEventType.SystemExclusiveError || this.EventType == MidiMessageEventType.SystemExclusiveDone;
			}
		}

		private readonly MidiMessageEventType _eventType;

		private readonly object _message;

		private readonly int _deviceID;

		private readonly IntPtr _device;
	}
}
