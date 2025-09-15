// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// "Verb" of a drag-and-drop operation as negotiated between the source and
/// destination. These constants match their equivalents in WebCore's
/// DragActions.h and should not be renumbered.
/// </summary>
public enum CefDragOperationsMask : uint
{
    None = 0,
    Copy = 1,
    Link = 2,
    Generic = 4,
    Private = 8,
    Move = 16,
    Delete = 32,
    Every = UInt32.MaxValue,
}
