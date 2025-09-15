// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;
/// <summary>
/// Factory class for creating <see cref="FolderMappingResourceHandler"/> instances
/// to handle custom scheme requests mapped to local folders.
/// </summary>
class FolderMappingResourceSchemeHandlerFactory : WebResourceSchemeHandlerFactory
{
    /// <summary>
    /// Gets the options used for folder mapping.
    /// </summary>
    public FolderMappingOptions Options { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderMappingResourceSchemeHandlerFactory"/> class
    /// with the specified <see cref="FolderMappingOptions"/>.
    /// </summary>
    /// <param name="options">The folder mapping options.</param>
    public FolderMappingResourceSchemeHandlerFactory(FolderMappingOptions options)
        : base(options.Scheme, options.DomainName)
    {
        Options = options;
    }

    /// <summary>
    /// Creates a new <see cref="FolderMappingResourceHandler"/> to handle the incoming request.
    /// </summary>
    /// <param name="browser">The browser instance that originated the request.</param>
    /// <param name="frame">The frame that originated the request.</param>
    /// <param name="request">The request object.</param>
    /// <returns>A <see cref="CefResourceHandler"/> instance for handling the request.</returns>
    protected override CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return new FolderMappingResourceHandler(browser, frame, request, Options);
    }
}
