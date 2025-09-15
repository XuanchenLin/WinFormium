// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Supported context menu edit state bit flags. These constants match their
/// equivalents in Chromium's ContextMenuDataEditFlags and should not be
/// renumbered.
/// </summary>
[Flags]
public enum CefContextMenuEditStateFlags
{
    None = 0,
    CanUndo = 1 << 0,
    CanRedo = 1 << 1,
    CanCut = 1 << 2,
    CanCopy = 1 << 3,
    CanPaste = 1 << 4,
    CanDelete = 1 << 5,
    CanSelectAll = 1 << 6,
    CanTranslate = 1 << 7,
    CanEditRichly = 1 << 8,
}
