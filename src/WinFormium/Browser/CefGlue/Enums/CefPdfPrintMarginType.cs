// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Margin type for PDF printing.
/// </summary>
public enum CefPdfPrintMarginType
{
    /// <summary>
    /// Default margins of 1cm (~0.4 inches).
    /// </summary>
    Default,

    /// <summary>
    /// No margins.
    /// </summary>
    None,

    /// <summary>
    /// Custom margins using the |margin_*| values from cef_pdf_print_settings_t.
    /// </summary>
    Custom,
}
