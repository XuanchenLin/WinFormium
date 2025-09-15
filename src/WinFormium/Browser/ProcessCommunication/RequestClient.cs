// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using WinFormium.Browser.ProcessCommunication.NamedPipe;

namespace WinFormium.Browser.ProcessCommunication;
/// <summary>
/// Provides client functionality for sending process requests and receiving responses via named pipes.
/// </summary>
class RequestClient(string pipeName) : PipeClient(pipeName)
{
    /// <summary>
    /// Sends a <see cref="ProcessRequest"/> to the server and receives a <see cref="ProcessResponse"/> synchronously.
    /// </summary>
    /// <param name="request">The process request to send.</param>
    /// <returns>
    /// The <see cref="ProcessResponse"/> received from the server. If the response is empty or invalid, a failed <see cref="ProcessResponse"/> is returned.
    /// </returns>
    public ProcessResponse Request(ProcessRequest request)
    {
        var message = request.ToJson();
        var response = SendMessage(message);
        if (response == null)
        {
            return new ProcessResponse
            {
                Success = false,
                ExceptionMessage = "Empty response.",
            };
        }

        return ProcessResponse.FromJson(response) ?? new ProcessResponse
        {
            Success = false,
            ExceptionMessage = "Empty response.",
        };
    }

    /// <summary>
    /// Sends a <see cref="ProcessRequest"/> to the server and receives a <see cref="ProcessResponse"/> asynchronously.
    /// </summary>
    /// <param name="request">The process request to send.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="ProcessResponse"/> received from the server.
    /// </returns>
    public Task<ProcessResponse> RequestAsync(ProcessRequest request)
    {
        return Task.Run(() => Request(request));
    }
}
