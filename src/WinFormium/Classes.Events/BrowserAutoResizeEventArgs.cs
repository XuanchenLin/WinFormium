// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the browser auto resize event.
/// </summary>
public class BrowserAutoResizeEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets or sets the new size of the browser.
    /// </summary>
    public CefSize NewSize { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserAutoResizeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    /// <param name="newSize">The new size of the browser.</param>
    internal BrowserAutoResizeEventArgs(CefBrowser browser, CefSize newSize)
    {
        Browser = browser;
        NewSize = newSize;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the event has been handled.
    /// </summary>
    public bool Handled { get; set; } = false;
}
