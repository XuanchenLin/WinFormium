// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Media access permissions used by OnRequestMediaAccessPermission.
/// </summary>
[Flags]
public enum CefMediaAccessPermissionTypes
{
    /// <summary>
    /// No permission.
    /// </summary>
    None = 0,

    /// <summary>
    /// Device audio capture permission.
    /// </summary>
    DeviceAudioCapture = 1 << 0,

    /// <summary>
    /// Device video capture permission.
    /// </summary>
    DeviceVideoCapture = 1 << 1,

    /// <summary>
    /// Desktop audio capture permission.
    /// </summary>
    DesktopAudioCapture = 1 << 2,

    /// <summary>
    /// Desktop video capture permission.
    /// </summary>
    DesktopVideoCapture = 1 << 3,
}
