// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Supported context menu media types. These constants match their equivalents
/// in Chromium's ContextMenuDataMediaType and should not be renumbered.
/// </summary>
public enum CefContextMenuMediaType
{
    /// <summary>
    /// No special node is in context.
    /// </summary>
    None,

    /// <summary>
    /// An image node is selected.
    /// </summary>
    Image,

    /// <summary>
    /// A video node is selected.
    /// </summary>
    Video,

    /// <summary>
    /// An audio node is selected.
    /// </summary>
    Audio,

    /// <summary>
    /// An canvas node is selected.
    /// </summary>
    Canvas,

    /// <summary>
    /// A file node is selected.
    /// </summary>
    File,

    /// <summary>
    /// A plugin node is selected.
    /// </summary>
    Plugin,
}
