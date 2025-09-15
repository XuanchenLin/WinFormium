// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling context menu and quick menu events in the browser.
/// </summary>
public interface IContextMenuHandler
{
    /// <summary>
    /// Called before a context menu is displayed. Allows modification of the menu model.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context menu is invoked.</param>
    /// <param name="state">The context menu parameters.</param>
    /// <param name="model">The menu model to be displayed.</param>
    void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model);

    /// <summary>
    /// Called to run a custom context menu. Return true if the menu is handled, false to use the default implementation.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context menu is invoked.</param>
    /// <param name="parameters">The context menu parameters.</param>
    /// <param name="model">The menu model to be displayed.</param>
    /// <param name="callback">Callback for continuing or canceling the menu.</param>
    /// <returns>True if the menu is handled, otherwise false.</returns>
    bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback);

    /// <summary>
    /// Called when a command is selected from the context menu.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context menu is invoked.</param>
    /// <param name="state">The context menu parameters.</param>
    /// <param name="commandId">The selected command ID.</param>
    /// <param name="eventFlags">Event flags for the command.</param>
    /// <returns>True if the command is handled, otherwise false.</returns>
    bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags);

    /// <summary>
    /// Called when the context menu is dismissed.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the context menu was invoked.</param>
    void OnContextMenuDismissed(CefBrowser browser, CefFrame frame);

    /// <summary>
    /// Called to run a custom quick menu. Return true if the menu is handled, false to use the default implementation.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the quick menu is invoked.</param>
    /// <param name="location">The location of the quick menu.</param>
    /// <param name="size">The size of the quick menu.</param>
    /// <param name="editStateFlags">Edit state flags for the quick menu.</param>
    /// <param name="callback">Callback for continuing the quick menu.</param>
    /// <returns>True if the menu is handled, otherwise false.</returns>
    bool RunQuickMenu(CefBrowser browser, CefFrame frame, CefPoint location, CefSize size, CefQuickMenuEditStateFlags editStateFlags, CefRunQuickMenuCallback callback);

    /// <summary>
    /// Called when a command is selected from the quick menu.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the quick menu is invoked.</param>
    /// <param name="commandId">The selected command ID.</param>
    /// <param name="eventFlags">Event flags for the command.</param>
    /// <returns>True if the command is handled, otherwise false.</returns>
    bool OnQuickMenuCommand(CefBrowser browser, CefFrame frame, int commandId, CefEventFlags eventFlags);

    /// <summary>
    /// Called when the quick menu is dismissed.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame where the quick menu was invoked.</param>
    void OnQuickMenuDismissed(CefBrowser browser, CefFrame frame);
}
