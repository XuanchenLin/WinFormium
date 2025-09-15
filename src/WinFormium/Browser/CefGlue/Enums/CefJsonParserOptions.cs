// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Options that can be passed to CefParseJSON.
/// </summary>
[Flags]
public enum CefJsonParserOptions
{
    /// <summary>
    /// Parses the input strictly according to RFC 4627. See comments in
    /// Chromium's base/json/json_reader.h file for known limitations/
    /// deviations from the RFC.
    /// </summary>
    Rfc = 0,

    /// <summary>
    /// Allows commas to exist after the last element in structures.
    /// </summary>
    AllowTrailingCommas = 1 << 0,
}
