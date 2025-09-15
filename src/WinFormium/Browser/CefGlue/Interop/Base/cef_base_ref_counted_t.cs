// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_base_ref_counted_t
{
    internal UIntPtr _size;
    internal IntPtr _add_ref;
    internal IntPtr _release;
    internal IntPtr _has_one_ref;
    internal IntPtr _has_at_least_one_ref;

    [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
#if !DEBUG
    [SuppressUnmanagedCodeSecurity]
#endif
    public delegate void add_ref_delegate(cef_base_ref_counted_t* self);

    [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
#if !DEBUG
    [SuppressUnmanagedCodeSecurity]
#endif
    public delegate int release_delegate(cef_base_ref_counted_t* self);

    [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
#if !DEBUG
    [SuppressUnmanagedCodeSecurity]
#endif
    public delegate int has_one_ref_delegate(cef_base_ref_counted_t* self);

    [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
#if !DEBUG
    [SuppressUnmanagedCodeSecurity]
#endif
    public delegate int has_at_least_one_ref_delegate(cef_base_ref_counted_t* self);
}
