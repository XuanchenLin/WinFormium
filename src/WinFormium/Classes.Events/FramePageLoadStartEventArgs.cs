// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when a frame starts loading a page.
/// </summary>
public class FramePageLoadStartEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the <see cref="CefFrame"/> instance representing the frame that started loading.
    /// </summary>
    public CefFrame Frame { get; }

    /// <summary>
    /// Gets the <see cref="CefTransitionType"/> indicating the type of transition for the navigation.
    /// </summary>
    public CefTransitionType TransitionType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FramePageLoadStartEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance associated with the event.</param>
    /// <param name="frame">The frame that started loading.</param>
    /// <param name="transitionType">The transition type for the navigation.</param>
    internal FramePageLoadStartEventArgs(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
    {
        Browser = browser;
        Frame = frame;
        TransitionType = transitionType;
    }
}
