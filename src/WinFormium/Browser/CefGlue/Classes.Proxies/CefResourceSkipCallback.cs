// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback for asynchronous continuation of CefResourceHandler::Skip().
/// </summary>
public sealed unsafe partial class CefResourceSkipCallback
{
    /// <summary>
    /// Callback for asynchronous continuation of Skip(). If |bytes_skipped| > 0
    /// then either Skip() will be called again until the requested number of
    /// bytes have been skipped or the request will proceed. If |bytes_skipped|
    /// &lt;= 0 the request will fail with ERR_REQUEST_RANGE_NOT_SATISFIABLE.
    /// </summary>
    public void Continue(long bytesSkipped)
    {
        cef_resource_skip_callback_t.cont(_self, bytesSkipped);
    }
}
