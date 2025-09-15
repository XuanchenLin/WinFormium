// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Delegate for providing a custom resource handler for a web resource request.
/// </summary>
internal delegate CefResourceHandler GetResourceHandlerDelegate(CefBrowser browser, CefFrame frame, CefRequest request);

partial class WebViewCore
{
    /// <summary>
    /// Gets a value indicating whether the browser is valid.
    /// </summary>
    public bool IsBrowserValid => Browser?.IsValid ?? false;

    /// <summary>
    /// Gets a value indicating whether the browser is currently loading.
    /// </summary>
    public bool IsLoading => Browser?.IsLoading ?? false;

    /// <summary>
    /// Gets a value indicating whether the browser can navigate back.
    /// </summary>
    public bool CanGoBack => Browser?.CanGoBack ?? false;

    /// <summary>
    /// Gets a value indicating whether the browser can navigate forward.
    /// </summary>
    public bool CanGoForward => Browser?.CanGoForward ?? false;

    /// <summary>
    /// Gets a value indicating whether the browser is a popup window.
    /// </summary>
    public bool IsPopup => Browser?.IsPopup ?? false;

    /// <summary>
    /// Gets a value indicating whether the browser has a loaded document.
    /// </summary>
    public bool HasDocument => Browser?.HasDocument ?? false;

    /// <summary>
    /// Navigates the browser back to the previous page, if possible.
    /// </summary>
    public void GoBack()
    {
        if (Browser != null && Browser.CanGoBack)
        {
            ActionTask.Run(() => Browser.GoBack());
        }
    }

    /// <summary>
    /// Navigates the browser forward to the next page, if possible.
    /// </summary>
    public void GoForward()
    {
        if (Browser != null && Browser.CanGoForward)
        {
            ActionTask.Run(() => Browser.GoForward());
        }
    }

    /// <summary>
    /// Reloads the current page in the browser.
    /// </summary>
    public void Reload()
    {
        if (Browser != null)
        {
            ActionTask.Run(() => Browser.Reload());
        }
    }

    /// <summary>
    /// Reloads the current page in the browser, ignoring any cached data.
    /// </summary>
    public void ReloadIgnoreCache()
    {
        if (Browser != null)
        {
            ActionTask.Run(() => Browser.ReloadIgnoreCache());
        }
    }

    /// <summary>
    /// Stops loading the current page in the browser.
    /// </summary>
    public void StopLoad()
    {
        if (Browser != null)
        {
            ActionTask.Run(() => Browser.StopLoad());
        }
    }

    /// <summary>
    /// Gets or sets the zoom level of the browser.
    /// </summary>
    public double ZoomLevel
    {
        get => BrowserHost?.GetZoomLevel() ?? 0;
        set => BrowserHost?.SetZoomLevel(value);
    }

#pragma warning disable CS8604
    /// <summary>
    /// Executes JavaScript code in the specified frame.
    /// </summary>
    /// <param name="frame">The frame in which to execute the script.</param>
    /// <param name="code">The JavaScript code to execute.</param>
    /// <param name="url">The URL associated with the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    public void ExecuteJavaScript(CefFrame frame, string code, string? url = null, int line = 0)
    {

        frame.ExecuteJavaScript(code, url, line);
    }

    /// <summary>
    /// Executes JavaScript code in the main frame of the browser.
    /// </summary>
    /// <param name="code">The JavaScript code to execute.</param>
    /// <param name="url">The URL associated with the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    public void ExecuteJavaScript(string code, string? url = null, int line = 0)
    {
        if (Browser?.GetMainFrame() == null) throw new NullReferenceException("Browser is null.");

        ExecuteJavaScript(Browser?.GetMainFrame(), code, url, line);
    }
#pragma warning restore CS8604

