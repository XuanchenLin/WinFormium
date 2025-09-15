// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using WinFormium.Browser.ProcessCommunication.NamedPipe;

namespace WinFormium.Browser.ProcessCommunication;

/// <summary>
/// Represents a server for handling process communication requests over named pipes.
/// </summary>
class RequestServer(string pipeName) : PipeServer(pipeName)
{

    /// <summary>
    /// Gets or sets the callback that is invoked when a process request is received.
    /// </summary>
    internal Func<ProcessRequest, ProcessResponse?>? OnRequestReceived { get; set; }

    /// <inheritdoc/>
    protected override string OnMessageReceived(string message, bool success = true, Exception? exception = null)
    {
        if (success)
        {
            var request = ProcessRequest.FromJson(message);

            if (request == null)
            {
                return new ProcessResponse
                {
                    Success = false,
                    ExceptionMessage = "Empty request.",
                }.ToJson();
            }

            var response = OnRequestReceived?.Invoke(request);

            if (response == null)
            {
                return new ProcessResponse
                {
                    Success = false,
                    ExceptionMessage = "Empty response.",
                }.ToJson();
            }

            return response.ToJson();
        }
        else
        {
            return new ProcessResponse
            {
                Success = false,
                ExceptionMessage = message,
            }.ToJson();
        }
    }
}
