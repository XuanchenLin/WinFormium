// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Resource type for a request. These constants match their equivalents in
/// Chromium's ResourceType and should not be renumbered.
/// </summary>
public enum CefResourceType
{
    /// <summary>
    /// Top level page.
    /// </summary>
    MainFrame = 0,

    /// <summary>
    /// Frame or iframe.
    /// </summary>
    SubFrame,

    /// <summary>
    /// CSS stylesheet.
    /// </summary>
    Stylesheet,

    /// <summary>
    /// External script.
    /// </summary>
    Script,

    /// <summary>
    /// Image (jpg/gif/png/etc).
    /// </summary>
    Image,

    /// <summary>
    /// Font.
    /// </summary>
    FontResource,

    /// <summary>
    /// Some other subresource. This is the default type if the actual type is
    /// unknown.
    /// </summary>
    SubResource,

    /// <summary>
    /// Object (or embed) tag for a plugin, or a resource that a plugin requested.
    /// </summary>
    Object,

    /// <summary>
    /// Media resource.
    /// </summary>
    Media,

    /// <summary>
    /// Main resource of a dedicated worker.
    /// </summary>
    Worker,

    /// <summary>
    /// Main resource of a shared worker.
    /// </summary>
    SharedWorker,

    /// <summary>
    /// Explicitly requested prefetch.
    /// </summary>
    Prefetch,

    /// <summary>
    /// Favicon.
    /// </summary>
    Favicon,

    /// <summary>
    /// XMLHttpRequest.
    /// </summary>
    Xhr,

    /// <summary>
    /// A request for a &lt;ping&gt;.
    /// </summary>
    Ping,

    /// <summary>
    /// Main resource of a service worker.
    /// </summary>
    ServiceWorker,

    /// <summary>
    /// A report of Content Security Policy violations.
    /// </summary>
    CspReport,

    /// <summary>
    /// A resource that a plugin requested.
    /// </summary>
    PluginResource,

    /// <summary>
    /// A main-frame service worker navigation preload request.
    /// </summary>
    NavigationPreloadMainFrame = 19,

    /// <summary>
    /// A sub-frame service worker navigation preload request.
    /// </summary>
    NavigationPreloadSubFrame,
}
