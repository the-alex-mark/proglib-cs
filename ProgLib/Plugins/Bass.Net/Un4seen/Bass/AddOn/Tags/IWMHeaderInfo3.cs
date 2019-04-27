using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Tags
{
	[Guid("15CC68E3-27CC-4ecd-B222-3F5D02D80BD5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface IWMHeaderInfo3
	{
		uint GetAttributeCount([In] ushort wStreamNum, out ushort pcAttributes);

		uint GetAttributeByIndex([In] ushort wIndex, [In] [Out] ref ushort pwStreamNum, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszName, [In] [Out] ref ushort pcchNameLen, out WMT_ATTR_DATATYPE pType, IntPtr pValue, [In] [Out] ref ushort pcbLength);

		uint GetAttributeByName([In] [Out] ref ushort pwStreamNum, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pszName, out WMT_ATTR_DATATYPE pType, IntPtr pValue, [In] [Out] ref ushort pcbLength);

		uint SetAttribute([In] ushort wStreamNum, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszName, [In] WMT_ATTR_DATATYPE Type, IntPtr pValue, [In] ushort cbLength);

		uint GetMarkerCount(out ushort pcMarkers);

		uint GetMarker([In] ushort wIndex, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszMarkerName, [In] [Out] ref ushort pcchMarkerNameLen, out ulong pcnsMarkerTime);

		uint AddMarker([MarshalAs(UnmanagedType.LPWStr)] [In] string pwszMarkerName, [In] ulong cnsMarkerTime);

		uint RemoveMarker([In] ushort wIndex);

		uint GetScriptCount(out ushort pcScripts);

		uint GetScript([In] ushort wIndex, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszType, [In] [Out] ref ushort pcchTypeLen, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszCommand, [In] [Out] ref ushort pcchCommandLen, out ulong pcnsScriptTime);

		uint AddScript([MarshalAs(UnmanagedType.LPWStr)] [In] string pwszType, [MarshalAs(UnmanagedType.LPWStr)] [In] string pwszCommand, [In] ulong cnsScriptTime);

		uint RemoveScript([In] ushort wIndex);

		uint GetCodecInfoCount(out uint pcCodecInfos);

		uint GetCodecInfo([In] uint wIndex, [In] [Out] ref ushort pcchName, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszName, [In] [Out] ref ushort pcchDescription, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszDescription, out WMT_CODEC_INFO_TYPE pCodecType, [In] [Out] ref ushort pcbCodecInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] pbCodecInfo);

		uint GetAttributeCountEx([In] ushort wStreamNum, out ushort pcAttributes);

		uint GetAttributeIndices([In] ushort wStreamNum, [MarshalAs(UnmanagedType.LPWStr)] [In] string pwszName, [In] ref ushort pwLangIndex, [MarshalAs(UnmanagedType.LPArray)] [Out] ushort[] pwIndices, [In] [Out] ref ushort pwCount);

		uint GetAttributeByIndexEx([In] ushort wStreamNum, [In] ushort wIndex, [MarshalAs(UnmanagedType.LPWStr)] [Out] string pwszName, [In] [Out] ref ushort pwNameLen, out WMT_ATTR_DATATYPE pType, out ushort pwLangIndex, IntPtr pValue, [In] [Out] ref uint pdwDataLength);

		uint ModifyAttribute([In] ushort wStreamNum, [In] ushort wIndex, [In] WMT_ATTR_DATATYPE Type, [In] ushort wLangIndex, IntPtr pValue, [In] uint dwLength);

		uint AddAttribute([In] ushort wStreamNum, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszName, out ushort pwIndex, [In] WMT_ATTR_DATATYPE Type, [In] ushort wLangIndex, IntPtr pValue, [In] uint dwLength);

		uint DeleteAttribute([In] ushort wStreamNum, [In] ushort wIndex);

		uint AddCodecInfo([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName, [MarshalAs(UnmanagedType.LPWStr)] [In] string pwszDescription, [In] WMT_CODEC_INFO_TYPE codecType, [In] ushort cbCodecInfo, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] pbCodecInfo);
	}
}
