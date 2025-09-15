// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Class representing SSL information.
/// </summary>
public sealed unsafe partial class CefSslInfo
{
    /// <summary>
    /// Returns a bitmask containing any and all problems verifying the server
    /// certificate.
    /// </summary>
    public CefCertStatus CertStatus
    {
        get { return cef_sslinfo_t.get_cert_status(_self); }
    }

    /// <summary>
    /// Returns the X.509 certificate.
    /// </summary>
    public CefX509Certificate GetX509Certificate()
    {
        return CefX509Certificate.FromNative(
            cef_sslinfo_t.get_x509certificate(_self)
            );
    }
}
