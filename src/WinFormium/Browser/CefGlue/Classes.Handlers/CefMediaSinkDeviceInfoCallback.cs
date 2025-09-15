// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface for CefMediaSink::GetDeviceInfo. The methods of this
/// class will be called on the browser process UI thread.
/// </summary>
public abstract unsafe partial class CefMediaSinkDeviceInfoCallback
{
    private void on_media_sink_device_info(cef_media_sink_device_info_callback_t* self, cef_media_sink_device_info_t* device_info)
    {
        CheckSelf(self);

        var mIPAddress = cef_string_t.ToString(&device_info->ip_address);
        var mModelName = cef_string_t.ToString(&device_info->model_name);

        var mDeviceInfo = new CefMediaSinkDeviceInfo(
            ipAddress: mIPAddress,
            port: device_info->port,
            modelName: mModelName);

        OnMediaSinkDeviceInfo(in mDeviceInfo);
    }

    /// <summary>
    /// Method that will be executed asyncronously once device information has
    /// been retrieved.
    /// </summary>
    protected abstract void OnMediaSinkDeviceInfo(in CefMediaSinkDeviceInfo deviceInfo);
}
