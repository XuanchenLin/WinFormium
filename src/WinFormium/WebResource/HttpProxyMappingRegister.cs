// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Provides extension methods for registering global HTTP proxy mappings in the application builder.
/// </summary>
public static class HttpProxyMappingRegister
{
    /// <summary>
    /// Registers a global HTTP proxy mapping for the specified scheme and domain name, redirecting requests to the given target.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> instance to extend.</param>
    /// <param name="scheme">The URL scheme to map (e.g., "http" or "https").</param>
    /// <param name="domainName">The domain name to match for proxy mapping.</param>
    /// <param name="redirectionTarget">The target URL to which requests should be redirected.</param>
    /// <returns>The <see cref="AppBuilder"/> instance for method chaining.</returns>
    public static AppBuilder UseGlobalHttpProxyMapping(this AppBuilder appBuilder, string scheme, string domainName, string redirectionTarget)
    {
        if (appBuilder.ProcessType == CefProcessId.Browser)
        {
            appBuilder.AddResourceSchemeHandlerFactory(new Func<WebResourceSchemeHandlerFactory>(() =>
            {
                return new HttpProxyMappingResourceSchemeHandlerFactory(scheme, domainName, redirectionTarget);
            }));
        }
        return appBuilder;
    }
}
