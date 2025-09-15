// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Provides data for the event that occurs when a property is accessed on a <see cref="NativeProxyObject"/>.
/// </summary>
public class NativeProxyObjectGetPropertyEventArgs : EventArgs
{
    /// <summary>
    /// Gets the unique identifier of the native proxy object whose property is being accessed.
    /// </summary>
    internal int ObjectId { get; }

    /// <summary>
    /// Gets the name of the property being accessed.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NativeProxyObjectGetPropertyEventArgs"/> class.
    /// </summary>
    /// <param name="objectId">The unique identifier of the native proxy object.</param>
    /// <param name="propName">The name of the property being accessed.</param>
    internal NativeProxyObjectGetPropertyEventArgs(int objectId, string propName)
    {
        ObjectId = objectId;
        PropertyName = propName;
    }

    /// <summary>
    /// Gets or sets the result type to be returned to the JavaScript side.
    /// </summary>
    internal string? ResultType { get; set; }

    /// <summary>
    /// Gets or sets the return value as a JSON string.
    /// </summary>
    internal string? ReturnValue { get; set; }

    /// <summary>
    /// Returns a JSON value as the result of the property access.
    /// </summary>
    /// <param name="json">The JSON string representing the value to return.</param>
    public void ReturnJson(string? json)
    {
        ResultType = "value";
        ReturnValue = json;
    }

    /// <summary>
    /// Returns a native proxy object as the result of the property access.
    /// </summary>
    /// <param name="childObject">The <see cref="NativeProxyObject"/> to return.</param>
    public void AsNativeObject(NativeProxyObject childObject)
    {
        ResultType = "object";
        ReturnValue = childObject.ToJson();
    }

    /// <summary>
    /// Returns a function as the result of the property access.
    /// </summary>
    public void AsFunction()
    {
        ResultType = "function";
    }

}
