// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
public partial class Formium : IDisplayHandler
{
    /// <summary>
    /// Called when the browser's address changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame whose address changed.</param>
    /// <param name="url">The new address.</param>
    void IDisplayHandler.OnAddressChange(CefBrowser browser, CefFrame frame, string url)
    {
        DisplayHandler?.OnAddressChange(browser, frame, url);

        if (frame.IsMain)
        {
            InvokeIfRequired(() =>
            {
                BrowserAddressChange?.Invoke(this, new BrowserAddressChangeEventArgs(browser, frame, url));
            });
        }
        else
        {
            InvokeIfRequired(() =>
            {
                FrameAddressChange?.Invoke(this, new FrameAddressChangeEventArgs(browser, frame, url));
            });
        }
    }

    /// <summary>
    /// Called when the browser is about to auto-resize.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="newSize">The new size requested for the browser.</param>
    /// <returns>True if the resize was handled, otherwise false.</returns>
    bool IDisplayHandler.OnAutoResize(CefBrowser browser, ref CefSize newSize)
    {
        var retval = DisplayHandler?.OnAutoResize(browser, ref newSize) ?? false;

        var size = new CefSize(newSize.Width, newSize.Height);

        var args = new BrowserAutoResizeEventArgs(browser, size);


        InvokeIfRequired(() =>
        {
            BrowserAutoResize?.Invoke(this, args);
        });

        if (args.Handled)
        {
            newSize.Width = args.NewSize.Width;
            newSize.Height = args.NewSize.Height;
        }

        return retval || args.Handled;
    }

    /// <summary>
    /// Called when a console message is received.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="level">The log severity level.</param>
    /// <param name="message">The console message.</param>
    /// <param name="source">The source of the message.</param>
    /// <param name="line">The line number of the message.</param>
    /// <returns>True if the message was handled, otherwise false.</returns>
    bool IDisplayHandler.OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
    {
        var retval = DisplayHandler?.OnConsoleMessage(browser, level, message, source, line) ?? false;

        var args = new ConsoleMessageEventArgs(browser, level, message, source, line);

        InvokeIfRequired(() =>
        {
            ConsoleMessage?.Invoke(this, args);
        });

        return retval || args.Handled;
    }

