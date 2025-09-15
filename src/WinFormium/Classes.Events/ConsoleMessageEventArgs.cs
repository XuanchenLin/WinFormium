// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the console message event raised by the browser.
/// </summary>
public class ConsoleMessageEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMessageEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance that generated the console message.</param>
    /// <param name="level">The severity level of the console message.</param>
    /// <param name="message">The content of the console message.</param>
    /// <param name="source">The source file or URL of the console message.</param>
    /// <param name="line">The line number in the source where the message originated.</param>
    internal ConsoleMessageEventArgs(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
    {
        Level = level;
        Message = message;
        Source = source;
        Line = line;
    }

    /// <summary>
    /// Gets the severity level of the console message.
    /// </summary>
    public CefLogSeverity Level { get; }

    /// <summary>
    /// Gets the content of the console message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the source file or URL of the console message.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Gets the line number in the source where the message originated.
    /// </summary>
    public int Line { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the console message event has been handled.
    /// If set to <c>true</c>, the default handling will be suppressed.
    /// </summary>
    public bool Handled { get; set; } = false;
}
