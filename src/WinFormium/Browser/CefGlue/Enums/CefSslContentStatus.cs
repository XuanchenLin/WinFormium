// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Supported SSL content status flags. See content/public/common/ssl_status.h
/// for more information.
/// </summary>
[Flags]
public enum CefSslContentStatus
{
    Normal = 0,
    DisplayedInsecure = 1 << 0,
    RanInsecure = 1 << 1,
}
