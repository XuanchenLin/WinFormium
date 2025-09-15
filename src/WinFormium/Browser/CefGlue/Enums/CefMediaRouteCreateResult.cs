// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Result codes for CefMediaRouter::CreateRoute. Should be kept in sync with
/// Chromium's media_router::mojom::RouteRequestResultCode type.
/// renumbered.
/// </summary>
public enum CefMediaRouteCreateResult
{
    UnknownError = 0,
    Ok = 1,
    TimedOut = 2,
    RouteNotFound = 3,
    SinkNotFound = 4,
    InvalidOrigin = 5,
    NoSupportedProvider = 7,
    Cancelled = 8,
    RouteAlreadyExists = 9,
    RouteAlreadyTerminated = 11,
}
