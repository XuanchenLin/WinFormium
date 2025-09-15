// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Return value types.
/// </summary>
public enum CefReturnValue
{
    /// <summary>
    /// Cancel immediately.
    /// </summary>
    Cancel = 0,

    /// <summary>
    /// Continue immediately.
    /// </summary>
    Continue,

    /// <summary>
    /// Continue asynchronously (usually via a callback).
    /// </summary>
    ContinueAsync,
}
