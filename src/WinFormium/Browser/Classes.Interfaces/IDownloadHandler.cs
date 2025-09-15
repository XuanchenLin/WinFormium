// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling file downloads in the browser.
/// Implement this interface to control download behavior and receive download events.
/// </summary>
public interface IDownloadHandler
{
    /// <summary>
    /// Called before a download begins to determine whether the download should proceed.
    /// </summary>
    /// <param name="browser">The browser instance initiating the download.</param>
    /// <param name="url">The URL of the file to be downloaded.</param>
    /// <param name="requestMethod">The HTTP request method (e.g., "GET", "POST").</param>
    /// <returns>True if the download is allowed, false to cancel.</returns>
    bool CanDownload(CefBrowser browser, string url, string requestMethod);

    /// <summary>
    /// Called before a download starts, allowing the client to specify the download path and whether to show a dialog.
    /// </summary>
    /// <param name="browser">The browser instance initiating the download.</param>
    /// <param name="downloadItem">The download item representing the file to be downloaded.</param>
    /// <param name="suggestedName">The suggested file name for the download.</param>
    /// <param name="callback">Callback interface used to continue or cancel the download.</param>
    void OnBeforeDownload(CefBrowser browser, CefDownloadItem downloadItem, string suggestedName, CefBeforeDownloadCallback callback);

    /// <summary>
    /// Called when the download status or progress is updated.
    /// </summary>
    /// <param name="browser">The browser instance associated with the download.</param>
    /// <param name="downloadItem">The download item representing the file being downloaded.</param>
    /// <param name="callback">Callback interface used to pause, resume, or cancel the download.</param>
    void OnDownloadUpdated(CefBrowser browser, CefDownloadItem downloadItem, CefDownloadItemCallback callback);
}
