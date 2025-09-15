// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Describes how to interpret the components of a pixel.
/// </summary>
public enum CefColorType
{
    /// <summary>
    /// RGBA with 8 bits per pixel (32bits total).
    /// </summary>
    Rgba8888,

    /// <summary>
    /// BGRA with 8 bits per pixel (32bits total).
    /// </summary>
    Bgra8888,
}
