// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the browser closing event.
/// </summary>
public class BrowserClosingEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets a value indicating whether the browser closing event should be canceled.
    /// Set to <c>true</c> to cancel the closing; otherwise, <c>false</c>.
    /// </summary>
    public bool Cancel { get; set; } = false;

    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the closing event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserClosingEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance that is being closed.</param>
    internal BrowserClosingEventArgs(CefBrowser browser)
    {
        Browser = browser;
    }
}
