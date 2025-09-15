// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
internal sealed class ActionTask : CefTask
{
    private Action? _action;

    public ActionTask(Action action)
    {
        _action = action;
    }

    protected override void Execute()
    {
        _action?.Invoke();
        _action = null;
    }

    public static void Run(Action action, CefThreadId threadId = CefThreadId.UI)
    {
        CefRuntime.PostTask(threadId, new ActionTask(action));
    }
}
