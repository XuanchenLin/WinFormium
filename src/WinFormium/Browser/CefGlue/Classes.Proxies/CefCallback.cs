// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Generic callback interface used for asynchronous continuation.
/// </summary>
public sealed unsafe partial class CefCallback
{
    /// <summary>
    /// Continue processing.
    /// </summary>
    public void Continue()
    {
        cef_callback_t.cont(_self);
    }

    /// <summary>
    /// Cancel processing.
    /// </summary>
    public void Cancel()
    {
        cef_callback_t.cancel(_self);
    }
}
