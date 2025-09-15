// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when the browser receives or loses focus.
/// </summary>
public class SetFocusEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetFocusEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the focus event.</param>
    /// <param name="source">The <see cref="CefFocusSource"/> indicating the source of the focus event.</param>
    internal SetFocusEventArgs(CefBrowser browser, CefFocusSource source)
    {
        Browser = browser;
        Source = source;
    }

    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the focus event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the <see cref="CefFocusSource"/> indicating the source of the focus event.
    /// </summary>
    public CefFocusSource Source { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the focus event has been handled.
    /// If set to <c>true</c>, the default focus handling will be suppressed.
    /// </summary>
    public bool Handled { get; set; }
}
