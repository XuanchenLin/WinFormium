// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Input mode of a virtual keyboard. These constants match their equivalents
/// in Chromium's text_input_mode.h and should not be renumbered.
/// See https://html.spec.whatwg.org/#input-modalities:-the-inputmode-attribute
/// </summary>
public enum CefTextInputMode
{
    Default,
    None,
    Text,
    Tel,
    Url,
    Email,
    Numeric,
    Decimal,
    Search,

    Max = Search,
}
