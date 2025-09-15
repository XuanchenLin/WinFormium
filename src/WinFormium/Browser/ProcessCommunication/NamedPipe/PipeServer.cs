// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.IO.Pipes;




namespace WinFormium.Browser.ProcessCommunication.NamedPipe;

internal delegate string PipeMessageReceivedDelegate(string message, bool success, Exception? exception);

/// <summary>
/// Represents a named pipe server for inter-process communication.
/// </summary>
class PipeServer : IDisposable
{
    /// <summary>
    /// The maximum number of allowed consecutive errors before the server stops.
    /// </summary>
    const int MAX_ERRORS_ALLOWED = 5;

    /// <summary>
    /// The cancellation token source used to cancel the server operations.
    /// </summary>
    private CancellationTokenSource? _cancellationTokenSource = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PipeServer"/> class with the specified pipe name.
    /// </summary>
    /// <param name="pipeName">The name of the pipe to create.</param>
    public PipeServer(string pipeName)
    {
        Task.Run(async () =>
        {
            var errorCount = 0;

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    using var server = new NamedPipeServerStream(pipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                    await server.WaitForConnectionAsync(_cancellationTokenSource.Token);

                    AcceptClientConnection(server);

                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    errorCount++;

                    if (errorCount > MAX_ERRORS_ALLOWED)
                    {
                        break;
                    }
                }
            }

            var cancellationTokenSource = _cancellationTokenSource;
            _cancellationTokenSource = null;
            cancellationTokenSource?.Dispose();
        });
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }

    /// <summary>
    /// Accepts a client connection and processes the received message.
    /// </summary>
    /// <param name="server">The <see cref="NamedPipeServerStream"/> representing the client connection.</param>
    private void AcceptClientConnection(NamedPipeServerStream server)
    {
        using var stream = new PipeStream(server);

        string response = string.Empty;

        try
        {
            var message = stream.Read();

            response = OnMessageReceived(message);

        }
        catch (Exception ex)
        {
            response = OnMessageReceived(ex.Message, false, ex);
        }

        try
        {
            stream.Write(response);

            server.Flush();

            server.WaitForPipeDrain();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[PIPE SERVER][ERROR]:{ex}");
        }
        finally
        {
            server.Disconnect();
            server.Dispose();
        }
    }

    /// <summary>
    /// Raises the <see cref="MessageReceived"/> event when a message is received from the client.
    /// </summary>
    /// <param name="message">The received message.</param>
    /// <param name="success">Indicates whether the message was received successfully.</param>
    /// <param name="exception">The exception that occurred, if any.</param>
    /// <returns>The response to send back to the client.</returns>
    protected virtual string OnMessageReceived(string message, bool success = true, Exception? exception = null)
    {
        return MessageReceived?.Invoke(message, success, exception) ?? string.Empty;
    }

    /// <summary>
    /// Occurs when a message is received from a client.
    /// </summary>
    public event PipeMessageReceivedDelegate? MessageReceived;

}
