// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the browser load end event.
/// </summary>
public class BrowserLoadEndEventArgs : EventArgs
{
    /// <summary>
    /// Gets the browser instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the frame in which the load ended.
    /// </summary>
    public CefFrame Frame { get; }

    /// <summary>
    /// Gets the HTTP status code of the completed load.
    /// </summary>
    public int HttpStatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserLoadEndEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance associated with the event.</param>
    /// <param name="frame">The frame in which the load ended.</param>
    /// <param name="httpStatusCode">The HTTP status code of the completed load.</param>
    internal BrowserLoadEndEventArgs(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {
        Browser = browser;
        Frame = frame;
        HttpStatusCode = httpStatusCode;
    }
}
