﻿// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;
using System.Diagnostics;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Structure representing mouse event information.
/// </summary>
public unsafe struct CefMouseEvent
{
    private int _x;
    private int _y;
    private CefEventFlags _modifiers;

    public CefMouseEvent(int x, int y, CefEventFlags modifiers)
    {
        _x = x;
        _y = y;
        _modifiers = modifiers;
    }

    internal CefMouseEvent(cef_mouse_event_t* ptr)
    {
        Debug.Assert(ptr != null);

        _x = ptr->x;
        _y = ptr->y;
        _modifiers = ptr->modifiers;
    }

    internal cef_mouse_event_t ToNative()
    {
        return new cef_mouse_event_t(_x, _y, _modifiers);
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

    public CefEventFlags Modifiers
    {
        get { return _modifiers; }
        set { _modifiers = value; }
    }
}
