// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Supported quick menu state bit flags.
/// </summary>
[Flags]
public enum CefQuickMenuEditStateFlags
{
    None = 0,
    CanEllipsis = 1 << 0,
    CanCut = 1 << 1,
    CanCopy = 1 << 2,
    CanPaste = 1 << 3,
}
