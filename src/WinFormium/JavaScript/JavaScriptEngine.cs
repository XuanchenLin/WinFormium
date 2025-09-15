// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;
internal partial class JavaScriptEngine : ProcessCommunicationBridgeHandler
{
    #region Messages

    public struct JsResolveOrRejectRemotePromise
    {
        public const string Name = nameof(JsResolveOrRejectRemotePromise);
        public string CallId;
        public bool Resolve;
        public string? Data;

        public CefProcessMessage ToCefProcessMessage()
        {
            var message = CefProcessMessage.Create(Name);
            using (var arguments = message.Arguments!)
            {
                arguments.SetString(0, CallId);
                arguments.SetBool(1, Resolve);
                arguments.SetString(2, Data!);
            }

            return message;
        }

        public static JsResolveOrRejectRemotePromise FromCefProcessMessage(CefProcessMessage message)
        {
            using (var arguments = message.Arguments!)
            {
                return new JsResolveOrRejectRemotePromise
                {
                    CallId = arguments.GetString(0),
                    Resolve = arguments.GetBool(1),
                    Data = arguments.GetString(2)
                };
            }
        }
    }

    public struct JsPostWebMessageToBrowserMessage
    {
        public const string Name = nameof(JsPostWebMessageToBrowserMessage);
        public string? Source;
        public string? Data;
        public long FrameId;
        public int BrowserId;

        public CefProcessMessage ToCefProcessMessage()
        {
            var message = CefProcessMessage.Create(Name)!;
            using (var arguments = message.Arguments!)
            {
                arguments.SetInt(0, BrowserId);
                arguments.SetInt(1, (int)(FrameId >> 16));
                arguments.SetInt(2, (int)(FrameId & 0xFFFFFFFF));
                arguments.SetString(3, Source ?? string.Empty);
                arguments.SetString(4, Data ?? string.Empty);
            }
            return message;
        }

        public static JsPostWebMessageToBrowserMessage FromCefProcessMessage(CefProcessMessage message)
        {
            using (var args = message.Arguments!)
            {
                return new JsPostWebMessageToBrowserMessage
                {
                    BrowserId = args.GetInt(0),
                    FrameId = (long)args.GetInt(1) << 16 | (uint)args.GetInt(2),
                    Source = args.GetString(3),
                    Data = args.GetString(4)
                };
            }
        }

    }

    public struct JsPostWebMessageToRenderMessage
    {
        public const string Name = nameof(JsPostWebMessageToRenderMessage);
        public string? Data;
        public bool AsString;

        public CefProcessMessage ToCefProcessMessage()
        {
            var message = CefProcessMessage.Create(Name)!;
            using (var arguments = message.Arguments!)
            {
                arguments.SetString(0, Data ?? string.Empty);
                arguments.SetBool(1, AsString);
            }
            return message;
        }

        public static JsPostWebMessageToRenderMessage FromCefProcessMessage(CefProcessMessage message)
        {
            using (var args = message.Arguments!)
            {
                return new JsPostWebMessageToRenderMessage
                {
                    Data = args.GetString(0),
                    AsString = args.GetBool(1)
                };
            }
        }

    }
    public struct JsEvaluationMessage
    {
        public const string Name = nameof(JsEvaluationMessage);
        public int TaskId;
        public string Script;
        public string Url;
        public int Line;
        public bool WithResult;

        public CefProcessMessage ToCefProcessMessage()
        {
            var message = CefProcessMessage.Create(Name)!;
            using (var arguments = message.Arguments!)
            {
                arguments.SetInt(0, TaskId);
                arguments.SetString(1, Script);
                arguments.SetString(2, Url);
                arguments.SetInt(3, Line);
                arguments.SetBool(4, WithResult);
            }
            return message;
        }

