// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Nodes;

namespace WinFormium.JavaScript;

internal static class JavaScriptEngineStringExtensions
{
    public static bool CheckIfJsonObjectString(this string str)
    {
        return str.StartsWith("{") && str.EndsWith("}");
    }

    public static bool CheckIfJsonArrayString(this string str)
    {
        return str.StartsWith("[") && str.EndsWith("]");
    }

    public static bool CheckIfJsonString(this string str)
    {
        return CheckIfJsonObjectString(str) || CheckIfJsonArrayString(str);
    }

    public static bool CheckIfQuoteString(this string str)
    {
        return str.StartsWith("\"") && str.EndsWith("\"");
    }

    public static CefV8Value JsonStringToCefV8Value(this string? data, CefV8Context context)
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

                var jsonParser = context.GetGlobal()?.GetValue("JSON")?.GetValue("parse");

                if (jsonParser is null)
                {

                    value = CefV8Value.CreateUndefined();

                    return value;
                }

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