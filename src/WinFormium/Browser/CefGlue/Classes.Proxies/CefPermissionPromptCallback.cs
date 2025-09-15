// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface used for asynchronous continuation of permission prompts.
/// </summary>
public sealed unsafe partial class CefPermissionPromptCallback
{
    /// <summary>
    /// Complete the permissions request with the specified |result|.
    /// </summary>
    public void Continue(CefPermissionRequestResult result)
    {
        cef_permission_prompt_callback_t.cont(_self, result);
    }
}
