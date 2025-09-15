// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
public partial class Formium : ILoadHandler
{
    /// <inheritdoc/>
    void ILoadHandler.OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
    {
        LoadHandler?.OnLoadStart(browser, frame, transitionType);


        if (frame.IsMain)
        {
            InvokeIfRequired(() =>
            {
                BrowserLoadStart?.Invoke(this, new BrowserLoadStartEventArgs(browser, frame, transitionType));
            });
        }

        InvokeIfRequired(() =>
        {
            FrameLoadStart?.Invoke(this, new FramePageLoadStartEventArgs(browser, frame, transitionType));
        });

    }

    /// <inheritdoc/>
    void ILoadHandler.OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {
        LoadHandler?.OnLoadEnd(browser, frame, httpStatusCode);

        if (frame.IsMain)
        {
            InvokeIfRequired(() =>
            {
                BrowserLoadEnd?.Invoke(this, new BrowserLoadEndEventArgs(browser, frame, httpStatusCode));
            });
        }

        InvokeIfRequired(() =>
        {
            FrameLoadEnd?.Invoke(this, new FramePageLoadEndEventArgs(browser, frame, httpStatusCode));
        });
    }

    /// <inheritdoc/>
    void ILoadHandler.OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
    {
        LoadHandler?.OnLoadError(browser, frame, errorCode, errorText, failedUrl);

        if (frame.IsMain)
        {
            InvokeIfRequired(() =>
            {
                BrowserLoadError?.Invoke(this, new BrowserLoadErrorEventArgs(browser, frame, errorCode, errorText, failedUrl));
            });

            return;
        }


        InvokeIfRequired(() =>
        {
            FrameLoadError?.Invoke(this, new FramePageLoadErrorEventArgs(browser, frame, errorCode, errorText, failedUrl));
        });
    }

    /// <inheritdoc/>
    void ILoadHandler.OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
    {
        LoadHandler?.OnLoadingStateChange(browser, isLoading, canGoBack, canGoForward);

        InvokeIfRequired(() =>
        {
            BrowserLoadingStateChange?.Invoke(this, new BrowserLoadingStateChangeEventArgs(browser, isLoading, canGoBack, canGoForward));
        });
    }


    /// <summary>
    /// Occurs when the page loading state has changed.
    /// </summary>
    public event EventHandler<BrowserLoadingStateChangeEventArgs>? BrowserLoadingStateChange;
    /// <summary>
    /// Occurs when the page load has started.
    /// </summary>
    public event EventHandler<BrowserLoadStartEventArgs>? BrowserLoadStart;
    /// <summary>
    /// Occurs when the page load has ended with one or more errors.
    /// </summary>
    public event EventHandler<BrowserLoadErrorEventArgs>? BrowserLoadError;
    /// <summary>
    /// Occurs when the page load has ended..
    /// </summary>
    public event EventHandler<BrowserLoadEndEventArgs>? BrowserLoadEnd;
    /// <summary>
    /// Occurs when the frame page load has started.
    /// </summary>
    public event EventHandler<FramePageLoadStartEventArgs>? FrameLoadStart;
    /// <summary>
    /// Occurs when the frame page load has ended with one or more errors.
    /// </summary>
    public event EventHandler<FramePageLoadErrorEventArgs>? FrameLoadError;
    /// <summary>
    /// Occurs when the frame page load has ended.
    /// </summary>
    public event EventHandler<FramePageLoadEndEventArgs>? FrameLoadEnd;

}
