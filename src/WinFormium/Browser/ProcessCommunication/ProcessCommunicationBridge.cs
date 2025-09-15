// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.ProcessCommunication;

/// <summary>
/// Provides a bridge for process communication between browser and render processes in WinFormium.
/// Manages message and request handlers, context notifications, and inter-process messaging.
/// </summary>
class ProcessCommunicationBridge : IDisposable
{
    /// <summary>
    /// The prefix used for naming the process communication pipe.
    /// </summary>
    const string PIPE_NAME_PREFIX = "WinFormiumProcessCommunicationProxy";

    /// <summary>
    /// The list of registered process communication bridge handlers.
    /// </summary>
    private readonly List<ProcessCommunicationBridgeHandler> _handlers = new();

    /// <summary>
    /// Dispatches a process message to the appropriate registered message handler.
    /// </summary>
    /// <param name="browser">The browser instance associated with the message.</param>
    /// <param name="frame">The frame associated with the message.</param>
    /// <param name="sourceProcess">The source process ID of the message.</param>
    /// <param name="message">The process message to dispatch.</param>
    public void DispatchProcessMessage(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        var handlers = _handlers.SelectMany(handler => handler.MessageHandlers)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        if (handlers.TryGetValue(message.Name, out var handler))
        {
            var processMessage = new ProcessMessage(browser, frame, sourceProcess, message);
            handler(processMessage);
        }
    }

    /// <summary>
    /// Sends a synchronous process request and returns the response.
    /// </summary>
    /// <param name="request">The process request to send.</param>
    /// <returns>The response to the process request.</returns>
    public ProcessResponse Request(ProcessRequest request)
    {

        var client = new RequestClient(GetPipeName(request.BrowserId));
        return client.Request(request);
    }

    /// <summary>
    /// Sends an asynchronous process request and returns a task representing the response.
    /// </summary>
    /// <param name="request">The process request to send.</param>
    /// <returns>A task representing the asynchronous operation, with the process response as result.</returns>
    public Task<ProcessResponse> RequestAsync(ProcessRequest request)
    {
        return Task.Run(() => Request(request));
    }

    /// <summary>
    /// Gets the current <see cref="WinFormiumApp"/> application context.
    /// </summary>
    public static WinFormiumApp AppContext => WinFormiumApp.Current!;

    /// <summary>
    /// Gets the pipe name for the specified browser ID.
    /// </summary>
    /// <param name="browserId">The browser identifier.</param>
    /// <returns>The pipe name for inter-process communication.</returns>
    public static string GetPipeName(int browserId)
    {
        var browserProcessId = AppContext.BrowserProcessId;
        return $"{PIPE_NAME_PREFIX}-{browserProcessId}-{browserId}";
    }

    /// <summary>
    /// Gets or sets the process type for this bridge.
    /// </summary>
    public CefProcessId ProcessType { get; set; }

    /// <summary>
    /// Gets the associated browser instance.
    /// </summary>
    public CefBrowser Browser { get; private set; }

    /// <summary>
    /// Gets the request server for handling incoming requests, if available.
    /// </summary>
    private RequestServer? RequestServer { get; }

    /// <summary>
    /// Gets the internal process message communicator for context notifications.
    /// </summary>
    private InternalProcessMessageCommunicator InternalCommunicator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessCommunicationBridge"/> class.
    /// </summary>
    /// <param name="browser">The associated browser instance.</param>
    /// <param name="processId">The process type for this bridge.</param>
    public ProcessCommunicationBridge(CefBrowser browser, CefProcessId processId)
    {
        Browser = browser;
        ProcessType = processId;

        if (ProcessType == CefProcessId.Browser)
        {
            RequestServer = new RequestServer(GetPipeName(browser.Identifier));
            RequestServer.OnRequestReceived = OnRequestReceived;
        }

        InternalCommunicator = new InternalProcessMessageCommunicator(this);

        RegisterProcessCommunicationBridgeHandler(InternalCommunicator);


        InternalCommunicator.NotifyContextCreated = ReceiveContextCreatedMessageOnBrowser;
        InternalCommunicator.NotifyContextReleased = ReceiveContextReleasedMessageOnBrowser;
    }

    /// <summary>
    /// Registers a process communication bridge handler.
    /// </summary>
    /// <param name="handler">The handler to register.</param>
    public void RegisterProcessCommunicationBridgeHandler(ProcessCommunicationBridgeHandler handler)
    {
        _handlers.Add(handler);
    }

