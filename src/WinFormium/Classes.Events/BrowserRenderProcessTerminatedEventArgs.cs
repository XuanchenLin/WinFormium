// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when the browser render process is terminated.
/// </summary>
public class BrowserRenderProcessTerminatedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the terminated render process.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the termination status of the render process.
    /// </summary>
    public CefTerminationStatus Status { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserRenderProcessTerminatedEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the terminated process.</param>
    /// <param name="status">The <see cref="CefTerminationStatus"/> indicating the reason for termination.</param>
    internal BrowserRenderProcessTerminatedEventArgs(CefBrowser browser, CefTerminationStatus status)
    {
        Browser = browser;
        Status = status;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the browser render process should be restarted.
    /// </summary>
    public bool RestartProcess { get; set; } = false;
}
