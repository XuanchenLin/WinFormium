// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Provides off-screen rendering handler implementation for WinFormium browser.
/// </summary>
internal partial class FormiumOffscreenRenderHandler : IDisposable
{
    /// <inheritdoc/>
    public void Dispose()
    {
        OffscreenRender.Dispose();
    }

    /// <summary>
    /// Called when a popup is shown or hidden.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="show">True if the popup is shown, false if hidden.</param>
    void IRenderHandler.OnPopupShow(CefBrowser browser, bool show)
    {
        if (!show)
        {
            ShownPopupRect = null;
        }
    }

    /// <summary>
    /// Called to retrieve the root screen rectangle.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="rect">The rectangle to be set.</param>
    /// <returns>True if the rectangle was set, otherwise false.</returns>
    bool IRenderHandler.GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
    {
        return false;
    }

    /// <summary>
    /// Called to translate view coordinates to screen coordinates.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="viewX">X coordinate in view.</param>
    /// <param name="viewY">Y coordinate in view.</param>
    /// <param name="screenX">Resulting X coordinate in screen.</param>
    /// <param name="screenY">Resulting Y coordinate in screen.</param>
    /// <returns>True if the translation was successful, otherwise false.</returns>
    bool IRenderHandler.GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY)
    {
        return false;
    }

    /// <summary>
    /// Called to retrieve screen information for the browser.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="screenInfo">The screen information to be filled.</param>
    /// <returns>True if the information was set, otherwise false.</returns>
    bool IRenderHandler.GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
    {
        var screen = Screen.FromHandle(WindowHandle);

        screenInfo.DeviceScaleFactor = ScaleFactor;
        screenInfo.AvailableRectangle = new CefRectangle(
            screen.WorkingArea.X,
            screen.WorkingArea.Y,
            screen.WorkingArea.Width,
            screen.WorkingArea.Height);

        screenInfo.Rectangle = new CefRectangle(
            screen.Bounds.X,
            screen.Bounds.Y,
            screen.Bounds.Width,
            screen.Bounds.Height);


        return true;
    }

    /// <summary>
    /// Called when the popup size changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="rect">The new popup rectangle.</param>
    void IRenderHandler.OnPopupSize(CefBrowser browser, CefRectangle rect)
    {
        ShownPopupRect = new CefRectangle
        {
            X = (int)(Math.Floor(rect.X * ScaleFactor)),
            Y = (int)(Math.Floor(rect.Y * ScaleFactor)),
            Width = (int)(Math.Ceiling(rect.Width * ScaleFactor)),
            Height = (int)(Math.Ceiling(rect.Height * ScaleFactor))
        };
    }

    /// <summary>
    /// Called to retrieve the view rectangle for rendering.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="rect">The rectangle to be set.</param>
    void IRenderHandler.GetViewRect(CefBrowser browser, out CefRectangle rect)
    {
        rect = new CefRectangle();

        var hwnd = (HWND)WindowHandle;

        GetClientRect(hwnd, out var clientRect);

        rect.X = rect.Y = 0;

        RenderViewRect = new CefRectangle
        {
            X = 0,
            Y = 0,
            Width = clientRect.Width,
            Height = clientRect.Height
        };

        if (IsIconic(hwnd) || clientRect.Width == 0 || clientRect.Height == 0)
        {
            var placement = new WINDOWPLACEMENT();

            GetWindowPlacement(hwnd, ref placement);

            clientRect = placement.rcNormalPosition;

            rect.Width = (int)Math.Floor(clientRect.Width / ScaleFactor);
            rect.Height = (int)Math.Floor(clientRect.Height / ScaleFactor);
        }
        else
        {
            rect.Width = (int)Math.Floor(clientRect.Width / ScaleFactor);
            rect.Height = (int)Math.Floor(clientRect.Height / ScaleFactor);
        }

        if (rect.Width <= 0) rect.Width = 1;
        if (rect.Height <= 0) rect.Height = 1;


    }

    /// <summary>
    /// Called when accelerated painting is requested.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="type">The paint element type.</param>
    /// <param name="dirtyRects">The dirty rectangles.</param>
    /// <param name="sharedHandle">The shared handle for the surface.</param>
    void IRenderHandler.OnAcceleratedPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, nint sharedHandle)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Called when painting is required for the browser or popup.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="type">The paint element type.</param>
    /// <param name="dirtyRects">The dirty rectangles.</param>
    /// <param name="buffer">Pointer to the pixel buffer.</param>
    /// <param name="width">Width of the buffer.</param>
    /// <param name="height">Height of the buffer.</param>
    void IRenderHandler.OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, nint buffer, int width, int height)
    {
        OffscreenRender.OnPaint(browser, type, dirtyRects, buffer, width, height);
    }

    /// <summary>
    /// Gets a value indicating whether the popup is currently shown.
    /// </summary>
    public bool IsPopupShow => ShownPopupRect is not null;

    /// <summary>
    /// Gets or sets the rectangle of the shown popup.
    /// </summary>
    public CefRectangle? ShownPopupRect { get; set; }
    /// <summary>
    /// Gets or sets the rectangle of the rendered view.
    /// </summary>
    public CefRectangle? RenderViewRect { get; set; }

}

