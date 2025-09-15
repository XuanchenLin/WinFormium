// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
public struct CefRange
{
    private int _from;
    private int _to;

    public CefRange(int from, int to)
    {
        _from = from;
        _to = to;
    }

    public int From
    {
        get { return _from; }
        set { _from = value; }
    }

    public int To
    {
        get { return _to; }
        set { _to = value; }
    }
}
