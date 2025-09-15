// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Provides data for the event that occurs when a property is set on a native proxy object.
/// </summary>
public class NativeProxyObjectSetPropertyEventArgs : EventArgs
{
    /// <summary>
    /// The raw JSON data representing the value being set.
    /// </summary>
    private readonly string? _data;

    /// <summary>
    /// Gets the identifier of the native proxy object whose property is being set.
    /// </summary>
    internal int ObjectId { get; }

    /// <summary>
    /// Gets the name of the property being set.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the value being set as a JSON string, with surrounding quotes removed and unescaped if necessary.
    /// </summary>
    public string? ValueAsJson
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
    /// Initializes a new instance of the <see cref="NativeProxyObjectSetPropertyEventArgs"/> class.
    /// </summary>
    /// <param name="objectId">The identifier of the native proxy object.</param>
    /// <param name="propName">The name of the property being set.</param>
    /// <param name="json">The JSON string representing the value being set.</param>
    internal NativeProxyObjectSetPropertyEventArgs(int objectId, string propName, string? json)
    {
        ObjectId = objectId;
        PropertyName = propName;
        _data = json;
    }

}
