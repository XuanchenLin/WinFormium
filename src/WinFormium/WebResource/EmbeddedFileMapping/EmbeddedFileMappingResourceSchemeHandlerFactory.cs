// THIS FILE IS PART OF NanUI PROJECT
// THE NanUI PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.WebResource;

/// <summary>
/// Factory class for creating <see cref="EmbeddedFileMappingResourceHandler"/> instances
/// to handle requests for embedded file mapping web resources.
/// </summary>
class EmbeddedFileMappingResourceSchemeHandlerFactory : WebResourceSchemeHandlerFactory
{
    /// <summary>
    /// Gets the options used for embedded file mapping.
    /// </summary>
    public EmbeddedFileMappingOptions Options { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedFileMappingResourceSchemeHandlerFactory"/> class
    /// with the specified <see cref="EmbeddedFileMappingOptions"/>.
    /// </summary>
    /// <param name="options">The options for embedded file mapping.</param>
    public EmbeddedFileMappingResourceSchemeHandlerFactory(EmbeddedFileMappingOptions options)
        : base(options.Scheme, options.DomainName)
    {
        Options = options;
    }

    /// <summary>
    /// Creates a new <see cref="EmbeddedFileMappingResourceHandler"/> to handle the specified request.
    /// </summary>
    /// <param name="browser">The browser that originated the request.</param>
    /// <param name="frame">The frame that originated the request.</param>
    /// <param name="request">The request to handle.</param>
    /// <returns>
    /// A <see cref="CefResourceHandler"/> instance to handle the request.
    /// </returns>
    protected override CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return new EmbeddedFileMappingResourceHandler(browser, frame, request, Options);
    }
}
