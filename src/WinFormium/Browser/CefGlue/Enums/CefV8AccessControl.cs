// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// V8 access control values.
/// </summary>
[Flags]
public enum CefV8AccessControl
{
    Default = 0,
    AllCanRead = 1,
    AllCanWrite = 1 << 1,
    ProhibitsOverwriting = 1 << 2,
}
