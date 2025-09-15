// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling browser request events. Implement this interface to customize
/// request handling, authentication, certificate selection, and process/browser events.
/// </summary>
public interface IRequestHandler
{
    /// <summary>
    /// Called to retrieve a resource request handler for the specified request.
    /// </summary>
    /// <param name="browser">The browser generating the request.</param>
    /// <param name="frame">The frame generating the request.</param>
    /// <param name="request">The request object.</param>
    /// <param name="isNavigation">True if the request is a navigation.</param>
    /// <param name="isDownload">True if the request is a download.</param>
    /// <param name="requestInitiator">The origin of the request.</param>
    /// <param name="disableDefaultHandling">Set to true to disable default handling.</param>
    /// <returns>A <see cref="CefResourceRequestHandler"/> instance or null for default handling.</returns>
    CefResourceRequestHandler? GetResourceRequestHandler(
        CefBrowser browser,
        CefFrame frame,
        CefRequest request,
        bool isNavigation,
        bool isDownload,
        string requestInitiator,
        ref bool disableDefaultHandling);

    /// <summary>
    /// Called when authentication credentials are required for a request.
    /// </summary>
    /// <param name="browser">The browser requesting authentication.</param>
    /// <param name="originUrl">The origin URL of the request.</param>
    /// <param name="isProxy">True if the host is a proxy server.</param>
    /// <param name="host">The host requiring authentication.</param>
    /// <param name="port">The port of the host.</param>
    /// <param name="realm">The authentication realm.</param>
    /// <param name="scheme">The authentication scheme.</param>
    /// <param name="callback">The authentication callback.</param>
    /// <returns>True to handle the credentials, false to cancel or use default handling.</returns>
    bool GetAuthCredentials(
        CefBrowser browser,
        string originUrl,
        bool isProxy,
        string host,
        int port,
        string realm,
        string scheme,
        CefAuthCallback callback);

    /// <summary>
    /// Called before a navigation is initiated in the browser.
    /// </summary>
    /// <param name="browser">The browser where navigation is occurring.</param>
    /// <param name="frame">The frame where navigation is occurring.</param>
    /// <param name="request">The navigation request.</param>
    /// <param name="userGesture">True if the navigation was initiated by a user gesture.</param>
    /// <param name="isRedirect">True if the navigation is a redirect.</param>
    /// <returns>True to cancel navigation, false to allow it.</returns>
    bool OnBeforeBrowse(
        CefBrowser browser,
        CefFrame frame,
        CefRequest request,
        bool userGesture,
        bool isRedirect);

    /// <summary>
    /// Called when a new URL is opened from a tab (e.g., via target="_blank").
    /// </summary>
    /// <param name="browser">The source browser.</param>
    /// <param name="frame">The source frame.</param>
    /// <param name="targetUrl">The target URL to open.</param>
    /// <param name="targetDisposition">The disposition for the new tab or window.</param>
    /// <param name="userGesture">True if the action was initiated by a user gesture.</param>
    /// <returns>True to handle the URL opening, false to use default behavior.</returns>
    bool OnOpenUrlFromTab(
        CefBrowser browser,
        CefFrame frame,
        string targetUrl,
        CefWindowOpenDisposition targetDisposition,
        bool userGesture);

    /// <summary>
    /// Called when a certificate error occurs during a request.
    /// </summary>
    /// <param name="browser">The browser where the error occurred.</param>
    /// <param name="certError">The certificate error code.</param>
    /// <param name="requestUrl">The URL that failed the certificate check.</param>
    /// <param name="sslInfo">SSL information for the request.</param>
    /// <param name="callback">Callback to continue or cancel the request.</param>
    /// <returns>True to handle the error, false to use default handling.</returns>
    bool OnCertificateError(
        CefBrowser browser,
        CefErrorCode certError,
        string requestUrl,
        CefSslInfo sslInfo,
        CefCallback callback);

    /// <summary>
    /// Called to select a client certificate for authentication.
    /// </summary>
    /// <param name="browser">The browser requesting the certificate.</param>
    /// <param name="isProxy">True if the request is for a proxy server.</param>
    /// <param name="host">The host requiring the certificate.</param>
    /// <param name="port">The port of the host.</param>
    /// <param name="certificates">Available client certificates.</param>
    /// <param name="callback">Callback to select a certificate.</param>
    /// <returns>True to handle certificate selection, false to use default handling.</returns>
    bool OnSelectClientCertificate(
        CefBrowser browser,
        bool isProxy,
        string host,
        int port,
        CefX509Certificate[] certificates,
        CefSelectClientCertificateCallback callback);

    /// <summary>
    /// Called when the render view for the browser is ready.
    /// </summary>
    /// <param name="browser">The browser whose render view is ready.</param>
    void OnRenderViewReady(CefBrowser browser);

    /// <summary>
    /// Called when the render process for the browser has terminated.
    /// </summary>
    /// <param name="browser">The browser whose render process terminated.</param>
    /// <param name="status">The termination status.</param>
    void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status);

    /// <summary>
    /// Called when the document in the main frame is available.
    /// </summary>
    /// <param name="browser">The browser whose main frame document is available.</param>
    void OnDocumentAvailableInMainFrame(CefBrowser browser);
}
