﻿// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;
using WinFormium.Browser.CefGlue.Platform.Windows;

namespace WinFormium.Browser.CefGlue.Platform;
internal sealed unsafe class CefWindowInfoWindowsImpl : CefWindowInfo
{
    private cef_window_info_t_windows* _self;

    public CefWindowInfoWindowsImpl()
        : base(true)
    {
        _self = cef_window_info_t_windows.Alloc();
    }

    public CefWindowInfoWindowsImpl(cef_window_info_t* ptr)
        : base(false)
    {
        if (CefRuntime.Platform != CefRuntimePlatform.Windows)
            throw new InvalidOperationException();

        _self = (cef_window_info_t_windows*)ptr;
    }

    internal override cef_window_info_t* GetNativePointer()
    {
        return (cef_window_info_t*)_self;
    }

    protected internal override void DisposeNativePointer()
    {
        cef_window_info_t_windows.Free(_self);
        _self = null;
    }

    public override IntPtr ParentHandle
    {
        get { ThrowIfDisposed(); return _self->parent_window; }
        set { ThrowIfDisposed(); _self->parent_window = value; }
    }

    public override IntPtr Handle
    {
        get { ThrowIfDisposed(); return _self->window; }
        set { ThrowIfDisposed(); _self->window = value; }
    }

    public override string Name
    {
        get { ThrowIfDisposed(); return cef_string_t.ToString(&_self->window_name); }
        set { ThrowIfDisposed(); cef_string_t.Copy(value, &_self->window_name); }
    }

    public override CefRectangle Bounds
    {
        get
        {
            ThrowIfDisposed();
            return new CefRectangle(_self->bounds);
        }
        set
        {
            ThrowIfDisposed();
            _self->bounds = value.AsNative();
        }
    }

    public override WindowStyle Style
    {
        get { ThrowIfDisposed(); return (WindowStyle)_self->style; }
        set { ThrowIfDisposed(); _self->style = (uint)value; }
    }

    public override WindowStyleEx StyleEx
    {
        get { ThrowIfDisposed(); return (WindowStyleEx)_self->ex_style; }
        set { ThrowIfDisposed(); _self->ex_style = (uint)value; }
    }

    public override IntPtr MenuHandle
    {
        get { ThrowIfDisposed(); return _self->menu; }
        set { ThrowIfDisposed(); _self->menu = value; }
    }

    public override bool Hidden
    {
        get { return default(bool); }
        set { }
    }

    public override bool WindowlessRenderingEnabled
    {
        get { ThrowIfDisposed(); return _self->windowless_rendering_enabled != 0; }
        set { ThrowIfDisposed(); _self->windowless_rendering_enabled = value ? 1 : 0; }
    }

    public override bool SharedTextureEnabled
    {
        get { ThrowIfDisposed(); return _self->shared_texture_enabled != 0; }
        set { ThrowIfDisposed(); _self->shared_texture_enabled = value ? 1 : 0; }
    }

    public override bool ExternalBeginFrameEnabled
    {
        get { ThrowIfDisposed(); return _self->external_begin_frame_enabled != 0; }
        set { ThrowIfDisposed(); _self->external_begin_frame_enabled = value ? 1 : 0; }
    }
}
