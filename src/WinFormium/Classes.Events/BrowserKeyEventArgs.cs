// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Occus when a key event is received from the browser.
/// </summary>
public sealed class BrowserKeyEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserKeyEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="keyEvent">The key event.</param>
    /// <param name="osEvent">The operating system event.</param>
    internal BrowserKeyEventArgs(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent)
    {
        Browser = browser;
        KeyEvent = keyEvent;
        OsEvent = osEvent;
    }
    /// <summary>
    /// Gets the browser instance.
    /// </summary>
    public CefBrowser Browser { get; }
    /// <summary>
    /// Gets the key event.
    /// </summary>
    public CefKeyEvent KeyEvent { get; }
    /// <summary>
    /// Gets the operating system event.
    /// </summary>
    public nint OsEvent { get; }
    /// <summary>
    /// Gets or sets a value indicating whether the key event has been handled. If set to true, the event will not be propagated further.
    /// </summary>
    public bool Handled { get; set; } = false;

}