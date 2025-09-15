// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// The device type that caused the event.
/// </summary>
public enum CefPointerType
{
    Touch = 0,
    Mouse,
    Pen,
    Eraser,
    Unknown,
}
