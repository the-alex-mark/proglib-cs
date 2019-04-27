using System;
using System.Runtime.InteropServices;
using System.Text;

namespace radio42.Multimedia.Midi
{
	[Serializable]
	public sealed class MIDI_HEADER
	{
		public byte[] Data
		{
			get
			{
				return this._data;
			}
			set
			{
				if (!this.IsPrepared)
				{
					this._data = value;
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
				if (!this.IsPrepared)
				{
					this._user = value;
				}
			}
		}

		public MIDIHeader Flags
		{
			get
			{
				return this._flags;
			}
		}

		public IntPtr HeaderPtr
		{
			get
			{
				return this._headerPtr;
			}
		}

		public MIDI_HEADER(int size)
		{
			if (size >= 2 && size <= 65536)
			{
				this._data = new byte[size];
				return;
			}
			this._data = new byte[256];
		}

		public MIDI_HEADER(byte[] data)
		{
			this._data = data;
		}

		public MIDI_HEADER(string data)
		{
			int length = data.Length;
			this._data = new byte[length + 1];
			for (int i = 0; i < length; i++)
			{
				this._data[i] = (byte)data[i];
			}
			this._data[length] = 0;
		}

		public MIDI_HEADER(IntPtr headerPtr)
		{
			if (headerPtr == IntPtr.Zero)
			{
				return;
			}
			this._headerPtr = headerPtr;
			MIDIHDR midihdr = null;
			try
			{
				midihdr = (MIDIHDR)Marshal.PtrToStructure(headerPtr, typeof(MIDIHDR));
			}
			catch
			{
				midihdr = null;
			}
			if (midihdr != null)
			{
				this._data = midihdr.GetData();
				this._flags = midihdr.flags;
				this._user = midihdr.user;
			}
		}

		public bool IsDone
		{
			get
			{
				return (this._flags & MIDIHeader.MHDR_DONE) > MIDIHeader.MHDR_NONE;
			}
		}

		public bool IsPrepared
		{
			get
			{
				return (this._flags & MIDIHeader.MHDR_PREPARED) > MIDIHeader.MHDR_NONE;
			}
		}

		public bool IsStreamBuffer
		{
			get
			{
				return (this._flags & MIDIHeader.MHDR_ISSTRM) > MIDIHeader.MHDR_NONE;
			}
		}

		public override string ToString()
		{
			if (this.Data == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(this.Data.Length);
			foreach (byte b in this.Data)
			{
				stringBuilder.Append(string.Format("{0:X2} ", b));
			}
			return stringBuilder.ToString();
		}

		public bool Prepare(bool input, IntPtr handle)
		{
			if (this._headerPtr != IntPtr.Zero)
			{
				this.Unprepare(input, handle);
			}
			IntPtr headerPtr = IntPtr.Zero;
			MIDIHDR header = new MIDIHDR();
			if (this.InitData(this._data, header))
			{
				headerPtr = this.AllocHeader(header);
				if (!this.PrepareHeader(input, handle, headerPtr))
				{
					headerPtr = IntPtr.Zero;
				}
			}
			this._headerPtr = headerPtr;
			return this._headerPtr != IntPtr.Zero;
		}

		public void Unprepare(bool input, IntPtr handle)
		{
			this.UnprepareHeader(input, handle, this._headerPtr);
		}

		private IntPtr AllocHeader(MIDIHDR header)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MIDIHDR)));
			}
			catch (Exception)
			{
				try
				{
					if (header.data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(header.data);
					}
				}
				catch
				{
				}
				header.data = IntPtr.Zero;
			}
			try
			{
				Marshal.StructureToPtr(header, intPtr, true);
			}
			catch (Exception)
			{
				try
				{
					if (header.data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(header.data);
					}
				}
				catch
				{
				}
				header.data = IntPtr.Zero;
				try
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(intPtr);
					}
				}
				catch
				{
				}
				intPtr = IntPtr.Zero;
			}
			return intPtr;
		}

		private bool InitData(byte[] buffer, MIDIHDR header)
		{
			if (buffer == null || header == null)
			{
				return false;
			}
			try
			{
				header.Reset();
				header.bufferLength = buffer.Length;
				header.data = Marshal.AllocHGlobal(buffer.Length);
				if (header.data != IntPtr.Zero)
				{
					for (int i = 0; i < buffer.Length; i++)
					{
						Marshal.WriteByte(header.data, i, buffer[i]);
					}
				}
			}
			catch
			{
				header.Reset();
			}
			return header.data != IntPtr.Zero;
		}

		private bool PrepareHeader(bool input, IntPtr handle, IntPtr headerPtr)
		{
			MIDIError midierror = MIDIError.MIDI_OK;
			if (headerPtr != IntPtr.Zero)
			{
				if (input)
				{
					midierror = Midi.MIDI_InPrepareHeader(handle, headerPtr);
				}
				else
				{
					midierror = Midi.MIDI_OutPrepareHeader(handle, headerPtr);
				}
			}
			if (midierror != MIDIError.MIDI_OK)
			{
				MIDIHDR midihdr = null;
				try
				{
					midihdr = (MIDIHDR)Marshal.PtrToStructure(headerPtr, typeof(MIDIHDR));
				}
				catch
				{
				}
				if (midihdr != null)
				{
					try
					{
						if (midihdr.data != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(midihdr.data);
						}
					}
					catch
					{
					}
					midihdr.data = IntPtr.Zero;
					try
					{
						if (headerPtr != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(headerPtr);
						}
					}
					catch
					{
					}
				}
			}
			return midierror == MIDIError.MIDI_OK;
		}

		private void UnprepareHeader(bool input, IntPtr handle, IntPtr headerPtr)
		{
			if (headerPtr == IntPtr.Zero)
			{
				return;
			}
			MIDIHDR midihdr = null;
			try
			{
				midihdr = (MIDIHDR)Marshal.PtrToStructure(headerPtr, typeof(MIDIHDR));
			}
			catch
			{
			}
			if (midihdr != null)
			{
				try
				{
					if (input)
					{
						Midi.MIDI_InUnprepareHeader(handle, headerPtr);
					}
					else
					{
						Midi.MIDI_OutUnprepareHeader(handle, headerPtr);
					}
				}
				catch
				{
				}
				try
				{
					if (midihdr.data != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(midihdr.data);
					}
				}
				catch
				{
				}
				try
				{
					Marshal.FreeHGlobal(headerPtr);
				}
				catch
				{
				}
			}
		}

		private byte[] _data;

		private IntPtr _user = IntPtr.Zero;

		private MIDIHeader _flags;

		private IntPtr _headerPtr = IntPtr.Zero;
	}
}