    /// <summary>
    /// Called when the browser's cursor changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="cursorHandle">The handle to the new cursor.</param>
    /// <param name="type">The type of the cursor.</param>
    /// <param name="customCursorInfo">Custom cursor information, if any.</param>
    /// <returns>True if the cursor change was handled, otherwise false.</returns>
    bool IDisplayHandler.OnCursorChange(CefBrowser browser, nint cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
    {
        var retval = DisplayHandler?.OnCursorChange(browser, cursorHandle, type, customCursorInfo) ?? false;

        var args = new CursorChangeEventArgs(browser, cursorHandle, type, customCursorInfo);


        InvokeIfRequired(() =>
        {
            if (WindowStyleSettings.IsOffScreenRendering)
            {
                HostWindow.Cursor = new Cursor(cursorHandle);
            }

            CursorChange?.Invoke(this, args);
        });

        return retval || args.Handled;
    }

    /// <summary>
    /// Called when the browser's favicon URLs change.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="iconUrls">The new favicon URLs.</param>
    void IDisplayHandler.OnFaviconUrlChange(CefBrowser browser, string[] iconUrls)
    {
        DisplayHandler?.OnFaviconUrlChange(browser, iconUrls);

        InvokeIfRequired(() =>
        {
            FaviconUrlChange?.Invoke(this, new FaviconUrlChangeEventArgs(browser, iconUrls));
        });
    }

    /// <summary>
    /// Called when the browser's fullscreen mode changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="fullscreen">True if the browser is in fullscreen mode.</param>
    void IDisplayHandler.OnFullscreenModeChange(CefBrowser browser, bool fullscreen)
    {
        DisplayHandler?.OnFullscreenModeChange(browser, fullscreen);

        InvokeIfRequired(() =>
        {
            FullscreenModeChange?.Invoke(this, new FullscreenModeChangeEventArgs(browser, fullscreen));
        });
    }

    /// <summary>
    /// Called when the browser's loading progress changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="progress">The loading progress (0.0 to 1.0).</param>
    void IDisplayHandler.OnLoadingProgressChange(CefBrowser browser, double progress)
    {
        DisplayHandler?.OnLoadingProgressChange(browser, progress);

        InvokeIfRequired(() =>
        {
            PageLoadingProgressChange?.Invoke(this, new PageLoadingProgressChangeEventArgs(browser, (decimal)progress));
        });
    }

    /// <summary>
    /// Called when the browser's media access changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="hasVideoAccess">True if video access is granted.</param>
    /// <param name="hasAudioAccess">True if audio access is granted.</param>
    void IDisplayHandler.OnMediaAccessChange(CefBrowser browser, bool hasVideoAccess, bool hasAudioAccess)
    {
        DisplayHandler?.OnMediaAccessChange(browser, hasVideoAccess, hasAudioAccess);

        InvokeIfRequired(() =>
        {
            MediaAccessChange?.Invoke(this, new MediaAccessChangeEventArgs(browser, hasVideoAccess, hasAudioAccess));
        });
    }

    /// <summary>
    /// Called when the browser's status message changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="value">The new status message.</param>
    void IDisplayHandler.OnStatusMessage(CefBrowser browser, string value)
    {
        DisplayHandler?.OnStatusMessage(browser, value);

        InvokeIfRequired(() =>
        {
            StatusMessageChange?.Invoke(this, new StatusMessageChangeEventArgs(browser, value));
        });
    }

    /// <summary>
    /// Called when the browser's title changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="title">The new title.</param>
    void IDisplayHandler.OnTitleChange(CefBrowser browser, string title)
    {
        DisplayHandler?.OnTitleChange(browser, title);

        if (title == DocumentTitle) return;

        DocumentTitle = title;

        InvokeIfRequired(() =>
        {
            PageTitleChange?.Invoke(this, new PageTitleChangeEventArgs(browser, title));
        });

    }

    /// <summary>
    /// Called when the browser is about to display a tooltip.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="text">The tooltip text.</param>
    /// <returns>True if the tooltip was handled, otherwise false.</returns>
    bool IDisplayHandler.OnTooltip(CefBrowser browser, string text)
    {
        var retval = DisplayHandler?.OnTooltip(browser, text) ?? false;
        var args = new TooltipEventArgs(browser, text);
        InvokeIfRequired(() =>
        {
            Tooltip?.Invoke(this, args);
        });
        return retval || args.Handled;
    }

    /// <summary>
    /// Occurs when the browser's title has changed.
    /// </summary>
    public event EventHandler<PageTitleChangeEventArgs>? PageTitleChange;
    /// <summary>
    /// Occurs when the browser's address has changed.
    /// </summary>
    public event EventHandler<BrowserAddressChangeEventArgs>? BrowserAddressChange;
    /// <summary>
    /// Occurs when one frame of the browser's address has changed.
    /// </summary>
    public event EventHandler<FrameAddressChangeEventArgs>? FrameAddressChange;
    /// <summary>
    /// Occurs when the browser's cursor has changed.
    /// </summary>
    public event EventHandler<CursorChangeEventArgs>? CursorChange;
    /// <summary>
    /// Occurs when the browser's loading progress has changed.
    /// </summary>
    public event EventHandler<PageLoadingProgressChangeEventArgs>? PageLoadingProgressChange;
    /// <summary>
    /// Occurs when the browser's favicon has changed.
    /// </summary>
    public event EventHandler<FaviconUrlChangeEventArgs>? FaviconUrlChange;
    /// <summary>
    /// Occurs when the browser's status message has changed.
    /// </summary>
    public event EventHandler<StatusMessageChangeEventArgs>? StatusMessageChange;
    /// <summary>
    /// Occurs when the console message has changed.
    /// </summary>
    public event EventHandler<ConsoleMessageEventArgs>? ConsoleMessage;
    /// <summary>
    /// Occurs when the browser's fullscreen mode has changed.
    /// </summary>
    public event EventHandler<FullscreenModeChangeEventArgs>? FullscreenModeChange;
    /// <summary>
    /// Occurs when the browser's access to an audio and/or video source has changed.
    /// </summary>
    public event EventHandler<MediaAccessChangeEventArgs>? MediaAccessChange;
    /// <summary>
    /// Occurs when the browser is about to display a tooltip.
    /// </summary>
    public event EventHandler<TooltipEventArgs>? Tooltip;
    /// <summary>
    /// Occurs when the browser is about to resize.
    /// </summary>
    public event EventHandler<BrowserAutoResizeEventArgs>? BrowserAutoResize;
}
