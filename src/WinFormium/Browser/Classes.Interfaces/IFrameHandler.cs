// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling browser frame events.
/// </summary>
public interface IFrameHandler
{
    /// <summary>
    /// Called when a new frame is created in the browser.
    /// </summary>
    /// <param name="browser">The browser instance where the frame is created.</param>
    /// <param name="frame">The newly created frame.</param>
    void OnFrameCreated(CefBrowser browser, CefFrame frame);

    /// <summary>
    /// Called when a frame is attached to the browser.
    /// </summary>
    /// <param name="browser">The browser instance where the frame is attached.</param>
    /// <param name="frame">The attached frame.</param>
    /// <param name="reattached">Indicates whether the frame was reattached.</param>
    void OnFrameAttached(CefBrowser browser, CefFrame frame, bool reattached);

    /// <summary>
    /// Called when a frame is detached from the browser.
    /// </summary>
    /// <param name="browser">The browser instance from which the frame is detached.</param>
    /// <param name="frame">The detached frame.</param>
    void OnFrameDetached(CefBrowser browser, CefFrame frame);

    /// <summary>
    /// Called when the main frame of the browser changes.
    /// </summary>
    /// <param name="browser">The browser instance where the main frame changed.</param>
    /// <param name="oldFrame">The previous main frame, or null if not available.</param>
    /// <param name="newFrame">The new main frame, or null if not available.</param>
    void OnMainFrameChanged(CefBrowser browser, CefFrame? oldFrame, CefFrame? newFrame);
}
