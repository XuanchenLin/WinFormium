// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that reports changes in the page loading progress.
/// </summary>
public class PageLoadingProgressChangeEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageLoadingProgressChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance associated with the loading progress change.</param>
    /// <param name="progress">The current loading progress, represented as a decimal value between 0 and 1.</param>
    internal PageLoadingProgressChangeEventArgs(CefBrowser browser, decimal progress)
    {
        Progress = progress;
    }

    /// <summary>
    /// Gets the current loading progress of the page, represented as a decimal value between 0 and 1.
    /// </summary>
    public decimal Progress { get; }
}
