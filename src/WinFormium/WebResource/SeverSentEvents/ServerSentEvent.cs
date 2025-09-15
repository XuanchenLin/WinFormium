// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;

/// <summary>
/// Represents a single Server-Sent Event (SSE) message.
/// </summary>
public sealed class ServerSentEvent
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the event type.
    /// </summary>
    public string? Event { get; set; }

    /// <summary>
    /// Gets or sets the data payload of the event.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Gets or sets the reconnection time in milliseconds.
    /// </summary>
    public uint? Retry { get; set; }
}
