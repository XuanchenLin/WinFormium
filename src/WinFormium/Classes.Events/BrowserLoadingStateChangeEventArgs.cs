// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the browser loading state change event.
/// </summary>
public class BrowserLoadingStateChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets a value indicating whether the browser is currently loading.
    /// </summary>
    public bool IsLoading { get; }

    /// <summary>
    /// Gets a value indicating whether the browser can navigate backwards.
    /// </summary>
    public bool CanGoBack { get; }

    /// <summary>
    /// Gets a value indicating whether the browser can navigate forwards.
    /// </summary>
    public bool CanGoForward { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserLoadingStateChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    /// <param name="isLoading">A value indicating whether the browser is currently loading.</param>
    /// <param name="canGoBack">A value indicating whether the browser can navigate backwards.</param>
    /// <param name="canGoForward">A value indicating whether the browser can navigate forwards.</param>
    internal BrowserLoadingStateChangeEventArgs(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
    {
        Browser = browser;
        IsLoading = isLoading;
        CanGoBack = canGoBack;
        CanGoForward = canGoForward;
    }
}
