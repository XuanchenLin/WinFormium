// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Generic callback interface used for asynchronous completion.
/// </summary>
public abstract unsafe partial class CefCompletionCallback
{
    private void on_complete(cef_completion_callback_t* self)
    {
        CheckSelf(self);

        OnComplete();
    }
    
    /// <summary>
    /// Method that will be called once the task is complete.
    /// </summary>
    protected abstract void OnComplete();
}
