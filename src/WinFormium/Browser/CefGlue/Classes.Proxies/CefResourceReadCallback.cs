// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback for asynchronous continuation of CefResourceHandler::Read().
/// </summary>
public sealed unsafe partial class CefResourceReadCallback
{
    /// <summary>
    /// Callback for asynchronous continuation of Read(). If |bytes_read| == 0
    /// the response will be considered complete. If |bytes_read| > 0 then Read()
    /// will be called again until the request is complete (based on either the
    /// result or the expected content length). If |bytes_read| &lt; 0 then the
    /// request will fail and the |bytes_read| value will be treated as the error
    /// code.
    /// </summary>
    public void Continue(int bytesRead)
    {
        cef_resource_read_callback_t.cont(_self, bytesRead);
    }
}
