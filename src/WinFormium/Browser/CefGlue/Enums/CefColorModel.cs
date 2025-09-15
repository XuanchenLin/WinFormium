// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Print job color mode values.
/// </summary>
public enum CefColorModel
{
    Unknown = 0,
    Gray,
    Color,
    Cmyk,
    Cmy,
    Kcmy,
    Cmy_K,  // CMY_K represents CMY+K.
    Black,
    Grayscale,
    Rgb,
    Rgb16,
    Rgba,

    ColorMode_Color,       // Used in samsung printer ppds.
    ColorMode_Monochrome,  // Used in samsung printer ppds.

    HP_Color_Color,  // Used in HP color printer ppds.
    HP_Color_Black,  // Used in HP color printer ppds.

    PrintoutMode_Normal,       // Used in foomatic ppds.
    PrintoutMode_Normal_Gray,  // Used in foomatic ppds.

    ProcessColorModel_Cmyk,       // Used in canon printer ppds.
    ProcessColorModel_Greyscale,  // Used in canon printer ppds.
    ProcessColorModel_Rgb,        // Used in canon printer ppds
}
