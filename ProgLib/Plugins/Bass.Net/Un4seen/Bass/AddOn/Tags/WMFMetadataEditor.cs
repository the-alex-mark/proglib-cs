using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Tags
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	internal class WMFMetadataEditor
	{
		[DllImport("WMVCore.dll", SetLastError = true)]
		internal static extern uint WMCreateEditor([MarshalAs(UnmanagedType.Interface)] out IWMMetadataEditor ppMetadataEditor);

		public WMFMetadataEditor(IWMHeaderInfo3 headerInfo3)
		{
			this.HeaderInfo3 = headerInfo3;
		}

		internal List<TagPicture> GetAllPictures()
		{
			if (this.HeaderInfo3 == null)
			{
				return null;
			}
			List<TagPicture> list = null;
			List<Tag> list2 = this.WMGetAllAttrib("WM/Picture");
			if (list2 != null && list2.Count > 0)
			{
				list = new List<TagPicture>(list2.Count);
				foreach (Tag pTag in list2)
				{
					TagPicture picture = this.GetPicture(pTag);
					if (picture != null)
					{
						list.Add(picture);
					}
				}
			}
			return list;
		}

		private TagPicture GetPicture(Tag pTag)
		{
			TagPicture result = null;
			string mimeType = "Unknown";
			TagPicture.PICTURE_TYPE pictureType = TagPicture.PICTURE_TYPE.Unknown;
			MemoryStream memoryStream = null;
			BinaryReader binaryReader = null;
			if (pTag.Name == "WM/Picture")
			{
				try
				{
					memoryStream = new MemoryStream((byte[])pTag);
					binaryReader = new BinaryReader(memoryStream);
					if (Utils.Is64Bit)
					{
						mimeType = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt64()));
					}
					else
					{
						mimeType = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt32()));
					}
					byte b = binaryReader.ReadByte();
					try
					{
						pictureType = (TagPicture.PICTURE_TYPE)b;
					}
					catch
					{
						pictureType = TagPicture.PICTURE_TYPE.Unknown;
					}
					string description;
					if (Utils.Is64Bit)
					{
						description = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt64()));
					}
					else
					{
						description = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt32()));
					}
					int num = binaryReader.ReadInt32();
					byte[] array = new byte[num];
					if (Utils.Is64Bit)
					{
						Marshal.Copy(new IntPtr(binaryReader.ReadInt64()), array, 0, num);
					}
					else
					{
						Marshal.Copy(new IntPtr(binaryReader.ReadInt32()), array, 0, num);
					}
					result = new TagPicture(pTag.Index, mimeType, pictureType, description, array);
				}
				catch
				{
				}
				finally
				{
					if (binaryReader != null)
					{
						binaryReader.Close();
					}
					if (memoryStream != null)
					{
						memoryStream.Close();
						memoryStream.Dispose();
					}
				}
			}
			return result;
		}

		internal List<Tag> ReadAllAttributes()
		{
			if (this.HeaderInfo3 == null)
			{
				return null;
			}
			List<Tag> list = new List<Tag>(42);
			try
			{
				ushort num = 0;
				ushort[] pwIndices = null;
				ushort num2 = 0;
				this.HeaderInfo3.GetAttributeIndices(0, null, ref num, pwIndices, ref num2);
				ushort[] array = new ushort[(int)num2];
				this.HeaderInfo3.GetAttributeIndices(0, null, ref num, array, ref num2);
				if (array != null && array.Length != 0)
				{
					foreach (ushort num3 in array)
					{
						string text = null;
						ushort count = 0;
						uint num4 = 0u;
						try
						{
							WMT_ATTR_DATATYPE wmt_ATTR_DATATYPE;
							this.HeaderInfo3.GetAttributeByIndexEx(0, num3, text, ref count, out wmt_ATTR_DATATYPE, out num, IntPtr.Zero, ref num4);
							text = new string('\0', (int)count);
							object obj;
							switch (wmt_ATTR_DATATYPE)
							{
							case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
							case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
								obj = 0u;
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
							case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
								obj = new byte[num4];
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
								obj = 0UL;
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
								obj = 0;
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
								obj = Guid.NewGuid();
								break;
							default:
								throw new InvalidOperationException(string.Format("Not supported data type: {0}", wmt_ATTR_DATATYPE.ToString()));
							}
							GCHandle gchandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
							try
							{
								IntPtr intPtr = gchandle.AddrOfPinnedObject();
								this.HeaderInfo3.GetAttributeByIndexEx(0, num3, text, ref count, out wmt_ATTR_DATATYPE, out num, intPtr, ref num4);
								if (wmt_ATTR_DATATYPE != WMT_ATTR_DATATYPE.WMT_TYPE_STRING)
								{
									if (wmt_ATTR_DATATYPE == WMT_ATTR_DATATYPE.WMT_TYPE_BOOL)
									{
										obj = ((uint)obj > 0u);
									}
								}
								else
								{
									obj = Marshal.PtrToStringUni(intPtr);
								}
								list.Add(new Tag((int)num3, text, wmt_ATTR_DATATYPE, obj));
							}
							finally
							{
								gchandle.Free();
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
			return list;
		}

		private List<Tag> WMGetAllAttrib(string pAttribName)
		{
			List<Tag> list = new List<Tag>();
			try
			{
				if (!pAttribName.EndsWith("\0"))
				{
					pAttribName += "\0";
				}
				ushort num = 0;
				ushort[] pwIndices = null;
				ushort num2 = 0;
				this.HeaderInfo3.GetAttributeIndices(0, pAttribName, ref num, pwIndices, ref num2);
				ushort[] array = new ushort[(int)num2];
				this.HeaderInfo3.GetAttributeIndices(0, pAttribName, ref num, array, ref num2);
				if (array != null && array.Length != 0)
				{
					foreach (ushort num3 in array)
					{
						string text = null;
						ushort count = 0;
						uint num4 = 0u;
						try
						{
							WMT_ATTR_DATATYPE wmt_ATTR_DATATYPE;
							this.HeaderInfo3.GetAttributeByIndexEx(0, num3, text, ref count, out wmt_ATTR_DATATYPE, out num, IntPtr.Zero, ref num4);
							text = new string('\0', (int)count);
							object obj;
							switch (wmt_ATTR_DATATYPE)
							{
							case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
							case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
								obj = 0u;
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
							case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
								obj = new byte[num4];
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
								obj = 0UL;
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
								obj = 0;
								break;
							case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
								obj = Guid.NewGuid();
								break;
							default:
								throw new InvalidOperationException(string.Format("Not supported data type: {0}", wmt_ATTR_DATATYPE.ToString()));
							}
							GCHandle gchandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
							try
							{
								IntPtr intPtr = gchandle.AddrOfPinnedObject();
								this.HeaderInfo3.GetAttributeByIndexEx(0, num3, text, ref count, out wmt_ATTR_DATATYPE, out num, intPtr, ref num4);
								if (wmt_ATTR_DATATYPE != WMT_ATTR_DATATYPE.WMT_TYPE_STRING)
								{
									if (wmt_ATTR_DATATYPE == WMT_ATTR_DATATYPE.WMT_TYPE_BOOL)
									{
										obj = ((uint)obj > 0u);
									}
								}
								else
								{
									obj = Marshal.PtrToStringUni(intPtr);
								}
								list.Add(new Tag((int)num3, text, wmt_ATTR_DATATYPE, obj));
							}
							finally
							{
								gchandle.Free();
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
			return list;
		}

		private ushort[] WMGetAttribIndices(string pAttribName)
		{
			ushort[] array = null;
			try
			{
				if (!pAttribName.EndsWith("\0"))
				{
					pAttribName += "\0";
				}
				ushort num = 0;
				ushort[] pwIndices = null;
				ushort num2 = 0;
				this.HeaderInfo3.GetAttributeIndices(0, pAttribName, ref num, pwIndices, ref num2);
				array = new ushort[(int)num2];
				this.HeaderInfo3.GetAttributeIndices(0, pAttribName, ref num, array, ref num2);
			}
			catch
			{
				array = null;
			}
			return array;
		}

		private IWMHeaderInfo3 HeaderInfo3;
	}
}
