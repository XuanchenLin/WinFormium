// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using static WinFormium.JavaScript.JavaScriptEngine;

namespace WinFormium.JavaScript;

// Browser side
internal partial class JavaScriptEngine
{
    protected override void InitializeOnBrowserSide()
    {

    }

    private bool _isContextCreated = false;

    public override void ContextCreatedOnBrowserSide(CefBrowser browser, CefFrame frame)
    {
        //if(frame.IsMain)
        //{
        //}
        ExecuteScriptOnDocumentCreated(frame);
    }

    public override void ContextReleasedOnBrowserSide(CefBrowser browser, CefFrame frame)
    {
        foreach (var task in _pendingJavaScriptEvaluationTasks)
        {
            task.Value.TrySetCanceled();
        }
    }

    #region Evaluate
    private static volatile int lastTaskId;

    private readonly ConcurrentDictionary<int, TaskCompletionSource<string?>> _pendingJavaScriptEvaluationTasks = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<JavaScriptExecuteScriptResult>> _pendingJavaScriptEvaluationWithResultTasks = new();

    public Task<string?> EvaluateJavaScriptAsync(CefFrame frame, string script, string? url = null, int line = 0)
    {
        ArgumentNullException.ThrowIfNull(script);

        return EvaluateAsync(frame, script, url ?? (string.IsNullOrEmpty(frame.Url) ? "about:blank" : frame.Url), line);
    }

    private Task<string?> EvaluateAsync(CefFrame frame, string script, string url = "about:blank", int line = 0)
    {
        var taskId = lastTaskId++;

        var message = new JsEvaluationMessage
        {
            TaskId = taskId,
            Script = script,
            Url = url,
            Line = line,
            WithResult = false
        };

        var messageReceiveCompletionSource = new TaskCompletionSource<string?>();

        if (_pendingJavaScriptEvaluationTasks.TryAdd(taskId, messageReceiveCompletionSource))
        {
            try
            {
                var cefMessage = message.ToCefProcessMessage();
                frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
            }
            catch (Exception ex)
            {
                messageReceiveCompletionSource.SetException(ex);
            }
        }
        else
        {
            messageReceiveCompletionSource.SetException(new InvalidOperationException($"TaskId {taskId} already exists in pending tasks."));
        }

        return messageReceiveCompletionSource.Task;
    }

    public Task<JavaScriptExecuteScriptResult> EvaluateJavaScriptWithResultAsync(CefFrame frame, string script, string? url = null, int line = 0)
    {
        ArgumentNullException.ThrowIfNull(script);

        return EvaluateWithResultAsync(frame, script, url ?? (string.IsNullOrEmpty(frame.Url) ? "about:blank" : frame.Url), line);
    }

    private Task<JavaScriptExecuteScriptResult> EvaluateWithResultAsync(CefFrame frame, string script, string url = "about:blank", int line = 0)
    {
        var taskId = lastTaskId++;

        var message = new JsEvaluationMessage
        {
            TaskId = taskId,
            Script = script,
            Url = url,
            Line = line,
            WithResult = true
        };

        var messageReceiveCompletionSource = new TaskCompletionSource<JavaScriptExecuteScriptResult>();

        if (_pendingJavaScriptEvaluationWithResultTasks.TryAdd(taskId, messageReceiveCompletionSource))
        {
            try
            {
                var cefMessage = message.ToCefProcessMessage();
                frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
            }
            catch (Exception ex)
            {
                messageReceiveCompletionSource.SetException(ex);
            }
        }
        else
        {
            messageReceiveCompletionSource.SetException(new InvalidOperationException($"TaskId {taskId} already exists in pending tasks."));
        }

        return messageReceiveCompletionSource.Task;
    }


