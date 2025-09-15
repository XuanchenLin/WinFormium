// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;
/// <summary>
/// Handles Server-Sent Events (SSE) resources for CEF requests.
/// </summary>
class SeverSentEventsResourceHandler : CefResourceHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeverSentEventsResourceHandler"/> class.
    /// </summary>
    /// <param name="controller">The controller managing SSE clients and events.</param>
    public SeverSentEventsResourceHandler(ServerSentEventsController controller)
    {
        _controller = controller;
    }

    /// <summary>
    /// Gets a value indicating whether the client is currently connected.
    /// </summary>
    internal bool IsConnected => _isConnected;

    /// <summary>
    /// Adds a server-sent event to the event queue and notifies the handler if connected.
    /// </summary>
    /// <param name="sentEvent">The server-sent event to add.</param>
    internal void AddEvent(ServerSentEvent sentEvent)
    {
        if (!IsConnected) return;

        _events.Add(sentEvent);

        if (_taskCompletionSource is null || _taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource = new TaskCompletionSource();
        }
        else
        {
            _taskCompletionSource.SetResult();

        }

    }

    /// <summary>
    /// Marks the client as disconnected and signals any waiting tasks.
    /// </summary>
    internal void SetDisconnect()
    {
        _isConnected = false;

        _taskCompletionSource?.SetResult();
        _taskCompletionSource = null;


    }

    /// <summary>
    /// Cancels the resource handler, disconnecting the client and signaling any waiting tasks.
    /// </summary>
    protected override void Cancel()
    {
        _isConnected = false;
        _taskCompletionSource?.SetResult();
        _taskCompletionSource = null;

    }

    /// <summary>
    /// Sets the response headers for the SSE stream.
    /// </summary>
    /// <param name="response">The response object to set headers on.</param>
    /// <param name="responseLength">The length of the response. Set to 0 for streaming.</param>
    /// <param name="redirectUrl">The redirect URL, if any. Set to empty for SSE.</param>
    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    {
        response.SetHeaderByName("X-Accel-Buffering", "no", true);
        response.SetHeaderByName("Content-Type", "text/event-stream,utf-8", true);
        response.SetHeaderByName("Cache-Control", "no-cache", true);
        response.SetHeaderByName("Access-Control-Allow-Origin", "*", true);
        response.MimeType = "text/event-stream";
        redirectUrl = string.Empty;
        responseLength = 0;
    }

    /// <summary>
    /// Opens the SSE resource stream and notifies the controller of a new client connection.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="handleRequest">Indicates whether the request should be handled immediately.</param>
    /// <param name="callback">The callback to continue or cancel the request.</param>
    /// <returns>True if the request is handled.</returns>
    protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
    {


        _taskCompletionSource = new TaskCompletionSource();

        handleRequest = true;

        _isConnected = true;

        OnClientConnected(request, callback);

        return true;
    }

    /// <summary>
    /// Reads SSE data and writes it to the output buffer asynchronously.
    /// </summary>
    /// <param name="dataOut">Pointer to the output buffer.</param>
    /// <param name="bytesToRead">The number of bytes to read.</param>
    /// <param name="bytesRead">The number of bytes actually read.</param>
    /// <param name="callback">The callback to continue reading.</param>
    /// <returns>True if the read operation is handled asynchronously.</returns>
    protected override bool Read(nint dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
    {

        HandleServerSentEvents(dataOut, callback);

        bytesRead = 0;
        return true;
    }

    /// <summary>
    /// Skips the specified number of bytes in the response stream.
    /// </summary>
    /// <param name="bytesToSkip">The number of bytes to skip.</param>
    /// <param name="bytesSkipped">The number of bytes actually skipped.</param>
    /// <param name="callback">The callback to continue skipping.</param>
    /// <returns>True if the skip operation is handled.</returns>
    protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
    {
        bytesSkipped = bytesToSkip;
        return true;
    }

    /// <summary>
    /// Task completion source used to signal when new events are available.
    /// </summary>
    TaskCompletionSource? _taskCompletionSource;

    /// <summary>
    /// Thread-safe collection of pending server-sent events.
    /// </summary>
    ConcurrentBag<ServerSentEvent> _events = new();

    /// <summary>
    /// Indicates whether the client is currently connected.
    /// </summary>
    bool _isConnected = true;

    /// <summary>
    /// The controller managing SSE clients and events.
    /// </summary>
    private ServerSentEventsController _controller;

    /// <summary>
    /// Invoked when a client connects to the SSE endpoint.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="callback">The callback to continue or cancel the request.</param>
    private async void OnClientConnected(CefRequest request, CefCallback callback)
    {
        await _controller.OnClientConnectedAsync(new SSEClientConnectedEventArgs(request, this));
    }

    /// <summary>
    /// Handles sending server-sent events to the client asynchronously.
    /// </summary>
    /// <param name="dataOut">Pointer to the output buffer.</param>
    /// <param name="callback">The callback to continue reading.</param>
    private async void HandleServerSentEvents(nint dataOut, CefResourceReadCallback callback)
    {


        if (!_isConnected || _taskCompletionSource is null)
        {
            callback.Continue(0);
            return;
        }

        await _taskCompletionSource.Task;


        if (!_isConnected)
        {
            callback.Continue(0);
            return;
        }

        _taskCompletionSource = new TaskCompletionSource();


        if (_events.TryTake(out var msg))
        {
            var sb = new StringBuilder();
            if (msg.Retry.HasValue)
            {
                sb.Append($"retry: {msg.Retry.Value}\n");
            }

            if (!string.IsNullOrWhiteSpace(msg.Event))
            {
                sb.Append($"event: {msg.Event}\n");
            }

            if (!string.IsNullOrWhiteSpace(msg.Id))
            {
                sb.Append($"id: {msg.Id}\n");
            }

            var lines = msg.Data?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries) ?? [""];
            foreach (var line in lines)
            {
                sb.Append($"data: {line}\n");
            }

            if (sb.Length > 0)
            {
                sb.Append('\n');

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());

                callback.Continue(bytes.Length);

                Marshal.Copy(bytes, 0, dataOut, bytes.Length);
            }
        }

        //var s = $"data: {(char)iChar++}\n\n";

        //var bytes = Encoding.UTF8.GetBytes(s);

        //callback.Continue(bytes.Length);

        //Marshal.Copy(bytes, 0, dataOut, bytes.Length);


        //Debug.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]: {s}");


        //if (iChar > 57) iChar = 48;







    }
}