    /// <summary>
    /// Handles an incoming process request and dispatches it to the appropriate handler.
    /// </summary>
    /// <param name="request">The process request received.</param>
    /// <returns>The response to the process request, or null if no handler is found.</returns>
    public ProcessResponse? OnRequestReceived(ProcessRequest request)
    {
        var requestHandlers = _handlers.SelectMany(handler => handler.RequestHandlers)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        if (requestHandlers.TryGetValue(request.Name, out var handler))
        {
            return handler(request);
        }

        return null;
    }

    /// <summary>
    /// Notifies all handlers that a V8 context has been created on the render side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was created.</param>
    /// <param name="context">The V8 context that was created.</param>
    public void OnRenderContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
        Browser = browser;
        foreach (var handler in _handlers)
        {
            handler.ContextCreatedOnRenderSide(browser, frame, context);
        }
    }

    /// <summary>
    /// Notifies all handlers that a V8 context has been released on the render side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was released.</param>
    /// <param name="context">The V8 context that was released.</param>
    public void OnRenderContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
        foreach (var handler in _handlers)
        {
            handler.ContextReleasedOnRenderSide(browser, frame, context);
        }
    }

    /// <summary>
    /// Notifies all handlers that a context created message was received on the browser side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was created.</param>
    public void ReceiveContextCreatedMessageOnBrowser(CefBrowser browser, CefFrame frame)
    {
        foreach (var handler in _handlers)
        {
            handler.ContextCreatedOnBrowserSide(browser, frame);
        }
        //Console.WriteLine($"BrowserContext created: BrowserId=0x{browser.Identifier:X}, FrameId=0x{frame.Identifier:X}");
    }

    /// <summary>
    /// Notifies all handlers that a context released message was received on the browser side.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context was released.</param>
    public void ReceiveContextReleasedMessageOnBrowser(CefBrowser browser, CefFrame frame)
    {
        foreach (var handler in _handlers)
        {
            handler.ContextReleasedOnBrowserSide(browser, frame);
        }
        //Console.WriteLine($"BrowserContext released: BrowserId=0x{browser.Identifier:X}, FrameId=0x{frame.Identifier:X}");
    }

    /// <summary>
    /// Releases resources used by the <see cref="ProcessCommunicationBridge"/>.
    /// </summary>
    public void Dispose()
    {
        RequestServer?.Dispose();
    }

    #region InternalProcessMessageCommunicator
    /// <summary>
    /// Provides internal communication for context creation and release messages between processes.
    /// </summary>
    class InternalProcessMessageCommunicator : ProcessCommunicationBridgeHandler
    {
        /// <summary>
        /// Represents a message indicating that a context has been created.
        /// </summary>
        public struct ContextCreatedMessage
        {
            /// <summary>
            /// The name of the context created message.
            /// </summary>
            public const string Name = nameof(ContextCreatedMessage);

            /// <summary>
            /// The browser identifier.
            /// </summary>
            public int BrowserId;
            /// <summary>
            /// The frame identifier.
            /// </summary>
            public long FrameId;

            /// <summary>
            /// Converts this message to a <see cref="CefProcessMessage"/> instance.
            /// </summary>
            /// <returns>The created <see cref="CefProcessMessage"/>.</returns>
            public CefProcessMessage ToCefProcessMessage()
            {
                var message = CefProcessMessage.Create(Name)!;
                using (var arguments = message.Arguments!)
                {
                    arguments.SetInt(0, BrowserId);
                    arguments.SetInt(1, (int)(FrameId >> 16));
                    arguments.SetInt(2, (int)(FrameId & 0xFFFFFFFF));
                }

                return message;
            }

            /// <summary>
            /// Creates a <see cref="ContextCreatedMessage"/> from a <see cref="CefProcessMessage"/>.
            /// </summary>
            /// <param name="message">The process message to parse.</param>
            /// <returns>The parsed <see cref="ContextCreatedMessage"/>.</returns>
            public static ContextCreatedMessage FromCefProcessMessage(CefProcessMessage message)
            {
                using (var arguments = message.Arguments!)
                {
                    return new ContextCreatedMessage
                    {
                        BrowserId = arguments.GetInt(0),
                        FrameId = (long)arguments.GetInt(1) << 16 | (uint)arguments.GetInt(2)
                    };
                }
            }
        }

        /// <summary>
        /// Represents a message indicating that a context has been released.
        /// </summary>
        public struct ContextReleasedMessage
        {
            /// <summary>
            /// The name of the context released message.
            /// </summary>
            public const string Name = nameof(ContextReleasedMessage);
            /// <summary>
            /// The browser identifier.
            /// </summary>
            public int BrowserId;
            /// <summary>
            /// The frame identifier.
            /// </summary>
            public long FrameId;
            /// <summary>
            /// Converts this message to a <see cref="CefProcessMessage"/> instance.
            /// </summary>
            /// <returns>The created <see cref="CefProcessMessage"/>.</returns>
            public CefProcessMessage ToCefProcessMessage()
            {
                var message = CefProcessMessage.Create(Name)!;
                using (var arguments = message.Arguments!)
                {
                    arguments.SetInt(0, BrowserId);
                    arguments.SetInt(1, (int)(FrameId >> 16));
                    arguments.SetInt(2, (int)(FrameId & 0xFFFFFFFF));
                }
                return message;
            }

            /// <summary>
            /// Creates a <see cref="ContextReleasedMessage"/> from a <see cref="CefProcessMessage"/>.
            /// </summary>
            /// <param name="message">The process message to parse.</param>
            /// <returns>The parsed <see cref="ContextReleasedMessage"/>.</returns>
            public static ContextReleasedMessage FromCefProcessMessage(CefProcessMessage message)
            {
                using (var arguments = message.Arguments!)
                {
                    return new ContextReleasedMessage
                    {
                        BrowserId = arguments.GetInt(0),
                        FrameId = (long)arguments.GetInt(1) << 16 | (uint)arguments.GetInt(2)
                    };
                }
            }
        }

        /// <summary>
        /// Gets or sets the action to notify when a context is created.
        /// </summary>
        public Action<CefBrowser, CefFrame>? NotifyContextCreated { get; set; }
        /// <summary>
        /// Gets or sets the action to notify when a context is released.
        /// </summary>
        public Action<CefBrowser, CefFrame>? NotifyContextReleased { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalProcessMessageCommunicator"/> class.
        /// </summary>
        /// <param name="bridge">The parent process communication bridge.</param>
        public InternalProcessMessageCommunicator(ProcessCommunicationBridge bridge) : base(bridge)
        {
            RegisterMessageHandler(ContextCreatedMessage.Name, OnContextCreated);
            RegisterMessageHandler(ContextReleasedMessage.Name, OnContextReleased);
        }

        /// <summary>
        /// Handles the context created process message.
        /// </summary>
        /// <param name="args">The process message arguments.</param>
        private void OnContextCreated(ProcessMessage args)
        {
            var message = ContextCreatedMessage.FromCefProcessMessage(args.Message);

            //Console.WriteLine($"RenderContext created: BrowserId=0x{message.BrowserId:X}, FrameId=0x{message.FrameId:X}");

            //OnBrowserContextCreated(args.Browser, args.Frame);

            NotifyContextCreated?.Invoke(args.Browser, args.Frame);

        }

        /// <summary>
        /// Handles the context released process message.
        /// </summary>
        /// <param name="args">The process message arguments.</param>
        private void OnContextReleased(ProcessMessage args)
        {
            var message = ContextReleasedMessage.FromCefProcessMessage(args.Message);

            //Console.WriteLine($"RenderContext released: BrowserId=0x{message.BrowserId:X}, FrameId=0x{message.FrameId:X}");

            //OnBrowserContextReleased(args.Browser, args.Frame);

            NotifyContextReleased?.Invoke(args.Browser, args.Frame);

        }

        /// <inheritdoc/>
        public override void ContextCreatedOnRenderSide(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            var message = new ContextCreatedMessage
            {
                BrowserId = browser.Identifier,
                FrameId = frame.Identifier
            };
            var processMessage = message.ToCefProcessMessage();
            frame.SendProcessMessage(CefProcessId.Browser, processMessage);
        }

        /// <inheritdoc/>
        public override void ContextReleasedOnRenderSide(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            var message = new ContextReleasedMessage
            {
                BrowserId = browser.Identifier,
                FrameId = frame.Identifier
            };
            var processMessage = message.ToCefProcessMessage();
            frame.SendProcessMessage(CefProcessId.Browser, processMessage);
        }

        /// <inheritdoc/>
        public override void ContextCreatedOnBrowserSide(CefBrowser browser, CefFrame frame)
        {
            // ignore
        }

        /// <inheritdoc/>
        public override void ContextReleasedOnBrowserSide(CefBrowser browser, CefFrame frame)
        {
            // ignore
        }
    }
    #endregion

}
