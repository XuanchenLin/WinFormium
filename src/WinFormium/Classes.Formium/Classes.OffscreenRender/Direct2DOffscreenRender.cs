// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using Vortice.Direct2D1;
using Vortice.Mathematics;

using Windows.Win32.Graphics.Gdi;

namespace WinFormium;


/// <summary>
/// Provides offscreen rendering using Direct2D for WinFormium.
/// Handles device context initialization, resizing, painting, and resource management.
/// </summary>
class Direct2DOffscreenRender : IOffscreenRender
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Direct2DOffscreenRender"/> class.
    /// </summary>
    /// <param name="osrRenderHandler">The core offscreen render handler.</param>
    public Direct2DOffscreenRender(FormiumOffscreenRenderHandler osrRenderHandler)
    {
        OsrRenderHandler = osrRenderHandler;
    }

    /// <inheritdoc />
    public bool IsPopupShow => ShownPopupRect is not null;

    /// <inheritdoc />
    public CefRectangle? ShownPopupRect => OsrRenderHandler.ShownPopupRect;

    /// <inheritdoc />
    public CefRectangle? RenderViewRect => OsrRenderHandler.RenderViewRect;

    /// <inheritdoc />
    public Form HostWindow => OsrRenderHandler.HostWindow;

    /// <inheritdoc />
    public int DeviceDpi => OsrRenderHandler.DeviceDpi;

    /// <inheritdoc />
    public nint Handle => OsrRenderHandler.WindowHandle;

    /// <summary>
    /// Gets the core offscreen render handler.
    /// </summary>
    public FormiumOffscreenRenderHandler OsrRenderHandler { get; }

    /// <summary>
    /// Gets the Direct2D factory instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the factory is not initialized.</exception>
    public ID2D1Factory1 D2D1Factory => _d2d1Factory ?? throw new InvalidOperationException("D2D1Factory is not initialized.");

    /// <summary>
    /// Gets the Direct2D DC render target.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the render target is not initialized.</exception>
    public ID2D1DCRenderTarget DCRenderTarget => _dcRenderTarget ?? throw new InvalidOperationException("DCRenderTarget is not initialized.");

    /// <summary>
    /// Releases all resources used by the renderer.
    /// </summary>
    public void Dispose()
    {
        //DeleteDC(ScreenDC);
        DeleteDC(MemDC);
        //ReleaseDC(HWND.Null, ScreenDC);
        ReleaseDC(HWND.Null, MemDC);
        DCRenderTarget.Dispose();
        D2D1Factory.Dispose();
    }

    /// <summary>
    /// Initializes the Direct2D device context and related resources.
    /// </summary>
    public void InitializeDeviceContext()
    {
        InitD2D1Device();
    }

    /// <summary>
    /// Notifies the renderer that the host window has been resized.
    /// </summary>
    public void NotifyResize()
    {
        ResizeRenderDC();
    }

    /// <summary>
    /// Handles paint events from CEF and updates the Direct2D render target.
    /// </summary>
    /// <param name="browser">The CEF browser instance.</param>
    /// <param name="type">The paint element type (view or popup).</param>
    /// <param name="dirtyRects">The dirty rectangles to update.</param>
    /// <param name="buffer">Pointer to the pixel buffer.</param>
    /// <param name="width">The width of the buffer.</param>
    /// <param name="height">The height of the buffer.</param>
    public void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, nint buffer, int width, int height)
    {
        if (width <= 0 || height <= 0) return;

        lock (_lockThread)
        {

            if (type == CefPaintElementType.View)
            {
                var dx = Math.Abs(HostWindow.ClientSize.Width - width);
                var dy = Math.Abs(HostWindow.ClientSize.Height - height);

                if (dx > 1 || dy > 1)
                {
                    return;
                }

                if (HostWindow.WindowState == FormWindowState.Maximized)
                {
                    offsetX = (HostWindow.Size.Width - width) / 2;
                    offsetY = (HostWindow.Size.Height - height) / 2;
                }
                else
                {
                    offsetX = 0;
                    offsetY = 0;
                }
            }

            if (!IsRenderTargetAvailable)
            {
                CreateRenderDC();
            }

            var dc = DCRenderTarget;

            if (type == CefPaintElementType.View)
            {
                using var bmp = dc.CreateBitmap(new SizeI(width, height), buffer, (uint)width * 4, new BitmapProperties
                {
                    PixelFormat = new Vortice.DCommon.PixelFormat
                    {
                        AlphaMode = Vortice.DCommon.AlphaMode.Premultiplied,
                        Format = Vortice.DXGI.Format.B8G8R8A8_UNorm
                    },
                });

                if (D2D1ViewSharedBitmap is not null)
                {
                    D2D1ViewSharedBitmap.Dispose();
                    D2D1ViewSharedBitmap = null;
                }

                D2D1ViewSharedBitmap = dc.CreateSharedBitmap(bmp);
            }
            else
            {
                using var bmp = dc.CreateBitmap(new SizeI(width, height), buffer, (uint)width * 4, new BitmapProperties
                {
                    PixelFormat = new Vortice.DCommon.PixelFormat
                    {
                        AlphaMode = Vortice.DCommon.AlphaMode.Premultiplied,
                        Format = Vortice.DXGI.Format.B8G8R8A8_UNorm
                    },
                });

                if (D2D1PopupSharedBitmap is not null)
                {
                    D2D1PopupSharedBitmap.Dispose();
                    D2D1PopupSharedBitmap = null;
                }

                D2D1PopupSharedBitmap = dc.CreateSharedBitmap(bmp);
            }

            dc.BeginDraw();
            dc.Clear(new Color4(0, 0, 0, 0));

            if (D2D1ViewSharedBitmap is not null && RenderViewRect.HasValue)
            {
                var rect = RenderViewRect.Value;

                var targetRect = new Rect(offsetX, offsetY, D2D1ViewSharedBitmap.Size.Width, D2D1ViewSharedBitmap.Size.Height);
                var sourceRect = new Rect(0, 0, D2D1ViewSharedBitmap.Size.Width, D2D1ViewSharedBitmap.Size.Height);

                dc.DrawBitmap(D2D1ViewSharedBitmap, targetRect, 1.0f, BitmapInterpolationMode.Linear, sourceRect);
            }

            if (IsPopupShow && D2D1PopupSharedBitmap is not null && ShownPopupRect.HasValue)
            {
                var rect = ShownPopupRect.Value;

                dc.DrawBitmap(D2D1PopupSharedBitmap, new Rect(rect.X + offsetX, rect.Y + offsetY, rect.Width, rect.Height), 1.0f, BitmapInterpolationMode.Linear, new Rect(0, 0, rect.Width, rect.Height));
            }

            if (dc.EndDraw().Failure)
            {
                DiscardRenderDC();
                return;
            }

            if (HostWindow.WindowState == FormWindowState.Normal)
            {
                var size = HostWindow.Size;
                var location = HostWindow.Location;

                if (size.Width <= 0 || size.Height <= 0)
                {
                    return;
                }

                UpdateLayeredWindow((HWND)Handle, HDC.Null, location, size, MemDC, Point.Empty, (COLORREF)0, _blend, UPDATE_LAYERED_WINDOW_FLAGS.ULW_ALPHA);
            }
            else if (HostWindow.WindowState == FormWindowState.Maximized)
            {
                var size = HostWindow.Size;
                var location = HostWindow.Location;

                if (size.Width <= 0 || size.Height <= 0)
                {
                    return;
                }

                UpdateLayeredWindow((HWND)Handle, HDC.Null, location, size, MemDC, Point.Empty, (COLORREF)0, _blend, UPDATE_LAYERED_WINDOW_FLAGS.ULW_ALPHA);
            }
        }
    }

    /// <summary>
    /// The Direct2D factory instance.
    /// </summary>
    private ID2D1Factory1? _d2d1Factory;

    /// <summary>
    /// A lock object to ensure thread safety during rendering operations.
    /// </summary>
    private object _lockThread = new object();

    /// <summary>
    /// The Direct2D DC render target.
    /// </summary>
    private ID2D1DCRenderTarget? _dcRenderTarget;
    /// <summary>
    /// The X offset for rendering, used when the window is maximized.
    /// </summary>
    int offsetX = 0;

    /// <summary>
    /// The Y offset for rendering, used when the window is maximized.
    /// </summary>
    int offsetY = 0;

    /// <summary>
    /// The blend function used for layered window updates.
    /// </summary>
    BLENDFUNCTION _blend = new BLENDFUNCTION
    {
        BlendOp = (byte)AC_SRC_OVER,
        BlendFlags = 0,
        SourceConstantAlpha = 255,
        AlphaFormat = (byte)AC_SRC_ALPHA
    };

    /// <summary>
    /// Gets or sets the Direct2D shared bitmap for popup rendering.
    /// </summary>
    private ID2D1Bitmap? D2D1PopupSharedBitmap { get; set; }

    /// <summary>
    /// Gets or sets the Direct2D shared bitmap for view rendering.
    /// </summary>
    private ID2D1Bitmap? D2D1ViewSharedBitmap { get; set; }
    /// <summary>
    /// Gets or sets the cached GDI bitmap for rendering.
    /// </summary>
    Bitmap? CachedBitmap { get; set; }

    /// <summary>
    /// Gets or sets the handle to the GDI bitmap.
    /// </summary>
    HGDIOBJ HBitmap { get; set; }

    /// <summary>
    /// Gets or sets the handle to the old GDI bitmap.
    /// </summary>
    HGDIOBJ HOldBitmap { get; set; }

    ///// <summary>
    ///// Gets or sets the screen device context.
    ///// </summary>
    //HDC ScreenDC { get; set; }

    /// <summary>
    /// Gets or sets the memory device context.
    /// </summary>
    HDC MemDC { get; set; }

    /// <summary>
    /// Gets a value indicating whether the render target is available.
    /// </summary>
    private bool IsRenderTargetAvailable => _dcRenderTarget is not null;

    /// <summary>
    /// Initializes the Direct2D device, device context, and render target.
    /// </summary>
    private void InitD2D1Device()
    {
        var screenDC = GetDC(HWND.Null);
        MemDC = CreateCompatibleDC(screenDC);

        D2D1.D2D1CreateFactory(out _d2d1Factory).CheckError();

        _dcRenderTarget = D2D1Factory.CreateDCRenderTarget(new RenderTargetProperties
        {
            Type = RenderTargetType.Default,
            PixelFormat = new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied),
            MinLevel = FeatureLevel.Default,
        });
        DeleteDC(screenDC);
        ReleaseDC(HWND.Null, screenDC);


        CreateRenderDC();
    }
    /// <summary>
    /// Creates the Direct2D render target and binds the device context.
    /// </summary>
    private void CreateRenderDC()
    {
        if (!IsRenderTargetAvailable)
        {
            _dcRenderTarget = D2D1Factory.CreateDCRenderTarget(new RenderTargetProperties
            {
                Type = RenderTargetType.Default,
                PixelFormat = new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied),
                MinLevel = FeatureLevel.Default,
                Usage = RenderTargetUsage.GdiCompatible
            });
        }

        var width = HostWindow.ClientRectangle.Width;
        var height = HostWindow.ClientRectangle.Height;

        BindDC(width, height);
    }

    /// <summary>
    /// Resizes the Direct2D render target and rebinds the device context.
    /// </summary>
    private void ResizeRenderDC()
    {
        if (!IsRenderTargetAvailable) return;

        var width = HostWindow.ClientRectangle.Width;
        var height = HostWindow.ClientRectangle.Height;

        if (HostWindow.WindowState == FormWindowState.Maximized)
        {
            width = HostWindow.Width;
            height = HostWindow.Height;
        }

        if (width <= 0 || height <= 0)
        {
            return;
        }

        RestoreMemDC();

        BindDC(width, height);
    }

    /// <summary>
    /// Discards the current render target and releases associated resources.
    /// </summary>
    private void DiscardRenderDC()
    {
        RestoreMemDC();

        _dcRenderTarget?.Dispose();
    }

    /// <summary>
    /// Binds the memory device context to the Direct2D render target.
    /// </summary>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    private void BindDC(int width, int height)
    {
        if (_dcRenderTarget == null)
        {
            CreateRenderDC();
        }

        CachedBitmap = new Bitmap(width, height);

        HBitmap = (HGDIOBJ)CachedBitmap.GetHbitmap();

        HOldBitmap = SelectObject(MemDC, HBitmap);

        DCRenderTarget.BindDC(MemDC, new Vortice.RawRect(0, 0, width, height));
    }

    /// <summary>
    /// Restores the memory device context and releases GDI and Direct2D resources.
    /// </summary>
    private void RestoreMemDC()
    {
        if (!HOldBitmap.IsNull)
        {
            SelectObject(MemDC, HOldBitmap);
        }

        if (CachedBitmap is not null)
        {
            DeleteObject(HBitmap);
            CachedBitmap.Dispose();
            HBitmap = default;
            CachedBitmap = null;
        }

    }
}

