// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

public partial class Formium : IKeyboardHandler
{
    bool IKeyboardHandler.OnKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent)
    {
        var retval = KeyboardHandler?.OnKeyEvent(browser, keyEvent, osEvent) ?? false;

        if (!retval)
        {

            InvokeIfRequired(() =>
            {
                var args = new BrowserKeyEventArgs(browser, keyEvent, osEvent);

                OnBrowserKeyEvent(browser, args);

                BrowserKeyEvent?.Invoke(this, args);

                retval = args.Handled;
            });
        }

        return retval;
    }



    bool IKeyboardHandler.OnPreKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent, out bool isKeyboardShortcut)
    {
        var retval = KeyboardHandler?.OnPreKeyEvent(browser, keyEvent, osEvent, out isKeyboardShortcut) ?? false;

        isKeyboardShortcut = false;

        if (!retval)
        {

            InvokeIfRequired(() =>
            {
                var args = new BrowserPreKeyEventArgs(browser, keyEvent, osEvent);
                OnBrowserPreKeyEvent(browser, args);
                BrowserPreKeyEvent?.Invoke(this, args);

                retval = args.Handled;
            });
        }


        return retval;
    }


    /// <summary>
    /// Occurs when a key event is received from the browser.
    /// </summary>
    public event EventHandler<BrowserKeyEventArgs> BrowserKeyEvent;
    /// <summary>
    /// Occurs when a pre-key event is received from the browser.
    /// </summary>
    public event EventHandler<BrowserPreKeyEventArgs> BrowserPreKeyEvent;


    /// <summary>
    /// Occurs when a key event is received from the browser.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="args">The key event arguments.</param>
    protected virtual void OnBrowserKeyEvent(CefBrowser browser, BrowserKeyEventArgs args)
    {
    }

    /// <summary>
    /// Occurs when a pre-key event is received from the browser.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="args">The pre-key event arguments.</param>
    private void OnBrowserPreKeyEvent(CefBrowser browser, BrowserPreKeyEventArgs args)
    {
    }
}
