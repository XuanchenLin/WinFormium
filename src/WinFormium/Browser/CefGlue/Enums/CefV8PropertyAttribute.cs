// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// V8 property attribute values.
/// </summary>
[Flags]
public enum CefV8PropertyAttribute
{
    /// <summary>
    /// Writeable, Enumerable, Configurable
    /// </summary>
    None = 0,

    /// <summary>
    /// Not writeable
    /// </summary>
    ReadOnly = 1 << 0,

    /// <summary>
    /// Not enumerable
    /// </summary>
    DontEnum = 1 << 1,

    /// <summary>
    /// Not configurable
    /// </summary>
    DontDelete = 1 << 2,
}
