// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Partial implementation of <see cref="IRequestHandler"/> for handling browser request events in <see cref="WebViewCore"/>.
/// Delegates request handling to the associated <see cref="IWebViewClient"/> or uses internal handlers as fallback.
/// </summary>
internal partial class WebViewCore : IRequestHandler
{
    /// <summary>
    /// Internal resource request handler used as a fallback when no custom handler is provided.
    /// </summary>
    private WebResourceRequestHandler _internalResourceRequestHandler;

    /// <inheritdoc/>
    bool IRequestHandler.GetAuthCredentials(CefBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, CefAuthCallback callback)
    {
        return BrowserClient?.RequestHandler?.GetAuthCredentials(browser, originUrl, isProxy, host, port, realm, scheme, callback) ?? false;
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
    {
        return BrowserClient?.RequestHandler?.OnBeforeBrowse(browser, frame, request, userGesture, isRedirect) ?? false;
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnCertificateError(CefBrowser browser, CefErrorCode certError, string requestUrl, CefSslInfo sslInfo, CefCallback callback)
    {
        return BrowserClient?.RequestHandler?.OnCertificateError(browser, certError, requestUrl, sslInfo, callback) ?? false;
    }

    /// <inheritdoc/>
    void IRequestHandler.OnDocumentAvailableInMainFrame(CefBrowser browser)
    {
        var script = Resources.Files.version_js;
        var version = typeof(WinFormiumApp).Assembly.GetName().Version?.ToString() ?? CefRuntime.ChromeVersion;

        script = script.Replace("{{WINFORMIUM_VERSION_INFO}}", $"%cChromium%c{CefRuntime.ChromeVersion}%c %cWinFormium%c{version}%c %cArchitect%c{(IntPtr.Size == 4 ? "x86" : "x64")}%c");

        ExecuteJavaScript(script);

        BrowserClient?.RequestHandler?.OnDocumentAvailableInMainFrame(browser);
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnOpenUrlFromTab(CefBrowser browser, CefFrame frame, string targetUrl, CefWindowOpenDisposition targetDisposition, bool userGesture)
    {
        return BrowserClient?.RequestHandler?.OnOpenUrlFromTab(browser, frame, targetUrl, targetDisposition, userGesture) ?? false;
    }

    /// <inheritdoc/>
    void IRequestHandler.OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
    {
        BrowserClient?.RequestHandler?.OnRenderProcessTerminated(browser, status);
    }

    /// <inheritdoc/>
    void IRequestHandler.OnRenderViewReady(CefBrowser browser)
    {
        BrowserClient?.RequestHandler?.OnRenderViewReady(browser);
    }

    /// <inheritdoc/>
    bool IRequestHandler.OnSelectClientCertificate(CefBrowser browser, bool isProxy, string host, int port, CefX509Certificate[] certificates, CefSelectClientCertificateCallback callback)
    {
        return BrowserClient?.RequestHandler?.OnSelectClientCertificate(browser, isProxy, host, port, certificates, callback) ?? false;
    }

    /// <inheritdoc/>
    CefResourceRequestHandler? IRequestHandler.GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
    {
        return BrowserClient?.RequestHandler?.GetResourceRequestHandler(browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling) ?? _internalResourceRequestHandler;
    }
}

