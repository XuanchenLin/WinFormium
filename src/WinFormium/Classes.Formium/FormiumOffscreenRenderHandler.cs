// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides an implementation of <see cref="IRenderHandler"/> for off-screen rendering in WinFormium.
/// Handles IME, drag-and-drop, touch, and rendering-related events for the associated <see cref="WebViewCore"/>.
/// </summary>
internal partial class FormiumOffscreenRenderHandler : IRenderHandler
{
    /// <summary>
    /// Gets or sets the offscreen render implementation used for rendering.
    /// </summary>
    private IOffscreenRender OffscreenRender;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormiumOffscreenRenderHandler"/> class.
    /// Sets up IME handler, D3D11 device, window resize, and input event registration.
    /// </summary>
    /// <param name="webview">The associated <see cref="WebViewCore"/> instance.</param>
    /// <param name="windowSettings">The window settings used to configure the offscreen render.</param>
    public FormiumOffscreenRenderHandler(WebViewCore webview, WindowSettings windowSettings)
    {
        _webview = webview;

        WindowHandle = HostWindow.Handle;

        ImeHandler = new WebViewOsrImeHandler(this);

        OffscreenRender = windowSettings.GetOffscreenRender(this);

        OffscreenRender.InitializeDeviceContext();

        HostWindow.Resize += (_, _) =>
        {
            if (HostWindow.WindowState == FormWindowState.Minimized || !HostWindow.IsHandleCreated) return;

            OffscreenRender.NotifyResize();
        };

        //webview.BrowserWindowProc +=

        windowSettings.WndProc += (ref Message m) =>
        {
            var msg = (uint)m.Msg;

            switch (msg)
            {
                case WM_IME_SETCONTEXT:
                    {
                        ImeHandler.OnIMESetContext(ref m);
                    }
                    return true;

                case WM_IME_STARTCOMPOSITION:
                    {
                        ImeHandler.OnImeStartComposition();
                    }
                    return true;

                case WM_IME_COMPOSITION:
                    {
                        ImeHandler.OnImeComposition(ref m);
                    }
                    return true;

                case WM_IME_ENDCOMPOSITION:
                    {
                        ImeHandler.OnImeCancelComposition();
                    }
                    return true;
            }

            return false;
        };

        RegisterHostWindowInputEvents(HostWindow);
    }

    /// <inheritdoc/>
    CefAccessibilityHandler? IRenderHandler.GetAccessibilityHandler()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    void IRenderHandler.GetTouchHandleSize(CefBrowser browser, CefHorizontalAlignment orientation, out CefSize size)
    {
        size = new CefSize(0, 0);
    }

    /// <inheritdoc/>
    void IRenderHandler.OnScrollOffsetChanged(CefBrowser browser, double x, double y)
    {
    }

    /// <inheritdoc/>
    bool IRenderHandler.StartDragging(CefBrowser browser, CefDragData dragData, CefDragOperationsMask allowedOps, int x, int y)
    {
        return false;
    }

    /// <inheritdoc/>
    void IRenderHandler.UpdateDragCursor(CefBrowser browser, CefDragOperationsMask operation)
    {
    }

    /// <summary>
    /// Gets the host window <see cref="Form"/> associated with this render handler.
    /// </summary>
    internal Form HostWindow => _webview.ContainerForm;

    /// <summary>
    /// Gets the window handle of the host window.
    /// </summary>
    internal nint WindowHandle { get; }

    /// <summary>
    /// Gets the current DPI scale factor for rendering.
    /// </summary>
    internal float ScaleFactor => _webview.ScaleFactor;

    /// <summary>
    /// Gets the device DPI of the host window.
    /// </summary>
    internal int DeviceDpi => HostWindow.DeviceDpi;

    /// <summary>
    /// Gets the associated <see cref="WebViewCore"/> instance for this render handler.
    /// </summary>
    internal WebViewCore WebView => _webview;

    /// <summary>
    /// The associated <see cref="WebViewCore"/> instance.
    /// </summary>
    private readonly WebViewCore _webview;
}