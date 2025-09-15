// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
class CefV8ContextWrapper : IDisposable
{
    private readonly bool _shallDispose;

    internal CefV8ContextWrapper(CefV8Context context, bool shallDispose)
    {
        V8Context = context;
        _shallDispose = shallDispose;
        if (!shallDispose)
        {
            CefObjectTracker.Untrack(context);
        }
    }

    public CefV8Context V8Context { get; }

    public void Dispose()
    {
        V8Context.Exit();
        if (_shallDispose)
        {
            V8Context.Dispose();
        }
    }
}