// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.ProcessCommunication;

/// <summary>
/// Represents a request for process communication within the browser context.
/// </summary>
internal sealed class ProcessRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the browser instance.
    /// </summary>
    public required int BrowserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the frame within the browser.
    /// </summary>
    public required long FrameId { get; set; }

    /// <summary>
    /// Gets or sets the name of the process request.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the optional data associated with the request.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Serializes the current <see cref="ProcessRequest"/> instance to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the current instance.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, WinFormiumJsonSerializerContext.Default.ProcessRequest);
    }

    /// <summary>
    /// Deserializes a JSON string to a <see cref="ProcessRequest"/> instance.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>A <see cref="ProcessRequest"/> instance if deserialization is successful; otherwise, <c>null</c>.</returns>
    public static ProcessRequest? FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, WinFormiumJsonSerializerContext.Default.ProcessRequest);
    }

}
