// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that is raised when the address (URL) of a frame changes in a browser.
/// </summary>
public class FrameAddressChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the address change event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the <see cref="CefFrame"/> instance whose address has changed.
    /// </summary>
    public CefFrame Frame { get; }

    /// <summary>
    /// Gets the new address (URL) of the frame.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameAddressChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser instance where the address change occurred.</param>
    /// <param name="frame">The frame whose address has changed.</param>
    /// <param name="address">The new address (URL) of the frame.</param>
    internal FrameAddressChangeEventArgs(CefBrowser browser, CefFrame frame, string address)
    {
        Browser = browser;
        Frame = frame;
        Address = address;
    }
}
