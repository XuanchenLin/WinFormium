// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Supported file dialog modes.
/// </summary>
[Flags]
public enum CefFileDialogMode
{
    /// <summary>
    /// Requires that the file exists before allowing the user to pick it.
    /// </summary>
    Open = 0,

    /// <summary>
    /// Like Open, but allows picking multiple files to open.
    /// </summary>
    OpenMultiple,

    /// <summary>
    /// Like Open, but selects a folder to open.
    /// </summary>
    OpenFolder,

    /// <summary>
    /// Allows picking a nonexistent file, and prompts to overwrite if the file
    /// already exists.
    /// </summary>
    Save,
}
