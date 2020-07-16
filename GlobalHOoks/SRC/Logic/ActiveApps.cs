using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPick.SRC.Logic
{
    static class ActiveApps
    {
        public static void GetActiveApps()
        {
            var allProcesses = Process.GetProcesses();
        }

    }
}
