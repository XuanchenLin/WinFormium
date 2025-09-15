// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_insets_t
{
    public int top;
    public int left;
    public int bottom;
    public int right;

    public cef_insets_t(int top, int left, int bottom, int right)
    {
        this.top = top;
        this.left = left;
        this.bottom = bottom;
        this.right = right;
    }
}
