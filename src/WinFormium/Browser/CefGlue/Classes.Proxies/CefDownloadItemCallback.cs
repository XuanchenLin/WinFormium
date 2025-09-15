// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface used to asynchronously cancel a download.
/// </summary>
public sealed unsafe partial class CefDownloadItemCallback
{
    /// <summary>
    /// Call to cancel the download.
    /// </summary>
    public void Cancel()
    {
        cef_download_item_callback_t.cancel(_self);
    }

    /// <summary>
    /// Call to pause the download.
    /// </summary>
    public void Pause()
    {
        cef_download_item_callback_t.pause(_self);
    }

    /// <summary>
    /// Call to resume the download.
    /// </summary>
    public void Resume()
    {
        cef_download_item_callback_t.resume(_self);
    }
}
