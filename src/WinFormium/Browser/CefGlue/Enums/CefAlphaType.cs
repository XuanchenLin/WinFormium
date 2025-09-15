// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Describes how to interpret the alpha component of a pixel.
/// </summary>
public enum CefAlphaType
{
    /// <summary>
    /// No transparency. The alpha component is ignored.
    /// </summary>
    Opaque,

    /// <summary>
    /// Transparency with pre-multiplied alpha component.
    /// </summary>
    Premultiplied,

    /// <summary>
    /// Transparency with post-multiplied alpha component.
    /// </summary>
    Postmultiplied,
}