    private void OnJsEvaluationCompleteMessage(ProcessMessage message)
    {
        var args = JsEvaluationCompleteMessage.FromCefProcessMessage(message.Message);

        if (args.WithResult)
        {
            if (_pendingJavaScriptEvaluationWithResultTasks.TryRemove(args.TaskId, out var taskCompletionSource))
            {
                taskCompletionSource.SetResult(new JavaScriptExecuteScriptResult(args));
            }
        }
        else
        {
            if (_pendingJavaScriptEvaluationTasks.TryRemove(args.TaskId, out var taskCompletionSource))
            {

                if (args.Success)
                {

                    if (args.Data is null)
                    {
                        taskCompletionSource.SetResult(args.Data);
                    }
                    else
                    {
                        var data = args.Data;

                        if ((data.StartsWith("'") && data.EndsWith("'")) || data.StartsWith("\"") && data.EndsWith("\""))
                        {
                            data = data[1..^1];

                            data = Regex.Unescape(data);
                        }

                        taskCompletionSource.SetResult(data);
                    }


                }
                else
                {
                    JavaScriptEvaluationException jsException;
                    if (args.JsException != null)
                    {
                        jsException = JavaScriptEvaluationException.FromJson(args.JsException);
                    }
                    else
                    {
                        jsException = new JavaScriptEvaluationException
                        {

                            EndColumn = 0,
                            Message = "Unhandle exception.",
                            StartColumn = 0,
                            EndPosition = 0,
                            StartPosition = 0,
                            LineNumber = 0,
                            ScriptResourceName = message.Frame.Url ?? "about:blank",
                            SourceLine = string.Empty
                        };
                    }
                    taskCompletionSource.SetException(new JavaScriptException(jsException.Message, jsException));
                }
            }
        }


    }

    #endregion

    #region PostMessage

    /// <summary>
    /// Posts a web message as JSON to the specified frame.
    /// </summary>
    /// <param name="frame">
    /// The <see cref="CefFrame"/> to which the message will be posted. 
    /// </param>
    /// <param name="webMessageAsJson">
    /// The web message as a JSON string to be posted.
    /// </param>
    public static void PostWebMessageAsJson(CefFrame frame, string webMessageAsJson)
    {
        var msg = new JsPostWebMessageToRenderMessage()
        {
            Data = webMessageAsJson,
            AsString = false,
        };
        var cefMessage = msg.ToCefProcessMessage();


        if (frame is not null)
        {
            frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
        }
    }

    /// <summary>
    /// Posts a web message as a string to the specified frame.
    /// </summary>
    /// <param name="frame">
    /// The <see cref="CefFrame"/> to which the message will be posted.
    /// </param>
    /// <param name="webMessageAsString">
    /// The web message as a string to be posted.
    /// </param>
    public static void PostWebMessageAsString(CefFrame frame, string webMessageAsString)
    {
        var msg = new JsPostWebMessageToRenderMessage()
        {
            Data = webMessageAsString,
            AsString = true,
        };
        var cefMessage = msg.ToCefProcessMessage();

        if (frame is not null)
        {
            frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
        }
    }


    public WebMessageReceivedDelegate? WebMessageReceived { get; set; }

    private void OnJsPostWebMessageToBrowserMessage(ProcessMessage message)
    {
        var args = JsPostWebMessageToBrowserMessage.FromCefProcessMessage(message.Message);

        var eventArgs = new WebMessageReceivedEventArgs(args);

        WebMessageReceived?.Invoke(eventArgs);


        CefFrameExtensions.WebMessageReceived?.Invoke(eventArgs);

    }

    public void PostWebMessageAsJson(string webMessageAsJson)
    {
        var msg = new JsPostWebMessageToRenderMessage()
        {
            Data = webMessageAsJson,
            AsString = false,
        };
        var cefMessage = msg.ToCefProcessMessage();

        var frame = Browser.GetMainFrame();

        if (frame is not null)
        {
            frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
        }



    }

    public void PostWebMessageAsString(string webMessageAsString)
    {
        var msg = new JsPostWebMessageToRenderMessage()
        {
            Data = webMessageAsString,
            AsString = true,
        };
        var cefMessage = msg.ToCefProcessMessage();

        var frame = Browser.GetMainFrame();

        if (frame is not null)
        {
            frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
        }
    }


    #endregion

    #region AddScriptToExecuteOnDocumentCreated

    private volatile int _scriptToExecuteOnDocumentCreatedId;
    private Dictionary<int, string> _scriptsToExecuteOnDocumentCreated = new();


