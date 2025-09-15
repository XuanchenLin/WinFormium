// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Provides a base implementation for a scheme handler factory, supporting custom and standard schemes.
/// </summary>
public abstract class WebResourceSchemeHandlerFactory : CefSchemeHandlerFactory, IDisposable
{
    /// <summary>
    /// Handle to prevent garbage collection of this instance while registered.
    /// </summary>
    private GCHandle _gcHandler;

    /// <summary>
    /// Gets the scheme name (e.g., "http", "custom").
    /// </summary>
    public string Scheme { get; }

    /// <summary>
    /// Gets the domain name associated with this scheme handler.
    /// </summary>
    public string DomainName { get; }

    /// <summary>
    /// Gets a value indicating whether the scheme is a standard web scheme.
    /// </summary>
    public bool IsStandardScheme
    {
        get
        {
            return (Scheme?.ToLower()) switch
            {
                "http" or "https" or "file" or "ftp" or "about" or "data" => true,
                _ => false,
            };
        }
    }

    /// <summary>
    /// Stores the last created resource handler instance.
    /// </summary>
    private CefResourceHandler? _resourceHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebResourceSchemeHandlerFactory"/> class.
    /// </summary>
    /// <param name="scheme">The scheme name to handle.</param>
    /// <param name="domainName">The domain name associated with the scheme.</param>
    public WebResourceSchemeHandlerFactory(string scheme, string domainName)
    {
        _gcHandler = GCHandle.Alloc(this);

        Scheme = scheme;
        DomainName = domainName;
    }

    /// <summary>
    /// When implemented in a derived class, returns a <see cref="CefResourceHandler"/> to handle the request.
    /// </summary>
    /// <param name="browser">The browser that originated the request.</param>
    /// <param name="frame">The frame that originated the request.</param>
    /// <param name="request">The request object.</param>
    /// <returns>A <see cref="CefResourceHandler"/> instance or null to allow default handling.</returns>
    protected abstract CefResourceHandler? GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request);

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {

        _resourceHandler = GetResourceHandler(browser, frame, request);
        return _resourceHandler!;
    }

    /// <summary>
    /// Registers the scheme handler with the provided service provider.
    /// Can be overridden to perform custom registration logic.
    /// </summary>
    /// <param name="services">The service provider for registration.</param>
    internal protected virtual void ResourceSchemeHandlerRegister(IServiceProvider services)
    {
    }

    /// <inheritdoc/>
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _gcHandler.Free();
        }

        base.Dispose(isDisposing);

    }

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