        public static JsEvaluationMessage FromCefProcessMessage(CefProcessMessage message)
        {
            using (var args = message.Arguments!)
            {
                return new JsEvaluationMessage
                {
                    TaskId = args.GetInt(0),
                    Script = args.GetString(1),
                    Url = args.GetString(2),
                    Line = args.GetInt(3),
                    WithResult = args.GetBool(4)
                };
            }
        }
    }

    public struct JsEvaluationCompleteMessage
    {
        public const string Name = nameof(JsEvaluationCompleteMessage);

        public int TaskId;
        public bool Success;
        public string? Data;
        public string? JsException;
        public bool WithResult;


        public CefProcessMessage ToCefProcessMessage()
        {
            var message = CefProcessMessage.Create(Name)!;
            using (var args = message.Arguments!)
            {
                args.SetInt(0, TaskId);
                args.SetBool(1, Success);
                args.SetString(2, Data ?? string.Empty);
                args.SetString(3, JsException ?? string.Empty);
                args.SetBool(4, WithResult);
            }
            return message;
        }

        public static JsEvaluationCompleteMessage FromCefProcessMessage(CefProcessMessage message)
        {
            using (var args = message.Arguments!)
            {
                return new JsEvaluationCompleteMessage
                {
                    TaskId = args.GetInt(0),
                    Success = args.GetBool(1),
                    Data = args.GetString(2),
                    JsException = args.GetString(3),
                    WithResult = args.GetBool(4)
                };
            }
        }
    }

    #endregion



    public NativeProxyObjectRepository NativeObjectRepository { get; } = new();


    public JavaScriptEngine(ProcessCommunicationBridge bridge) : base(bridge)
    {
        RegisterMessageHandler(JsEvaluationMessage.Name, OnJsEvaluationMessage);
        RegisterMessageHandler(JsEvaluationCompleteMessage.Name, OnJsEvaluationCompleteMessage);
        RegisterMessageHandler(JsPostWebMessageToBrowserMessage.Name, OnJsPostWebMessageToBrowserMessage);
        RegisterMessageHandler(JsPostWebMessageToRenderMessage.Name, OnJsPostWebMessageToRenderMessage);
        RegisterMessageHandler(JsResolveOrRejectRemotePromise.Name, OnJsResolveOrRejectRemotePromise);

        RegisterRequestHandler("GetNativeObject", OnJsGetNativeObject);
        RegisterRequestHandler("GetNativeObjectProperty", OnJsGetNativeObjectProperty);
        RegisterRequestHandler("SetNativeObjectProperty", OnJsSetNativeObjectProperty);
        RegisterRequestHandler("CallNativeObjectMethod", OnJSCallNativeObjectMethod);

    }



