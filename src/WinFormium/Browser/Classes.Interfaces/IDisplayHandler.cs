// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling browser display events and UI changes.
/// Implement this interface to receive notifications about address, title, favicon, fullscreen, tooltips, status messages, console messages, auto-resize, loading progress, cursor changes, and media access changes.
/// </summary>
public interface IDisplayHandler
{
    /// <summary>
    /// Called when the browser address (URL) changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the address changed.</param>
    /// <param name="url">The new address (URL).</param>
    void OnAddressChange(CefBrowser browser, CefFrame frame, string url);

    /// <summary>
    /// Called when the page title changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="title">The new page title.</param>
    void OnTitleChange(CefBrowser browser, string title);

    /// <summary>
    /// Called when the favicon URL(s) change.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="iconUrls">Array of new favicon URLs.</param>
    void OnFaviconUrlChange(CefBrowser browser, string[] iconUrls);

    /// <summary>
    /// Called when the fullscreen mode changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="fullscreen">True if entering fullscreen, false if exiting.</param>
    void OnFullscreenModeChange(CefBrowser browser, bool fullscreen);

    /// <summary>
    /// Called when a tooltip is about to be shown.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="text">The tooltip text.</param>
    /// <returns>Return true to handle the tooltip display, false to use the default behavior.</returns>
    bool OnTooltip(CefBrowser browser, string text);

    /// <summary>
    /// Called when the status message changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="value">The new status message.</param>
    void OnStatusMessage(CefBrowser browser, string value);

    /// <summary>
    /// Called when a console message is output.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="level">The log severity level.</param>
    /// <param name="message">The console message.</param>
    /// <param name="source">The source of the message.</param>
    /// <param name="line">The line number of the message source.</param>
    /// <returns>Return true to suppress the message, false to allow default handling.</returns>
    bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line);

    /// <summary>
    /// Called when the browser requests to auto-resize.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="newSize">The new size requested for the browser.</param>
    /// <returns>Return true to allow the resize, false to ignore.</returns>
    bool OnAutoResize(CefBrowser browser, ref CefSize newSize);

    /// <summary>
    /// Called when the loading progress changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="progress">The loading progress value (0.0 to 1.0).</param>
    void OnLoadingProgressChange(CefBrowser browser, double progress);

    /// <summary>
    /// Called when the cursor changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="cursorHandle">The handle to the new cursor.</param>
    /// <param name="type">The type of the cursor.</param>
    /// <param name="customCursorInfo">Custom cursor information, if any.</param>
    /// <returns>Return true if the cursor change was handled, false for default handling.</returns>
    bool OnCursorChange(CefBrowser browser, nint cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo);

    /// <summary>
    /// Called when the browser's media access state changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="hasVideoAccess">True if video access is granted.</param>
    /// <param name="hasAudioAccess">True if audio access is granted.</param>
    void OnMediaAccessChange(CefBrowser browser, bool hasVideoAccess, bool hasAudioAccess);
}
