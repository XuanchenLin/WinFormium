// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Provides extension methods for registering folder mapping resource handlers in the application builder.
/// </summary>
public static class FolderMappingRegister
{
    /// <summary>
    /// Registers a global virtual host name for folder mapping using the specified options.
    /// This enables serving static files from a local folder via a custom scheme and domain name.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> to configure.</param>
    /// <param name="optiions">The <see cref="FolderMappingOptions"/> specifying the folder mapping configuration.</param>
    /// <returns>The configured <see cref="AppBuilder"/> instance.</returns>
    public static AppBuilder UseGlobalVirtualHostNameForFolderMapping(this AppBuilder appBuilder, FolderMappingOptions optiions)
    {
        if (appBuilder.ProcessType == CefProcessId.Browser)
        {
            appBuilder.AddResourceSchemeHandlerFactory(new Func<WebResourceSchemeHandlerFactory>(() =>
            {
                return new FolderMappingResourceSchemeHandlerFactory(optiions);
            }));
        }


        return appBuilder;
    }
}
