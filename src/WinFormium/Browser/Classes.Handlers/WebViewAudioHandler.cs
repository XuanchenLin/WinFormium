// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Provides an implementation of <see cref="CefAudioHandler"/> that delegates audio stream events to an <see cref="IAudioHandler"/> instance.
/// </summary>
class WebViewAudioHandler : CefAudioHandler
{
    /// <summary>
    /// Gets the <see cref="IAudioHandler"/> instance used to handle audio events.
    /// </summary>
    public IAudioHandler Handler { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebViewAudioHandler"/> class with the specified <see cref="IAudioHandler"/>.
    /// </summary>
    /// <param name="handler">The <see cref="IAudioHandler"/> to delegate audio events to.</param>
    public WebViewAudioHandler(IAudioHandler handler)
    {
        Handler = handler;
    }

    /// <inheritdoc/>
    protected override bool GetAudioParameters(CefBrowser browser, CefAudioParameters parameters)
    {
        return Handler.GetAudioParameters(browser, parameters);
    }

    /// <inheritdoc/>
    protected override void OnAudioStreamError(CefBrowser browser, string message)
    {
        Handler.OnAudioStreamError(browser, message);
    }

    /// <inheritdoc/>
    protected override void OnAudioStreamPacket(CefBrowser browser, nint data, int frames, long pts)
    {
        Handler.OnAudioStreamPacket(browser, data, frames, pts);
    }

    /// <inheritdoc/>
    protected override void OnAudioStreamStarted(CefBrowser browser, in CefAudioParameters parameters, int channels)
    {
        Handler.OnAudioStreamStarted(browser, parameters, channels);
    }

    /// <inheritdoc/>
    protected override void OnAudioStreamStopped(CefBrowser browser)
    {
        Handler.OnAudioStreamStopped(browser);
    }
}
