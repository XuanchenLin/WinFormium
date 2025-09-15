// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Permission types used with OnShowPermissionPrompt. Some types are
/// platform-specific or only supported with the Chrome runtime. Should be kept
/// in sync with Chromium's permissions::RequestType type.
/// </summary>
[Flags]
public enum CefPermissionRequestTypes
{
    None = 0,
    AccessibilityEvents = 1 << 0,
    ArSession = 1 << 1,
    CameraPanTiltZoom = 1 << 2,
    CameraStream = 1 << 3,
    Clipboard = 1 << 4,
    DiskQuota = 1 << 5,
    LocalFonts = 1 << 6,
    Geolocation = 1 << 7,
    IdleDetection = 1 << 8,
    MicStream = 1 << 9,
    MidiSysex = 1 << 10,
    MultipleDownloads = 1 << 11,
    Notifications = 1 << 12,
    ProtectedMediaIdentifier = 1 << 13,
    RegisterProtocolHandler = 1 << 14,
    SecurityAttestation = 1 << 15,
    StorageAccess = 1 << 16,
    U2fApiRequest = 1 << 17,
    VrSession = 1 << 18,
    WindowManagement = 1 << 19,
}
