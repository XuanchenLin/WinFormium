// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface used for continuation of custom context menu display.
/// </summary>
public sealed unsafe partial class CefRunContextMenuCallback
{
    /// <summary>
    /// Complete context menu display by selecting the specified |command_id| and
    /// |event_flags|.
    /// </summary>
    public void Continue(int commandId, CefEventFlags eventFlags)
    {
        cef_run_context_menu_callback_t.cont(_self, commandId, eventFlags);
    }

    /// <summary>
    /// Cancel context menu display.
    /// </summary>
    public void Cancel()
    {
        cef_run_context_menu_callback_t.cancel(_self);
    }
}
