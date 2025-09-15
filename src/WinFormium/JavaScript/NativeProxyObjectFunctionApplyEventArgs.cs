// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Provides data for the event that occurs when a native proxy object's function is applied from JavaScript.
/// </summary>
public class NativeProxyObjectFunctionApplyEventArgs : EventArgs
{

    /// <summary>
    /// Stores the JSON string representing the arguments passed from JavaScript.
    /// </summary>
    private readonly string? _data;

    /// <summary>
    /// Gets the unique identifier of the native proxy object.
    /// </summary>
    internal int ObjectId { get; }

    /// <summary>
    /// Gets or sets the JavaScript promise associated with the function call, if asynchronous.
    /// </summary>
    internal JavaScriptPromiseContext? Promise { get; set; }

    /// <summary>
    /// Gets the name of the property (function) being invoked on the proxy object.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the arguments passed to the function as a JSON string.
    /// The string is unescaped and stripped of surrounding quotes if present.
    /// </summary>
    public string? ArgumentsAsJson
    {
        get
        {
            if (_data is null) return _data;
            var data = _data;

            if ((data.StartsWith("'") && data.EndsWith("'")) || data.StartsWith("\"") && data.EndsWith("\""))
            {
                data = data[1..^1];

                data = Regex.Unescape(data);
            }

            return data;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the function call is asynchronous.
    /// </summary>
    internal bool Asynconous { get; set; }

    /// <summary>
    /// Gets or sets the return value as a JSON string.
    /// </summary>
    internal string? ReturnValue { get; set; }

    /// <summary>
    /// Sets the return value for the function call and marks the call as synchronous.
    /// </summary>
    /// <param name="json">The return value as a JSON string.</param>
    public void ReturnJson(string? json)
    {
        Asynconous = false;
        ReturnValue = json;
        Promise = null;
    }

    /// <summary>
    /// Marks the function call as asynchronous and returns a <see cref="JavaScriptPromiseContext"/> to be resolved or rejected later.
    /// </summary>
    /// <returns>A <see cref="JavaScriptPromiseContext"/> instance associated with this call.</returns>
    public JavaScriptPromiseContext ReturnPromise()
    {
        Asynconous = true;

        var promise = new JavaScriptPromiseContext(ObjectId);

        Promise = promise;

        return promise;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NativeProxyObjectFunctionApplyEventArgs"/> class.
    /// </summary>
    /// <param name="objectId">The unique identifier of the proxy object.</param>
    /// <param name="propName">The name of the property (function) being invoked.</param>
    /// <param name="json">The arguments as a JSON string.</param>
    internal NativeProxyObjectFunctionApplyEventArgs(int objectId, string propName, string? json)
    {
        ObjectId = objectId;
        PropertyName = propName;
        _data = json;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the function call has been canceled.
    /// </summary>
    internal bool Canceled { get; set; } = false;

    /// <summary>
    /// Cancels the function call, preventing further processing.
    /// </summary>
    public void Cancel()
    {
        Canceled = true;
    }

}
