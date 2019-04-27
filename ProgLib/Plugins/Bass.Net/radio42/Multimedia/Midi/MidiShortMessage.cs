using System;
using System.Security;

namespace radio42.Multimedia.Midi
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class MidiShortMessage : IFormattable
	{
		public MidiShortMessage()
		{
			this.InitID();
		}

		public MidiShortMessage(IntPtr param1, IntPtr param2)
		{
			this.InitID();
			this.InitData((uint)param1.ToInt32());
			this.InitTimestamp((uint)param2.ToInt32());
		}

		public MidiShortMessage(IntPtr param1, IntPtr param2, MidiShortMessage previous)
		{
			this.InitID();
			this.InitData((uint)param1.ToInt32());
			this.InitTimestamp((uint)param2.ToInt32());
			if (previous != null && this._status < 247 && this.StatusType == previous.StatusType)
			{
				previous.PreviousShortMessage = null;
				this._previous = previous;
			}
		}

		public MidiShortMessage(MIDIStatus status, byte channel, byte data1, byte data2, long timestamp)
		{
			this.InitID();
			this.BuildMessage(status, channel, data1, data2, timestamp);
		}

		public MidiShortMessage(byte status, byte data1, byte data2, byte data3, long timestamp)
		{
			this.InitID();
			this.BuildMessage(status, data1, data2, data3, timestamp);
		}

		public MidiShortMessage(MIDIStatus status, byte channel, int data, long timestamp)
		{
			this.InitID();
			this.BuildMessage(status, channel, data, timestamp);
		}

		public MidiShortMessage(byte status, int data, long timestamp)
		{
			this.InitID();
			this.BuildMessage(status, data, timestamp);
		}

		private void InitData(uint message)
		{
			this._status = (byte)(message & 255u);
			this._data1 = (byte)(message >> 8 & 255u);
			this._data2 = (byte)(message >> 16 & 255u);
			if (this.StatusType == MIDIStatus.None)
			{
				this._data3 = (byte)(message >> 24 & 255u);
			}
		}

		private void InitTimestamp(uint timestamp)
		{
			this._timestamp = timestamp;
		}

		private void InitID()
		{
			this._myid = MidiShortMessage._id;
			if (MidiShortMessage._id == 9223372036854775807L)
			{
				MidiShortMessage._id = 0L;
				return;
			}
			MidiShortMessage._id += 1L;
		}

		public long ID
		{
			get
			{
				return this._myid;
			}
		}

		public MidiShortMessage PreviousShortMessage
		{
			get
			{
				return this._previous;
			}
			set
			{
				this._previous = value;
			}
		}

		public byte Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
				if (this.StatusType == MIDIStatus.None)
				{
					this._data3 = 0;
				}
			}
		}

		public byte Data1
		{
			get
			{
				return this._data1;
			}
			set
			{
				this._data1 = value;
			}
		}

		public byte Data2
		{
			get
			{
				return this._data2;
			}
			set
			{
				this._data2 = value;
			}
		}

		public short PairedData2
		{
			get
			{
				if (!this.IsSetAsContinuousController)
				{
					return (short)(this._data2 & 127);
				}
				if (this._thisIsMSB)
				{
					return MidiShortMessage.GetPairedData(this._data2, this.PreviousShortMessage.Data2);
				}
				return MidiShortMessage.GetPairedData(this.PreviousShortMessage.Data2, this._data2);
			}
		}

		public byte Data3
		{
			get
			{
				return this._data3;
			}
			set
			{
				if (this.StatusType == MIDIStatus.None)
				{
					this._data3 = value;
					return;
				}
				this._data3 = 0;
			}
		}

		public TimeSpan Timespan
		{
			get
			{
				return TimeSpan.FromMilliseconds(this._timestamp);
			}
			set
			{
				this._timestamp = value.TotalMilliseconds;
			}
		}

		public long Timestamp
		{
			get
			{
				return (long)this._timestamp;
			}
			set
			{
				this._timestamp = (double)value;
			}
		}

		public IntPtr TimestampAsIntPtr
		{
			get
			{
				return new IntPtr((int)this.Timestamp);
			}
			set
			{
				this.Timestamp = (long)value.ToInt32();
			}
		}

		public int Message
		{
			get
			{
				int num = 0;
				num |= (int)this._status;
				num |= (int)this._data1 << 8;
				num |= (int)this._data2 << 16;
				if (this.StatusType == MIDIStatus.None)
				{
					num |= (int)this._data3 << 24;
				}
				return num;
			}
			set
			{
				this._status = (byte)(value & 255);
				this._data1 = (byte)(value >> 8 & 255);
				this._data2 = (byte)(value >> 16 & 255);
				if (this.StatusType == MIDIStatus.None)
				{
					this._data3 = (byte)(value >> 24 & 255);
					return;
				}
				this._data3 = 0;
			}
		}

		public IntPtr MessageAsIntPtr
		{
			get
			{
				return new IntPtr(this.Message);
			}
			set
			{
				this.Message = value.ToInt32();
			}
		}

		public MIDIMessageType MessageType
		{
			get
			{
				if (this._status >= 128 && this._status <= 239)
				{
					return MIDIMessageType.Channel;
				}
				if (this._status == 240 || this._status == 241 || this._status == 242 || this._status == 243 || this._status == 246 || this._status == 247)
				{
					return MIDIMessageType.SystemCommon;
				}
				if (this._status == 248 || this._status == 249 || this._status == 250 || this._status == 251 || this._status == 252 || this._status == 254 || this._status == 255)
				{
					return MIDIMessageType.SystemRealtime;
				}
				return MIDIMessageType.Unknown;
			}
			set
			{
				this._status |= (byte)value;
			}
		}

		public MIDIStatus StatusType
		{
			get
			{
				MIDIStatus midistatus = MIDIStatus.None;
				try
				{
					if (this._status >= 240)
					{
						midistatus = (MIDIStatus)this._status;
					}
					else
					{
						midistatus = (MIDIStatus)(this._status & 240);
					}
					if (midistatus == MIDIStatus.NoteOn && this.Velocity == 0)
					{
						midistatus = MIDIStatus.NoteOff;
					}
				}
				catch
				{
					midistatus = MIDIStatus.None;
				}
				return midistatus;
			}
			set
			{
				if (value >= MIDIStatus.SystemMsgs)
				{
					this._status = (byte)value;
				}
				else
				{
					this._status = (byte)((value & MIDIStatus.SystemMsgs) | (MIDIStatus)(this._status & 15));
				}
				if (value == MIDIStatus.None)
				{
					this._data3 = 0;
				}
			}
		}

		public byte Channel
		{
			get
			{
				return Convert.ToByte(this._status & 15);
			}
			set
			{
				if (value > 15)
				{
					return;
				}
				this._status = Convert.ToByte((this._status & 240) | (value & 15));
			}
		}

		public byte Note
		{
			get
			{
				return Convert.ToByte(this._data1 & 127);
			}
			set
			{
				this._data1 = Convert.ToByte(value & 127);
			}
		}

		public string NoteString
		{
			get
			{
				return string.Format("{0}{1}", (MIDINote)(this.Note % 12), (int)(this.Note / 12 - 2));
			}
		}

		public byte Velocity
		{
			get
			{
                return Convert.ToByte(this._data2 & 127);
			}
			set
			{
				this._data2 = Convert.ToByte(value & 127);
			}
		}

		public byte Aftertouch
		{
			get
			{
				return Convert.ToByte(this._data2 & 127);
			}
			set
			{
				this._data2 = Convert.ToByte(value & 127);
			}
		}

		public byte ChannelPressure
		{
			get
			{
				return Convert.ToByte(this._data1 & 127);
			}
			set
			{
				this._data1 = Convert.ToByte(value & 127);
			}
		}

		public byte Program
		{
			get
			{
				return Convert.ToByte(this._data1 & 127);
			}
			set
			{
				this._data1 = Convert.ToByte(value & 127);
			}
		}

		public short PitchBend
		{
			get
			{
				return (short)((this._data2 & 127) * 128 + (this._data1 & 127));
			}
			set
			{
				if (value >= 0 && value <= 16383)
				{
					this._data1 = (byte)(value % 128);
					this._data2 = (byte)(value / 128);
				}
			}
		}

		public byte Controller
		{
			get
			{
				return Convert.ToByte(this._data1 & 127);
			}
			set
			{
				this._data1 = Convert.ToByte(value & 127);
			}
		}

		public MIDIControllerType ControllerType
		{
			get
			{
				return (MIDIControllerType)(this._data1 & 127);
			}
			set
			{
				this._data1 = (byte)(value & MIDIControllerType.PolyOperation);
			}
		}

		public bool ThisIsMSB
		{
			get
			{
				return this._thisIsMSB;
			}
		}

		public bool PreviousIsMSB
		{
			get
			{
				return this._previousIsMSB;
			}
		}

		public void SetContinuousController(bool thisIsMSB, bool previousIsMSB)
		{
			if (this.StatusType != MIDIStatus.ControlChange || this.PreviousShortMessage == null || this.PreviousShortMessage.StatusType != MIDIStatus.ControlChange || this.PreviousShortMessage.Channel != this.Channel)
			{
				this._thisIsMSB = false;
				this._previousIsMSB = false;
				return;
			}
			if (thisIsMSB != previousIsMSB)
			{
				this._thisIsMSB = thisIsMSB;
				this._previousIsMSB = previousIsMSB;
				return;
			}
			this._thisIsMSB = false;
			this._previousIsMSB = false;
		}

		public bool IsSetAsContinuousController
		{
			get
			{
				return this.StatusType == MIDIStatus.ControlChange && this.PreviousShortMessage != null && this.PreviousShortMessage.StatusType == MIDIStatus.ControlChange && this.PreviousShortMessage.Channel == this.Channel && this._thisIsMSB != this._previousIsMSB;
			}
		}

		public short ControllerValue
		{
			get
			{
				if (!this.IsSetAsContinuousController)
				{
					return (short)(this._data2 & 127);
				}
				if (this._thisIsMSB)
				{
					return MidiShortMessage.GetPairedData(this._data2, this.PreviousShortMessage.Data2);
				}
				return MidiShortMessage.GetPairedData(this.PreviousShortMessage.Data2, this._data2);
			}
			set
			{
				if (!this.IsSetAsContinuousController)
				{
					this._data2 = (byte)(value & 127);
					return;
				}
				if (this._thisIsMSB)
				{
					this._data2 = Convert.ToByte((byte)(value >> 7) & 127);
					this.PreviousShortMessage.Data2 = (byte)(value & 127);
					return;
				}
				this._data2 = (byte)(value & 127);
				this.PreviousShortMessage.Data2 = Convert.ToByte((byte)(value >> 7) & 127);
			}
		}

		public MIDIMTCType TimeCodeType
		{
			get
			{
				return (MIDIMTCType)(this._data1 >> 4 & 7);
			}
			set
			{
				this._data1 = (byte)((value & (MIDIMTCType)240) | (MIDIMTCType)(this._data1 & 15));
			}
		}

		public byte TimeCodeValue
		{
			get
			{
				return Convert.ToByte(this._data1 & 15);
			}
			set
			{
				this._data1 = Convert.ToByte((this._data1 & 240) | (value & 15));
			}
		}

		public short SongPosition
		{
			get
			{
				return (short)((this._data2 & 127) * 128 + (this._data1 & 127));
			}
			set
			{
				if (value >= 0 && value <= 16383)
				{
					this._data1 = (byte)(value % 128);
					this._data2 = (byte)(value / 128);
				}
			}
		}

		public byte Song
		{
			get
			{
				return Convert.ToByte(this._data1 & 127);
			}
			set
			{
				this._data1 = Convert.ToByte(value & 127);
			}
		}

		public int Data
		{
			get
			{
				if (this.StatusType == MIDIStatus.None)
				{
					return (int)(this._data3 & 127) * 16384 + (int)((this._data2 & 127) * 128) + (int)(this._data1 & 127);
				}
				return (int)((this._data2 & 127) * 128 + (this._data1 & 127));
			}
			set
			{
				if (value >= 0 && value <= 2097151)
				{
					this._data1 = (byte)(value % 128);
					this._data2 = (byte)(value % 16384 / 128);
					if (this.StatusType == MIDIStatus.None)
					{
						this._data3 = (byte)(value / 16384);
					}
				}
			}
		}

		public static short GetPairedData(byte dataMSB, byte dataLSB)
		{
			return (short)((dataMSB & 127) * 128 + (dataLSB & 127));
		}

		public static short GetPairedData1(MidiShortMessage msgMSB, MidiShortMessage msgLSB)
		{
			return (short)((msgMSB.Data1 & 127) * 128 + (msgLSB.Data1 & 127));
		}

		public static short GetPairedData2(MidiShortMessage msgMSB, MidiShortMessage msgLSB)
		{
			return (short)((msgMSB.Data2 & 127) * 128 + (msgLSB.Data2 & 127));
		}

		public void BuildMessage(MIDIStatus status, byte channel, byte data1, byte data2, long timestamp)
		{
			if (channel > 15)
			{
				channel = 15;
			}
			this._status = (byte)(status + channel);
			this._data1 = data1;
			this._data2 = data2;
			this._timestamp = (double)timestamp;
		}

		public void BuildMessage(byte status, byte data1, byte data2, byte data3, long timestamp)
		{
			this._status = status;
			this._data1 = data1;
			this._data2 = data2;
			this._data3 = data3;
			this._timestamp = (double)timestamp;
		}

		public void BuildMessage(MIDIStatus status, byte channel, int data, long timestamp)
		{
			if (channel > 15)
			{
				channel = 15;
			}
			this._status = (byte)(status + channel);
			this.Data = data;
			this._timestamp = (double)timestamp;
		}

		public void BuildMessage(byte status, int data, long timestamp)
		{
			this._status = status;
			this.Data = data;
			this._timestamp = (double)timestamp;
		}

		public void BuildMessage(int message, long timestamp)
		{
			this._status = (byte)(message & 255);
			this._data1 = (byte)(message >> 8 & 255);
			this._data2 = (byte)(message >> 16 & 255);
			this._data3 = (byte)(message >> 24 & 255);
			this._timestamp = (double)timestamp;
			if (this.StatusType != MIDIStatus.None)
			{
				this._data3 = 0;
			}
		}

		public void BuildMessage(long message, long timestamp)
		{
			this._status = (byte)(message & 255L);
			this._data1 = (byte)(message >> 8 & 255L);
			this._data2 = (byte)(message >> 16 & 255L);
			this._data3 = (byte)(message >> 24 & 255L);
			this._timestamp = (double)timestamp;
			if (this.StatusType != MIDIStatus.None)
			{
				this._data3 = 0;
			}
		}

		public override string ToString()
		{
			return this.ToString("G", null);
		}

		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			string text = format;
			if (format == null || format.Length == 0)
			{
				format = "G";
			}
			string text2;
			if (format.Length == 1)
			{
				uint num = PrivateImplementationDetails.ComputeStringHash(format);
				if (num <= 3691781268u)
				{
					if (num <= 3356228888u)
					{
						if (num <= 3255563174u)
						{
							if (num != 3238785555u)
							{
								if (num != 3255563174u)
								{
									goto IL_367;
								}
								if (!(format == "G"))
								{
									goto IL_367;
								}
								if (this.MessageType == MIDIMessageType.Channel)
								{
									text2 = "{T}\t{M} {C}\t{S}\t{D}";
									goto IL_371;
								}
								text2 = "{T}\t{M}\t{S}\t{D}";
								goto IL_371;
							}
							else if (!(format == "D"))
							{
								goto IL_367;
							}
						}
						else if (num != 3289118412u)
						{
							if (num != 3322673650u)
							{
								if (num != 3356228888u)
								{
									goto IL_367;
								}
								if (!(format == "M"))
								{
									goto IL_367;
								}
							}
							else if (!(format == "C"))
							{
								goto IL_367;
							}
						}
						else if (!(format == "A"))
						{
							goto IL_367;
						}
					}
					else if (num <= 3490449840u)
					{
						if (num != 3440116983u)
						{
							if (num != 3490449840u)
							{
								goto IL_367;
							}
							if (!(format == "U"))
							{
								goto IL_367;
							}
						}
						else if (!(format == "H"))
						{
							goto IL_367;
						}
					}
					else if (num != 3507227459u)
					{
						if (num != 3591115554u)
						{
							if (num != 3691781268u)
							{
								goto IL_367;
							}
							if (!(format == "Y"))
							{
								goto IL_367;
							}
							text2 = "{A} {H}";
							goto IL_371;
						}
						else if (!(format == "S"))
						{
							goto IL_367;
						}
					}
					else if (!(format == "T"))
					{
						goto IL_367;
					}
				}
				else if (num <= 3893112696u)
				{
					if (num <= 3775669363u)
					{
						if (num != 3708558887u)
						{
							if (num != 3775669363u)
							{
								goto IL_367;
							}
							if (!(format == "d"))
							{
								goto IL_367;
							}
						}
						else
						{
							if (!(format == "X"))
							{
								goto IL_367;
							}
							text2 = "{U} {A} {H}";
							goto IL_371;
						}
					}
					else if (num != 3792446982u)
					{
						if (num != 3826002220u)
						{
							if (num != 3893112696u)
							{
								goto IL_367;
							}
							if (!(format == "m"))
							{
								goto IL_367;
							}
						}
						else if (!(format == "a"))
						{
							goto IL_367;
						}
					}
					else
					{
						if (!(format == "g"))
						{
							goto IL_367;
						}
						if (this.MessageType == MIDIMessageType.Channel)
						{
							text2 = "{t}\t{m} {C}\t{s}\t{d}";
							goto IL_371;
						}
						text2 = "{t}\t{m}\t{s}\t{d}";
						goto IL_371;
					}
				}
				else if (num <= 4044111267u)
				{
					if (num != 3977000791u)
					{
						if (num != 4027333648u)
						{
							if (num != 4044111267u)
							{
								goto IL_367;
							}
							if (!(format == "t"))
							{
								goto IL_367;
							}
						}
						else if (!(format == "u"))
						{
							goto IL_367;
						}
					}
					else if (!(format == "h"))
					{
						goto IL_367;
					}
				}
				else if (num != 4127999362u)
				{
					if (num != 4228665076u)
					{
						if (num != 4245442695u)
						{
							goto IL_367;
						}
						if (!(format == "x"))
						{
							goto IL_367;
						}
						text2 = "{u} {a} {h}";
						goto IL_371;
					}
					else
					{
						if (!(format == "y"))
						{
							goto IL_367;
						}
						text2 = "{a} {h}";
						goto IL_371;
					}
				}
				else if (!(format == "s"))
				{
					goto IL_367;
				}
				text2 = format;
				goto IL_371;
				IL_367:
				text2 = string.Empty;
			}
			else
			{
				text2 = format;
			}
			IL_371:
			if (text2 != string.Empty)
			{
				text = this.FormatString(text2, formatProvider);
			}
			return text.Trim();
		}

		private string FormatString(string format, IFormatProvider formatProvider)
		{
			return format.Replace("{G}", (this.MessageType == MIDIMessageType.Channel) ? "{T}\t{M} {C}\t{S}\t{D}" : "{T}\t{M}\t{S}\t{D}").Replace("{g}", (this.MessageType == MIDIMessageType.Channel) ? "{t}\t{m} {C}\t{s}\t{d}" : "{t}\t{m}\t{s}\t{d}").Replace("{X}", "{U} {A} {H}").Replace("{x}", "{u} {a} {h}").Replace("{Y}", "{A} {H}").Replace("{y}", "{a} {h}").Replace("{T}", this.FormatTimestamp("T", formatProvider)).Replace("{t}", this.FormatTimestamp("t", formatProvider)).Replace("{U}", this.FormatTimestamp("U", formatProvider)).Replace("{u}", this.FormatTimestamp("u", formatProvider)).Replace("{M}", this.FormatMessageType("M", formatProvider)).Replace("{m}", this.FormatMessageType("m", formatProvider)).Replace("{S}", this.FormatStatus("S", formatProvider)).Replace("{s}", this.FormatStatus("s", formatProvider)).Replace("{A}", this.FormatStatus("A", formatProvider)).Replace("{a}", this.FormatStatus("a", formatProvider)).Replace("{C}", this.FormatChannel("C", formatProvider)).Replace("{D}", this.FormatData("D", formatProvider)).Replace("{d}", this.FormatData("d", formatProvider)).Replace("{H}", this.FormatData("H", formatProvider)).Replace("{h}", this.FormatData("h", formatProvider));
		}

		private string FormatData(string format, IFormatProvider formatProvider)
		{
			string result = string.Empty;
			uint num = PrivateImplementationDetails.ComputeStringHash(format);
			if (num <= 3708558887u)
			{
				if (num <= 3255563174u)
				{
					if (num != 3238785555u)
					{
						if (num != 3255563174u)
						{
							return result;
						}
						if (!(format == "G"))
						{
							return result;
						}
					}
					else if (!(format == "D"))
					{
						return result;
					}
					switch (this.MessageType)
					{
					case MIDIMessageType.Unknown:
						result = string.Format(formatProvider, "Data1={0}, Data2={1}", new object[]
						{
							this.Data1,
							this.Data2
						});
						break;
					case MIDIMessageType.Channel:
					{
						MIDIStatus statusType = this.StatusType;
						if (statusType <= MIDIStatus.Aftertouch)
						{
							if (statusType != MIDIStatus.NoteOff)
							{
								if (statusType != MIDIStatus.NoteOn)
								{
									if (statusType == MIDIStatus.Aftertouch)
									{
										result = string.Format(formatProvider, "Key={0}, Pressure={1}", new object[]
										{
											this.NoteString,
											this.Aftertouch
										});
									}
								}
								else
								{
									result = string.Format(formatProvider, "Key={0}, Velocity={1}", new object[]
									{
										this.NoteString,
										this.Velocity
									});
								}
							}
							else
							{
								result = string.Format(formatProvider, "Key={0}, Velocity={1}", new object[]
								{
									this.NoteString,
									this.Velocity
								});
							}
						}
						else if (statusType <= MIDIStatus.ProgramChange)
						{
							if (statusType != MIDIStatus.ControlChange)
							{
								if (statusType == MIDIStatus.ProgramChange)
								{
									result = string.Format(formatProvider, "Program={0}", new object[]
									{
										this.Program
									});
								}
							}
							else
							{
								result = string.Format(formatProvider, "Controller={0}, Value={1}", new object[]
								{
									this.ControllerType,
									this.ControllerValue
								});
							}
						}
						else if (statusType != MIDIStatus.ChannelPressure)
						{
							if (statusType == MIDIStatus.PitchBend)
							{
								result = string.Format(formatProvider, "PitchBend={0}", new object[]
								{
									this.PitchBend
								});
							}
						}
						else
						{
							result = string.Format(formatProvider, "Aftertouch={0}", new object[]
							{
								this.ChannelPressure
							});
						}
						break;
					}
					case MIDIMessageType.SystemCommon:
						switch (this.StatusType)
						{
						case MIDIStatus.MidiTimeCode:
							result = string.Format(formatProvider, "Type={0}, Value={1}", new object[]
							{
								this.TimeCodeType,
								this.TimeCodeValue
							});
							break;
						case MIDIStatus.SongPosition:
							result = string.Format(formatProvider, "Position={0}", new object[]
							{
								this.SongPosition
							});
							break;
						case MIDIStatus.SongSelect:
							result = string.Format(formatProvider, "Song={0}", new object[]
							{
								this.Song
							});
							break;
						case MIDIStatus.TuneRequest:
							result = "";
							break;
						}
						break;
					case MIDIMessageType.SystemRealtime:
						result = "";
						break;
					}
				}
				else
				{
					if (num != 3440116983u)
					{
						if (num != 3691781268u)
						{
							if (num != 3708558887u)
							{
								return result;
							}
							if (!(format == "X"))
							{
								return result;
							}
						}
						else if (!(format == "Y"))
						{
							return result;
						}
					}
					else if (!(format == "H"))
					{
						return result;
					}
					if (this.StatusType == MIDIStatus.None)
					{
						result = string.Format(formatProvider, "0x{0:X02} 0x{1:X02} 0x{2:X02}", new object[]
						{
							this.Data1,
							this.Data2,
							this.Data3
						});
					}
					else
					{
						result = string.Format(formatProvider, "0x{0:X02} 0x{1:X02}", new object[]
						{
							this.Data1,
							this.Data2
						});
					}
				}
			}
			else if (num <= 3792446982u)
			{
				if (num != 3775669363u)
				{
					if (num != 3792446982u)
					{
						return result;
					}
					if (!(format == "g"))
					{
						return result;
					}
				}
				else if (!(format == "d"))
				{
					return result;
				}
				switch (this.MessageType)
				{
				case MIDIMessageType.Unknown:
					result = string.Format(formatProvider, "Data1={0}, Data2={1}", new object[]
					{
						this.Data1,
						this.Data2
					});
					break;
				case MIDIMessageType.Channel:
				{
					MIDIStatus statusType = this.StatusType;
					if (statusType <= MIDIStatus.Aftertouch)
					{
						if (statusType != MIDIStatus.NoteOff)
						{
							if (statusType != MIDIStatus.NoteOn)
							{
								if (statusType == MIDIStatus.Aftertouch)
								{
									result = string.Format(formatProvider, "Key={0}, Pressure={1}", new object[]
									{
										this.Note,
										this.Aftertouch
									});
								}
							}
							else
							{
								result = string.Format(formatProvider, "Key={0}, Velocity={1}", new object[]
								{
									this.Note,
									this.Velocity
								});
							}
						}
						else
						{
							result = string.Format(formatProvider, "Key={0}, Velocity={1}", new object[]
							{
								this.Note,
								this.Velocity
							});
						}
					}
					else if (statusType <= MIDIStatus.ProgramChange)
					{
						if (statusType != MIDIStatus.ControlChange)
						{
							if (statusType == MIDIStatus.ProgramChange)
							{
								result = string.Format(formatProvider, "Program={0}", new object[]
								{
									this.Program
								});
							}
						}
						else
						{
							result = string.Format(formatProvider, "Controller={0}, Value={1}", new object[]
							{
								this.Controller,
								this.ControllerValue
							});
						}
					}
					else if (statusType != MIDIStatus.ChannelPressure)
					{
						if (statusType == MIDIStatus.PitchBend)
						{
							result = string.Format(formatProvider, "PitchBend={0}", new object[]
							{
								this.PitchBend
							});
						}
					}
					else
					{
						result = string.Format(formatProvider, "Aftertouch={0}", new object[]
						{
							this.ChannelPressure
						});
					}
					break;
				}
				case MIDIMessageType.SystemCommon:
					switch (this.StatusType)
					{
					case MIDIStatus.MidiTimeCode:
						result = string.Format(formatProvider, "Type={0}, Value={1}", new object[]
						{
							(byte)this.TimeCodeType,
							this.TimeCodeValue
						});
						break;
					case MIDIStatus.SongPosition:
						result = string.Format(formatProvider, "Position={0}", new object[]
						{
							this.SongPosition
						});
						break;
					case MIDIStatus.SongSelect:
						result = string.Format(formatProvider, "Song={0}", new object[]
						{
							this.Song
						});
						break;
					case MIDIStatus.TuneRequest:
						result = "";
						break;
					}
					break;
				case MIDIMessageType.SystemRealtime:
					result = "";
					break;
				}
			}
			else
			{
				if (num != 3977000791u)
				{
					if (num != 4228665076u)
					{
						if (num != 4245442695u)
						{
							return result;
						}
						if (!(format == "x"))
						{
							return result;
						}
					}
					else if (!(format == "y"))
					{
						return result;
					}
				}
				else if (!(format == "h"))
				{
					return result;
				}
				if (this.StatusType == MIDIStatus.None)
				{
					result = string.Format(formatProvider, "0x{0:x02} 0x{1:x02} 0x{2:x02}", new object[]
					{
						this.Data1,
						this.Data2,
						this.Data3
					});
				}
				else
				{
					result = string.Format(formatProvider, "0x{0:x02} 0x{1:x02}", new object[]
					{
						this.Data1,
						this.Data2
					});
				}
			}
			return result;
		}

		private string FormatChannel(string format, IFormatProvider formatProvider)
		{
			string result = string.Empty;
			if (this.MessageType == MIDIMessageType.Channel && (format == "G" || format == "M" || format == "C" || format == "g" || format == "m" || format == "c"))
			{
				result = string.Format(formatProvider, "{0}", new object[]
				{
					this.Channel
				});
			}
			return result;
		}

		private string FormatStatus(string format, IFormatProvider formatProvider)
		{
			string result = string.Empty;
			uint num = PrivateImplementationDetails.ComputeStringHash(format);
			if (num <= 3708558887u)
			{
				if (num <= 3289118412u)
				{
					if (num != 3255563174u)
					{
						if (num != 3289118412u)
						{
							return result;
						}
						if (!(format == "A"))
						{
							return result;
						}
						goto IL_1AB;
					}
					else if (!(format == "G"))
					{
						return result;
					}
				}
				else if (num != 3591115554u)
				{
					if (num != 3691781268u)
					{
						if (num != 3708558887u)
						{
							return result;
						}
						if (!(format == "X"))
						{
							return result;
						}
						goto IL_1AB;
					}
					else
					{
						if (!(format == "Y"))
						{
							return result;
						}
						goto IL_1AB;
					}
				}
				else if (!(format == "S"))
				{
					return result;
				}
				return string.Format(formatProvider, "{0}", new object[]
				{
					this.StatusType
				});
				IL_1AB:
				result = string.Format(formatProvider, "0x{0:X02}", new object[]
				{
					this.Status
				});
			}
			else
			{
				if (num <= 3826002220u)
				{
					if (num != 3792446982u)
					{
						if (num != 3826002220u)
						{
							return result;
						}
						if (!(format == "a"))
						{
							return result;
						}
						goto IL_1CD;
					}
					else if (!(format == "g"))
					{
						return result;
					}
				}
				else if (num != 4127999362u)
				{
					if (num != 4228665076u)
					{
						if (num != 4245442695u)
						{
							return result;
						}
						if (!(format == "x"))
						{
							return result;
						}
						goto IL_1CD;
					}
					else
					{
						if (!(format == "y"))
						{
							return result;
						}
						goto IL_1CD;
					}
				}
				else if (!(format == "s"))
				{
					return result;
				}
				return string.Format(formatProvider, "{0}", new object[]
				{
					(byte)this.StatusType
				});
				IL_1CD:
				result = string.Format(formatProvider, "0x{0:x02}", new object[]
				{
					this.Status
				});
			}
			return result;
		}

		private string FormatMessageType(string format, IFormatProvider formatProvider)
		{
			string result = string.Empty;
			if (!(format == "G") && !(format == "M"))
			{
				if (format == "g" || format == "m")
				{
					result = string.Format(formatProvider, "{0}", new object[]
					{
						(byte)this.MessageType
					});
				}
			}
			else
			{
				result = string.Format(formatProvider, "{0}", new object[]
				{
					this.MessageType
				});
			}
			return result;
		}

		private string FormatTimestamp(string format, IFormatProvider formatProvider)
		{
			string result = string.Empty;
			uint num = PrivateImplementationDetails.ComputeStringHash(format);
			if (num <= 3708558887u)
			{
				if (num <= 3490449840u)
				{
					if (num != 3255563174u)
					{
						if (num != 3490449840u)
						{
							return result;
						}
						if (!(format == "U"))
						{
							return result;
						}
						goto IL_135;
					}
					else if (!(format == "G"))
					{
						return result;
					}
				}
				else if (num != 3507227459u)
				{
					if (num != 3708558887u)
					{
						return result;
					}
					if (!(format == "X"))
					{
						return result;
					}
					goto IL_135;
				}
				else if (!(format == "T"))
				{
					return result;
				}
				return this.Timespan.ToString();
				IL_135:
				result = "0x" + this.Timestamp.ToString("X08", formatProvider);
			}
			else
			{
				if (num <= 4027333648u)
				{
					if (num != 3792446982u)
					{
						if (num != 4027333648u)
						{
							return result;
						}
						if (!(format == "u"))
						{
							return result;
						}
						goto IL_156;
					}
					else if (!(format == "g"))
					{
						return result;
					}
				}
				else if (num != 4044111267u)
				{
					if (num != 4245442695u)
					{
						return result;
					}
					if (!(format == "x"))
					{
						return result;
					}
					goto IL_156;
				}
				else if (!(format == "t"))
				{
					return result;
				}
				return this.Timestamp.ToString(formatProvider);
				IL_156:
				result = "0x" + this.Timestamp.ToString("x08", formatProvider);
			}
			return result;
		}

		private byte _status;

		private byte _data1;

		private byte _data2;

		private byte _data3;

		private double _timestamp;

		private MidiShortMessage _previous;

		private static long _id;

		private long _myid;

		private bool _thisIsMSB;

		private bool _previousIsMSB;
	}
}
