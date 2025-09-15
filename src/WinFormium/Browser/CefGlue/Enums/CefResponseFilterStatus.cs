// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Return values for CefResponseFilter::Filter().
/// </summary>
public enum CefResponseFilterStatus
{
    /// <summary>
    /// Some or all of the pre-filter data was read successfully but more data is
    /// needed in order to continue filtering (filtered output is pending).
    /// </summary>
    NeedMoreData,

    /// <summary>
    /// Some or all of the pre-filter data was read successfully and all available
    /// filtered output has been written.
    /// </summary>
    Done,

    /// <summary>
    /// An error occurred during filtering.
    /// </summary>
    Error,
}
