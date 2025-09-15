// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

internal static class CefV8ContextExtensions
{
    public static CefV8ContextWrapper EnterOrFail(this CefV8Context context, bool shallDispose = true)
    {
        if (!context.Enter())
        {
            throw new InvalidOperationException("Could not enter context");
        }
        return new CefV8ContextWrapper(context, shallDispose);
    }

    public static (CefV8Value promiseFunc, CefV8Value resolveFunc, CefV8Value rejectFunc) CreatePromise(this CefV8Context context)
    {
        var internalObj = context.GetGlobal()?.GetValue("__winformium");

        var func = internalObj?.GetValue("createPromise");


        if (internalObj is null || func is null)
        {
            throw new InvalidOperationException("Unable to create promise");
        }

        var promiseHolder = func.ExecuteFunction(internalObj, []);

        var promise = promiseHolder?.GetValue("promise");
        var resolve = promiseHolder?.GetValue("resolve");
        var reject = promiseHolder?.GetValue("reject");

        if (promise is null || resolve is null || reject is null)
        {
            throw new InvalidOperationException("Unable to create promise");
        }

        CefObjectTracker.Untrack(promise);
        CefObjectTracker.Untrack(resolve);
        CefObjectTracker.Untrack(reject);

        return (promise, resolve, reject);

    }

    public static CefV8Value CreatePromise2(this CefV8Context context)
    {
        using var wrapper = context.EnterOrFail(false);
        var promise = CefV8Value.CreatePromise();

        

        return promise;
    }
}
