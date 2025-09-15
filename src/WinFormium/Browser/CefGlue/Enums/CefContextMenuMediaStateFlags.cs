// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Supported context menu media state bit flags. These constants match their
/// equivalents in Chromium's ContextMenuData::MediaFlags and should not be
/// renumbered.
/// </summary>
[Flags]
public enum CefContextMenuMediaStateFlags
{
    None = 0,
    InError = 1 << 0,
    Paused = 1 << 1,
    Muted = 1 << 2,
    Loop = 1 << 3,
    CanSave = 1 << 4,
    HasAudio = 1 << 5,
    CanToggleControls = 1 << 6,
    Controls = 1 << 7,
    CanPrint = 1 << 8,
    CanRotate = 1 << 9,
    CanPictureInPicture = 1 << 10,
    PictureInPicture = 1 << 11,
    CanLoop = 1 << 12,
}
