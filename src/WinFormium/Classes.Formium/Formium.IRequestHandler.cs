// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
public partial class Formium : IRequestHandler
{

    /// <summary>
    /// The passcode used for JavaScript message authentication.
    /// </summary>
    private string JS_MESSAGE_PASSCODE => $"{GetHashCode()}";

    /// <inheritdoc/>
    bool IRequestHandler.GetAuthCredentials(CefBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, CefAuthCallback callback)
    {
        return (RequestHandler?.GetAuthCredentials(browser, originUrl, isProxy, host, port, realm, scheme, callback) ?? false) || OnBrowserGetAuthCredentials(browser, originUrl, isProxy, host, port, realm, scheme, callback);
    }

    /// <inheritdoc/>
    CefResourceRequestHandler? IRequestHandler.GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
    {
        var retval = RequestHandler?.GetResourceRequestHandler(browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);

        if (retval != null) return retval;

        return OnBrowserGetResourceRequestHandler(browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
    {
        return (RequestHandler?.OnBeforeBrowse(browser, frame, request, userGesture, isRedirect) ?? false) || OnBrowserBeforeBrowse(browser, frame, request, userGesture, isRedirect);
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnCertificateError(CefBrowser browser, CefErrorCode certError, string requestUrl, CefSslInfo sslInfo, CefCallback callback)
    {
        return (RequestHandler?.OnCertificateError(browser, certError, requestUrl, sslInfo, callback) ?? false) || OnBrowserCertificateError(browser, certError, requestUrl, sslInfo, callback);
    }

    /// <inheritdoc/>
    void IRequestHandler.OnDocumentAvailableInMainFrame(CefBrowser browser)
    {
        InjectHostWindowScript(browser);

        RequestHandler?.OnDocumentAvailableInMainFrame(browser);

        InvokeIfRequired(() =>
        {
            BrowserDocumentAvailable?.Invoke(this, new BrowserEventArgs(browser));
        });
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnOpenUrlFromTab(CefBrowser browser, CefFrame frame, string targetUrl, CefWindowOpenDisposition targetDisposition, bool userGesture)
    {
        return (RequestHandler?.OnOpenUrlFromTab(browser, frame, targetUrl, targetDisposition, userGesture) ?? false) || OnBrowserOpenUrlFromTab(browser, frame, targetUrl, targetDisposition, userGesture);
    }

    /// <inheritdoc/>
    void IRequestHandler.OnRenderViewReady(CefBrowser browser)
    {
        RequestHandler?.OnRenderViewReady(browser);
    }

    /// <inheritdoc/>
    void IRequestHandler.OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
    {
        RequestHandler?.OnRenderProcessTerminated(browser, status);

        var args = new BrowserRenderProcessTerminatedEventArgs(browser, status);
        InvokeIfRequired(() =>
        {
            BrowserRenderProcessTerminated?.Invoke(this, args);
        });

        if (args.RestartProcess)
        {
            browser.Reload();
        }
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnSelectClientCertificate(CefBrowser browser, bool isProxy, string host, int port, CefX509Certificate[] certificates, CefSelectClientCertificateCallback callback)
    {
        return (RequestHandler?.OnSelectClientCertificate(browser, isProxy, host, port, certificates, callback) ?? false) || OnSelectClientCertificate(browser, isProxy, host, port, certificates, callback);
    }

    /// <summary>
    /// Occurs when everything is ready.
    /// </summary>
    public event EventHandler<BrowserEventArgs>? Load;

    /// <summary>
    /// Occurs when the document is available in the main frame.
    /// </summary>
    public event EventHandler<BrowserEventArgs>? BrowserDocumentAvailable;

    /// <summary>
    /// Occurs when the render process has terminated.
    /// </summary>
    public event EventHandler<BrowserRenderProcessTerminatedEventArgs>? BrowserRenderProcessTerminated;

    /// <summary>
    /// Called when authentication credentials are required for a request.
    /// Override to provide custom authentication handling.
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
    protected virtual bool OnBrowserGetAuthCredentials(CefBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, CefAuthCallback callback)
    {
        return false;
    }

    /// <summary>
    /// Called to retrieve a resource request handler for the specified request.
    /// Override to provide a custom resource request handler.
    /// </summary>
    /// <param name="browser">The browser generating the request.</param>
    /// <param name="frame">The frame generating the request.</param>
    /// <param name="request">The request object.</param>
    /// <param name="isNavigation">True if the request is a navigation.</param>
    /// <param name="isDownload">True if the request is a download.</param>
    /// <param name="requestInitiator">The origin of the request.</param>
    /// <param name="disableDefaultHandling">Set to true to disable default handling.</param>
    /// <returns>A <see cref="CefResourceRequestHandler"/> instance or null for default handling.</returns>
    protected virtual CefResourceRequestHandler? OnBrowserGetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
    {
        return null;
    }

    /// <summary>
    /// Called before a navigation is initiated in the browser.
    /// Override to provide custom navigation handling.
    /// </summary>
    /// <param name="browser">The browser where navigation is occurring.</param>
    /// <param name="frame">The frame where navigation is occurring.</param>
    /// <param name="request">The navigation request.</param>
    /// <param name="userGesture">True if the navigation was initiated by a user gesture.</param>
    /// <param name="isRedirect">True if the navigation is a redirect.</param>
    /// <returns>True to cancel navigation, false to allow it.</returns>
    protected virtual bool OnBrowserBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
    {
        return false;
    }

    /// <summary>
    /// Called when a certificate error occurs during a request.
    /// Override to provide custom certificate error handling.
    /// </summary>
    /// <param name="browser">The browser where the error occurred.</param>
    /// <param name="certError">The certificate error code.</param>
    /// <param name="requestUrl">The URL that failed the certificate check.</param>
    /// <param name="sslInfo">SSL information for the request.</param>
    /// <param name="callback">Callback to continue or cancel the request.</param>
    /// <returns>True to handle the error, false to use default handling.</returns>
    protected virtual bool OnBrowserCertificateError(CefBrowser browser, CefErrorCode certError, string requestUrl, CefSslInfo sslInfo, CefCallback callback)
    {
        return false;
    }

    /// <summary>
    /// Called when a new URL is opened from a tab (e.g., via target="_blank").
    /// Override to provide custom handling for opening URLs from tabs.
    /// </summary>
    /// <param name="browser">The source browser.</param>
    /// <param name="frame">The source frame.</param>
    /// <param name="targetUrl">The target URL to open.</param>
    /// <param name="targetDisposition">The disposition for the new tab or window.</param>
    /// <param name="userGesture">True if the action was initiated by a user gesture.</param>
    /// <returns>True to handle the URL opening, false to use default behavior.</returns>
    protected virtual bool OnBrowserOpenUrlFromTab(CefBrowser browser, CefFrame frame, string targetUrl, CefWindowOpenDisposition targetDisposition, bool userGesture)
    {
        return false;
    }

    /// <summary>
    /// Called to select a client certificate for authentication.
    /// Override to provide custom client certificate selection.
    /// </summary>
    /// <param name="browser">The browser requesting the certificate.</param>
    /// <param name="isProxy">True if the request is for a proxy server.</param>
    /// <param name="host">The host requiring the certificate.</param>
    /// <param name="port">The port of the host.</param>
    /// <param name="certificates">Available client certificates.</param>
    /// <param name="callback">Callback to select a certificate.</param>
    /// <returns>True to handle certificate selection, false to use default handling.</returns>
    protected virtual bool OnSelectClientCertificate(CefBrowser browser, bool isProxy, string host, int port, CefX509Certificate[] certificates, CefSelectClientCertificateCallback callback)
    {
        return false;
    }

}
