// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Implement this interface to receive string values asynchronously.
/// </summary>
public abstract unsafe partial class CefStringVisitor
{
    private void visit(cef_string_visitor_t* self, cef_string_t* @string)
    {
        CheckSelf(self);

        Visit(cef_string_t.ToString(@string));
    }

    /// <summary>
    /// Method that will be executed.
    /// </summary>
    protected abstract void Visit(string value);

}
