// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Represents a message sent to or from the render process.
/// </summary>
class RenderProcessMessage
{
    /// <summary>
    /// Gets or sets the name of the message.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the message.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderProcessMessage"/> class.
    /// </summary>
    public RenderProcessMessage()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Serializes this message to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of this message.</returns>
    internal string ToJson()
    {
        return JsonSerializer.Serialize(this, WinFormiumJsonSerializerContext.Default.RenderProcessMessage);
    }

    /// <summary>
    /// Deserializes a JSON string to a <see cref="RenderProcessMessage"/> instance.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>A <see cref="RenderProcessMessage"/> instance.</returns>
    internal static RenderProcessMessage FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, WinFormiumJsonSerializerContext.Default.RenderProcessMessage)!;
    }
}
