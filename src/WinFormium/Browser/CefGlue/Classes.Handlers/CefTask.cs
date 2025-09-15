// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Implement this interface for asynchronous task execution. If the task is
/// posted successfully and if the associated message loop is still running then
/// the Execute() method will be called on the target thread. If the task fails
/// to post then the task object may be destroyed on the source thread instead
/// of the target thread. For this reason be cautious when performing work in
/// the task object destructor.
/// </summary>
public abstract unsafe partial class CefTask
{
    private void execute(cef_task_t* self)
    {
        CheckSelf(self);

        Execute();
    }

    /// <summary>
    /// Method that will be executed on the target thread.
    /// </summary>
    protected abstract void Execute();
}
