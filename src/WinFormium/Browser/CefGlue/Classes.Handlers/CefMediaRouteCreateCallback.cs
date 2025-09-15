// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface for CefMediaRouter::CreateRoute. The methods of this
/// class will be called on the browser process UI thread.
/// </summary>
public abstract unsafe partial class CefMediaRouteCreateCallback
{
    private void on_media_route_create_finished(cef_media_route_create_callback_t* self, CefMediaRouteCreateResult result, cef_string_t* error, cef_media_route_t* route)
    {
        CheckSelf(self);

        var mError = cef_string_t.ToString(error);
        var mRoute = CefMediaRoute.FromNativeOrNull(route);

        OnMediaRouteCreateFinished(result, mError, mRoute);
    }

    /// <summary>
    /// Method that will be executed when the route creation has finished.
    /// |result| will be CEF_MRCR_OK if the route creation succeeded. |error| will
    /// be a description of the error if the route creation failed. |route| is the
    /// resulting route, or empty if the route creation failed.
    /// </summary>
    protected abstract void OnMediaRouteCreateFinished(CefMediaRouteCreateResult result, string error, CefMediaRoute route);
}
