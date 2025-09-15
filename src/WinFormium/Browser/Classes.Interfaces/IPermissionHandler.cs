// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling permission-related events in the browser.
/// </summary>
public interface IPermissionHandler
{
    /// <summary>
    /// Called when media access permission is requested (e.g., camera or microphone).
    /// Implement to allow or deny the request. Return true if the application will handle the request and call <paramref name="callback"/>, or false to use the default behavior.
    /// </summary>
    /// <param name="browser">The browser requesting media access.</param>
    /// <param name="frame">The frame requesting media access.</param>
    /// <param name="requestingOrigin">The origin requesting access.</param>
    /// <param name="requestedPermissions">The types of media access requested.</param>
    /// <param name="callback">Callback for continuing or canceling the request.</param>
    /// <returns>True if the application will handle the request, false to use default behavior.</returns>
    bool OnRequestMediaAccessPermission(CefBrowser browser, CefFrame frame, string requestingOrigin, CefMediaAccessPermissionTypes requestedPermissions, CefMediaAccessCallback callback);

    /// <summary>
    /// Called when a permission prompt is shown (e.g., geolocation, notifications).
    /// Implement to allow or deny the prompt. Return true if the application will handle the prompt and call <paramref name="callback"/>, or false to use the default behavior.
    /// </summary>
    /// <param name="browser">The browser showing the permission prompt.</param>
    /// <param name="promptId">The unique identifier for the prompt.</param>
    /// <param name="requestingOrigin">The origin requesting permission.</param>
    /// <param name="requestedPermissions">The types of permissions requested.</param>
    /// <param name="callback">Callback for continuing or canceling the prompt.</param>
    /// <returns>True if the application will handle the prompt, false to use default behavior.</returns>
    bool OnShowPermissionPrompt(CefBrowser browser, ulong promptId, string requestingOrigin, CefPermissionRequestTypes requestedPermissions, CefPermissionPromptCallback callback);

    /// <summary>
    /// Called when a permission prompt is dismissed.
    /// </summary>
    /// <param name="browser">The browser where the prompt was dismissed.</param>
    /// <param name="promptId">The unique identifier for the prompt.</param>
    /// <param name="result">The result of the permission request.</param>
    void OnDismissPermissionPrompt(CefBrowser browser, ulong promptId, CefPermissionRequestResult result);
}
