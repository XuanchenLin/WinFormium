// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewLifeSpanHandler : CefLifeSpanHandler
{
    public ILifeSpanHandler Handler { get; }

    public WebViewLifeSpanHandler(ILifeSpanHandler handler)
    {
        Handler = handler;
    }

    protected override void OnAfterCreated(CefBrowser browser)
    {
        Handler.OnAfterCreated(browser);
    }

    protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
    {
        return Handler.OnBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref extraInfo, ref noJavascriptAccess);
    }

    protected override void OnBeforeClose(CefBrowser browser)
    {
        Handler.OnBeforeClose(browser);
    }

    protected override bool DoClose(CefBrowser browser)
    {
        return Handler.DoClose(browser);
    }

}
