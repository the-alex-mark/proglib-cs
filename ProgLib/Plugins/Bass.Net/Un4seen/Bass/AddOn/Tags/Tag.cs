using System;
using System.Security;

namespace Un4seen.Bass.AddOn.Tags
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	internal sealed class Tag
	{
		public Tag(int index, string name, WMT_ATTR_DATATYPE type, object val)
		{
			this._index = index;
			this._name = name.TrimEnd(new char[1]);
			this._dataType = type;
			switch (type)
			{
			case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
				this._value = Convert.ToUInt32(val);
				return;
			case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
				this._value = Convert.ToString(val).TrimWithBOM();
				return;
			case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
				this._value = (byte[])val;
				return;
			case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
				this._value = Convert.ToBoolean(val);
				return;
			case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
				this._value = Convert.ToUInt64(val);
				return;
			case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
				this._value = Convert.ToUInt16(val);
				return;
			case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
				this._value = (Guid)val;
				return;
			default:
				throw new ArgumentException("Invalid data type", "type");
			}
		}

		public override string ToString()
		{
			return string.Format("{0}={1}", this._name, this.ValueAsString);
		}

		public int Index
		{
			get
			{
				return this._index;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public WMT_ATTR_DATATYPE DataType
		{
			get
			{
				return this._dataType;
			}
		}

		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				switch (this._dataType)
				{
				case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
					this._value = (uint)value;
					return;
				case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
					this._value = (string)value;
					return;
				case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
					this._value = (byte[])value;
					return;
				case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
					this._value = (bool)value;
					return;
				case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
					this._value = (ulong)value;
					return;
				case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
					this._value = (ushort)value;
					return;
				case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
					this._value = (Guid)value;
					return;
				default:
					return;
				}
			}
		}

		public string ValueAsString
		{
			get
			{
				string result = string.Empty;
				switch (this._dataType)
				{
				case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
					result = ((uint)this._value).ToString();
					break;
				case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
					result = (string)this._value;
					break;
				case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
					result = "[" + ((byte[])this._value).Length.ToString() + " bytes]";
					break;
				case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
					result = ((bool)this._value).ToString();
					break;
				case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
					result = ((ulong)this._value).ToString();
					break;
				case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
					result = ((ushort)this._value).ToString();
					break;
				case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
					result = ((Guid)this._value).ToString();
					break;
				}
				return result;
			}
		}

		public static explicit operator string(Tag tag)
		{
			if (tag._dataType == WMT_ATTR_DATATYPE.WMT_TYPE_STRING)
			{
				return (string)tag._value;
			}
			throw new InvalidCastException("Tag can not be converted to a string.");
		}

		public static explicit operator bool(Tag tag)
		{
			if (tag._dataType == WMT_ATTR_DATATYPE.WMT_TYPE_BOOL)
			{
				return (bool)tag._value;
			}
			throw new InvalidCastException("Tag can not be converted to a bool.");
		}

		public static explicit operator Guid(Tag tag)
		{
			if (tag._dataType == WMT_ATTR_DATATYPE.WMT_TYPE_GUID)
			{
				return (Guid)tag._value;
			}
			throw new InvalidCastException("Tag can not be converted to a Guid.");
		}

		public static explicit operator byte[](Tag tag)
		{
			if (tag._dataType == WMT_ATTR_DATATYPE.WMT_TYPE_BINARY)
			{
				return (byte[])tag._value;
			}
			throw new InvalidCastException("Tag can not be converted to a byte array.");
		}

		public static explicit operator ulong(Tag tag)
		{
			WMT_ATTR_DATATYPE dataType = tag._dataType;
			if (dataType == WMT_ATTR_DATATYPE.WMT_TYPE_DWORD || dataType == WMT_ATTR_DATATYPE.WMT_TYPE_QWORD || dataType == WMT_ATTR_DATATYPE.WMT_TYPE_WORD)
			{
				return (ulong)tag._value;
			}
			throw new InvalidCastException("Tag can not be converted to a number.");
		}

		public static explicit operator long(Tag tag)
		{
			return (long)((ulong)tag);
		}

		public static explicit operator int(Tag tag)
		{
			return (int)((ulong)tag);
		}

		public static explicit operator uint(Tag tag)
		{
			return (uint)((ulong)tag);
		}

		public static explicit operator ushort(Tag tag)
		{
			return (ushort)((ulong)tag);
		}

		private WMT_ATTR_DATATYPE _dataType;

		private object _value;

		private string _name;

		private int _index;
	}
}
