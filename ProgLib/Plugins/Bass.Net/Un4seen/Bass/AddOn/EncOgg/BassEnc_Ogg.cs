using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Un4seen.Bass.AddOn.Enc;

namespace Un4seen.Bass.AddOn.EncOgg
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassEnc_Ogg
	{
		private BassEnc_Ogg()
		{
		}

		[DllImport("bassenc_ogg")]
		public static extern int BASS_Encode_OGG_GetVersion();

		public static Version BASS_Encode_OGG_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BassEnc_Ogg.BASS_Encode_OGG_GetVersion();
			Version result = new Version(2, 4);
			switch (fieldcount)
			{
			case 1:
				result = new Version(num >> 24 & 255, 0);
				break;
			case 2:
				result = new Version(num >> 24 & 255, num >> 16 & 255);
				break;
			case 3:
				result = new Version(num >> 24 & 255, num >> 16 & 255, num >> 8 & 255);
				break;
			case 4:
				result = new Version(num >> 24 & 255, num >> 16 & 255, num >> 8 & 255, num & 255);
				break;
			}
			return result;
		}

		[DllImport("bassenc_ogg", EntryPoint = "BASS_Encode_OGG_Start")]
		private static extern int BASS_Encode_OGG_StartUnicode(int chan, [MarshalAs(UnmanagedType.LPWStr)] [In] string options, BASSEncode flags, ENCODERPROC proc, IntPtr user);

		public static int BASS_Encode_OGG_Start(int handle, string options, BASSEncode flags, ENCODERPROC proc, IntPtr user)
		{
			flags |= BASSEncode.BASS_UNICODE;
			return BassEnc_Ogg.BASS_Encode_OGG_StartUnicode(handle, options, flags, proc, user);
		}

		[DllImport("bassenc_ogg", EntryPoint = "BASS_Encode_OGG_StartFile")]
		private static extern int BASS_Encode_OGG_StartFileUnicode(int chan, [MarshalAs(UnmanagedType.LPWStr)] [In] string options, BASSEncode flags, [MarshalAs(UnmanagedType.LPWStr)] [In] string filename);

		public static int BASS_Encode_OGG_StartFile(int handle, string options, BASSEncode flags, string filename)
		{
			flags |= BASSEncode.BASS_UNICODE;
			return BassEnc_Ogg.BASS_Encode_OGG_StartFileUnicode(handle, options, flags, filename);
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bassenc_ogg", ref BassEnc_Ogg._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bassenc_ogg"), ref BassEnc_Ogg._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassEnc_Ogg._myModuleHandle);
		}

		public const int BASSENCOGGVERSION = 516;

		private static int _myModuleHandle;

		private const string _myModuleName = "bassenc_ogg";
	}
}
