// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Partial implementation of <see cref="IWebViewClient"/> for handling various browser events and customizing browser behavior.
/// </summary>
public partial class Formium : IWebViewClient
{

    /// <summary>
    /// Gets or sets the handler for audio stream events.
    /// </summary>
    public IAudioHandler? AudioHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for Chrome command events.
    /// </summary>
    public ICommandHandler? CommandHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for context menu and quick menu events.
    /// </summary>
    public IContextMenuHandler? ContextMenuHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for file dialog events.
    /// </summary>
    public IDialogHandler? DialogHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for browser display events and UI changes.
    /// </summary>
    public IDisplayHandler? DisplayHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for file download events.
    /// </summary>
    public IDownloadHandler? DownloadHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for drag-and-drop events.
    /// </summary>
    public IDragHandler? DragHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for find results in the browser.
    /// </summary>
    public IFindHandler? FindHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for focus events in the browser.
    /// </summary>
    public IFocusHandler? FocusHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for browser frame events.
    /// </summary>
    public IFrameHandler? FrameHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for permission-related events in the browser.
    /// </summary>
    public IPermissionHandler? PermissionHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for JavaScript dialogs in the browser.
    /// </summary>
    public IJSDialogHandler? JSDialogHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for keyboard events in the browser.
    /// </summary>
    public IKeyboardHandler? KeyboardHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for browser life span events.
    /// </summary>
    public ILifeSpanHandler? LifeSpanHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for browser loading events.
    /// </summary>
    public ILoadHandler? LoadHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for print-related events and operations.
    /// </summary>
    public IPrintHandler? PrintHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for rendering and related events.
    /// </summary>
    public IRenderHandler? RenderHandler { get; set; }
    /// <summary>
    /// Gets or sets the handler for browser request events.
    /// </summary>
    public IRequestHandler? RequestHandler { get; set; }

    /// <summary>
    /// Called when a new message is received from a different process. Return true if the message was handled or false otherwise.
    /// </summary>
    /// <param name="browser">The browser receiving the message.</param>
    /// <param name="frame">The frame receiving the message.</param>
    /// <param name="sourceProcess">The source process of the message.</param>
    /// <param name="message">The process message received.</param>
    /// <returns>True if the message was handled, false otherwise.</returns>
    internal bool OnProcessMessageReceivedCore(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        return false;
    }

    /// <inheritdoc/>
    IAudioHandler? IWebViewClient.AudioHandler { get => AudioHandler; set => AudioHandler = value; }
    /// <inheritdoc/>
    ICommandHandler? IWebViewClient.CommandHandler { get => CommandHandler; set => CommandHandler = value; }
    /// <inheritdoc/>
    IContextMenuHandler? IWebViewClient.ContextMenuHandler { get => ContextMenuHandler; set => ContextMenuHandler = value; }
    /// <inheritdoc/>
    IDialogHandler? IWebViewClient.DialogHandler { get => DialogHandler; set => DialogHandler = value; }
    /// <inheritdoc/>
    IDisplayHandler? IWebViewClient.DisplayHandler { get => this; set => new InvalidOperationException(); }
    /// <inheritdoc/>
    IDownloadHandler? IWebViewClient.DownloadHandler { get => DownloadHandler; set => DownloadHandler = value; }
    /// <inheritdoc/>
    IDragHandler? IWebViewClient.DragHandler { get => this; set => new InvalidOperationException(); }
    /// <inheritdoc/>
    IFindHandler? IWebViewClient.FindHandler { get => FindHandler; set => FindHandler = value; }
    /// <inheritdoc/>
    IFocusHandler? IWebViewClient.FocusHandler { get => FocusHandler; set => FocusHandler = value; }
    /// <inheritdoc/>
    IFrameHandler? IWebViewClient.FrameHandler { get => this; set => new InvalidOperationException(); }
    /// <inheritdoc/>
    IPermissionHandler? IWebViewClient.PermissionHandler { get => PermissionHandler; set => PermissionHandler = value; }
    /// <inheritdoc/>
    IJSDialogHandler? IWebViewClient.JSDialogHandler { get => JSDialogHandler; set => JSDialogHandler = value; }
    /// <inheritdoc/>
    IKeyboardHandler? IWebViewClient.KeyboardHandler { get => KeyboardHandler; set => KeyboardHandler = value; }
    /// <inheritdoc/>
    ILifeSpanHandler? IWebViewClient.LifeSpanHandler { get => this; set => new InvalidOperationException(); }
    /// <inheritdoc/>
    ILoadHandler? IWebViewClient.LoadHandler { get => this; set => new InvalidOperationException(); }
    /// <inheritdoc/>
    IPrintHandler? IWebViewClient.PrintHandler { get => PrintHandler; set => PrintHandler = value; }
    /// <inheritdoc/>
    IRenderHandler? IWebViewClient.RenderHandler { get => GetRenderHandler() ; set => new InvalidOperationException(); }


    private IRenderHandler? _renderHandler;

    private IRenderHandler? GetRenderHandler()
    {
        if(WindowStyleSettings.IsOffScreenRendering)
        {
            return _renderHandler?? (_renderHandler =  new FormiumOffscreenRenderHandler(WebView, WindowStyleSettings)) ?? RenderHandler;
        }

        return RenderHandler;
    }

    /// <inheritdoc/>
    IRequestHandler? IWebViewClient.RequestHandler { get => this; set => new InvalidOperationException(); }

    /// <inheritdoc/>
    bool IWebViewClient.OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        return OnProcessMessageReceivedCore(browser, frame, sourceProcess, message);
    }

    /// <inheritdoc/>
    WebResourceResponse? IWebViewClient.OnWebResourceRequesting(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return RequestWebResource(browser, frame, request);
    }

    /// <summary>
    /// Called when a web resource is being requested. Override to provide a custom response.
    /// </summary>
    /// <param name="browser">The browser making the request.</param>
    /// <param name="frame">The frame making the request.</param>
    /// <param name="request">The request object.</param>
    /// <returns>A <see cref="WebResourceResponse"/> to provide a custom response, or null to use the default behavior.</returns>
    protected virtual WebResourceResponse? RequestWebResource(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return null;
    }

    /// <inheritdoc/>
    WebResponseFilterHandlerDelegate? IWebViewClient.OnWebResponseFiltering(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
    {
        return FilterWebResponse(browser, frame, request, response);
    }

    /// <summary>
    /// Called when a web response is available for filtering. Override to provide a custom filter.
    /// </summary>
    /// <param name="browser">The browser receiving the response.</param>
    /// <param name="frame">The frame receiving the response.</param>
    /// <param name="request">The request object.</param>
    /// <param name="response">The response object.</param>
    /// <returns>A delegate to handle response filtering, or null to use the default behavior.</returns>
    protected virtual WebResponseFilterHandlerDelegate? FilterWebResponse(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
    {
        return null;
    }
}


/// <summary>
/// Delegate for filtering web response data.
/// </summary>
/// <param name="data">The response data to filter.</param>
/// <returns>The filtered response data.</returns>
public delegate byte[] WebResponseFilterHandlerDelegate(byte[] data);
