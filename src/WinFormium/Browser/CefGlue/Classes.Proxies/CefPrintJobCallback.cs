// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface for asynchronous continuation of print job requests.
/// </summary>
public sealed unsafe partial class CefPrintJobCallback
{
    /// <summary>
    /// Indicate completion of the print job.
    /// </summary>
    public void Continue()
    {
        cef_print_job_callback_t.cont(_self);
    }
}
