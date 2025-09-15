// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Connection state for a MediaRoute object.
/// </summary>
public enum CefMediaRouteConnectionState
{
    Unknown,
    Connecting,
    Connected,
    Closed,
    Terminated,
}
