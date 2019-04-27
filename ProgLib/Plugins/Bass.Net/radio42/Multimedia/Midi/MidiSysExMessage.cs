using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace radio42.Multimedia.Midi
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class MidiSysExMessage
	{
		public MidiSysExMessage(bool input, IntPtr handle)
		{
			this.InitID();
			this._input = input;
			this._device = handle;
		}

		public MidiSysExMessage(bool input, IntPtr handle, IntPtr headerPtr)
		{
			this.InitID();
			this._input = input;
			this._device = handle;
			MIDI_HEADER midi_HEADER = new MIDI_HEADER(headerPtr);
			if (midi_HEADER.IsDone)
			{
				this._data = midi_HEADER.Data;
				this._user = midi_HEADER.User;
			}
			midi_HEADER.Unprepare(input, handle);
		}

		public MidiSysExMessage(bool input, IntPtr handle, IntPtr headerPtr, MidiSysExMessage previous)
		{
			this.InitID();
			this._input = input;
			this._device = handle;
			MIDI_HEADER midi_HEADER = new MIDI_HEADER(headerPtr);
			if (midi_HEADER.IsDone)
			{
				this._user = midi_HEADER.User;
				if (previous == null || previous.IsDone)
				{
					this._data = midi_HEADER.Data;
				}
				else
				{
					byte[] data = midi_HEADER.Data;
					if (data == null && previous.Message != null)
					{
						this._data = new byte[previous.Message.Length];
						Array.Copy(previous.Message, 0, this._data, 0, previous.Message.Length);
					}
					else if (previous.Message != null)
					{
						this._data = new byte[data.Length + previous.Message.Length];
						Array.Copy(previous.Message, 0, this._data, 0, previous.Message.Length);
						Array.Copy(data, 0, this._data, previous.Message.Length, data.Length);
					}
					else
					{
						this._data = data;
					}
				}
			}
			midi_HEADER.Unprepare(input, handle);
		}

		private void InitID()
		{
			this._myid = MidiSysExMessage._id;
			if (MidiSysExMessage._id == 9223372036854775807L)
			{
				MidiSysExMessage._id = 0L;
				return;
			}
			MidiSysExMessage._id += 1L;
		}

		public long ID
		{
			get
			{
				return this._myid;
			}
		}

		public bool IsInput
		{
			get
			{
				return this._input;
			}
		}

		public IntPtr Device
		{
			get
			{
				return this._device;
			}
		}

		public bool IsPrepared
		{
			get
			{
				return this._headerPtr != IntPtr.Zero;
			}
		}

		public MIDIMessageType MessageType
		{
			get
			{
				return MIDIMessageType.SystemExclusive;
			}
		}

		public IntPtr MessageAsIntPtr
		{
			get
			{
				return this._headerPtr;
			}
		}

		public byte[] Message
		{
			get
			{
				return this._data;
			}
		}

		public int MessageLength
		{
			get
			{
				if (this._data == null)
				{
					return 0;
				}
				return this._data.Length;
			}
		}

		public bool IsDone
		{
			get
			{
				return this._data != null && this.MessageLength >= 2 && this._data[0] == 240 && this._data[this.MessageLength - 1] == 247;
			}
		}

		public IntPtr User
		{
			get
			{
				return this._user;
			}
		}

		public MIDIStatus StatusType
		{
			get
			{
				MIDIStatus result = MIDIStatus.None;
				try
				{
					result = (MIDIStatus)this._data[0];
				}
				catch
				{
					result = MIDIStatus.None;
				}
				return result;
			}
		}

		public short Manufacturer
		{
			get
			{
				short num = short.MinValue;
				try
				{
					int num2 = 1;
					num = (short)this.Read8(ref this._data, ref num2);
					if (num == 0)
					{
						num = Convert.ToInt16(-1 * this.Read16(ref this._data, ref num2));
					}
				}
				catch
				{
					num = short.MinValue;
				}
				return num;
			}
		}

		public bool IsUniversalRealtime
		{
			get
			{
				return this.Manufacturer == 127;
			}
		}

		public bool IsUniversalNonRealtime
		{
			get
			{
				return this.Manufacturer == 126;
			}
		}

		public byte UniversalChannel
		{
			get
			{
				byte result = byte.MaxValue;
				try
				{
					int num = 1;
					byte b = this.Read8(ref this._data, ref num);
					if (b == 126 || b == 127)
					{
						result = this.Read8(ref this._data, ref num);
					}
				}
				catch
				{
					result = byte.MaxValue;
				}
				return result;
			}
		}

		public byte UniversalSubID
		{
			get
			{
				byte result = byte.MaxValue;
				try
				{
					int num = 1;
					byte b = this.Read8(ref this._data, ref num);
					if (b == 126 || b == 127)
					{
						num = 3;
						result = this.Read8(ref this._data, ref num);
					}
				}
				catch
				{
					result = byte.MaxValue;
				}
				return result;
			}
		}

		public byte UniversalSubID2
		{
			get
			{
				byte result = byte.MaxValue;
				try
				{
					int num = 1;
					byte b = this.Read8(ref this._data, ref num);
					if (b == 126 || b == 127)
					{
						num = 4;
						result = this.Read8(ref this._data, ref num);
					}
				}
				catch
				{
					result = byte.MaxValue;
				}
				return result;
			}
		}

		public byte MMCDeviceID
		{
			get
			{
				byte result = byte.MaxValue;
				try
				{
					int num = 1;
					if (this.Read8(ref this._data, ref num) == 127)
					{
						result = this.Read8(ref this._data, ref num);
					}
				}
				catch
				{
					result = byte.MaxValue;
				}
				return result;
			}
		}

		public byte MMCCommand
		{
			get
			{
				byte result = byte.MaxValue;
				try
				{
					int num = 1;
					if (this.Read8(ref this._data, ref num) == 127)
					{
						num = 4;
						result = this.Read8(ref this._data, ref num);
					}
				}
				catch
				{
					result = byte.MaxValue;
				}
				return result;
			}
		}

		public bool CreateBuffer(int size)
		{
			if (size < 2 || size > 65536 || this.IsPrepared)
			{
				return false;
			}
			this._data = new byte[size];
			return true;
		}

		public bool CreateBuffer(byte[] data)
		{
			if (data == null || data.Length < 2)
			{
				return false;
			}
			this._data = new byte[data.Length];
			data.CopyTo(this._data, 0);
			return true;
		}

		public bool CreateBuffer(IEnumerable<byte> data)
		{
			if (data == null || data.Count<byte>() < 2)
			{
				return false;
			}
			this._data = data.ToArray<byte>();
			return true;
		}

		public bool Prepare()
		{
			return this.Prepare(IntPtr.Zero);
		}

		public bool Prepare(IntPtr user)
		{
			this._user = user;
			MIDI_HEADER midi_HEADER = new MIDI_HEADER(this._data);
			midi_HEADER.User = this._user;
			if (midi_HEADER.Prepare(this._input, this._device))
			{
				this._headerPtr = midi_HEADER.HeaderPtr;
			}
			return this._headerPtr != IntPtr.Zero;
		}

		public bool Send()
		{
			if (this._headerPtr == IntPtr.Zero)
			{
				return false;
			}
			MIDIError midierror;
			if (this._input)
			{
				midierror = Midi.MIDI_InAddBuffer(this._device, this._headerPtr);
			}
			else
			{
				midierror = Midi.MIDI_OutLongMsg(this._device, this._headerPtr);
			}
			return midierror == MIDIError.MIDI_OK;
		}

		public bool Validate()
		{
			return this.Validate(this._data);
		}

		public override string ToString()
		{
			if (this.Message == null || this.MessageLength <= 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(this.MessageLength);
			foreach (byte b in this.Message)
			{
				stringBuilder.Append(string.Format("{0:X2} ", b));
			}
			return stringBuilder.ToString();
		}

		public byte MessageRead(ref int offset)
		{
			byte result = this._data[offset];
			offset++;
			return result;
		}

		public void MessageWrite(ref int offset, byte value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this._data[offset] = value;
			offset++;
		}

		public void MessageWriteSoX()
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this._data[0] = 240;
		}

		public void MessageWriteEoX()
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this._data[this._data.Length - 1] = 247;
		}

		public byte MessageRead8(ref int offset)
		{
			return this.Read8(ref this._data, ref offset);
		}

		public void MessageWrite8(ref int offset, byte value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write8(ref this._data, ref offset, value);
		}

		public byte MessageRead8Wave(ref int offset)
		{
			return this.Read8Wave(ref this._data, ref offset);
		}

		public void MessageWrite8Wave(ref int offset, byte value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write8Wave(ref this._data, ref offset, value);
		}

		public short MessageRead16(ref int offset)
		{
			return this.Read16(ref this._data, ref offset);
		}

		public void MessageWrite16(ref int offset, short value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write16(ref this._data, ref offset, value);
		}

		public short MessageRead16Wave(ref int offset)
		{
			return this.Read16Wave(ref this._data, ref offset);
		}

		public void MessageWrite16Wave(ref int offset, short value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write16Wave(ref this._data, ref offset, value);
		}

		public int MessageRead24(ref int offset)
		{
			return (int)this.Read16(ref this._data, ref offset);
		}

		public void MessageWrite24(ref int offset, int value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write24(ref this._data, ref offset, value);
		}

		public int MessageRead24Wave(ref int offset)
		{
			return this.Read24Wave(ref this._data, ref offset);
		}

		public void MessageWrite16Wave(ref int offset, int value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write24Wave(ref this._data, ref offset, value);
		}

		public int MessageRead32(ref int offset)
		{
			return (int)this.Read16(ref this._data, ref offset);
		}

		public void MessageWrite32(ref int offset, int value)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				return;
			}
			this.Write32(ref this._data, ref offset, value);
		}

		private byte Read8(ref byte[] data, ref int offset)
		{
			byte result = Convert.ToByte(data[offset] & 127);
			offset++;
			return result;
		}

		private void Write8(ref byte[] data, ref int offset, byte value)
		{
			data[offset] = Convert.ToByte(value & 127);
			offset++;
		}

		private short Read16(ref byte[] data, ref int offset)
		{
			short result = (short)((data[offset] & 127) * 128 + (this._data[offset + 1] & 127));
			offset += 2;
			return result;
		}

		private void Write16(ref byte[] data, ref int offset, short value)
		{
			data[offset] = (byte)(value >> 7 & 127);
			data[offset + 1] = (byte)(value & 127);
			offset += 2;
		}

		private int Read24(ref byte[] data, ref int offset)
		{
			int result = (int)(data[offset] & 127) * 16384 + (int)((data[offset + 1] & 127) * 128) + (int)(this._data[offset + 2] & 127);
			offset += 3;
			return result;
		}

		private void Write24(ref byte[] data, ref int offset, int value)
		{
			data[offset] = (byte)(value >> 14 & 127);
			data[offset + 1] = (byte)(value >> 7 & 127);
			data[offset + 2] = (byte)(value & 127);
			offset += 3;
		}

		private int Read32(ref byte[] data, ref int offset)
		{
			int result = (int)(data[offset] & 127) * 2097152 + (int)(data[offset + 1] & 127) * 16384 + (int)((data[offset + 2] & 127) * 128) + (int)(this._data[offset + 3] & 127);
			offset += 4;
			return result;
		}

		private void Write32(ref byte[] data, ref int offset, int value)
		{
			data[offset] = (byte)(value >> 21 & 127);
			data[offset + 1] = (byte)(value >> 14 & 127);
			data[offset + 2] = (byte)(value >> 7 & 127);
			data[offset + 3] = (byte)(value & 127);
			offset += 4;
		}

		private byte Read8Wave(ref byte[] data, ref int offset)
		{
			byte b = (byte)((int)data[offset] << 1 | data[offset + 1] >> 6);
			offset += 2;
			return b;
		}

		private void Write8Wave(ref byte[] data, ref int offset, byte value)
		{
			data[offset] = (byte)(value >> 1 & 127);
			data[offset + 1] = (byte)((int)value << 6 & 127);
			offset += 2;
		}

		private short Read16Wave(ref byte[] data, ref int offset)
		{
			short num = (short)(((int)data[offset] << 9 | (int)data[offset + 1] << 2 | data[offset] >> 5) - 32768);
			offset += 3;
			return num;
		}

		private void Write16Wave(ref byte[] data, ref int offset, short value)
		{
			data[offset] = (byte)(value >> 9 & 127);
			data[offset + 1] = (byte)(value >> 2 & 127);
			data[offset + 2] = (byte)((int)value << 5 & 127);
			offset += 3;
		}

		private int Read24Wave(ref byte[] data, ref int offset)
		{
			int result = ((int)data[offset] << 17 | (int)data[offset + 1] << 10 | (int)data[offset] << 3 | data[offset] >> 4) - 8388608;
			offset += 4;
			return result;
		}

		private void Write24Wave(ref byte[] data, ref int offset, int value)
		{
			data[offset] = (byte)(value >> 17 & 127);
			data[offset + 1] = (byte)(value >> 10 & 127);
			data[offset + 2] = (byte)(value >> 3 & 127);
			data[offset + 3] = (byte)(value << 4 & 127);
			offset += 4;
		}

		private bool Validate(byte[] data)
		{
			if (data == null)
			{
				return false;
			}
			int num = data.Length;
			if (data[0] != 240 || data[num - 1] != 247)
			{
				return false;
			}
			num--;
			bool result = true;
			for (int i = 1; i < num; i--)
			{
				if ((data[i] & 128) != 0)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		private bool _input = true;

		private IntPtr _device = IntPtr.Zero;

		private IntPtr _headerPtr = IntPtr.Zero;

		private IntPtr _user = IntPtr.Zero;

		private byte[] _data;

		private static long _id;

		private long _myid;
	}
}
