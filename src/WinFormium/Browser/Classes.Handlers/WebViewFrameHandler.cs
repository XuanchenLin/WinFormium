// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewFrameHandler : CefFrameHandler
{
    public IFrameHandler Handler { get; }

    public WebViewFrameHandler(IFrameHandler handler)
    {
        Handler = handler;
    }

    protected override void OnFrameAttached(CefBrowser browser, CefFrame frame, bool reattached)
    {
        Handler.OnFrameAttached(browser, frame, reattached);
    }

    protected override void OnFrameCreated(CefBrowser browser, CefFrame frame)
    {
        Handler.OnFrameCreated(browser, frame);
    }

    protected override void OnFrameDetached(CefBrowser browser, CefFrame frame)
    {
        Handler.OnFrameDetached(browser, frame);
    }

    protected override void OnMainFrameChanged(CefBrowser browser, CefFrame? oldFrame, CefFrame? newFrame)
    {
        Handler.OnMainFrameChanged(browser, oldFrame, newFrame);
    }
}
