// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when another instance of the application is already running.
/// </summary>
public class InstanceIsRunningEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceIsRunningEventArgs"/> class.
    /// </summary>
    /// <param name="processId">The process ID of the running instance.</param>
    /// <param name="mainWindowHandle">The main window handle of the running instance.</param>
    internal InstanceIsRunningEventArgs(int processId, nint mainWindowHandle)
    {
        RunningProcessId = processId;
        MainWindowHandle = mainWindowHandle;
    }

    /// <summary>
    /// Gets the process ID of the running instance.
    /// </summary>
    public int RunningProcessId { get; }

    /// <summary>
    /// Gets the main window handle of the running instance.
    /// </summary>
    public nint MainWindowHandle { get; }

    /// <summary>
    /// Gets a value indicating whether the running instance has a main window.
    /// </summary>
    public bool HasWindow => MainWindowHandle != 0;

    /// <summary>
    /// Gets or sets a value indicating whether to activate the main window of the running instance.
    /// </summary>
    public bool ActivateMainWindow { get; set; } = false;
}

/// <summary>
/// Represents the method that will handle the event when another instance of the application is already running.
/// </summary>
/// <param name="args">The event data.</param>
public delegate void HandleInstanceIsRunningDelegate(InstanceIsRunningEventArgs args);
