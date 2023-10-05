using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateInstaller.Updates;
internal interface IUpdateManager
{

    void CloseApplication(int processId);
    void LaunchUpdatedApplication(string targetPath, string[] args);

}

public class UpdateManager : IUpdateManager
{
    IApplicationCloser _appCloser;

    public UpdateManager(IApplicationCloser appCloser)
    {
        _appCloser = appCloser;
    }

    public void CloseApplication(int processId)
    {
        _appCloser.CloseApplication(processId);
    }


    public void LaunchUpdatedApplication(string targetPath, string[]? args = null)
    {
        if (args != null)
            Process.Start(targetPath, args);

        else
            Process.Start(targetPath);
    }
}
