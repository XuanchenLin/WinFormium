// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using Vortice.Direct2D1;
using Vortice.Direct3D11;
using Vortice.DirectComposition;
using Vortice.DXGI;
using Vortice.Mathematics;

using D3D = Vortice.Direct3D;

namespace WinFormium;

/// <summary>
/// Provides offscreen rendering using DirectComposition, Direct3D 11, and Direct2D for WinFormium.
/// </summary>
internal class DirectCompositionOffscreenRender : IOffscreenRender
{
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
    /// Initializes a new instance of the <see cref="DirectCompositionOffscreenRender"/> class.
    /// </summary>
    /// <param name="osrRenderHandler">The core render handler.</param>
    public DirectCompositionOffscreenRender(FormiumOffscreenRenderHandler osrRenderHandler)
    {
        OsrRenderHandler = osrRenderHandler;
    }

    /// <summary>
    /// Gets the DirectComposition visual for rendering.
    /// </summary>
    internal IDCompositionVisual2 Visual => _visual ?? throw new InvalidOperationException("Visual is not initialized.");

    /// <summary>
    /// Supported Direct3D feature levels.
    /// </summary>
    private static readonly D3D.FeatureLevel[] FEATURE_LEVEL_SUPPORTED = [
            D3D.FeatureLevel.Level_11_1,
            D3D.FeatureLevel.Level_11_0,
            D3D.FeatureLevel.Level_10_1,
            D3D.FeatureLevel.Level_10_0,
            D3D.FeatureLevel.Level_9_3,
            D3D.FeatureLevel.Level_9_2,
            D3D.FeatureLevel.Level_9_1,
    ];

    /// <summary>
    /// The Direct3D 11 device.
    /// </summary>
    private ID3D11Device1? _d3d11Device;

    /// <summary>
    /// The DXGI factory.
    /// </summary>
    private IDXGIFactory2? _dxgiFactory;

    /// <summary>
    /// The DXGI device.
    /// </summary>
    private IDXGIDevice? _dxgiDevice;

    /// <summary>
    /// The Direct2D factory.
    /// </summary>
    private ID2D1Factory1? _d2d1Factory;

    /// <summary>
    /// The Direct2D device.
    /// </summary>
    private ID2D1Device? _d2d1Device;

    /// <summary>
    /// The Direct2D device context.
    /// </summary>
    private ID2D1DeviceContext? _d2d1DeviceContext;

    /// <summary>
    /// The DirectComposition device.
    /// </summary>
    private IDCompositionDevice3? _dcompDevice;

    /// <summary>
    /// The DXGI swap chain.
    /// </summary>
    private IDXGISwapChain1? _dxgiSwapChain;

    /// <summary>
    /// The DirectComposition target.
    /// </summary>
    private IDCompositionTarget? _compositionTarget;

    /// <summary>
    /// The DirectComposition visual.
    /// </summary>
    private IDCompositionVisual2? _visual;

    /// <summary>
    /// Creates a new Direct3D 11 device.
    /// </summary>
    /// <returns>The created Direct3D 11 device.</returns>
    private ID3D11Device CreateD3D11Device()
    {
        ID3D11Device? d3d11Device = null;

        var hr = D3D11.D3D11CreateDevice(null!, D3D.DriverType.Hardware, DeviceCreationFlags.BgraSupport, FEATURE_LEVEL_SUPPORTED, out d3d11Device);

        if (hr.Failure)
        {
            hr = D3D11.D3D11CreateDevice(null, D3D.DriverType.Warp, DeviceCreationFlags.BgraSupport, FEATURE_LEVEL_SUPPORTED, out d3d11Device);
        }

        if (hr.Failure || d3d11Device is null)
        {
            hr.CheckError();
        }

        return d3d11Device!;
    }

    /// <summary>
    /// Discards and disposes the swap chain and related resources.
    /// </summary>
    private void DiscardSwapChain()
    {
        BackBufferBitmap?.Dispose();
        BackBufferBitmap = null;
        DXGISuface?.Dispose();
        DXGISuface = null;
        _dxgiSwapChain?.Dispose();
        _dxgiSwapChain = null;
    }

    /// <summary>
    /// Resizes the swap chain and updates Direct2D resources for the new size.
    /// </summary>
    private void ResizeSwapChain()
    {
        BackBufferBitmap?.Dispose();
        BackBufferBitmap = null;
        DXGISuface?.Dispose();
        DXGISuface = null;

        D2D1DeviceContext.Target = null;

        var hr = DXGISwapChain.ResizeBuffers(2, (uint)HostWindow.ClientRectangle.Width, (uint)HostWindow.ClientRectangle.Height, Format.B8G8R8A8_UNorm, SwapChainFlags.None);

        if (hr.Failure)
        {
            //hr.CheckError();
            return;
        }

        DXGISuface = DXGISwapChain.GetBuffer<IDXGISurface1>(0);
        BackBufferBitmap = D2D1DeviceContext.CreateBitmapFromDxgiSurface(DXGISuface, new BitmapProperties1
        {
            PixelFormat = new Vortice.DCommon.PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied),
            BitmapOptions = BitmapOptions.CannotDraw | BitmapOptions.Target,
            DpiX = DeviceDpi,
            DpiY = DeviceDpi,
        });

