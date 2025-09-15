﻿// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewJSDialogHandler : CefJSDialogHandler
{
    public IJSDialogHandler Handler { get; }

    public WebViewJSDialogHandler(IJSDialogHandler handler)
    {
        Handler = handler;
    }

    protected override bool OnBeforeUnloadDialog(CefBrowser browser, string messageText, bool isReload, CefJSDialogCallback callback)
    {
        return Handler.OnBeforeUnloadDialog(browser, messageText, isReload, callback);
    }

    protected override void OnDialogClosed(CefBrowser browser)
    {
        Handler.OnDialogClosed(browser);
    }

    protected override bool OnJSDialog(CefBrowser browser, string originUrl, CefJSDialogType dialogType, string message_text, string default_prompt_text, CefJSDialogCallback callback, out bool suppress_message)
    {
        return Handler.OnJSDialog(browser, originUrl, dialogType, message_text, default_prompt_text, callback, out suppress_message);
    }

    protected override void OnResetDialogState(CefBrowser browser)
    {
        Handler.OnResetDialogState(browser);
    }
}
