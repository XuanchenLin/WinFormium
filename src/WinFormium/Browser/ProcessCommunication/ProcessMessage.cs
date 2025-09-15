// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.ProcessCommunication;

/// <summary>
/// Represents a process message in the context of browser process communication.
/// </summary>
/// <param name="browser">The browser instance associated with the message.</param>
/// <param name="frame">The frame in which the message is processed.</param>
/// <param name="processId">The process ID indicating the target process.</param>
/// <param name="message">The process message being communicated.</param>
class ProcessMessage(CefBrowser browser, CefFrame frame, CefProcessId processId, CefProcessMessage message)
{
    /// <summary>
    /// Gets the browser instance associated with the message.
    /// </summary>
    public CefBrowser Browser { get; } = browser;

    /// <summary>
    /// Gets the frame in which the message is processed.
    /// </summary>
    public CefFrame Frame { get; } = frame;

    /// <summary>
    /// Gets the process ID indicating the target process.
    /// </summary>
    public CefProcessId ProcessId { get; } = processId;

    /// <summary>
    /// Gets the process message being communicated.
    /// </summary>
    public CefProcessMessage Message { get; } = message;
}
