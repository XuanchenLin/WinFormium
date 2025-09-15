// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_touch_handle_state_t
{
    public int touch_handle_id;
    public CefTouchHandleStateFlags flags;
    public int enabled;
    public CefHorizontalAlignment orientation;
    public int mirror_vertical;
    public int mirror_horizontal;
    public cef_point_t origin;
    public float alpha;
}
