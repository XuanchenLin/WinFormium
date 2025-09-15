// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Implements the <see cref="ILoadHandler"/> interface to handle browser load events.
/// </summary>
partial class WebViewCore : ILoadHandler
{
    /// <inheritdoc/>
    void ILoadHandler.OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
    {
        BrowserClient?.LoadHandler?.OnLoadStart(browser, frame, transitionType);
    }

    /// <inheritdoc/>
    void ILoadHandler.OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {

        if (MainFrameChanging && frame.IsMain)
        {
            MainFrameChanging = false;
        }

        BrowserClient?.LoadHandler?.OnLoadEnd(browser, frame, httpStatusCode);

        //InvokeIfRequired(() => SetupBrowserMessageInterceptor());

        SetupBrowserMessageInterceptor();


    }

    /// <inheritdoc/>
    void ILoadHandler.OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
    {
        BrowserClient?.LoadHandler?.OnLoadError(browser, frame, errorCode, errorText, failedUrl);
    }

    /// <inheritdoc/>
    void ILoadHandler.OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
    {
        BrowserClient?.LoadHandler?.OnLoadingStateChange(browser, isLoading, canGoBack, canGoForward);
    }

}

