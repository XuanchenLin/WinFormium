// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_touch_event_t
{
    public int id;
    public float x;
    public float y;
    public float radius_x;
    public float radius_y;
    public float rotation_angle;
    public float pressure;
    public CefTouchEventType type;
    public CefEventFlags modifiers;
    public CefPointerType pointer_type;

#if __DISABLED__
    #region Alloc & Free
    private static int _sizeof;

    static cef_touch_event_t()
    {
        _sizeof = Marshal.SizeOf(typeof(cef_touch_event_t));
    }

    public static cef_touch_event_t* Alloc()
    {
        var ptr = (cef_touch_event_t*)Marshal.AllocHGlobal(_sizeof);
        *ptr = new cef_touch_event_t();
        ptr->size = (UIntPtr)_sizeof;
        return ptr;
    }

    public static void Free(cef_touch_event_t* ptr)
    {
        Marshal.FreeHGlobal((IntPtr)ptr);
    }
    #endregion
#endif
}
