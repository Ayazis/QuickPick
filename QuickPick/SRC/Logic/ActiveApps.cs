using QuickPick.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Win32Interop.WinHandles;

namespace QuickPick.SRC.Logic
{
    static class ActiveApps
    {

        public static void GetAllOpenWindows()
        {
            var windows = TopLevelWindowUtils.FindWindows(wh => wh.IsVisible());
            foreach (var w in windows)
            {
                Debug.WriteLine(w.GetWindowText());
            }
        }




    }
}
