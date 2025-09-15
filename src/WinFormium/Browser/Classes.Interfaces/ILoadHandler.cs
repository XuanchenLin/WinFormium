// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling browser load events.
/// </summary>
public interface ILoadHandler
{
    /// <summary>
    /// Called when the loading state has changed.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="isLoading">True if the browser is loading.</param>
    /// <param name="canGoBack">True if the browser can navigate back.</param>
    /// <param name="canGoForward">True if the browser can navigate forward.</param>
    void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward);

    /// <summary>
    /// Called when a frame starts loading.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame that started loading.</param>
    /// <param name="transitionType">The transition type for the navigation.</param>
    void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType);

    /// <summary>
    /// Called when a frame finishes loading.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame that finished loading.</param>
    /// <param name="httpStatusCode">The HTTP status code of the response.</param>
    void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode);

    /// <summary>
    /// Called when a frame fails to load.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame that failed to load.</param>
    /// <param name="errorCode">The error code describing the failure.</param>
    /// <param name="errorText">A string describing the error.</param>
    /// <param name="failedUrl">The URL that failed to load.</param>
    void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl);
}
