// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the browser load start event.
/// </summary>
public class BrowserLoadStartEventArgs : EventArgs
{
    /// <summary>
    /// Gets the browser instance where the load started.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the frame in which the load started.
    /// </summary>
    public CefFrame Frame { get; }

    /// <summary>
    /// Gets the transition type for the navigation.
    /// </summary>
    public CefTransitionType TransitionType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserLoadStartEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance where the load started.</param>
    /// <param name="frame">The frame in which the load started.</param>
    /// <param name="transitionType">The transition type for the navigation.</param>
    internal BrowserLoadStartEventArgs(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
    {
        Browser = browser;
        Frame = frame;
        TransitionType = transitionType;
    }
}
