// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Nodes;

using static WinFormium.JavaScript.JavaScriptEngine;

namespace WinFormium.JavaScript;
class InitializationScriptV8Handler : CefV8Handler
{

    public static InitializationScriptV8Handler Create(JavaScriptEngine engine)
    {
        return new InitializationScriptV8Handler(engine);
    }

    public CefBrowser Browser => Engine.Browser;
    public JavaScriptEngine Engine { get; }

    public InitializationScriptV8Handler(JavaScriptEngine engine)
    {
        Engine = engine;
    }
    bool CheckIfJsonObjectString(string str)
    {
        return str.StartsWith("{") && str.EndsWith("}");
    }

    bool CheckIfJsonArrayString(string str)
    {
        return str.StartsWith("[") && str.EndsWith("]");
    }

    bool CheckIfJsonString(string str)
    {
        return CheckIfJsonObjectString(str) || CheckIfJsonArrayString(str);
    }

    bool CheckIfQuoteString(string str)
    {
        return str.StartsWith("\"") && str.EndsWith("\"");
    }

    protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {
        switch (name)
        {
            case "GetWinFormiumVersion":
                {
                    returnValue = CefV8Value.CreateString(typeof(WinFormiumApp).Assembly.GetName().Version?.ToString() ?? "Unknown");
                    exception = null!;
                }
                return true;

            case "GetChromiumVersion":
                {
                    returnValue = CefV8Value.CreateString(CefRuntime.ChromeVersion);
                    exception = null!;
                }
                return true;
            case "PostMessage":
                return HandlePostMessage(arguments, out returnValue, out exception);

            case "GetNativeObject":
                return HandleGetNativeObject(arguments, out returnValue, out exception);
            case "GetNativeObjectProperty":
                return HandleGetNativeObjectProperty(arguments, out returnValue, out exception);
            case "SetNativeObjectProperty":
                return HandleSetNativeObjectProperty(arguments, out returnValue, out exception);
            case "CallNativeObjectMethod":
                return HandleCallNativeObjectMethod(arguments, out returnValue, out exception);
            case "GetCulture":
                return HandleGetCulture(arguments, out returnValue, out exception);
        }

        returnValue = CefV8Value.CreateUndefined();
        exception = string.Empty;
        return false;
    }

