﻿// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Represents the route between a media source and sink. Instances of this
/// object are created via CefMediaRouter::CreateRoute and retrieved via
/// CefMediaObserver::OnRoutes. Contains the status and metadata of a
/// routing operation. The methods of this class may be called on any browser
/// process thread unless otherwise indicated.
/// </summary>
public sealed unsafe partial class CefMediaRoute
{
    /// <summary>
    /// Returns the ID for this route.
    /// </summary>
    public string Id
    {
        get
        {
            var n_result = cef_media_route_t.get_id(_self);
            return cef_string_userfree.ToString(n_result);
        }
    }

    /// <summary>
    /// Returns the source associated with this route.
    /// </summary>
    public CefMediaSource GetSource()
    {
        return CefMediaSource.FromNative(
            cef_media_route_t.get_source(_self)
            );
    }

    /// <summary>
    /// Returns the sink associated with this route.
    /// </summary>
    public CefMediaSink GetSink()
    {
        return CefMediaSink.FromNative(
            cef_media_route_t.get_sink(_self)
            );
    }

    /// <summary>
    /// Send a message over this route. |message| will be copied if necessary.
    /// </summary>
    public void SendRouteMessage(IntPtr message, int messageSize)
    {
        var n_messageSize = checked((UIntPtr)messageSize);
        cef_media_route_t.send_route_message(_self, (void*)message, n_messageSize);
    }

    /// <summary>
    /// Terminate this route. Will result in an asynchronous call to
    /// CefMediaObserver::OnRoutes on all registered observers.
    /// </summary>
    public void Terminate()
    {
        cef_media_route_t.terminate(_self);
    }
}
