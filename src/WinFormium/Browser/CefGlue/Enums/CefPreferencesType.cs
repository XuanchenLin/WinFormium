// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Preferences type passed to
/// CefBrowserProcessHandler::OnRegisterCustomPreferences.
/// </summary>
public enum CefPreferencesType
{
    /// <summary>
    /// Global preferences registered a single time at application startup.
    /// </summary>
    Global,

    /// <summary>
    /// Request context preferences registered each time a new CefRequestContext
    /// is created.
    /// </summary>
    RequestContext,
}
