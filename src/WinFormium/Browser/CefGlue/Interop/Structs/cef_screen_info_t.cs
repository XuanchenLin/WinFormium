// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_screen_info_t
{
    public float device_scale_factor;
    public int depth;
    public int depth_per_component;
    public int is_monochrome;
    public cef_rect_t rect;
    public cef_rect_t available_rect;
}
