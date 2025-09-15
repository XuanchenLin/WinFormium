// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the event that occurs when media access permissions change in a browser instance.
/// </summary>
public class MediaAccessChangeEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MediaAccessChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance where the media access change occurred.</param>
    /// <param name="hasVideoAccess">A value indicating whether video access is granted.</param>
    /// <param name="hasAudioAccess">A value indicating whether audio access is granted.</param>
    internal MediaAccessChangeEventArgs(CefBrowser browser, bool hasVideoAccess, bool hasAudioAccess)
    {
        Browser = browser;
        HasVideoAccess = hasVideoAccess;
        HasAudioAccess = hasAudioAccess;
    }

    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance associated with the media access change event.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets a value indicating whether video access is currently granted.
    /// </summary>
    public bool HasVideoAccess { get; }

    /// <summary>
    /// Gets a value indicating whether audio access is currently granted.
    /// </summary>
    public bool HasAudioAccess { get; }
}
