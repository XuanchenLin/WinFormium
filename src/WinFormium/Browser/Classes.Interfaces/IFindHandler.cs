// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling find results in the browser.
/// Implement this interface to receive notifications about the results of find operations.
/// </summary>
public interface IFindHandler
{
    /// <summary>
    /// Called to report find results.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance where the find operation was performed.</param>
    /// <param name="identifier">The identifier for the find request.</param>
    /// <param name="count">The number of matches found.</param>
    /// <param name="selectionRect">The <see cref="CefRectangle"/> representing the location of the current match.</param>
    /// <param name="activeMatchOrdinal">The 1-based ordinal of the currently selected match.</param>
    /// <param name="finalUpdate">True if this is the final update for the find operation.</param>
    /// <returns>
    /// Return true if the result was handled, or false to allow default processing.
    /// </returns>
    bool OnFindResult(CefBrowser browser, int identifier, int count, CefRectangle selectionRect, int activeMatchOrdinal, bool finalUpdate);
}
