// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
public partial class Formium : ILifeSpanHandler
{
    /// <inheritdoc cref="ILifeSpanHandler.DoClose(CefBrowser)"/>
    bool ILifeSpanHandler.DoClose(CefBrowser browser)
    {
        var retval = LifeSpanHandler?.DoClose(browser) ?? false;

        if (!retval)
        {
            var args = new BrowserClosingEventArgs(browser);
            InvokeIfRequired(() =>
            {
                BrowserClosing?.Invoke(this, args);
            });
            retval = args.Cancel;
        }

        return retval;
    }

    /// <inheritdoc cref="ILifeSpanHandler.OnAfterCreated(CefBrowser)"/>
    void ILifeSpanHandler.OnAfterCreated(CefBrowser browser)
    {

        LifeSpanHandler?.OnAfterCreated(browser);

        InvokeIfRequired(() =>
        {
            OnBrowserCreatedCore(browser);
            BrowserCreated?.Invoke(this, new BrowserEventArgs(browser));
            Load?.Invoke(this, new BrowserEventArgs(browser));

        });
    }

    /// <inheritdoc cref="ILifeSpanHandler.OnBeforeClose(CefBrowser)"/>
    void ILifeSpanHandler.OnBeforeClose(CefBrowser browser)
    {
        LifeSpanHandler?.OnBeforeClose(browser);

        InvokeIfRequired(() =>
        {
            OnBrowserClosed(browser);
            BrowserClosed?.Invoke(this, new BrowserEventArgs(browser));

            if (IsOffScreenRendering)
            {
                Close();
            }
        });
    }

    /// <inheritdoc cref="ILifeSpanHandler.OnBeforePopup(CefBrowser, CefFrame, string, string, CefWindowOpenDisposition, bool, CefPopupFeatures, CefWindowInfo, ref CefClient, CefBrowserSettings, ref CefDictionaryValue, ref bool)"/>
    bool ILifeSpanHandler.OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
    {

        return OnBrowserBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref extraInfo, ref noJavascriptAccess);
    }

    /// <summary>
    /// Occurs when the browser is closing.
    /// </summary>
    public event EventHandler<BrowserClosingEventArgs>? BrowserClosing;

    /// <summary>
    /// Occurs when the browser is created.
    /// </summary>
    public event EventHandler<BrowserEventArgs>? BrowserCreated;

    /// <summary>
    /// Occurs when the browser is closed.
    /// </summary>
    public event EventHandler<BrowserEventArgs>? BrowserClosed;


    /// <summary>
    /// Called before a new popup browser is created. Allows modification of popup parameters or cancellation of the popup.
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
    protected virtual bool OnBrowserBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
    {
        return false;
    }

    /// <summary>
    /// Occurs when the browser is created.
    /// </summary>
    /// <param name="browser">
    /// The <see cref="CefBrowser"/> that is created.
    /// </param>
    protected virtual void OnBrowserCreated(CefBrowser browser)
    {

    }


    /// <summary>
    /// Occurs when the browser is closed.
    /// </summary>
    /// <param name="browser">
    /// The <see cref="CefBrowser"/> that is closed.
    /// </param>
    protected virtual void OnBrowserClosed(CefBrowser browser)
    {
    }




}
