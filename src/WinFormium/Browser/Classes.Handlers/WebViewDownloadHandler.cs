// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewDownloadHandler : CefDownloadHandler
{
    public IDownloadHandler Handler { get; }

    public WebViewDownloadHandler(IDownloadHandler handler)
    {
        Handler = handler;
    }

    protected override bool CanDownload(CefBrowser browser, string url, string requestMethod)
    {
        return Handler.CanDownload(browser, url, requestMethod);
    }

    protected override void OnBeforeDownload(CefBrowser browser, CefDownloadItem downloadItem, string suggestedName, CefBeforeDownloadCallback callback)
    {
        Handler.OnBeforeDownload(browser, downloadItem, suggestedName, callback);
    }

    protected override void OnDownloadUpdated(CefBrowser browser, CefDownloadItem downloadItem, CefDownloadItemCallback callback)
    {
        Handler.OnDownloadUpdated(browser, downloadItem, callback);
    }

}
