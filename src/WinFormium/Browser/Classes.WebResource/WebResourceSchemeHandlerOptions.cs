// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Provides options for the web resource scheme handler, including default file names to use when resolving resources.
/// </summary>
public sealed class WebResourceSchemeHandlerOptions
{
    /// <summary>
    /// Gets or sets the list of default file names to use when a directory is requested.
    /// </summary>
    public string[] DefaultFileName { get; set; } = new string[] { "index.html", "index.htm", "default.html" };
}
