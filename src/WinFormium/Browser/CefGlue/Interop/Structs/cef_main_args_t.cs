// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
internal struct cef_main_args_t
{
}

[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_main_args_t_windows
{
    public IntPtr instance;

    #region Alloc & Free
    private static int _sizeof;

    static cef_main_args_t_windows()
    {
        _sizeof = Marshal.SizeOf(typeof(cef_main_args_t_windows));
    }

    public static cef_main_args_t_windows* Alloc()
    {
        var ptr = (cef_main_args_t_windows*)Marshal.AllocHGlobal(_sizeof);
        *ptr = new cef_main_args_t_windows();
        return ptr;
    }

    public static void Free(cef_main_args_t_windows* ptr)
    {
        Marshal.FreeHGlobal((IntPtr)ptr);
    }
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_main_args_t_posix
{
    public int argc;
    public IntPtr argv;

    #region Alloc & Free
    private static int _sizeof;

    static cef_main_args_t_posix()
    {
        _sizeof = Marshal.SizeOf(typeof(cef_main_args_t_posix));
    }

    public static cef_main_args_t_posix* Alloc()
    {
        var ptr = (cef_main_args_t_posix*)Marshal.AllocHGlobal(_sizeof);
        *ptr = new cef_main_args_t_posix();
        return ptr;
    }

    public static void Free(cef_main_args_t_posix* ptr)
    {
        Marshal.FreeHGlobal((IntPtr)ptr);
    }
    #endregion
}
