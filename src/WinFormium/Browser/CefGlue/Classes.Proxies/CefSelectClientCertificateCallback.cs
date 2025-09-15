// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface used to select a client certificate for authentication.
/// </summary>
public sealed unsafe partial class CefSelectClientCertificateCallback
{
    /// <summary>
    /// Chooses the specified certificate for client certificate authentication.
    /// NULL value means that no client certificate should be used.
    /// </summary>
    public void Select(CefX509Certificate cert)
    {
        cef_select_client_certificate_callback_t.select(_self, cert != null ? cert.ToNative() : null);
    }
}
