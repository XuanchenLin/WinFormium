// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
internal static unsafe partial class libcef
{
    // CefSetOSModalLoop
    [DllImport(DllName, EntryPoint = "cef_set_osmodal_loop", CallingConvention = libcef.CEF_CALL)]
    public static extern void set_osmodal_loop(int osModalLoop);

    // CefEnableHighDPISupport
    [DllImport(DllName, EntryPoint = "cef_enable_highdpi_support", CallingConvention = libcef.CEF_CALL)]
    public static extern void enable_highdpi_support();
}
