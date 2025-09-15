// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace WinFormium;


/// <summary>
/// Provides functionality to monitor the main (parent) process and exit the current process when the parent exits.
/// </summary>
internal static class MainProcessMonitor
{
    /// <summary>
    /// Starts monitoring the specified parent process. When the parent process exits, the current process will also exit.
    /// </summary>
    /// <param name="parentProcessId">The process ID of the parent process to monitor.</param>
    public static void StartMonitoring(int parentProcessId)
    {
        Task.Factory.StartNew(() => AwaitParentProcessExit(parentProcessId), TaskCreationOptions.LongRunning);
    }

    /// <summary>
    /// Waits asynchronously for the parent process to exit, then terminates the current process after a short delay.
    /// </summary>
    /// <param name="parentProcessId">The process ID of the parent process to monitor.</param>
    private static async void AwaitParentProcessExit(int parentProcessId)
    {
        try
        {
            var parentProcess = Process.GetProcessById(parentProcessId);
            parentProcess.WaitForExit();
        }
        catch
        {
            //main process probably died already
        }

        await Task.Delay(TimeSpan.FromSeconds(5)); // wait a bit before exiting

        Environment.Exit(0);
    }
}
