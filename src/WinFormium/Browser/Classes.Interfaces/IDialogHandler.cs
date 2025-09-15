// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling file dialog events in the browser.
/// Implement this interface to customize file dialog behavior,
/// such as open/save dialogs triggered by the browser.
/// </summary>
public interface IDialogHandler
{
    /// <summary>
    /// Called when a file dialog is about to be displayed.
    /// Implement this method to handle the file dialog event.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance requesting the dialog.</param>
    /// <param name="mode">The <see cref="CefFileDialogMode"/> indicating the type of dialog (e.g., open, save).</param>
    /// <param name="title">The title to be used for the dialog. May be empty to show the default title.</param>
    /// <param name="defaultFilePath">The path with optional directory and/or file name component that will be initially selected in the dialog.</param>
    /// <param name="acceptFilters">An array of filters used to restrict selectable file types (e.g., MIME types, file extensions).</param>
    /// <param name="callback">The <see cref="CefFileDialogCallback"/> to execute after the dialog is dismissed.</param>
    /// <returns>
    /// Return true if the dialog is handled and the callback will be executed, or false to use the default dialog implementation.
    /// </returns>
    bool OnFileDialog(CefBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, string[] acceptFilters, CefFileDialogCallback callback);
}
