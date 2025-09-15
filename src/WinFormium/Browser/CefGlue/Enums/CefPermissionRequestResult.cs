// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Permission request results.
/// </summary>
public enum CefPermissionRequestResult
{
    /// <summary>
    /// Accept the permission request as an explicit user action.
    /// </summary>
    Accept,

    /// <summary>
    /// Deny the permission request as an explicit user action.
    /// </summary>
    Deny,

    /// <summary>
    /// Dismiss the permission request as an explicit user action.
    /// </summary>
    Dismiss,

    /// <summary>
    /// Ignore the permission request. If the prompt remains unhandled (e.g.
    /// OnShowPermissionPrompt returns false and there is no default permissions
    /// UI) then any related promises may remain unresolved.
    /// </summary>
    Ignore,
}
