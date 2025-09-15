// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewKeyboardHandler : CefKeyboardHandler
{
    public IKeyboardHandler Handler { get; }

    public WebViewKeyboardHandler(IKeyboardHandler handler)
    {
        Handler = handler;
    }

    protected override bool OnKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent)
    {
        return Handler.OnKeyEvent(browser, keyEvent, osEvent);
    }

    protected override bool OnPreKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint os_event, out bool isKeyboardShortcut)
    {
        return Handler.OnPreKeyEvent(browser, keyEvent, os_event, out isKeyboardShortcut);
    }
}
