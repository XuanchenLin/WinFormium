// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Supported SSL version values. See net/ssl/ssl_connection_status_flags.h
/// for more information.
/// </summary>
public enum CefSslVersion
{
    /// <summary>
    /// Unknown SSL version.
    /// </summary>
    Unknown = 0,
    Ssl2 = 1,
    Ssl3 = 2,
    Tls1 = 3,
    Tls1_1 = 4,
    Tls1_2 = 5,
    Tls1_3 = 6,
    Quic = 7,
}
