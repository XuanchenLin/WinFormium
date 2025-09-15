// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
public class CefRuntimeException : Exception
{
    public CefRuntimeException(string message)
        : base(message)
    {
    }
}