        D2D1DeviceContext.Target = BackBufferBitmap;
    }

    /// <summary>
    /// Creates the DXGI swap chain and associated Direct2D resources.
    /// </summary>
    private void CreateSwapChain()
    {
        _dxgiSwapChain = DXGIFactory.CreateSwapChainForComposition(DXGIDevice, new SwapChainDescription1
        {
            Format = Format.B8G8R8A8_UNorm,
            BufferCount = 2,
            SwapEffect = SwapEffect.FlipSequential,
            //Stereo = true,
            Scaling = Scaling.Stretch,
            AlphaMode = AlphaMode.Premultiplied,
            BufferUsage = Usage.RenderTargetOutput,
            SampleDescription = new SampleDescription(1, 0),
            Width = (uint)HostWindow.ClientRectangle.Width,
            Height = (uint)HostWindow.ClientRectangle.Height,
            Flags = SwapChainFlags.None
        });

        DXGISuface = DXGISwapChain.GetBuffer<IDXGISurface1>(0);
        BackBufferBitmap = D2D1DeviceContext.CreateBitmapFromDxgiSurface(DXGISuface, new BitmapProperties1
        {
            PixelFormat = new Vortice.DCommon.PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied),
            BitmapOptions = BitmapOptions.CannotDraw | BitmapOptions.Target,
            DpiX = DeviceDpi,
            DpiY = DeviceDpi,
        });

        D2D1DeviceContext.Target = BackBufferBitmap;

        _visual = DCompDevice.CreateVisual();

        Visual.SetContent(DXGISwapChain);

        DCompTarget.SetRoot(Visual);
    }

    /// <summary>
    /// Gets the Direct3D 11 device.
    /// </summary>
    private ID3D11Device1 D3D11Device => _d3d11Device ?? throw new InvalidOperationException("D3D11 device is not initialized.");

    /// <summary>
    /// Gets the DXGI factory.
    /// </summary>
    private IDXGIFactory2 DXGIFactory => _dxgiFactory ?? throw new InvalidOperationException("DXGI factory is not initialized.");

    /// <summary>
    /// Gets the DXGI device.
    /// </summary>
    private IDXGIDevice DXGIDevice => _dxgiDevice ?? throw new InvalidOperationException("DXGI device is not initialized.");

    /// <summary>
    /// Gets the Direct2D device context.
    /// </summary>
    private ID2D1DeviceContext D2D1DeviceContext => _d2d1DeviceContext ?? throw new InvalidOperationException("D2D1 device context is not initialized.");

    /// <summary>
    /// Gets the DirectComposition device.
    /// </summary>
    private IDCompositionDevice3 DCompDevice => _dcompDevice ?? throw new InvalidOperationException("DComp device is not initialized.");

    /// <summary>
    /// Gets the DXGI swap chain.
    /// </summary>
    private IDXGISwapChain1 DXGISwapChain => _dxgiSwapChain ?? throw new InvalidOperationException("Swap chain is not initialized.");

    /// <summary>
    /// Gets the Direct2D factory.
    /// </summary>
    private ID2D1Factory1 D2D1Factory => _d2d1Factory ?? throw new InvalidOperationException("D2D1 factory is not initialized.");

    /// <summary>
    /// Gets the Direct2D device.
    /// </summary>
    private ID2D1Device D2D1Device => _d2d1Device ?? throw new InvalidOperationException("D2D1 device is not initialized.");

    /// <summary>
    /// Gets or sets the DXGI surface.
    /// </summary>
    private IDXGISurface1? DXGISuface { get; set; }

    /// <summary>
    /// Gets or sets the Direct2D back buffer bitmap.
    /// </summary>
    private ID2D1Bitmap1? BackBufferBitmap { get; set; }

    /// <summary>
    /// Gets a value indicating whether the swap chain is available.
    /// </summary>
    private bool IsSwapChainAvailable => _dxgiSwapChain != null;

    /// <summary>
    /// Gets the DirectComposition target.
    /// </summary>
    private IDCompositionTarget DCompTarget => _compositionTarget ?? throw new InvalidOperationException("Composition target is not initialized.");

    /// <summary>
    /// Gets or sets the Direct2D shared bitmap for popup rendering.
    /// </summary>
    private ID2D1Bitmap? D2D1PopupSharedBitmap { get; set; }

    /// <summary>
    /// Gets or sets the Direct2D shared bitmap for view rendering.
    /// </summary>
    private ID2D1Bitmap? D2D1ViewSharedBitmap { get; set; }

    /// <summary>
    /// Gets the core render handler.
    /// </summary>
    public FormiumOffscreenRenderHandler OsrRenderHandler { get; }

    /// <summary>
    /// Initializes the Direct3D 11 device and related resources.
    /// </summary>
    private void InitD3D11Device()
    {
        _dxgiFactory = DXGI.CreateDXGIFactory1<IDXGIFactory2>();

        _dxgiFactory.MakeWindowAssociation(Handle, WindowAssociationFlags.IgnoreAltEnter | WindowAssociationFlags.IgnoreAll);

        using var d3d11Device = CreateD3D11Device();

        _d3d11Device = d3d11Device.QueryInterfaceOrNull<ID3D11Device1>();

        _dxgiDevice = D3D11Device.QueryInterfaceOrNull<IDXGIDevice>();

        _d2d1Factory = D2D1.D2D1CreateFactory<ID2D1Factory1>();

        _d2d1Device = D2D1Factory.CreateDevice(DXGIDevice);

        _d2d1DeviceContext = D2D1Device.CreateDeviceContext();

        using var dcompDevice = DComp.DCompositionCreateDevice3<IDCompositionDevice>(DXGIDevice);

        _dcompDevice = dcompDevice.QueryInterface<IDCompositionDevice3>();

        dcompDevice.CreateTargetForHwnd(Handle, true, out _compositionTarget).CheckError();

        CreateSwapChain();
    }

    /// <summary>
    /// Releases all resources used by the <see cref="DirectCompositionOffscreenRender"/> instance.
    /// </summary>
    public void Dispose()
    {
        DiscardSwapChain();

        DCompTarget.Dispose();
        DCompDevice.Dispose();
        D2D1Device.Dispose();
        D2D1DeviceContext.Dispose();
        D2D1DeviceContext.Dispose();
        DXGIDevice.Dispose();
        DXGIFactory.Dispose();
        D3D11Device.Dispose();
    }

    /// <summary>
    /// Initializes the device context and related resources.
    /// </summary>
    public void InitializeDeviceContext()
    {
        InitD3D11Device();
    }

    /// <summary>
    /// Notifies the renderer that the host window has been resized.
    /// </summary>
    public void NotifyResize()
    {
        ResizeSwapChain();
    }

    /// <summary>
    /// Handles paint events for the browser or popup, updating the Direct2D bitmaps and presenting the swap chain.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="type">The paint element type.</param>
    /// <param name="dirtyRects">The dirty rectangles.</param>
    /// <param name="buffer">Pointer to the pixel buffer.</param>
    /// <param name="width">Width of the buffer.</param>
    /// <param name="height">Height of the buffer.</param>
    public void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, nint buffer, int width, int height)
    {
        if (width <= 0 || height <= 0) return;

        if (type == CefPaintElementType.View)
        {
            var dx = Math.Abs(HostWindow.ClientSize.Width - width);
            var dy = Math.Abs(HostWindow.ClientSize.Height - height);

            if (dx > 1 || dy > 1)
            {
                return;
            }
        }

        if (!IsSwapChainAvailable)
        {
            CreateSwapChain();
        }

        var dc = D2D1DeviceContext;

        if (type == CefPaintElementType.View)
        {
            using var bmp = dc.CreateBitmap(new SizeI(width, height), buffer, (uint)width * 4, new BitmapProperties1
            {
                DpiX = HostWindow.DeviceDpi,
                DpiY = HostWindow.DeviceDpi,
                PixelFormat = new Vortice.DCommon.PixelFormat
                {
                    AlphaMode = Vortice.DCommon.AlphaMode.Premultiplied,
                    Format = Format.B8G8R8A8_UNorm
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
            using var bmp = dc.CreateBitmap(new SizeI(width, height), buffer, (uint)width * 4, new BitmapProperties1
            {
                PixelFormat = new Vortice.DCommon.PixelFormat
                {
                    AlphaMode = Vortice.DCommon.AlphaMode.Premultiplied,
                    Format = Format.B8G8R8A8_UNorm
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

            var targetRect = new Rect(0, 0, rect.Width, rect.Height);
            var sourceRect = new Rect(0, 0, D2D1ViewSharedBitmap.Size.Width, D2D1ViewSharedBitmap.Size.Height);

            dc.DrawBitmap(D2D1ViewSharedBitmap, targetRect, 1.0f, Vortice.Direct2D1.BitmapInterpolationMode.Linear, sourceRect);
        }

        if (IsPopupShow && D2D1PopupSharedBitmap is not null && ShownPopupRect.HasValue)
        {
            var rect = ShownPopupRect.Value;

            dc.DrawBitmap(D2D1PopupSharedBitmap, new Rect(rect.X, rect.Y, rect.Width, rect.Height), 1.0f, Vortice.Direct2D1.BitmapInterpolationMode.Linear, new Rect(0, 0, rect.Width, rect.Height));
        }

        dc.EndDraw();

        if (DXGISwapChain.Present(1, PresentFlags.None).Success)
        {
            DCompDevice.Commit();
        }
        else
        {
            DiscardSwapChain();
        }
    }
}