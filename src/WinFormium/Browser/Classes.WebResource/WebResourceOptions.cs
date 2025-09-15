// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


namespace WinFormium.Browser;

/// <summary>
/// A delegate that will be called when the resource file is not found.
/// </summary>
/// <param name="requestUrl">
/// The request url.
/// </param>
/// <returns>
/// Returns a <see cref="string"/> value indicating a exist path that will handle this request url.
/// </returns>
public delegate string ResourceFileFallbackDelegate(string requestUrl);

/// <summary>
/// Represents the base options for configuring a web resource.
/// </summary>
public abstract class WebResourceOptions
{
    /// <summary>
    /// Gets the scheme used for the web resource (e.g., "http").
    /// </summary>
    public string Scheme { get; init; } = "http";

    /// <summary>
    /// Gets the domain name associated with the web resource.
    /// </summary>
    public required string DomainName { get; init; }

    /// <summary>
    /// Gets the delegate that will be called when the resource file is not found.
    /// </summary>
    public ResourceFileFallbackDelegate? OnFallback { get; init; }
}
