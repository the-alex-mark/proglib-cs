using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ProgLib.Windows.Taskbar
{
	internal class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, StringBuilder lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, ref NativeMethods.REBARBANDINFO lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll")]
		public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

		[DllImport("dwmapi.dll")]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref NativeMethods.MARGINS margins);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern bool DwmIsCompositionEnabled();

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int SetWindowTheme(IntPtr hWnd, int pszSubAppName, string pszSubIdList);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, int pszSubIdList);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int SetWindowTheme(IntPtr hWnd, int pszSubAppName, int pszSubIdList);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr CreateWindowEx(NativeMethods.WindowStylesEx dwExStyle, [MarshalAs(UnmanagedType.LPStr)] string lpClassName, [MarshalAs(UnmanagedType.LPStr)] string lpWindowName, NativeMethods.WindowStyles dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr CreateWindowEx(NativeMethods.WindowStylesEx dwExStyle, [MarshalAs(UnmanagedType.LPStr)] string lpClassName, [MarshalAs(UnmanagedType.LPStr)] string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
		{
			bool flag = IntPtr.Size == 8;
			IntPtr result;
			if (flag)
			{
				result = NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			}
			else
			{
				result = new IntPtr(NativeMethods.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
			}
			return result;
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("comctl32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern bool InitCommonControlsEx(ref NativeMethods.INITCOMMONCONTROLSEX iccex);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetCursor(IntPtr hCursor);

		public static int GetHiWord(long value, int size)
		{
			return (int)((short)(value >> size));
		}

		public static int GetLoWord(long value)
		{
			return (int)((short)(value & 65535L));
		}

		public const int SC_CLOSE = 61536;

		public const int MF_BYCOMMAND = 0;

		public const int MF_ENABLED = 0;

		public const int MF_GRAYED = 1;

		public const int CS_NOCLOSE = 512;

		public const int IMAGE_BITMAP = 0;

		public const int IMAGE_ICON = 1;

		public const int GWL_EXSTYLE = -20;

		public const string WC_IPADDRESS = "SysIPAddress32";

		public const string REBARCLASSNAME = "ReBarWindow32";

		public const int WM_USER = 1024;

		public const int WM_PAINT = 15;

		public const int WM_COMMAND = 273;

		public const int WM_SETCURSOR = 32;

		public const int WS_VISIBLE = 268435456;

		public const int WS_CHILD = 1073741824;

		public const int WS_EX_CLIENTEDGE = 512;

		public const int WS_EX_NOPARENTNOTIFY = 4;

		public const int WS_EX_LAYOUTRTL = 4194304;

		public const int WS_EX_RIGHT = 4096;

		public const int WS_EX_RTLREADING = 8192;

		public const int WS_EX_LEFTSCROLLBAR = 16384;

		public const int ECM_FIRST = 5376;

		public const int EM_SETCUEBANNER = 5377;

		public const int CS_VREDRAW = 1;

		public const int CS_HREDRAW = 2;

		public const int CS_DBLCLKS = 8;

		public const int CS_GLOBALCLASS = 16384;

		public const int BCM_FIRST = 5632;

		public const int BCM_SETNOTE = 5641;

		public const int BCM_SETDROPDOWNSTATE = 5638;

		public const int BCM_SETSHIELD = 5644;

		public const int BS_COMMANDLINK = 14;

		public const int BS_DEFCOMMANDLINK = 15;

		public const int BS_PUSHBUTTON = 0;

		public const int BS_DEFPUSHBUTTON = 1;

		public const int BS_SPLITBUTTON = 12;

		public const int BS_DEFSPLITBUTTON = 13;

		public const int BS_ICON = 64;

		public const int BM_SETIMAGE = 247;

		public const int PBM_SETSTATE = 1040;

		public const int PBS_SMOOTHREVERSE = 16;

		public const int PBST_NORMAL = 1;

		public const int PBST_ERROR = 2;

		public const int PBST_PAUSED = 3;

		public const int LVM_FIRST = 4096;

		public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;

		public const int LVS_EX_DOUBLEBUFFER = 65536;

		public const int TV_FIRST = 4352;

		public const int TVM_GETEXTENDEDSTYLE = 4397;

		public const int TVM_SETEXTENDEDSTYLE = 4396;

		public const int TVS_EX_AUTOHSCROLL = 32;

		public const int TVS_EX_FADEINOUTEXPANDOS = 64;

		public const int TVS_EX_DOUBLEBUFFER = 4;

		public const int ICC_COOL_CLASSES = 1024;

		public const int ICC_BAR_CLASSES = 4;

		public const int RB_INSERTBAND = 1034;

		public const int MIM_STYLE = 16;

		public const int MNS_CHECKORBMP = 67108864;

		public const int MIIM_BITMAP = 128;

		public const int IDC_HAND = 32649;

		public enum THUMBBUTTONFLAGS
		{
			THBF_ENABLED,
			THBF_DISABLED,
			THBF_DISMISSONCLICK,
			THBF_NOBACKGROUND = 4,
			THBF_HIDDEN = 8,
			THBF_NONINTERACTIVE = 16
		}

		public enum THUMBBUTTONMASK
		{
			THB_BITMAP = 1,
			THB_ICON,
			THB_TOOLTIP = 4,
			THB_FLAGS = 8
		}

		[Flags]
		public enum WindowStylesEx : uint
		{
			WS_EX_ACCEPTFILES = 16u,
			WS_EX_APPWINDOW = 262144u,
			WS_EX_CLIENTEDGE = 512u,
			WS_EX_COMPOSITED = 33554432u,
			WS_EX_CONTEXTHELP = 1024u,
			WS_EX_CONTROLPARENT = 65536u,
			WS_EX_DLGMODALFRAME = 1u,
			WS_EX_LAYERED = 524288u,
			WS_EX_LAYOUTRTL = 4194304u,
			WS_EX_LEFT = 0u,
			WS_EX_LEFTSCROLLBAR = 16384u,
			WS_EX_LTRREADING = 0u,
			WS_EX_MDICHILD = 64u,
			WS_EX_NOACTIVATE = 134217728u,
			WS_EX_NOINHERITLAYOUT = 1048576u,
			WS_EX_NOPARENTNOTIFY = 4u,
			WS_EX_OVERLAPPEDWINDOW = 768u,
			WS_EX_PALETTEWINDOW = 392u,
			WS_EX_RIGHT = 4096u,
			WS_EX_RIGHTSCROLLBAR = 0u,
			WS_EX_RTLREADING = 8192u,
			WS_EX_STATICEDGE = 131072u,
			WS_EX_TOOLWINDOW = 128u,
			WS_EX_TOPMOST = 8u,
			WS_EX_TRANSPARENT = 32u,
			WS_EX_WINDOWEDGE = 256u
		}

		[Flags]
		public enum WindowStyles : uint
		{
			WS_BORDER = 8388608u,
			WS_CAPTION = 12582912u,
			WS_CHILD = 1073741824u,
			WS_CLIPCHILDREN = 33554432u,
			WS_CLIPSIBLINGS = 67108864u,
			WS_DISABLED = 134217728u,
			WS_DLGFRAME = 4194304u,
			WS_GROUP = 131072u,
			WS_HSCROLL = 1048576u,
			WS_MAXIMIZE = 16777216u,
			WS_MAXIMIZEBOX = 65536u,
			WS_MINIMIZE = 536870912u,
			WS_MINIMIZEBOX = 131072u,
			WS_OVERLAPPED = 0u,
			WS_OVERLAPPEDWINDOW = 13565952u,
			WS_POPUP = 2147483648u,
			WS_POPUPWINDOW = 2156396544u,
			WS_SIZEFRAME = 262144u,
			WS_THICKFRAME = 262144u,
			WS_SYSMENU = 524288u,
			WS_TABSTOP = 65536u,
			WS_VISIBLE = 268435456u,
			WS_VSCROLL = 2097152u,
			RBS_VARHEIGHT = 512u,
			CCS_NODIVIDER = 64u
		}

		public struct MARGINS
		{
			public int leftWidth;

			public int rightWidth;

			public int topHeight;

			public int bottomHeight;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 8)]
		public struct THUMBBUTTON
		{
			public const int Clicked = 6144;

			[MarshalAs(UnmanagedType.U4)]
			public NativeMethods.THUMBBUTTONMASK dwMask;

			public int iId;

			public int iBitmap;

			public IntPtr hIcon;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szTip;

			public NativeMethods.THUMBBUTTONFLAGS dwFlags;
		}

		public struct INITCOMMONCONTROLSEX
		{
			public int dwSize;

			public uint dwICC;
		}

		public struct REBARBANDINFO
		{
			public int cbSize;

			public int fMask;

			public int fStyle;

			public int clrFore;

			public int clrBack;

			public IntPtr lpText;

			public int cch;

			public int iImage;

			public IntPtr hwndChild;

			public int cxMinChild;

			public int cyMinChild;

			public int cx;

			public IntPtr hbmBack;

			public int wID;

			public int cyChild;

			public int cyMaxChild;

			public int cyIntegral;

			public int cxIdeal;

			public int lParam;

			public int cxHeader;
		}

		[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ITaskbarList3
		{
			void HrInit();

			void AddTab(IntPtr hWnd);

			void DeleteTab(IntPtr hWnd);

			void ActivateTab(IntPtr hWnd);

			void SetActiveAlt(IntPtr hWnd);

			void MarkFullscreenWindow(IntPtr hWnd, int fFullscreen);

			void SetProgressValue(IntPtr hWnd, ulong ullCompleted, ulong ullTotal);

			void SetProgressState(IntPtr hWnd, TBPFLAG tbpFlags);

			void RegisterTab(IntPtr hWndTab, IntPtr hWndMDI);

			void UnregisterTab(IntPtr hWndTab);

			void SetTabOrder(IntPtr hWndTab, IntPtr hWndInsertBefore);

			void SetTabActive(IntPtr hWndTab, IntPtr hWndMDI, int tbatFlags);

			void ThumbBarAddButtons(IntPtr hWnd, int cButtons, [MarshalAs(UnmanagedType.LPArray)] NativeMethods.THUMBBUTTON[] pButton);

			void ThumbBarUpdateButtons(IntPtr hWnd, int cButtons, [MarshalAs(UnmanagedType.LPArray)] NativeMethods.THUMBBUTTON[] pButton);

			void ThumbBarSetImageList(IntPtr hWnd, IntPtr himl);

			void SetOverlayIcon(IntPtr hWnd, IntPtr hIcon, string pszDescription);

			void SetThumbnailTooltip(IntPtr hWnd, string pszTip);

			void SetThumbnailClip(IntPtr hWnd, IntPtr prcClip);
		}
	}
}
