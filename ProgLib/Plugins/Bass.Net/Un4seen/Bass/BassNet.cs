using System;
using System.Drawing;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassNet
	{
		private BassNet()
		{
		}

		static BassNet()
		{
			AssemblyName name = Assembly.GetExecutingAssembly().GetName();
			BassNet._internalName = string.Format("{0} v{1}", name.Name, name.Version);
		}

		public static string InternalName
		{
			get
			{
				return BassNet._internalName;
			}
		}

		public static bool UseBrokenLatin1Behavior
		{
			get
			{
				return BassNet._useBrokenLatin1;
			}
			set
			{
				BassNet._useBrokenLatin1 = value;
			}
		}

		public static bool UseRiffInfoUTF8
		{
			get
			{
				return BassNet._useRiffInfoUTF8;
			}
			set
			{
				BassNet._useRiffInfoUTF8 = value;
			}
		}

		public static void Registration(string eMail, string registrationKey)
		{
			BassNet._eMail = eMail;
			BassNet._registrationKey = registrationKey;
		}

		public static void ShowSplash(Form owner, int wait, double opacity, int pos)
		{
			SplashScreen splashScreen = new SplashScreen(false, wait);
			splashScreen.SetOpacity(opacity);
			splashScreen.SetPosition(pos);
			if (owner != null && pos == 2)
			{
				splashScreen.StartPosition = FormStartPosition.Manual;
				Point location = owner.Location;
				location.Offset(owner.Width / 2 - splashScreen.Width / 2, owner.Height / 2 - splashScreen.Height / 2);
				splashScreen.Location = location;
			}
			if (wait <= 0)
			{
				splashScreen.SetClose(false);
			}
			else
			{
				splashScreen.SetClose(true);
			}
			splashScreen.Show();
			Application.DoEvents();
			if (wait > 0)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(BassNet.WaitMe), splashScreen);
			}
		}

		public static void ShowAbout(Form owner)
		{
			SplashScreen splashScreen = new SplashScreen(false, 0);
			if (owner != null)
			{
				splashScreen.SetPosition(2);
			}
			splashScreen.ShowDialog(owner);
		}

		private static void WaitMe(object splash)
		{
			if (splash != null)
			{
				SplashScreen splashScreen = splash as SplashScreen;
				if (splashScreen != null)
				{
					Thread.Sleep(splashScreen._waitTime);
					if (!splashScreen.IsDisposed)
					{
						splashScreen.Invoke(new MethodInvoker(splashScreen.Close));
					}
					splashScreen.Dispose();
					splash = null;
				}
			}
		}

		internal static string _eMail = string.Empty;

		internal static string _registrationKey = string.Empty;

		internal static string _internalName = "BASS.NET";

		public static bool OmitCheckVersion = false;

		private static bool _useBrokenLatin1 = false;

		private static bool _useRiffInfoUTF8 = false;
	}
}
