// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewFindHandler : CefFindHandler
{
    public IFindHandler Handler { get; }

    public WebViewFindHandler(IFindHandler handler)
    {
        Handler = handler;
    }

    protected override void OnFindResult(CefBrowser browser, int identifier, int count, CefRectangle selectionRect, int activeMatchOrdinal, bool finalUpdate)
    {
        Handler.OnFindResult(browser, identifier, count, selectionRect, activeMatchOrdinal, finalUpdate);
    }

}
