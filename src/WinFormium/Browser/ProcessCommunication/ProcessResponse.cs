// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.ProcessCommunication;

/// <summary>
/// Represents the response of a process communication operation.
/// </summary>
internal sealed class ProcessResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public required bool Success { get; set; }

    /// <summary>
    /// Gets or sets the exception message if the operation failed; otherwise, <c>null</c>.
    /// </summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>
    /// Gets or sets the data returned by the operation, if any.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Serializes the current <see cref="ProcessResponse"/> instance to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of the current instance.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, WinFormiumJsonSerializerContext.Default.ProcessResponse);
    }

    /// <summary>
    /// Deserializes a JSON string to a <see cref="ProcessResponse"/> instance.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>
    /// A <see cref="ProcessResponse"/> instance if deserialization is successful; otherwise, <c>null</c>.
    /// </returns>
    public static ProcessResponse? FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, WinFormiumJsonSerializerContext.Default.ProcessResponse);
    }
}
