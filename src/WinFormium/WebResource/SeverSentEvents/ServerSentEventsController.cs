// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;

/// <summary>
/// Controls the lifecycle and events for Server-Sent Events (SSE) clients.
/// </summary>
public sealed class ServerSentEventsController
{
    /// <summary>
    /// Occurs when a new SSE client has connected.
    /// </summary>
    public event EventHandler<SSEClientConnectedEventArgs>? ClientConnected;

    /// <summary>
    /// Invokes the <see cref="ClientConnected"/> event asynchronously.
    /// </summary>
    /// <param name="e">The event arguments containing client connection information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal Task OnClientConnectedAsync(SSEClientConnectedEventArgs e)
    {
        return Task.Run(() =>
        {
            ClientConnected?.Invoke(this, e);
        });
    }


}
