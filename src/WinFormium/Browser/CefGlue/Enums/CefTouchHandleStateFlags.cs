// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Values indicating what state of the touch handle is set.
/// </summary>
[Flags]
public enum CefTouchHandleStateFlags : uint
{
    None = 0,
    Enabled = 1 << 0,
    Orientation = 1 << 1,
    Origin = 1 << 2,
    Alpha = 1 << 3,
}
