// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Log severity levels.
/// </summary>
public enum CefLogSeverity
{
    /// <summary>
    /// Default logging (currently INFO logging).
    /// </summary>
    Default,

    /// <summary>
    /// Verbose logging.
    /// </summary>
    Verbose,

    /// <summary>
    /// DEBUG logging.
    /// </summary>
    Debug = Verbose,

    /// <summary>
    /// INFO logging.
    /// </summary>
    Info,

    /// <summary>
    /// WARNING logging.
    /// </summary>
    Warning,

    /// <summary>
    /// ERROR logging.
    /// </summary>
    Error,

    /// <summary>
    /// FATAL logging.
    /// </summary>
    Fatal,

    /// <summary>
    /// Disable logging to file for all messages, and to stderr for messages with
    /// severity less than FATAL.
    /// </summary>
    Disable = 99,
}
