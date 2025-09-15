// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
internal unsafe struct cef_popup_features_t
{
    public int x;
    public int xSet;
    public int y;
    public int ySet;
    public int width;
    public int widthSet;
    public int height;
    public int heightSet;

    public int menuBarVisible;
    public int statusBarVisible;
    public int toolBarVisible;
    public int scrollbarsVisible;
}
