// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides a custom implementation of <see cref="CefClient"/> for WinFormium, 
/// delegating handler creation to the associated <c>BrowserClient</c> or using default handlers.
/// </summary>
partial class WebViewCore : CefClient
{

    /// <inheritdoc/>
    protected override CefAudioHandler? GetAudioHandler()
    {
        return BrowserClient?.AudioHandler == null ? base.GetAudioHandler() : _webViewAudioHandler ?? (_webViewAudioHandler = new WebViewAudioHandler(BrowserClient.AudioHandler));
    }

    /// <inheritdoc/>
    protected override CefCommandHandler? GetCommandHandler()
    {
        return BrowserClient?.CommandHandler == null ? base.GetCommandHandler() : _webViewCommandHandler ?? (_webViewCommandHandler = new WebViewCommandHandler(BrowserClient.CommandHandler));
    }

    /// <inheritdoc/>
    protected override CefContextMenuHandler? GetContextMenuHandler()
    {
        return _webViewContextMenuHandler ?? (_webViewContextMenuHandler = new WebViewContextMenuHandler(this));
    }

    /// <inheritdoc/>
    protected override CefDialogHandler? GetDialogHandler()
    {
        return BrowserClient?.DialogHandler == null ? base.GetDialogHandler() : _webViewDialogHandler ?? (_webViewDialogHandler = new WebViewDialogHandler(BrowserClient.DialogHandler));
    }

    /// <inheritdoc/>
    protected override CefDisplayHandler? GetDisplayHandler()
    {
        return BrowserClient?.DisplayHandler == null ? base.GetDisplayHandler() : _webViewDisplayHandler ?? (_webViewDisplayHandler = new WebViewDisplayHandler(BrowserClient.DisplayHandler));
    }

    /// <inheritdoc/>
    protected override CefDownloadHandler? GetDownloadHandler()
    {
        return BrowserClient?.DownloadHandler == null ? base.GetDownloadHandler() : _webViewDownloadHandler ?? (_webViewDownloadHandler = new WebViewDownloadHandler(BrowserClient.DownloadHandler));
    }

    /// <inheritdoc/>
    protected override CefDragHandler? GetDragHandler()
    {
        return BrowserClient?.DragHandler == null ? base.GetDragHandler() : _webViewDragHandler ?? (_webViewDragHandler = new WebViewDragHandler(BrowserClient.DragHandler));
    }

    /// <inheritdoc/>
    protected override CefFindHandler? GetFindHandler()
    {
        return BrowserClient?.FindHandler == null ? base.GetFindHandler() : _webViewFindHandler ?? (_webViewFindHandler = new WebViewFindHandler(BrowserClient.FindHandler));
    }

    /// <inheritdoc/>
    protected override CefFocusHandler? GetFocusHandler()
    {
        return BrowserClient?.FocusHandler == null ? base.GetFocusHandler() : _webViewFocusHandler ?? (_webViewFocusHandler = new WebViewFocusHandler(BrowserClient.FocusHandler));
    }

    /// <inheritdoc/>
    protected override CefFrameHandler? GetFrameHandler()
    {
        return _webViewFrameHandler ?? (_webViewFrameHandler = new WebViewFrameHandler(this));
    }

    /// <inheritdoc/>
    protected override CefJSDialogHandler? GetJSDialogHandler()
    {
        return BrowserClient?.JSDialogHandler == null ? base.GetJSDialogHandler() : _webViewJSDialogHandler ?? (_webViewJSDialogHandler = new WebViewJSDialogHandler(BrowserClient.JSDialogHandler));
    }

    /// <inheritdoc/>
    protected override CefKeyboardHandler? GetKeyboardHandler()
    {
        return BrowserClient?.KeyboardHandler == null ? base.GetKeyboardHandler() : _cefKeyboardHandler ?? (_cefKeyboardHandler = new WebViewKeyboardHandler(BrowserClient.KeyboardHandler));
    }

    /// <inheritdoc/>
    protected override CefLifeSpanHandler? GetLifeSpanHandler()
    {
        return _webViewLifeSpanHandler ?? (_webViewLifeSpanHandler = new WebViewLifeSpanHandler(this));
    }

    /// <inheritdoc/>
    protected override CefLoadHandler? GetLoadHandler()
    {
        return _webViewLoadHandler ?? (_webViewLoadHandler = new WebViewLoadHandler(this));
    }

    /// <inheritdoc/>
    protected override CefPrintHandler? GetPrintHandler()
    {
        return BrowserClient?.PrintHandler == null ? base.GetPrintHandler() : _webViewPrintHandler ?? (_webViewPrintHandler = new WebViewPrintHandler(BrowserClient.PrintHandler));
    }

    /// <inheritdoc/>
    protected override CefPermissionHandler? GetPermissionHandler()
    {
        return BrowserClient?.PermissionHandler == null ? base.GetPermissionHandler() : _webViewPermissionHandler ?? (_webViewPermissionHandler = new WebViewPermissionHandler(BrowserClient.PermissionHandler));
    }

    /// <inheritdoc/>
    protected override CefRenderHandler? GetRenderHandler()
    {
        return BrowserClient?.RenderHandler == null ? base.GetRenderHandler() : _webViewRenderHandler ?? (_webViewRenderHandler = new WebViewRenderHandler(BrowserClient.RenderHandler));
    }

    /// <inheritdoc/>
    protected override CefRequestHandler? GetRequestHandler()
    {
        return _webViewRequestHandler ?? (_webViewRequestHandler = new WebViewRequestHandler(this));
    }

    /// <inheritdoc/>
    protected override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        CommunicationBridge?.DispatchProcessMessage(browser, frame, sourceProcess, message);
        return BrowserClient == null ? base.OnProcessMessageReceived(browser, frame, sourceProcess, message) : BrowserClient.OnProcessMessageReceived(browser, frame, sourceProcess, message);
    }

    private WebViewAudioHandler? _webViewAudioHandler = null;
    private WebViewCommandHandler? _webViewCommandHandler = null;
    private WebViewContextMenuHandler? _webViewContextMenuHandler = null;
    private WebViewDialogHandler? _webViewDialogHandler = null;
    private WebViewDisplayHandler? _webViewDisplayHandler = null;
    private WebViewDownloadHandler? _webViewDownloadHandler = null;
    private WebViewDragHandler? _webViewDragHandler = null;
    private WebViewFindHandler? _webViewFindHandler = null;
    private WebViewFocusHandler? _webViewFocusHandler = null;
    private WebViewFrameHandler? _webViewFrameHandler = null;
    private WebViewJSDialogHandler? _webViewJSDialogHandler = null;
    private CefKeyboardHandler? _cefKeyboardHandler = null;
    private WebViewLifeSpanHandler? _webViewLifeSpanHandler = null;
    private WebViewLoadHandler? _webViewLoadHandler = null;
    private WebViewPrintHandler? _webViewPrintHandler = null;
    private WebViewPermissionHandler? _webViewPermissionHandler = null;
    private WebViewRenderHandler? _webViewRenderHandler = null;
    private WebViewRequestHandler? _webViewRequestHandler = null;
}

