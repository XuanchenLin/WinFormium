// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Provides a mechanism for handling Chrome command events in the browser.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Called when a Chrome command is triggered in the browser.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance where the command was invoked.</param>
    /// <param name="commandId">The identifier of the Chrome command.</param>
    /// <param name="disposition">The window disposition for the command.</param>
    /// <returns>
    /// <c>true</c> if the command was handled and no further processing should occur; 
    /// otherwise, <c>false</c> to allow default handling.
    /// </returns>
    bool OnChromeCommand(CefBrowser browser, int commandId, CefWindowOpenDisposition disposition);
}