    private bool HandleGetCulture(CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {

        exception = null!;
        returnValue = CefV8Value.CreateString($"{Application.CurrentCulture.Name}".ToLower());
        return true;
    }

    private bool HandleCallNativeObjectMethod(CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {
        if (arguments.Length != 3)
        {
            exception = "[WinFormium] CallNativeObjectMethod argument out of range.";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var context = CefV8Context.GetCurrentContext();

        var frame = context.GetFrame();

        if (frame is null)
        {
            exception = "[WinFormium] CallNativeObjectMethod failed due to target frame is null";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var objectId = arguments[0].GetIntValue();
        var propertyName = arguments[1].GetStringValue();
        var callArgs = arguments[2].GetStringValue();

        if (objectId == 0 || string.IsNullOrEmpty(propertyName))
        {
            exception = "[WinFormium] CallNativeObjectMethod failed due to invalid objectId or propertyName";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var response = Engine.Request(new ProcessRequest
        {
            BrowserId = frame.Browser.Identifier,
            FrameId = frame.Identifier,
            Name = "CallNativeObjectMethod",
            Data = JsonSerializer.Serialize(new JsonNativeObjectApplyFunction(objectId, propertyName, callArgs), WinFormiumJsonSerializerContext.Default.JsonNativeObjectApplyFunction)
        });

        exception = null!;

        if (response.Success && response.Data is not null)
        {
            var json = JsonSerializer.Deserialize(response.Data, WinFormiumJsonSerializerContext.Default.JsonNativeObjectApplyFunctionReturnValue);

            if (json is null)
            {
                exception = "[WinFormium] CallNativeObjectMethod failed due to deserialize result.";
                returnValue = CefV8Value.CreateUndefined();
                return true;
            }

            if (!json.IsAsynconous)
            {
                var jsonParser = context.GetGlobal()?.GetValue("JSON")?.GetValue("parse");

                if (jsonParser is null)
                {
                    exception = "[WinFormium] JSON.parse is not available.";
                    returnValue = CefV8Value.CreateUndefined();
                    return true;
                }

                returnValue = JsonStringToCefV8Value(json.JsonValue, context, jsonParser);

                return true;
            }
            else
            {


                var callId = json.JsonValue;

                if(callId is null)
                {
                    exception = "[WinFormium] CallNativeObjectMethod failed due to invalid callId.";
                    returnValue = CefV8Value.CreateUndefined();
                    return true;
                }

                var promiseData  = new PromiseData { 
                    CallId = callId,
                    V8Context = context,
                    Promise = CefV8Value.CreatePromise(),
                };


                if(Engine.FunctionApplyPendingPromises.TryAdd(promiseData.CallId, promiseData))
                {
                    returnValue = promiseData.Promise;
                    return true;
                }
                else
                {
                    exception = "[WinFormium] Failed to create promise object.";

                }
            }
        }
        else if (response.ExceptionMessage is not null)
        {
            exception = response.ExceptionMessage;
        }
        else
        {
            exception = "[WinFormium] CallNativeObjectMethod failed due to unknown error.";
        }


        returnValue = CefV8Value.CreateUndefined();

        return true;
    }


    private bool HandleGetNativeObjectProperty(CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {
        if (arguments.Length != 1 || arguments[0].GetStringValue() == null)
        {
            exception = "[WinFormium] GetNativeObjectProperty requires a single json string argument";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var context = CefV8Context.GetCurrentContext();

        var frame = context.GetFrame();

        if (frame is null)
        {
            exception = "[WinFormium] GetNativeObjectProperty failed due to target frame is null";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }



        var response = Engine.Request(new ProcessRequest
        {
            BrowserId = frame.Browser.Identifier,
            FrameId = frame.Identifier,
            Name = "GetNativeObjectProperty",
            Data = arguments[0].GetStringValue()
        });

        if (response.Success && response.Data is not null)
        {
            var json = JsonSerializer.Deserialize(response.Data, WinFormiumJsonSerializerContext.Default.JsonNativeObjectGetPropertyResult);

            if (json is null)
            {
                exception = "[WinFormium] GetNativeObjectProperty failed due to deserialize result.";
                returnValue = CefV8Value.CreateUndefined();
                return true;
            }

            var data = json.Data;
            var valueType = json.Type;

            if (valueType is null)
            {
                exception = $"[WinFormium] Element '{json.Property}' does not exist in this NativeObject.";

                returnValue = CefV8Value.CreateUndefined();
                return true;
            }

            var jsonParser = context.GetGlobal()?.GetValue("JSON")?.GetValue("parse");

            if (jsonParser is null)
            {
                exception = "[WinFormium] JSON.parse is not available.";
                returnValue = CefV8Value.CreateUndefined();
                return true;
            }

            var result = CefV8Value.CreateObject();

            result.SetValue("type", CefV8Value.CreateString(valueType));

            exception = null!;

            if (valueType == "value")
            {
                var value = JsonStringToCefV8Value(data, context, jsonParser);

                result.SetValue("data", value);
            }
            else if(valueType == "function")
            {
                result.SetValue("data", CefV8Value.CreateUndefined());
            }
            else
            {
                result.SetValue("data", data == null ? CefV8Value.CreateNull() : CefV8Value.CreateString(data));
            }

            returnValue = result;

            return true;
        }
        else if (response.ExceptionMessage is not null)
        {
            exception = response.ExceptionMessage;
        }
        else
        {
            exception = "[WinFormium] GetNativeObjectProperty failed due to unknown error.";
        }


        returnValue = CefV8Value.CreateUndefined();

        return true;
    }


    private bool HandleSetNativeObjectProperty(CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {

        if (arguments.Length != 3)
        {
            exception = "[WinFormium] SetNativeObjectProperty argument out of range.";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var context = CefV8Context.GetCurrentContext();

        var frame = context.GetFrame();

        if (frame is null)
        {
            exception = "[WinFormium] SetNativeObjectProperty failed due to target frame is null";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var objectId = arguments[0].GetIntValue();
        var propertyName = arguments[1].GetStringValue();
        var propertyValue = arguments[2].GetStringValue();

        if (objectId == 0 || string.IsNullOrEmpty(propertyName))
        {
            exception = "[WinFormium] SetNativeObjectProperty failed due to invalid objectId or propertyName";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var response = Engine.Request(new ProcessRequest
        {
            BrowserId = frame.Browser.Identifier,
            FrameId = frame.Identifier,
            Name = "SetNativeObjectProperty",
            Data = JsonSerializer.Serialize(new JsonNativeObjectSetProperty(objectId, propertyName, propertyValue), WinFormiumJsonSerializerContext.Default.JsonNativeObjectSetProperty)
        });

        exception = null!;

        returnValue = CefV8Value.CreateUndefined();

        if (response.Success)
        {
            return true;
        }
        else if (response.ExceptionMessage is not null)
        {
            exception = response.ExceptionMessage;
        }
        else
        {
            exception = $"[WinFormium] Element '{propertyName}' does not exist in this NativeObject or it can not be set.";
        }

        return true;
    }


    private bool HandleGetNativeObject(CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {
        if (arguments.Length != 1 || arguments[0].GetStringValue() == null)
        {
            exception = "[WinFormium] GetNativeObject requires a single string argument";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        using var context = CefV8Context.GetCurrentContext();

        var frame = context.GetFrame();

        if (frame is null)
        {
            exception = "[WinFormium] GetNativeObject failed due to target frame is null";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        var response = Engine.Request(new ProcessRequest
        {
            BrowserId = frame.Browser.Identifier,
            FrameId = frame.Identifier,
            Name = "GetNativeObject",
            Data = arguments[0].GetStringValue()
        });

        if (response.Success && response.Data is not null)
        {

            var objectId = Convert.ToInt32(response.Data);
            var result = CefV8Value.CreateObject();
            result.SetValue("__objectId", CefV8Value.CreateInt(objectId));

            returnValue = result;
            exception = null!;

            return true;
        }
        else if (response.ExceptionMessage is not null)
        {
            exception = response.ExceptionMessage;
        }
        else
        {
            exception = "[WinFormium] Failed to get the NativeObject.";
        }


        returnValue = CefV8Value.CreateUndefined();

        return true;
    }

    private bool HandlePostMessage(CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
    {
        if (arguments.Length != 1 || arguments[0].GetStringValue() == null)
        {
            exception = "[WinFormium] PostMessage requires a single string argument";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        using var context = CefV8Context.GetCurrentContext();
        var browser = context.GetBrowser();
        var frame = CefV8Context.GetCurrentContext().GetFrame();

        if (frame is null)
        {
            exception = "[WinFormium] PostMessage failed due to target frame is null";
            returnValue = CefV8Value.CreateUndefined();
            return true;
        }

        frame.SendProcessMessage(CefProcessId.Browser, new JsPostWebMessageToBrowserMessage
        {
            BrowserId = browser.Identifier,
            FrameId = frame.Identifier,
            Source = frame.Url,
            Data = arguments[0].GetStringValue()
        }.ToCefProcessMessage());


        returnValue = CefV8Value.CreateUndefined();
        exception = null!;

        return true;
    }

    private CefV8Value JsonStringToCefV8Value(string? data, CefV8Context context, CefV8Value jsonParser)
    {
        if (data is null)
        {
            return CefV8Value.CreateNull();
        }
        else
        {
            CefV8Value? value = null;
            try
            {

                if (!double.TryParse(data, out _) &&
                   !bool.TryParse(data, out _) &&
                   !CheckIfQuoteString(data) &&
                   !CheckIfJsonString(data.Trim('"')))
                {
                    data = $"\"{data}\"";
                }

            JSON_STRING_CONVERT_PROCEDURE:

                var node = JsonNode.Parse(data);

                var valueKind = node?.GetValueKind() ?? JsonValueKind.Undefined;


                switch (valueKind)
                {
                    case JsonValueKind.Undefined:
                        value = CefV8Value.CreateUndefined();
                        break;
                    case JsonValueKind.Object:
                        value = jsonParser.ExecuteFunctionWithContext(context, null!, [CefV8Value.CreateString(data)]);
                        break;
                    case JsonValueKind.Array:
                        value = jsonParser.ExecuteFunctionWithContext(context, null!, [CefV8Value.CreateString(data)]);
                        break;
                    case JsonValueKind.String:
                        {
                            if (CheckIfQuoteString(data))
                            {

                                var tmpdata = data.Trim('"');

                                tmpdata = Regex.Unescape(tmpdata);

                                if (DateTime.TryParse(tmpdata, out var datetime) && CefBaseTime.FromUtcExploded(new CefTime(datetime), out var basetime))
                                {
                                    value = CefV8Value.CreateDate(basetime);
                                }
                                else
                                {
                                    if (CheckIfJsonString(tmpdata))
                                    {
                                        value = CefV8Value.CreateString(tmpdata);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var tmpkind = JsonNode.Parse(tmpdata)?.GetValueKind();

                                            data = tmpdata;

                                            goto JSON_STRING_CONVERT_PROCEDURE;
                                        }
                                        catch
                                        {
                                            value = CefV8Value.CreateString(tmpdata);
                                        }
                                    }


                                }

                            }

                        }

                        break;
                    case JsonValueKind.Number:
                        {
                            if (double.TryParse(data, out var doubleValue))
                            {
                                if (int.TryParse(data, out var intValue) && doubleValue == intValue)
                                {
                                    value = CefV8Value.CreateInt(intValue);
                                }
                                else
                                {
                                    value = CefV8Value.CreateDouble(doubleValue);
                                }
                            }
                            else
                            {
                                value = CefV8Value.CreateDouble(0);
                            }
                        }
                        break;
                    case JsonValueKind.True:
                        value = CefV8Value.CreateBool(true);
                        break;
                    case JsonValueKind.False:
                        value = CefV8Value.CreateBool(false);
                        break;
                    case JsonValueKind.Null:
                        value = CefV8Value.CreateNull();
                        break;
                }
            }
            catch (JsonException)
            {
                value = CefV8Value.CreateString(data);
            }


            return value ?? CefV8Value.CreateUndefined();
        }

    }
}
