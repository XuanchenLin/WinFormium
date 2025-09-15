// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling focus events in the browser.
/// </summary>
public interface IFocusHandler
{
    /// <summary>
    /// Called when the browser component is about to lose focus.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance.</param>
    /// <param name="next">If true, focus will be transferred to the next component; otherwise, to the previous component.</param>
    void OnTakeFocus(CefBrowser browser, bool next);

    /// <summary>
    /// Called when the browser component is requesting focus.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance.</param>
    /// <param name="source">The source of the focus request.</param>
    /// <returns>Return true to allow the browser to take focus, or false to cancel the request.</returns>
    bool OnSetFocus(CefBrowser browser, CefFocusSource source);

    /// <summary>
    /// Called when the browser component has received focus.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance.</param>
    void OnGotFocus(CefBrowser browser);
}
