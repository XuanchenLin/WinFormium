// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface used for asynchronous continuation of
/// CefExtensionHandler::GetExtensionResource.
/// </summary>
public sealed unsafe partial class CefGetExtensionResourceCallback
{
    /// <summary>
    /// Continue the request. Read the resource contents from |stream|.
    /// </summary>
    public void Continue(CefStreamReader stream)
    {
        var n_stream = stream.ToNative();
        cef_get_extension_resource_callback_t.cont(_self, n_stream);
    }
    
    /// <summary>
    /// Cancel the request.
    /// </summary>
    public void Cancel()
    {
        cef_get_extension_resource_callback_t.cancel(_self);
    }
    
}
