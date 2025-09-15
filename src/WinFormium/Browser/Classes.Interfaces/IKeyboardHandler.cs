// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling keyboard events in a browser instance.
/// </summary>
public interface IKeyboardHandler
{
    /// <summary>
    /// Called before a keyboard event is sent to the renderer. 
    /// Allows interception and optional handling of the event.
    /// </summary>
    /// <param name="browser">The browser generating the event.</param>
    /// <param name="keyEvent">The keyboard event data.</param>
    /// <param name="osEvent">The operating system event handle.</param>
    /// <param name="isKeyboardShortcut">
    /// Set to true if the event is a keyboard shortcut; otherwise, false.
    /// </param>
    /// <returns>
    /// Return true if the event was handled and should not be passed to the renderer; otherwise, false.
    /// </returns>
    bool OnPreKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent, out bool isKeyboardShortcut);

    /// <summary>
    /// Called after a keyboard event is sent to the renderer.
    /// Allows interception and optional handling of the event.
    /// </summary>
    /// <param name="browser">The browser generating the event.</param>
    /// <param name="keyEvent">The keyboard event data.</param>
    /// <param name="osEvent">The operating system event handle.</param>
    /// <returns>
    /// Return true if the event was handled and should not be passed to the renderer; otherwise, false.
    /// </returns>
    bool OnKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent);
}
