// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Partial implementation of <see cref="IFrameHandler"/> for handling browser frame events in <see cref="Formium"/>.
/// </summary>
public partial class Formium : IFrameHandler
{
    /// <inheritdoc/>
    void IFrameHandler.OnFrameAttached(CefBrowser browser, CefFrame frame, bool reattached)
    {
        FrameHandler?.OnFrameAttached(browser, frame, reattached);
    }

    /// <inheritdoc/>
    void IFrameHandler.OnFrameCreated(CefBrowser browser, CefFrame frame)
    {
        FrameHandler?.OnFrameCreated(browser, frame);
    }

    /// <inheritdoc/>
    void IFrameHandler.OnFrameDetached(CefBrowser browser, CefFrame frame)
    {
        FrameHandler?.OnFrameDetached(browser, frame);
    }

    /// <inheritdoc/>
    void IFrameHandler.OnMainFrameChanged(CefBrowser browser, CefFrame? oldFrame, CefFrame? newFrame)
    {
        FrameHandler?.OnMainFrameChanged(browser, oldFrame, newFrame);
    }
}
