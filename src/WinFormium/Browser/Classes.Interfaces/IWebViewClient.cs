// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Provides an interface for handling various browser events and customizing browser behavior.
/// Implement this interface to receive notifications and control browser features such as audio, commands, context menus, dialogs, display, downloads, drag-and-drop, find, focus, frames, permissions, JavaScript dialogs, keyboard, life span, loading, printing, rendering, and requests.
/// </summary>
internal interface IWebViewClient
{
    /// <summary>
    /// Gets or sets the handler for audio stream events.
    /// </summary>
    IAudioHandler? AudioHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for Chrome command events.
    /// </summary>
    ICommandHandler? CommandHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for context menu and quick menu events.
    /// </summary>
    IContextMenuHandler? ContextMenuHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for file dialog events.
    /// </summary>
    IDialogHandler? DialogHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for browser display events and UI changes.
    /// </summary>
    IDisplayHandler? DisplayHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for file download events.
    /// </summary>
    IDownloadHandler? DownloadHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for drag-and-drop events.
    /// </summary>
    IDragHandler? DragHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for find results in the browser.
    /// </summary>
    IFindHandler? FindHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for focus events in the browser.
    /// </summary>
    IFocusHandler? FocusHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for browser frame events.
    /// </summary>
    IFrameHandler? FrameHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for permission-related events in the browser.
    /// </summary>
    IPermissionHandler? PermissionHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for JavaScript dialogs in the browser.
    /// </summary>
    IJSDialogHandler? JSDialogHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for keyboard events in the browser.
    /// </summary>
    IKeyboardHandler? KeyboardHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for browser life span events.
    /// </summary>
    ILifeSpanHandler? LifeSpanHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for browser loading events.
    /// </summary>
    ILoadHandler? LoadHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for print-related events and operations.
    /// </summary>
    IPrintHandler? PrintHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for rendering and related events.
    /// </summary>
    IRenderHandler? RenderHandler { get; set; }

    /// <summary>
    /// Gets or sets the handler for browser request events.
    /// </summary>
    IRequestHandler? RequestHandler { get; set; }

    /// <summary>
    /// Called when a new message is received from a different process.
    /// </summary>
    /// <param name="browser">The browser receiving the message.</param>
    /// <param name="frame">The frame receiving the message.</param>
    /// <param name="sourceProcess">The source process of the message.</param>
    /// <param name="message">The process message received.</param>
    /// <returns>True if the message was handled, false otherwise.</returns>
    bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message);

    /// <summary>
    /// Called when a web resource is being requested.
    /// </summary>
    /// <param name="browser">The browser making the request.</param>
    /// <param name="frame">The frame making the request.</param>
    /// <param name="request">The request object.</param>
    /// <returns>A <see cref="WebResourceResponse"/> to provide a custom response, or null to use the default behavior.</returns>
    WebResourceResponse? OnWebResourceRequesting(CefBrowser browser, CefFrame frame, CefRequest request);

    /// <summary>
    /// Called when a web response is available for filtering.
    /// </summary>
    /// <param name="browser">The browser receiving the response.</param>
    /// <param name="frame">The frame receiving the response.</param>
    /// <param name="request">The request object.</param>
    /// <param name="response">The response object.</param>
    /// <returns>A delegate to handle response filtering, or null to use the default behavior.</returns>
    WebResponseFilterHandlerDelegate? OnWebResponseFiltering(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response);



}
