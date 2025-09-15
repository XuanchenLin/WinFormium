// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
#if !DEBUG
[SuppressUnmanagedCodeSecurity]
#endif
internal static unsafe partial class libcef
{
    internal const string DllName = "libcef";

    internal const int ALIGN = 0;

    internal const CallingConvention CEF_CALL = CallingConvention.Cdecl;

    // Windows: CallingConvention.StdCall 
    //    Unix: CallingConvention.Cdecl
    // FIXME: CEF#598 (http://code.google.com/p/chromiumembedded/issues/detail?id=598)
    internal const CallingConvention CEF_CALLBACK = CallingConvention.Winapi;

    #region cef_version.h

    [DllImport(libcef.DllName, EntryPoint = "cef_build_revision", CallingConvention = libcef.CEF_CALL)]
    public static extern int build_revision();

    [DllImport(libcef.DllName, EntryPoint = "cef_version_info", CallingConvention = libcef.CEF_CALL)]
    public static extern int version_info(int entry);

    [DllImport(libcef.DllName, EntryPoint = "cef_api_hash", CallingConvention = libcef.CEF_CALL)]
    public static extern sbyte* api_hash(int entry);

    #endregion
}
