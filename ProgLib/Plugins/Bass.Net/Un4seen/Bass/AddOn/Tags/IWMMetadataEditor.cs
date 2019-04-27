using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Tags
{
	[Guid("96406BD9-2B2B-11d3-B36B-00C04F6108FF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface IWMMetadataEditor
	{
		uint Open([MarshalAs(UnmanagedType.LPWStr)] [In] string pwszFilename);

		uint Close();

		uint Flush();
	}
}
