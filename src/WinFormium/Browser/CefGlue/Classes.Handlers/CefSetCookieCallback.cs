// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Interface to implement to be notified of asynchronous completion via
/// CefCookieManager::SetCookie().
/// </summary>
public abstract unsafe partial class CefSetCookieCallback
{
    private void on_complete(cef_set_cookie_callback_t* self, int success)
    {
        CheckSelf(self);
        OnComplete(success != 0);
    }

    /// <summary>
    /// Method that will be called upon completion. |success| will be true if the
    /// cookie was set successfully.
    /// </summary>
    protected abstract void OnComplete(bool success);
}
