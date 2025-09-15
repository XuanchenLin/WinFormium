// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
public enum CefUrlRequestStatus
{
    /// <summary>
    /// Unknown status.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Request succeeded.
    /// </summary>
    Success,

    /// <summary>
    /// An IO request is pending, and the caller will be informed when it is
    /// completed.
    /// </summary>
    IOPending,

    /// <summary>
    /// Request was canceled programatically.
    /// </summary>
    Canceled,

    /// <summary>
    /// Request failed for some reason.
    /// </summary>
    Failed,
}
