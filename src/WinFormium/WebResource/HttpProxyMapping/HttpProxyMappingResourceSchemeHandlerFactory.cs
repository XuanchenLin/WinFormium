// THIS FILE IS PART OF NanUI PROJECT
// THE NanUI PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.WebResource;
/// <summary>
/// Factory class for creating <see cref="HttpProxyMappingResourceHandler"/> instances
/// that handle web resource requests using a specified HTTP proxy mapping.
/// </summary>
class HttpProxyMappingResourceSchemeHandlerFactory : WebResourceSchemeHandlerFactory
{
    /// <summary>
    /// Gets the proxy address used for HTTP proxy mapping.
    /// </summary>
    public string Proxy { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpProxyMappingResourceSchemeHandlerFactory"/> class.
    /// </summary>
    /// <param name="scheme">The URI scheme to handle.</param>
    /// <param name="domainName">The domain name associated with the scheme.</param>
    /// <param name="proxy">The proxy address to use for mapping requests.</param>
    public HttpProxyMappingResourceSchemeHandlerFactory(string scheme, string domainName, string proxy)
        : base(scheme, domainName)
    {
        Proxy = proxy;
    }

    /// <summary>
    /// Returns a new <see cref="HttpProxyMappingResourceHandler"/> instance to handle the specified request.
    /// </summary>
    /// <param name="browser">The browser that originated the request.</param>
    /// <param name="frame">The frame that originated the request.</param>
    /// <param name="request">The request to handle.</param>
    /// <returns>
    /// A <see cref="CefResourceHandler"/> instance for handling the request, or <c>null</c> to allow default handling.
    /// </returns>
    protected override CefResourceHandler? GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return new HttpProxyMappingResourceHandler(browser, frame, request, Proxy);
    }
}


