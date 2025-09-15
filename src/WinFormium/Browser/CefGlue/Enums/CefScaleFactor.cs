// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Supported UI scale factors for the platform. SCALE_FACTOR_NONE is used for
/// density independent resources such as string, html/js files or an image that
/// can be used for any scale factors (such as wallpapers).
/// </summary>
public enum CefScaleFactor : int
{
    None = 0,
    P100,
    P125,
    P133,
    P140,
    P150,
    P180,
    P200,
    P250,
    P300,
}
