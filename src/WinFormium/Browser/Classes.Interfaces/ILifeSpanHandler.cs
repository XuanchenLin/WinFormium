// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling browser life span events. Implement this interface to
/// receive notifications related to the creation and destruction of browser instances.
/// </summary>
public interface ILifeSpanHandler
{
    /// <summary>
    /// Called on the UI thread before a new popup browser is created.
    /// Allows modification of popup parameters or cancellation of the popup.
    /// </summary>
    /// <param name="browser">The source browser.</param>
    /// <param name="frame">The source frame.</param>
    /// <param name="targetUrl">The target URL for the popup.</param>
    /// <param name="targetFrameName">The target frame name for the popup.</param>
    /// <param name="targetDisposition">The disposition for the popup window.</param>
    /// <param name="userGesture">Indicates if the popup was opened via user gesture.</param>
    /// <param name="popupFeatures">Additional features for the popup window.</param>
    /// <param name="windowInfo">Window information for the new popup.</param>
    /// <param name="client">Client instance for the new popup. Can be modified.</param>
    /// <param name="settings">Browser settings for the new popup.</param>
    /// <param name="extraInfo">Extra information for the new popup. Can be modified.</param>
    /// <param name="noJavascriptAccess">Indicates if JavaScript access should be disabled. Can be modified.</param>
    /// <returns>Return true to cancel popup creation, or false to allow it.</returns>
    bool OnBeforePopup(
        CefBrowser browser,
        CefFrame frame,
        string targetUrl,
        string targetFrameName,
        CefWindowOpenDisposition targetDisposition,
        bool userGesture,
        CefPopupFeatures popupFeatures,
        CefWindowInfo windowInfo,
        ref CefClient client,
        CefBrowserSettings settings,
        ref CefDictionaryValue extraInfo,
        ref bool noJavascriptAccess);

    /// <summary>
    /// Called after a new browser is created and ready for use.
    /// </summary>
    /// <param name="browser">The newly created browser instance.</param>
    void OnAfterCreated(CefBrowser browser);

    /// <summary>
    /// Called when a browser receives a request to close.
    /// </summary>
    /// <param name="browser">The browser that is requested to close.</param>
    /// <returns>Return true to handle the close request, or false to proceed with default close behavior.</returns>
    bool DoClose(CefBrowser browser);

    /// <summary>
    /// Called just before a browser is destroyed.
    /// </summary>
    /// <param name="browser">The browser that is about to be closed.</param>
    void OnBeforeClose(CefBrowser browser);
}
