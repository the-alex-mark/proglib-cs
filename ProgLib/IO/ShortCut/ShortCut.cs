using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.IO.ShortCut
{
    public static class ShortCut
    {
        public static void Create(String PathToFile, String PathToLink, String Arguments, String Description)
        {
            ShellLink.IShellLinkW shlLink = ShellLink.CreateShellLink();
            Marshal.ThrowExceptionForHR(shlLink.SetDescription(Description));
            Marshal.ThrowExceptionForHR(shlLink.SetPath(PathToFile));
            Marshal.ThrowExceptionForHR(shlLink.SetArguments(Arguments));
            ((System.Runtime.InteropServices.ComTypes.IPersistFile)shlLink).Save(PathToLink, false);
        }
    }
}
