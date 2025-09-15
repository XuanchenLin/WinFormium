// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewCommandHandler : CefCommandHandler
{
    public ICommandHandler Handler { get; }

    public WebViewCommandHandler(ICommandHandler handler)
    {
        Handler = handler;
    }

    protected override bool OnChromeCommand(CefBrowser browser, int commandId, CefWindowOpenDisposition disposition)
    {
        return Handler.OnChromeCommand(browser, commandId, disposition);
    }

}
