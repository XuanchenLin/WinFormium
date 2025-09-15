// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface used to asynchronously continue a download.
/// </summary>
public sealed unsafe partial class CefBeforeDownloadCallback
{
    /// <summary>
    /// Call to continue the download. Set |download_path| to the full file path
    /// for the download including the file name or leave blank to use the
    /// suggested name and the default temp directory. Set |show_dialog| to true
    /// if you do wish to show the default "Save As" dialog.
    /// </summary>
    public void Continue(string downloadPath, bool showDialog)
    {
        fixed (char* downloadPath_ptr = downloadPath)
        {
            var n_downloadPath = new cef_string_t(downloadPath_ptr, downloadPath != null ? downloadPath.Length : 0);
            cef_before_download_callback_t.cont(_self, &n_downloadPath, showDialog ? 1 : 0);
        }
    }
}
