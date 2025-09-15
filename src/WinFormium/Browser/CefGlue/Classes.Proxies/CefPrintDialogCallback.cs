// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface for asynchronous continuation of print dialog requests.
/// </summary>
public sealed unsafe partial class CefPrintDialogCallback
{
    /// <summary>
    /// Continue printing with the specified |settings|.
    /// </summary>
    public void Continue(CefPrintSettings settings)
    {
        cef_print_dialog_callback_t.cont(_self, settings.ToNative());
    }

    /// <summary>
    /// Cancel the printing.
    /// </summary>
    public void Cancel()
    {
        cef_print_dialog_callback_t.cancel(_self);
    }
}