    /// <summary>
    /// Ensures that the JavaScript engine is initialized before use.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the JavaScript engine is not initialized.</exception>
    private void EnsureJavaScriptEngine()
    {
        if (JsEngine is null)
            throw new InvalidOperationException("JavaScript engine is not initialized. Please initialize the JavaScript engine before using it.");
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the main frame and returns the result as a string.
    /// </summary>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL associated with the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as a string.</returns>
    public Task<string?> EvaluateJavaScriptAsync(string code, string? url = null, int line = 0)
    {
        if (Browser?.GetMainFrame() == null) throw new NullReferenceException("Browser is null.");

        return EvaluateJavaScriptAsync(Browser?.GetMainFrame()!, code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the specified frame and returns the result as a string.
    /// </summary>
    /// <param name="frame">The frame in which to evaluate the script.</param>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL associated with the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as a string.</returns>
    public Task<string?> EvaluateJavaScriptAsync(CefFrame frame, string code, string? url = null, int line = 0)
    {

        EnsureJavaScriptEngine();

        return JsEngine!.EvaluateJavaScriptAsync(frame, code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the main frame and returns a detailed result.
    /// </summary>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL associated with the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as a <see cref="JavaScriptExecuteScriptResult"/>.</returns>
    public Task<JavaScriptExecuteScriptResult> EvaluateJavaScriptWithResultAsync(string code, string? url = null, int line = 0)
    {
        if (Browser?.GetMainFrame() == null) throw new NullReferenceException("Browser is null.");
        return EvaluateJavaScriptWithResultAsync(Browser?.GetMainFrame()!, code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the specified frame and returns a detailed result.
    /// </summary>
    /// <param name="frame">The frame in which to evaluate the script.</param>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL associated with the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as a <see cref="JavaScriptExecuteScriptResult"/>.</returns>
    public Task<JavaScriptExecuteScriptResult> EvaluateJavaScriptWithResultAsync(CefFrame frame, string code, string? url = null, int line = 0)
    {
        EnsureJavaScriptEngine();
        return JsEngine!.EvaluateJavaScriptWithResultAsync(frame, code, url, line);
    }

    /// <summary>
    /// Adds a script to be executed when a document is created.
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param>
    /// <returns>An identifier for the added script.</returns>
    public int AddScriptToExecuteOnDocumentCreated(string script)
    {
        ArgumentException.ThrowIfNullOrEmpty(script, nameof(script));

        EnsureJavaScriptEngine();

        return JsEngine!.AddScriptToExecuteOnDocumentCreated(script);
    }

    /// <summary>
    /// Removes a script that was set to execute on document creation.
    /// </summary>
    /// <param name="identifier">The identifier of the script to remove.</param>
    public void RemoveScriptToExecuteOnDocumentCreated(int identifier)
    {
        EnsureJavaScriptEngine();

        JsEngine!.RemoveScriptToExecuteOnDocumentCreated(identifier);
    }

    /// <summary>
    /// Posts a web message as a string to the JavaScript context.
    /// </summary>
    /// <param name="webMessageAsString">The message to post as a string.</param>
    public void PostWebMessageAsString(string webMessageAsString)
    {
        EnsureJavaScriptEngine();
        JsEngine!.PostWebMessageAsString(webMessageAsString);
    }

    /// <summary>
    /// Posts a web message as a JSON string to the JavaScript context.
    /// </summary>
    /// <param name="webMessageAsJson">The message to post as a JSON string.</param>
    public void PostWebMessageAsJson(string webMessageAsJson)
    {
        EnsureJavaScriptEngine();
        JsEngine!.PostWebMessageAsJson(webMessageAsJson);
    }

    /// <summary>
    /// Registers a native object for JavaScript interop.
    /// </summary>
    /// <param name="name">The name of the object in JavaScript.</param>
    /// <param name="obj">The native proxy object to register.</param>
    public void RegisterNativeObject(string name, NativeProxyObject obj)
    {
        JsEngine!.RegisterNativeObject(name, obj);
    }

    /// <summary>
    /// Unregisters a native object from JavaScript interop.
    /// </summary>
    /// <param name="name">The name of the object to unregister.</param>
    public void UnregisterNativeObject(string name)
    {
        JsEngine!.UnregisterNativeObject(name);
    }

    /// <summary>
    /// Handles a web resource request and returns a custom response if applicable.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame making the request.</param>
    /// <param name="request">The web request.</param>
    /// <returns>A <see cref="WebResourceResponse"/> if handled, otherwise null.</returns>
    internal WebResourceResponse? OnWebResourceRequesting(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return BrowserClient.OnWebResourceRequesting(browser, frame, request);
    }

    /// <summary>
    /// Handles web resource response filtering.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="frame">The frame making the request.</param>
    /// <param name="request">The web request.</param>
    /// <param name="response">The web response.</param>
    /// <returns>A delegate for response filtering, or null if not handled.</returns>
    internal WebResponseFilterHandlerDelegate? OnWebResourceResponseFilter(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
    {
        return BrowserClient.OnWebResponseFiltering(browser, frame, request, response);
    }

    /// <summary>
    /// Gets the collection of virtual host name resource handlers.
    /// </summary>
    internal Dictionary<string, GetResourceHandlerDelegate> WebResourceHandlers { get; } = new();

    /// <summary>
    /// Maps a virtual host name to an HTTP proxy target.
    /// </summary>
    /// <param name="scheme">The scheme of the virtual host.</param>
    /// <param name="domainName">The domain name of the virtual host.</param>
    /// <param name="proxyTarget">The target URL for the proxy.</param>
    /// <exception cref="ArgumentNullException">Thrown if any argument is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the host name is already mapped.</exception>
    public void SetVirtualHostNameToHttpProxyMapping(string scheme, string domainName, string proxyTarget)
    {
        var hostName = GetFilterUrl(scheme, domainName);
        if (string.IsNullOrEmpty(hostName))
            throw new ArgumentNullException(nameof(hostName));
        if (string.IsNullOrEmpty(proxyTarget))
            throw new ArgumentNullException(nameof(proxyTarget));
        if (WebResourceHandlers.ContainsKey(hostName))
            throw new ArgumentException($"The host name '{hostName}' is already mapped.");

        WebResourceHandlers[hostName] = (browser, frame, request) => new HttpProxyMappingResourceHandler(browser, frame, request, proxyTarget);
    }

    /// <summary>
    /// Maps a virtual host name to a local folder for resource requests.
    /// </summary>
    /// <param name="options">The folder mapping options.</param>
    /// <exception cref="ArgumentNullException">Thrown if options or host name is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the host name is already mapped.</exception>
    public void SetVirtualHostNameToFolderMapping(FolderMappingOptions options)
    {
        var hostName = GetFilterUrl(options.Scheme, options.DomainName);

        if (string.IsNullOrEmpty(hostName))
            throw new ArgumentNullException(nameof(hostName));

        if (options is null)
            throw new ArgumentNullException(nameof(options));

        if (WebResourceHandlers.ContainsKey(hostName))
            throw new ArgumentException($"The host name '{hostName}' is already mapped.");


        WebResourceHandlers[hostName] = (browser, frame, request) =>
        {
            return new FolderMappingResourceHandler(browser, frame, request, options);
        };

    }

    /// <summary>
    /// Maps a virtual host name to embedded file resources.
    /// </summary>
    /// <param name="options">The embedded file mapping options.</param>
    /// <exception cref="ArgumentNullException">Thrown if options or host name is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the host name is already mapped.</exception>
    public void SetVirtualHostNameToEmbeddedFileMapping(EmbeddedFileMappingOptions options)
    {
        var hostName = GetFilterUrl(options.Scheme, options.DomainName);

        if (string.IsNullOrEmpty(hostName))
            throw new ArgumentNullException(nameof(hostName));

        if (options == null)
            throw new ArgumentNullException(nameof(options));


        if (WebResourceHandlers.ContainsKey(hostName))
            throw new ArgumentException($"The host name '{hostName}' is already mapped.");

        WebResourceHandlers[hostName] = (browser, frame, request) =>
        {
            return new EmbeddedFileMappingResourceHandler(browser, frame, request, options);
        };

    }

    /// <summary>
    /// Maps a virtual host name to a Server-Sent Events (SSE) endpoint.
    /// </summary>
    /// <param name="hostName">The host name to map.</param>
    /// <returns>The <see cref="ServerSentEventsController"/> for the SSE endpoint.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the host name is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the host name is already mapped.</exception>
    public ServerSentEventsController SetVirtualHostNameToServerSentEvents(string hostName)
    {
        var hostname = hostName?.ToLower();

        if (string.IsNullOrEmpty(hostname))
            throw new ArgumentNullException(nameof(hostName));
        if (WebResourceHandlers.ContainsKey(hostname))
            throw new ArgumentException($"The host name '{hostname}' is already mapped.");

        var controller = new ServerSentEventsController();

        WebResourceHandlers[hostname] = (browser, frame, request) =>
        {
            return new SeverSentEventsResourceHandler(controller);
        };

        return controller;
    }

    /// <summary>
    /// Constructs a filter URL from the specified scheme and host name.
    /// </summary>
    /// <param name="scheme">The URL scheme.</param>
    /// <param name="hostName">The host name.</param>
    /// <returns>The constructed filter URL.</returns>
    private static string GetFilterUrl(string scheme, string hostName)
    {
        var url = $"{scheme}://{hostName}";

        if (url.Last() != '/') url += '/';

        url = url.ToLower();
        return url;
    }

}

