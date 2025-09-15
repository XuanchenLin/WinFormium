// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Represents the state of a setting.
/// </summary>
public enum CefState : int
{
    /// <summary>
    /// Use the default state for the setting.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Enable or allow the setting.
    /// </summary>
    Enabled,

    /// <summary>
    /// Disable or disallow the setting.
    /// </summary>
    Disabled,
}
