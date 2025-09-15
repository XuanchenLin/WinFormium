// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.IO.Pipes;



namespace WinFormium.Browser.ProcessCommunication.NamedPipe;
/// <summary>
/// Represents a client for communicating with a named pipe server.
/// </summary>
class PipeClient
{
    /// <summary>
    /// The name of the pipe to connect to.
    /// </summary>
    private readonly string _pipeName;

    /// <summary>
    /// Initializes a new instance of the <see cref="PipeClient"/> class with the specified pipe name.
    /// </summary>
    /// <param name="pipeName">The name of the named pipe.</param>
    public PipeClient(string pipeName)
    {
        _pipeName = pipeName;
    }

    /// <summary>
    /// Sends a message to the named pipe server and receives a response synchronously.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>The response from the server, or <c>null</c> if an error occurs.</returns>
    public string? SendMessage(string message)
    {
        var client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);


        try
        {
            client.Connect(5000);
            client.ReadMode = PipeTransmissionMode.Byte;
            var stream = new PipeStream(client);

            stream.Write(message);
            client.Flush();
            client.WaitForPipeDrain();
            return stream.Read();
        }
        catch
        {
            return null;
        }
        finally
        {
            client.Close();
            client.Dispose();
        }
    }

    /// <summary>
    /// Sends a message to the named pipe server and receives a response asynchronously.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the server, or <c>null</c> if an error occurs.</returns>
    public Task<string?> SendMessageAsync(string message)
    {
        return Task.Run(() => SendMessage(message));
    }
}
