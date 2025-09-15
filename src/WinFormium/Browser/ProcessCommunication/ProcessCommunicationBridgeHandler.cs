// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.ProcessCommunication;

/// <summary>
/// Abstract base class for handling process communication bridge logic between browser and render processes.
/// Manages registration of request and message handlers, and provides context lifecycle notifications.
/// </summary>
internal abstract class ProcessCommunicationBridgeHandler
{
    /// <summary>
    /// Gets the dictionary of registered request handlers, keyed by request name.
    /// </summary>
    internal readonly Dictionary<string, Func<ProcessRequest, ProcessResponse>> RequestHandlers = new();

    /// <summary>
    /// Gets the dictionary of registered message handlers, keyed by message name.
    /// </summary>
    internal readonly Dictionary<string, Action<ProcessMessage>> MessageHandlers = new();

    ProcessCommunicationBridge _bridge;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessCommunicationBridgeHandler"/> class.
    /// </summary>
    /// <param name="bridge">The process communication bridge to associate with this handler.</param>
    public ProcessCommunicationBridgeHandler(ProcessCommunicationBridge bridge)
    {
        _bridge = bridge;

        if (ProcessType == CefProcessId.Browser)
        {
            InitializeOnBrowserSide();
        }
        else
        {
            InitializeOnRenderSide();
        }
    }

    /// <summary>
    /// Performs initialization logic specific to the browser process side.
    /// </summary>
    protected virtual void InitializeOnBrowserSide()
    {
    }

    /// <summary>
    /// Performs initialization logic specific to the render process side.
    /// </summary>
    protected virtual void InitializeOnRenderSide()
    {
    }

    /// <summary>
    /// Sends a synchronous process request through the bridge.
    /// </summary>
    /// <param name="request">The process request to send.</param>
    /// <returns>The response to the process request.</returns>
    public ProcessResponse Request(ProcessRequest request) => _bridge.Request(request);

    /// <summary>
    /// Sends an asynchronous process request through the bridge.
    /// </summary>
    /// <param name="request">The process request to send.</param>
    /// <returns>A task representing the asynchronous operation, with the process response as result.</returns>
    public Task<ProcessResponse> RequestAsync(ProcessRequest request) => _bridge.RequestAsync(request);

    /// <summary>
    /// Gets the process type (browser or renderer) for this handler.
    /// </summary>
    protected CefProcessId ProcessType => _bridge.ProcessType;

    /// <summary>
    /// Gets the associated browser instance.
    /// </summary>
    internal protected CefBrowser Browser => _bridge.Browser;

    /// <summary>
    /// Gets the current application context.
    /// </summary>
    protected WinFormiumApp AppContext => WinFormiumApp.Current!;

    /// <summary>
    /// Registers a request handler for the specified request name.
    /// </summary>
    /// <param name="requestName">The name of the request to handle.</param>
    /// <param name="handler">The handler function for the request.</param>
    protected void RegisterRequestHandler(string requestName, Func<ProcessRequest, ProcessResponse> handler)
    {
        RequestHandlers[requestName] = handler;
    }

    /// <summary>
    /// Registers a message handler for the specified message name.
    /// </summary>
    /// <param name="messageName">The name of the message to handle.</param>
    /// <param name="handler">The handler action for the message.</param>
    protected void RegisterMessageHandler(string messageName, Action<ProcessMessage> handler)
    {
        MessageHandlers[messageName] = handler;
    }

    /// <summary>
    /// Called when a V8 context is created on the render process side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was created.</param>
    /// <param name="context">The V8 context that was created.</param>
    public abstract void ContextCreatedOnRenderSide(CefBrowser browser, CefFrame frame, CefV8Context context);

    /// <summary>
    /// Called when a V8 context is released on the render process side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was released.</param>
    /// <param name="context">The V8 context that was released.</param>
    public abstract void ContextReleasedOnRenderSide(CefBrowser browser, CefFrame frame, CefV8Context context);

    /// <summary>
    /// Called when a context is created on the browser process side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was created.</param>
    public abstract void ContextCreatedOnBrowserSide(CefBrowser browser, CefFrame frame);

    /// <summary>
    /// Called when a context is released on the browser process side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was released.</param>
    public abstract void ContextReleasedOnBrowserSide(CefBrowser browser, CefFrame frame);
}
