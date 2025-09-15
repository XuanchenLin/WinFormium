// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;
// render side
internal partial class JavaScriptEngine
{

    internal ConcurrentDictionary<string, PromiseData> FunctionApplyPendingPromises { get; } = new();

    protected override void InitializeOnRenderSide()
    {

        var formiumInitializationScript = Resources.Files.formium_js;

        CefRuntime.RegisterExtension(nameof(WinFormium), formiumInitializationScript, InitializationScriptV8Handler.Create(this));

    }
    public override void ContextCreatedOnRenderSide(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
    }

    public override void ContextReleasedOnRenderSide(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
        ReleasePendingPromises(context, frame.IsMain);
    }

    private void ReleasePendingPromises(CefV8Context context, bool isMain)
    {
        void ReleasePromiseHolder(PromiseData promiseHolder)
        {
            promiseHolder.V8Context.Dispose();
            promiseHolder.Dispose();
        }

        if (isMain)
        {
            foreach (var promiseHolder in FunctionApplyPendingPromises.Values)
            {
                ReleasePromiseHolder(promiseHolder);
            }
            FunctionApplyPendingPromises.Clear();
        }
        else
        {
            foreach (var promiseHolderEntry in FunctionApplyPendingPromises.ToArray())
            {
                if (promiseHolderEntry.Value.V8Context.IsSame(context))
                {
                    FunctionApplyPendingPromises.TryRemove(promiseHolderEntry.Key, out var dummy);
                    ReleasePromiseHolder(promiseHolderEntry.Value);
                }
            }
        }
    }

    private void OnJsResolveOrRejectRemotePromise(ProcessMessage args)
    {
        var frame = args.Frame;

        var message = JsResolveOrRejectRemotePromise.FromCefProcessMessage(args.Message);
        var callId = message.CallId;

        if (FunctionApplyPendingPromises.TryGetValue(callId, out var promiseData) && promiseData is not null)
        {
            var context = promiseData.V8Context;
            context.GetTaskRunner().PostTask(new ActionTask(() =>
            {
                using (CefObjectTracker.StartTracking())
                using (context.EnterOrFail())
                {
                    var promise = promiseData.Promise;

                    if (message.Resolve)
                    {
                        var result = message.Data;

                        CefV8Value? value = null;

                        if (result != null)
                        {
                            value = result.JsonStringToCefV8Value(context);
                        }

                        promise.ResolvePromise(value?? CefV8Value.CreateUndefined());
                    }
                    else
                    {
                        promise.RejectPromise(message.Data ?? string.Empty);
                    }

                    promise.Dispose();
                }
            }));
        }
    }

    private void OnJsEvaluationMessage(ProcessMessage args)
    {
        var frame = args.Frame;

        JsEvaluationCompleteMessage? result = null;

        using (var wrapper = frame.V8Context.EnterOrFail())
        {
            var message = JsEvaluationMessage.FromCefProcessMessage(args.Message);

            var context = wrapper.V8Context;
            var global = context.GetGlobal();


            if (context.TryEval(message.Script, message.Url, message.Line, out var value, out var exception))
            {
                var stringify = global?.GetValue("JSON")?.GetValue("stringify");

                if (global is not null && stringify is not null && value is not null)
                {
                    value = stringify.ExecuteFunction(global, [value]);

                    if (!value.HasException)
                    {
                        result = new JsEvaluationCompleteMessage
                        {
                            TaskId = message.TaskId,
                            Success = true,
                            Data = value?.GetStringValue(),
                            WithResult = message.WithResult,
                        };
                    }
                    else
                    {
                        exception = value.GetException();
                    }
                }
            }

            if (result == null)
            {
                result = new JsEvaluationCompleteMessage
                {
                    TaskId = message.TaskId,
                    Success = false,
                    WithResult = message.WithResult,
                    JsException = exception != null ? new JavaScriptEvaluationException
                    {
                        Message = exception.Message,
                        StartColumn = exception.StartColumn,
                        StartPosition = exception.StartPosition,
                        EndColumn = exception.EndColumn,
                        EndPosition = exception.EndPosition,
                        LineNumber = exception.LineNumber,
                        ScriptResourceName = exception.ScriptResourceName,
                        SourceLine = exception.SourceLine,
                    }.ToJson() : null
                };
            }



            frame.SendProcessMessage(CefProcessId.Browser, result.Value.ToCefProcessMessage());
        }




    }

    #region FormiumInitializationScriptHandler

    private void OnJsPostWebMessageToRenderMessage(ProcessMessage message)
    {
        var args = JsPostWebMessageToRenderMessage.FromCefProcessMessage(message.Message);

        var asString = args.AsString;
        var data = args.Data;

        var frame = message.Frame;


        try
        {
            using (var wrapper = frame.V8Context.EnterOrFail())
            {
                var context = wrapper.V8Context;
                var internalObj = context.GetGlobal()?.GetValue("__winformium");


                var func = internalObj?.GetValue("emitWebMessage");

                if (internalObj is null || func is null || !func.IsFunction) return;

                var retval = func.ExecuteFunction(internalObj, [
                    CefV8Value.CreateString("message"),
                    CefV8Value.CreateBool(asString),
                    CefV8Value.CreateString(data??string.Empty)
                ]);

                if (retval.HasException)
                {
                    Console.WriteLine($"PostWebMessageOnBrowserSide failed:{retval.GetException().Message}");
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PostWebMessageOnBrowserSide failed:{ex.Message}");
        }
    }
    #endregion
}
