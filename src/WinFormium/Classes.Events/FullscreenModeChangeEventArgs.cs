// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the fullscreen mode change event.
/// </summary>
public class FullscreenModeChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the fullscreen mode change event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets a value indicating whether the browser is entering or exiting fullscreen mode.
    /// </summary>
    public bool Fullscreen { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FullscreenModeChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance associated with the event.</param>
    /// <param name="fullscreen">A value indicating whether the browser is entering fullscreen mode.</param>
    internal FullscreenModeChangeEventArgs(CefBrowser browser, bool fullscreen)
    {
        Browser = browser;
        Fullscreen = fullscreen;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the fullscreen mode change event should be canceled.
    /// </summary>
    public bool Cancel { get; set; } = false;

}
