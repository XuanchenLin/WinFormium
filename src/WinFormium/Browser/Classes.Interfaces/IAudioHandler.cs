// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling audio stream events from a browser instance.
/// </summary>
public interface IAudioHandler
{
    /// <summary>
    /// Called to retrieve the audio parameters for the specified browser.
    /// </summary>
    /// <param name="browser">The browser instance requesting audio parameters.</param>
    /// <param name="parameters">The audio parameters to be set.</param>
    /// <returns>True if the parameters were set successfully; otherwise, false.</returns>
    bool GetAudioParameters(CefBrowser browser, CefAudioParameters parameters);

    /// <summary>
    /// Called when an error occurs in the audio stream.
    /// </summary>
    /// <param name="browser">The browser instance where the error occurred.</param>
    /// <param name="message">A message describing the error.</param>
    void OnAudioStreamError(CefBrowser browser, string message);

    /// <summary>
    /// Called when the audio stream has started.
    /// </summary>
    /// <param name="browser">The browser instance for which the audio stream started.</param>
    /// <param name="parameters">The audio parameters in use.</param>
    /// <param name="channels">The number of audio channels.</param>
    void OnAudioStreamStarted(CefBrowser browser, in CefAudioParameters parameters, int channels);

    /// <summary>
    /// Called when an audio packet is received from the stream.
    /// </summary>
    /// <param name="browser">The browser instance providing the audio packet.</param>
    /// <param name="data">Pointer to the audio data buffer.</param>
    /// <param name="frames">The number of audio frames in the packet.</param>
    /// <param name="pts">The presentation timestamp of the packet.</param>
    void OnAudioStreamPacket(CefBrowser browser, nint data, int frames, long pts);

    /// <summary>
    /// Called when the audio stream has stopped.
    /// </summary>
    /// <param name="browser">The browser instance for which the audio stream stopped.</param>
    void OnAudioStreamStopped(CefBrowser browser);
}
