using System;
using System.Diagnostics;
using System.Threading;

namespace UpdateInstaller;
public interface IApplicationCloser
{
    bool CloseApplication(int processId);
}

public class ApplicationCloser : IApplicationCloser
{
    public bool CloseApplication(int processId)
    {
        try
        {
            // Get the process by ID
            Process process = Process.GetProcessById(processId);

            if (process == null)
                return true; // process is already closed.

            // Attempt to close the application gracefully
            process.CloseMainWindow();

            // Initialize variables for polling
            int totalWaitTime = 0;
            int interval = 200;  // 100 ms interval
            int maxWaitTime = 5000;  // 5 seconds

            // Poll to see if process has exited
            while (!process.HasExited && totalWaitTime < maxWaitTime)
            {
                Thread.Sleep(interval);
                totalWaitTime += interval;
            }

            // If the process is still running, kill it
            if (!process.HasExited)
            {
                process.Kill();
            }

            // Make sure the process is terminated
            if (!process.WaitForExit(500))  // Additional wait for confirmation
            {
                throw new InvalidOperationException("Failed to close the application within the allotted time.");
            }

            return true;
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Failed to close the application.");
        }
    }
}
