// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewFocusHandler : CefFocusHandler
{
    public IFocusHandler Handler { get; }

    public WebViewFocusHandler(IFocusHandler handler)
    {
        Handler = handler;
    }

    protected override void OnGotFocus(CefBrowser browser)
    {
        Handler.OnGotFocus(browser);
    }

    protected override bool OnSetFocus(CefBrowser browser, CefFocusSource source)
    {
        return Handler.OnSetFocus(browser, source);
    }

    protected override void OnTakeFocus(CefBrowser browser, bool next)
    {
        Handler.OnTakeFocus(browser, next);
    }
}
