// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
public struct CefRectangle
{
    private int _x;
    private int _y;
    private int _width;
    private int _height;

    public CefRectangle(int x, int y, int width, int height)
    {
        _x = x;
        _y = y;
        _width = width;
        _height = height;
    }

    internal CefRectangle(cef_rect_t rect)
    {
        _x = rect.x;
        _y = rect.y;
        _width = rect.width;
        _height = rect.height;
    }

    public int X
    {
        get { return _x; }
        set { _x = value; }
    }

    public int Y
    {
        get { return _y; }
        set { _y = value; }
    }

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }

    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }

    internal cef_rect_t AsNative()
        => new cef_rect_t(_x, _y, _width, _height);
}
