// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Interface to implement to be notified of asynchronous completion via
/// CefCookieManager::DeleteCookies().
/// </summary>
public abstract unsafe partial class CefDeleteCookiesCallback
{
    private void on_complete(cef_delete_cookies_callback_t* self, int num_deleted)
    {
        CheckSelf(self);
        OnComplete(num_deleted);
    }

    /// <summary>
    /// Method that will be called upon completion. |num_deleted| will be the
    /// number of cookies that were deleted.
    /// </summary>
    protected abstract void OnComplete(int numDeleted);
}
