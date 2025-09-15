// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that occurs when a frame has finished loading a page.
/// </summary>
public class FramePageLoadEndEventArgs : EventArgs
{
    /// <summary>
    /// Gets the browser instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the frame that has finished loading.
    /// </summary>
    public CefFrame Frame { get; }

    /// <summary>
    /// Gets the HTTP status code of the page load operation.
    /// </summary>
    public int HttpStatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FramePageLoadEndEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance associated with the event.</param>
    /// <param name="frame">The frame that has finished loading.</param>
    /// <param name="httpStatusCode">The HTTP status code of the page load operation.</param>
    internal FramePageLoadEndEventArgs(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {
        Browser = browser;
        Frame = frame;
        HttpStatusCode = httpStatusCode;
    }
}
