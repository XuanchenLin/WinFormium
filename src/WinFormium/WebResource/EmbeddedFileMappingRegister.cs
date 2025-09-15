// THIS FILE IS PART OF NanUI PROJECT
// THE NanUI PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium;
/// <summary>
/// Provides extension methods for registering embedded file mapping resource handlers in the application builder.
/// </summary>
public static class EmbeddedFileMappingRegister
{
    /// <summary>
    /// Registers a global virtual host name for assembly embedded file mapping using the specified options.
    /// This method adds a <see cref="WebResourceSchemeHandlerFactory"/> for embedded file mapping to the application's resource scheme handler factories
    /// if the current process type is <see cref="CefProcessId.Browser"/>.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> instance to extend.</param>
    /// <param name="options">The <see cref="EmbeddedFileMappingOptions"/> to use for the embedded file mapping resource handler.</param>
    /// <returns>The <see cref="AppBuilder"/> instance for chaining.</returns>
    public static AppBuilder UseGlobalVirtualHostNameForAssemblyEmbeddedFileMapping(this AppBuilder appBuilder, EmbeddedFileMappingOptions options)
    {
        if (appBuilder.ProcessType == CefProcessId.Browser)
        {
            appBuilder.AddResourceSchemeHandlerFactory(new Func<WebResourceSchemeHandlerFactory>(() =>
            {
                return new EmbeddedFileMappingResourceSchemeHandlerFactory(options);
            }));
        }

        return appBuilder;
    }
}
