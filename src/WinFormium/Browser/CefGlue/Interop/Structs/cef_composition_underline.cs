// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal struct cef_composition_underline_t
{
    public cef_range_t range;
    public uint color;
    public uint background_color;
    public int thick;
    public CefCompositionUnderlineStyle style;
}
