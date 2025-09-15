// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Partial implementation of <see cref="IFrameHandler"/> for handling browser frame events in <see cref="WebViewCore"/>.
/// </summary>
partial class WebViewCore : IFrameHandler
{
    /// <inheritdoc/>
    void IFrameHandler.OnFrameAttached(CefBrowser browser, CefFrame frame, bool reattached)
    {
        BrowserClient?.FrameHandler?.OnFrameAttached(browser, frame, reattached);

        //Console.WriteLine($"[BROWSER(BrowserID:{browser.Identifier})] -> OnFrameAttached 0x{frame.Identifier:X}");

    }

    /// <inheritdoc/>
    void IFrameHandler.OnFrameCreated(CefBrowser browser, CefFrame frame)
    {
        BrowserClient?.FrameHandler?.OnFrameCreated(browser, frame);

        //Console.WriteLine($"[BROWSER(BrowserID:{browser.Identifier})] -> OnFrameCreated {frame.Identifier:X}");
    }

    /// <inheritdoc/>
    void IFrameHandler.OnFrameDetached(CefBrowser browser, CefFrame frame)
    {
        BrowserClient?.FrameHandler?.OnFrameDetached(browser, frame);

        //Console.WriteLine($"[BROWSER(BrowserID:{browser.Identifier})] -> OnFrameDetached 0x{frame.Identifier:X}");

    }

    /// <inheritdoc/>
    void IFrameHandler.OnMainFrameChanged(CefBrowser browser, CefFrame? oldFrame, CefFrame? newFrame)
    {
        MainFrameChanging = true;

        BrowserClient?.FrameHandler?.OnMainFrameChanged(browser, oldFrame, newFrame);

        //Console.WriteLine($"[BROWSER(BrowserID:{browser.Identifier})] -> OnMainFrameChanged_{oldFrame?.Identifier}->{newFrame?.Identifier}");

    }
}


