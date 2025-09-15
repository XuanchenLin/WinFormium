// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for browser-related events.
/// </summary>
public class BrowserEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    public BrowserEventArgs(CefBrowser browser)
    {
        Browser = browser;
    }

    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }
}
