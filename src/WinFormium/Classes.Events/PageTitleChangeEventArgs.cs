// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when the page title changes in a browser.
/// </summary>
public class PageTitleChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance where the title change occurred.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the new title of the page.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PageTitleChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance where the title change occurred.</param>
    /// <param name="title">The new title of the page.</param>
    internal PageTitleChangeEventArgs(CefBrowser browser, string title)
    {
        Browser = browser;
        Title = title;
    }
}
