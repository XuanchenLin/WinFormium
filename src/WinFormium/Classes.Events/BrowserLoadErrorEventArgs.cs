// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the browser load error event.
/// </summary>
public class BrowserLoadErrorEventArgs : EventArgs
{
    /// <summary>
    /// Gets the browser instance where the load error occurred.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the frame in which the load error occurred.
    /// </summary>
    public CefFrame Frame { get; }

    /// <summary>
    /// Gets the error code associated with the load failure.
    /// </summary>
    public CefErrorCode ErrorCode { get; }

    /// <summary>
    /// Gets the error text describing the load failure.
    /// </summary>
    public string ErrorText { get; }

    /// <summary>
    /// Gets the URL that failed to load.
    /// </summary>
    public string FailedUrl { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserLoadErrorEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance where the error occurred.</param>
    /// <param name="frame">The frame in which the error occurred.</param>
    /// <param name="errorCode">The error code associated with the failure.</param>
    /// <param name="errorText">The error text describing the failure.</param>
    /// <param name="failedUrl">The URL that failed to load.</param>
    internal BrowserLoadErrorEventArgs(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
    {
        Browser = browser;
        Frame = frame;
        ErrorCode = errorCode;
        ErrorText = errorText;
        FailedUrl = failedUrl;
    }
}
