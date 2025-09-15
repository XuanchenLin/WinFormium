// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the status message change event.
/// </summary>
public class StatusMessageChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the new status message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusMessageChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    /// <param name="value">The new status message.</param>
    internal StatusMessageChangeEventArgs(CefBrowser browser, string value)
    {
        Browser = browser;
        Message = value;
    }
}