    internal int AddScriptToExecuteOnDocumentCreated(string script)
    {


        var id = _scriptToExecuteOnDocumentCreatedId++;
        _scriptsToExecuteOnDocumentCreated.Add(id, script);

        if (_isContextCreated)
        {
            var frame = Browser.GetMainFrame();
            if (frame is not null)
            {
                frame.ExecuteJavaScript(script, frame.Url ?? "about:blank", 0);
            }
        }

        return id;
    }

    internal void RemoveScriptToExecuteOnDocumentCreated(int id)
    {
        if (_scriptsToExecuteOnDocumentCreated.ContainsKey(id))
        {
            _scriptsToExecuteOnDocumentCreated.Remove(id);
        }
    }

    private void ExecuteScriptOnDocumentCreated(CefFrame frame)
    {
        if (frame is not null && frame.IsMain)
        {
            foreach (var kv in _scriptsToExecuteOnDocumentCreated)
            {
                if (string.IsNullOrWhiteSpace(kv.Value)) continue;
                frame.ExecuteJavaScript(kv.Value, frame.Url ?? "about:blank", 0);
            }

            _isContextCreated = true;
        }
    }

    #endregion

    #region NativeObject
    public void RegisterNativeObject(string name, NativeProxyObject obj)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        NativeObjectRepository.Register(name, obj);
    }

    public void UnregisterNativeObject(string name)
    {
        NativeObjectRepository.Unregister(name);
    }

    #endregion
}

/// <summary>
/// Extensions for <see cref="CefFrame"/> to post web messages.
/// </summary>
internal static class CefFrameExtensions
{
    /// <summary>
    /// Posts a web message as JSON to the specified frame.
    /// </summary>
    /// <param name="frame">
    /// The <see cref="CefFrame"/> to which the message will be posted. 
    /// </param>
    /// <param name="webMessageAsJson">
    /// The web message as a JSON string to be posted.
    /// </param>
    public static void PostWebMessageAsJson(this CefFrame frame, string webMessageAsJson)
    {
        var msg = new JsPostWebMessageToRenderMessage()
        {
            Data = webMessageAsJson,
            AsString = false,
        };
        var cefMessage = msg.ToCefProcessMessage();


        if (frame is not null)
        {
            frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
        }
    }

    /// <summary>
    /// Posts a web message as a string to the specified frame.
    /// </summary>
    /// <param name="frame">
    /// The <see cref="CefFrame"/> to which the message will be posted.
    /// </param>
    /// <param name="webMessageAsString">
    /// The web message as a string to be posted.
    /// </param>
    public static void PostWebMessageAsString(this CefFrame frame, string webMessageAsString)
    {
        var msg = new JsPostWebMessageToRenderMessage()
        {
            Data = webMessageAsString,
            AsString = true,
        };
        var cefMessage = msg.ToCefProcessMessage();

        if (frame is not null)
        {
            frame.SendProcessMessage(CefProcessId.Renderer, cefMessage);
        }
    }

    internal static WebMessageReceivedDelegate? WebMessageReceived { get; set; }

    /// <summary>
    /// Registers a handler for web message received events on the specified frame.
    /// </summary>
    /// <param name="frame">
    /// The <see cref="CefFrame"/> on which to register the handler.
    /// </param>
    /// <param name="handler">
    /// The delegate to handle web message received events.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="handler"/> is <c>null</c>.
    /// </exception>
    public static void RegisterWebMessageReceivedHandler(this CefFrame frame, WebMessageReceivedDelegate handler)
    {
        if (handler is null) throw new ArgumentNullException(nameof(handler));

        WebMessageReceived += handler;
    }

    /// <summary>
    /// Unregisters a handler for web message received events on the specified frame.
    /// </summary>
    /// <param name="frame">
    /// The <see cref="CefFrame"/> from which to unregister the handler.
    /// </param>
    /// <param name="handler">
    /// The delegate to remove from the web message received events.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="handler"/> is <c>null</c>.
    /// </exception>
    public static void UnregisterWebMessageReceivedHandler(this CefFrame frame, WebMessageReceivedDelegate handler)
    {
        if (handler is null) throw new ArgumentNullException(nameof(handler));
        WebMessageReceived -= handler;
    }
}