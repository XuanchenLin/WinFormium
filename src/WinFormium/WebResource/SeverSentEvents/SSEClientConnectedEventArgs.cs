// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;

/// <summary>
/// Provides data for the event that is raised when a client connects to a Server-Sent Events (SSE) endpoint.
/// </summary>
public sealed class SSEClientConnectedEventArgs : EventArgs
{
    private readonly SeverSentEventsResourceHandler _handler;

    /// <summary>
    /// Gets a value indicating whether the client connection is still alive.
    /// </summary>
    public bool IsAlive => _handler.IsConnected;

    /// <summary>
    /// Initializes a new instance of the <see cref="SSEClientConnectedEventArgs"/> class.
    /// </summary>
    /// <param name="request">The incoming CEF request associated with the client connection.</param>
    /// <param name="handler">The resource handler managing the SSE connection.</param>
    internal SSEClientConnectedEventArgs(CefRequest request, SeverSentEventsResourceHandler handler)
    {
        Request = request;
        _handler = handler;
    }

    /// <summary>
    /// Gets the CEF request associated with the client connection.
    /// </summary>
    public CefRequest Request { get; }

    /// <summary>
    /// Disconnects the client from the SSE endpoint.
    /// </summary>
    public void Disconnect()
    {
        _handler.SetDisconnect();
    }

    /// <summary>
    /// Sends a server-sent event to the connected client.
    /// </summary>
    /// <param name="sentEvent">The server-sent event to send.</param>
    public void SendEvent(ServerSentEvent sentEvent)
    {
        _handler.AddEvent(sentEvent);
    }
}
