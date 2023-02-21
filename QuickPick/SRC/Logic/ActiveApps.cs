using QuickPick.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

using WPFImage = System.Windows.Controls.Image;

namespace QuickPick.SRC.Logic
{
    public static class ActiveApps
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool IsWindow(IntPtr hWnd);

        public static IEnumerable<Process> GetAllOpenWindows()
        {
            foreach (var process in Process.GetProcesses()
                   .Where(w=>IsWindow(w.MainWindowHandle)
                && !string.IsNullOrEmpty(w.MainWindowTitle)))
            {
                IntPtr hWnd = process.MainWindowHandle;                
                if (hWnd != IntPtr.Zero)
                {
                    yield return process;
                }
            }
        }        
    }
}

