// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Process termination status values.
/// </summary>
public enum CefTerminationStatus
{
    /// <summary>
    /// Non-zero exit status.
    /// </summary>
    Termination,

    /// <summary>
    /// SIGKILL or task manager kill.
    /// </summary>
    WasKilled,

    /// <summary>
    /// Segmentation fault.
    /// </summary>
    ProcessCrashed,

    /// <summary>
    /// Out of memory. Some platforms may use TS_PROCESS_CRASHED instead.
    /// </summary>
    OutOfMemory,
}
