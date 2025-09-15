// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewLoadHandler : CefLoadHandler
{
    public ILoadHandler Handler { get; }

    public WebViewLoadHandler(ILoadHandler handler)
    {
        Handler = handler;
    }

    protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
    {
        Handler.OnLoadStart(browser, frame, transitionType);
    }

    protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {
        if (frame.IsMain)
        {
            frame.ExecuteJavaScript("window.formium && formium?.hostWindow?.internal?.setDocumentReadyState()", string.Empty, 0);
        }
        Handler.OnLoadEnd(browser, frame, httpStatusCode);
    }

    protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
    {
        Handler.OnLoadError(browser, frame, errorCode, errorText, failedUrl);
    }

    protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
    {
        Handler.OnLoadingStateChange(browser, isLoading, canGoBack, canGoForward);
    }

}
