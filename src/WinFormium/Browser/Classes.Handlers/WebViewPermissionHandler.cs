// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewPermissionHandler : CefPermissionHandler
{
    public IPermissionHandler Handler { get; }

    public WebViewPermissionHandler(IPermissionHandler handler)
    {
        Handler = handler;
    }

    protected override void OnDismissPermissionPrompt(CefBrowser browser, ulong promptId, CefPermissionRequestResult result)
    {
        Handler.OnDismissPermissionPrompt(browser, promptId, result);
    }

    protected override bool OnRequestMediaAccessPermission(CefBrowser browser, CefFrame frame, string requestingOrigin, CefMediaAccessPermissionTypes requestedPermissions, CefMediaAccessCallback callback)
    {
        return Handler.OnRequestMediaAccessPermission(browser, frame, requestingOrigin, requestedPermissions, callback);
    }

    protected override bool OnShowPermissionPrompt(CefBrowser browser, ulong promptId, string requestingOrigin, CefPermissionRequestTypes requestedPermissions, CefPermissionPromptCallback callback)
    {
        return Handler.OnShowPermissionPrompt(browser, promptId, requestingOrigin, requestedPermissions, callback);
    }
}
