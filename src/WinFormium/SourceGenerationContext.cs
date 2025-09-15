// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(ProcessRequest))]
[JsonSerializable(typeof(ProcessResponse))]
[JsonSerializable(typeof(RenderProcessMessage))]
[JsonSerializable(typeof(JavaScriptEvaluationException))]
[JsonSerializable(typeof(JsonNativeObjectGetProperty))]
[JsonSerializable(typeof(JsonNativeObjectGetPropertyResult))]
[JsonSerializable(typeof(JsonNativeObjectSetProperty))]
[JsonSerializable(typeof(JsonNativeObjectApplyFunction))]
[JsonSerializable(typeof(JsonNativeObjectApplyFunctionReturnValue))]
[JsonSerializable(typeof(JsonNotifyWindowStateChange))]
[JsonSerializable(typeof(JsonNotifyWindowResize))]
[JsonSerializable(typeof(JsonNotifyWindowMove))]
[JsonSerializable(typeof(JsonNotifyWindowActivated))]
[JsonSerializable(typeof(String))]
internal partial class WinFormiumJsonSerializerContext : JsonSerializerContext
{

}



