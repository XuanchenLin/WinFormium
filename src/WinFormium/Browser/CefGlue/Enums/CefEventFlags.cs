// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Supported event bit flags.
/// </summary>
[Flags]
public enum CefEventFlags : uint
{
    None              = 0,

    CapsLockOn        = 1 << 0,

    ShiftDown         = 1 << 1,
    ControlDown       = 1 << 2,
    AltDown           = 1 << 3,

    LeftMouseButton   = 1 << 4,
    MiddleMouseButton = 1 << 5,
    RightMouseButton  = 1 << 6,

    /// <summary>
    /// Mac OS-X command key.
    /// </summary>
    CommandDown       = 1 << 7,

    NumLockOn         = 1 << 8,
    IsKeyPad          = 1 << 9,
    IsLeft            = 1 << 10,
    IsRight           = 1 << 11,
    AltGrDown         = 1 << 12,

    IsRepeat          = 1 << 13,
}