    private ProcessResponse OnJSCallNativeObjectMethod(ProcessRequest request)
    {
        var data = request.Data;

        if (data is null || string.IsNullOrEmpty(data))
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] CallNativeObjectMethod requires property data." };
        }
        var json = JsonSerializer.Deserialize(data, WinFormiumJsonSerializerContext.Default.JsonNativeObjectApplyFunction);

        if (json == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] CallNativeObjectMethod requires property data." };
        }

        var obj = NativeProxyObject.Get(json.ObjectId);

        if (obj == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = $"[WinFormium] The NativeObject is not registered." };
        }



        NativeProxyObjectFunctionApplyEventArgs args = new(json.ObjectId, json.Property, json.Arguments);

        obj.OnFunctionApply(args);

        if (args.Canceled)
        {
            return new ProcessResponse
            {
                Success = false,
                ExceptionMessage = $"[WinFormium] Function \"{json.Property}\" does not exist in this NativeObject or it can not be applied."
            };
        }


        if (args.Asynconous)
        {
            var promise = args.Promise;
            var frame = Browser.GetFrame(request.FrameId);

            if (promise is null || frame is null)
            {
                return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] CallNativeObjectMethod returned promise but it is null." };
            }

            args.ReturnValue = $"{promise.GenerateCallId()}";


            promise.CallRemoteResovle += (json) =>
            {
                frame.SendProcessMessage(CefProcessId.Renderer, new JsResolveOrRejectRemotePromise
                {
                    CallId = promise.GenerateCallId(),
                    Resolve = true,
                    Data = json
                }.ToCefProcessMessage());
            };

            promise.CallRemoteReject += (reason) =>
            {
                frame.SendProcessMessage(CefProcessId.Renderer, new JsResolveOrRejectRemotePromise
                {
                    CallId = promise.GenerateCallId(),
                    Resolve = false,
                    Data = reason
                }.ToCefProcessMessage());
            };
        }



        return new ProcessResponse
        {
            Success = true,
            Data = JsonSerializer.Serialize(new JsonNativeObjectApplyFunctionReturnValue(args.ObjectId, args.PropertyName, args.Asynconous, args.ReturnValue), WinFormiumJsonSerializerContext.Default.JsonNativeObjectApplyFunctionReturnValue)
        };

    }

    private ProcessResponse OnJsSetNativeObjectProperty(ProcessRequest request)
    {
        var data = request.Data;

        if (data is null || string.IsNullOrEmpty(data))
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] SetNativeObjectProperty requires property data." };
        }
        var json = JsonSerializer.Deserialize(data, WinFormiumJsonSerializerContext.Default.JsonNativeObjectSetProperty);

        if (json == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] SetNativeObjectProperty requires property data." };
        }

        var obj = NativeProxyObject.Get(json.ObjectId);

        if (obj == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = $"[WinFormium] The NativeObject is not registered." };
        }



        NativeProxyObjectSetPropertyEventArgs args = new(json.ObjectId, json.Property, json.Data);

        var retval = obj.OnSettingProperty(args);

        return new ProcessResponse
        {
            Success = retval
        };
    }

    private ProcessResponse OnJsGetNativeObjectProperty(ProcessRequest request)
    {
        var data = request.Data;


        if (string.IsNullOrEmpty(data))
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] GetNativeObjectProperty requires property data." };
        }

        var json = JsonSerializer.Deserialize(data, WinFormiumJsonSerializerContext.Default.JsonNativeObjectGetProperty);

        if (json == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] GetNativeObjectProperty requires property data." };
        }

        var obj = NativeProxyObject.Get(json.ObjectId);

        if (obj == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = $"[WinFormium] The NativeObject is not registered." };
        }

        NativeProxyObjectGetPropertyEventArgs args = new(obj.ObjectId, json.Property);

        obj.OnGettingProperty(args);

        return new ProcessResponse
        {
            Success = true,
            Data = JsonSerializer.Serialize(new JsonNativeObjectGetPropertyResult(args.ObjectId, args.PropertyName, args.ResultType, args.ReturnValue)
       , WinFormiumJsonSerializerContext.Default.JsonNativeObjectGetPropertyResult)
        };

    }

    private ProcessResponse OnJsGetNativeObject(ProcessRequest args)
    {
        var propName = args.Data;

        if (string.IsNullOrEmpty(propName))
        {
            return new ProcessResponse { Success = false, ExceptionMessage = "[WinFormium] Property name is null." };
        }

        var obj = NativeObjectRepository.Get(propName);

        if (obj == null)
        {
            return new ProcessResponse { Success = false, ExceptionMessage = $"[WinFormium] \"{propName}\" is not registered." };
        }

        return new ProcessResponse
        {
            Success = true,
            Data = obj.ToJson(),
        };

    }




}


internal record JsonNativeObjectGetProperty(int ObjectId, string Property);
internal record JsonNativeObjectGetPropertyResult(int ObjectId, string Property, string? Type, string? Data);
internal record JsonNativeObjectSetProperty(int ObjectId, string Property, string? Data);
internal record JsonNativeObjectApplyFunction(int ObjectId, string Property, string? Arguments);
internal record JsonNativeObjectApplyFunctionReturnValue(int ObjectId, string Propery, bool IsAsynconous, string? JsonValue);