// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Specifies where along the cross axis the CefBoxLayout child views should be
/// laid out.
/// </summary>
public enum CefCrossAxisAlignment
{
    /// <summary>
    /// Child views will be stretched to fit.
    /// </summary>
    Stretch,

    /// <summary>
    /// Child views will be left-aligned.
    /// </summary>
    Start,

    /// <summary>
    /// Child views will be center-aligned.
    /// </summary>
    Center,

    /// <summary>
    /// Child views will be right-aligned.
    /// </summary>
    End,
}
