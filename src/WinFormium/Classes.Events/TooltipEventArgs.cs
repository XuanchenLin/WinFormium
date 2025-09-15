// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for tooltip events in the WinFormium browser.
/// </summary>
public class TooltipEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the tooltip event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the tooltip text to be displayed.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip event has been handled.
    /// </summary>
    public bool Handled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TooltipEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    /// <param name="text">The tooltip text to be displayed.</param>
    internal TooltipEventArgs(CefBrowser browser, string text)
    {
        Browser = browser;
        Text = text;
    }
}
