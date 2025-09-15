// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_mouse_event_t
{
    public int x;
    public int y;
    public CefEventFlags modifiers;

    public cef_mouse_event_t(int x, int y, CefEventFlags modifiers)
    {
        this.x = x;
        this.y = y;
        this.modifiers = modifiers;
    }
}
