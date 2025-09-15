// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Partial implementation of <see cref="ILifeSpanHandler"/> for handling browser life span events in <c>WebViewCore</c>.
/// </summary>
partial class WebViewCore : ILifeSpanHandler
{
    /// <summary>
    /// Indicates whether the browser is in the process of closing.
    /// </summary>
    private bool _isClosing = false;

    /// <summary>
    /// Gets a value indicating whether the browser is closing.
    /// </summary>
    public bool IsClosing => _isClosing;

    /// <inheritdoc/>
    void ILifeSpanHandler.OnAfterCreated(CefBrowser browser)
    {
        Browser = browser;

        if (BrowserHost == null)
        {
            throw new NullReferenceException(nameof(BrowserHost));
        }


        CommunicationBridge = new ProcessCommunicationBridge(browser, CefProcessId.Browser);

        CommunicationBridge.RegisterProcessCommunicationBridgeHandler(JsEngine = new JavaScriptEngine(CommunicationBridge)
        {
            WebMessageReceived = OnWebMessageReceivedCore
        });

        Initialized = true;


        Resize();

        if (!string.IsNullOrEmpty(_defferedUrl))
        {
            Browser.GetMainFrame().LoadUrl(_defferedUrl);
            _defferedUrl = null;
        }

        OnBrowserCreatedCore(browser);

        BrowserClient?.LifeSpanHandler?.OnAfterCreated(browser);
    }

    /// <summary>
    /// Delegate to handle web message received events from JavaScript.
    /// </summary>
    public required WebMessageReceivedDelegate WebMessageReceived { get; init; }

    /// <summary>
    /// Invokes the <see cref="WebMessageReceived"/> delegate with the specified arguments.
    /// </summary>
    /// <param name="args">The event arguments containing the web message.</param>
    private void OnWebMessageReceivedCore(WebMessageReceivedEventArgs args)
    {
        WebMessageReceived.Invoke(args);
    }

    /// <inheritdoc/>
    bool ILifeSpanHandler.OnBeforePopup(
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
        ref bool noJavascriptAccess)
    {
        var retval = BrowserClient?.LifeSpanHandler?.OnBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref extraInfo, ref noJavascriptAccess) ?? false;

        if (retval)
        {
            return true;
        }

        var useEmbeddedBrowser = WinFormiumApp.Current?.EnableEmbeddedBrowser ?? false;

        if (!useEmbeddedBrowser)
        {
            var ps = new System.Diagnostics.ProcessStartInfo(targetUrl)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            System.Diagnostics.Process.Start(ps);

            return true;
        }

        noJavascriptAccess = false;
        return false;
    }

    /// <inheritdoc/>
    bool ILifeSpanHandler.DoClose(CefBrowser browser)
    {
        if (browser.IsPopup)
        {
            return false;
        }

        var retval = BrowserClient?.LifeSpanHandler?.DoClose(browser) ?? false;

        if (browser.Identifier == Browser?.Identifier)
        {
            _isClosing = !retval;
        }

        return retval;
    }

    /// <inheritdoc/>
    void ILifeSpanHandler.OnBeforeClose(CefBrowser browser)
    {
        BrowserClient?.LifeSpanHandler?.OnBeforeClose(browser);
        browser.Dispose();

        GC.SuppressFinalize(this);
    }
}


