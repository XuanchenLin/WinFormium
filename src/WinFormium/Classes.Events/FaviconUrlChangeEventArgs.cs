// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when the favicon URLs for a browser change.
/// </summary>
public class FaviconUrlChangeEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FaviconUrlChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    /// <param name="urls">An array of strings containing the new favicon URLs.</param>
    internal FaviconUrlChangeEventArgs(CefBrowser browser, string[] urls)
    {
        Browser = browser;
        Urls = urls;
    }

    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets an array of strings containing the new favicon URLs.
    /// </summary>
    public string[] Urls { get; }
}
