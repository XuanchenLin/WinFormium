// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling JavaScript dialogs in the browser.
/// </summary>
public interface IJSDialogHandler
{
    /// <summary>
    /// Called to run a JavaScript dialog (alert, confirm, prompt).
    /// </summary>
    /// <param name="browser">The browser instance generating the dialog.</param>
    /// <param name="originUrl">The origin URL of the dialog.</param>
    /// <param name="dialogType">The type of dialog (alert, confirm, prompt).</param>
    /// <param name="message_text">The message to be displayed.</param>
    /// <param name="default_prompt_text">The default prompt text (for prompt dialogs).</param>
    /// <param name="callback">Callback to continue or cancel the dialog.</param>
    /// <param name="suppress_message">Set to true to suppress the message, false otherwise.</param>
    /// <returns>Return true if the dialog was handled, false to use the default implementation.</returns>
    bool OnJSDialog(CefBrowser browser, string originUrl, CefJSDialogType dialogType, string message_text, string default_prompt_text, CefJSDialogCallback callback, out bool suppress_message);

    /// <summary>
    /// Called to run a dialog asking the user if they want to leave a page.
    /// </summary>
    /// <param name="browser">The browser instance generating the dialog.</param>
    /// <param name="messageText">The message to be displayed.</param>
    /// <param name="isReload">True if the dialog is for a reload, false for a close.</param>
    /// <param name="callback">Callback to continue or cancel the dialog.</param>
    /// <returns>Return true if the dialog was handled, false to use the default implementation.</returns>
    bool OnBeforeUnloadDialog(CefBrowser browser, string messageText, bool isReload, CefJSDialogCallback callback);

    /// <summary>
    /// Called to reset any saved dialog state when the browser navigates or is destroyed.
    /// </summary>
    /// <param name="browser">The browser instance whose dialog state should be reset.</param>
    void OnResetDialogState(CefBrowser browser);

    /// <summary>
    /// Called when a dialog is closed.
    /// </summary>
    /// <param name="browser">The browser instance whose dialog was closed.</param>
    void OnDialogClosed(CefBrowser browser);
}
