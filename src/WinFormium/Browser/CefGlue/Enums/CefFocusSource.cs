// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Focus sources.
/// </summary>
public enum CefFocusSource
{
    /// <summary>
    /// The source is explicit navigation via the API (LoadURL(), etc).
    /// </summary>
    Navigation = 0,

    /// <summary>
    /// The source is a system-generated focus event.
    /// </summary>
    System,
}
