// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;


using WinFormium.Browser;
using WinFormium.Browser.CefGlue;


namespace WinFormium.BlazorHybrid;


/// <summary>
/// Provides an abstract base class for integrating Blazor with WinFormium.
/// Handles initialization, resource management, and communication between the browser and Blazor components.
/// </summary>
public abstract class BlazorFormium : Formium
{


    /// <summary>
    /// The service collection used to register Blazor and application services.
    /// </summary>
    private ServiceCollection _serviceCollection = new();

    /// <summary>
    /// Indicates whether static resources are embedded.
    /// </summary>
    private bool _isEmbdeedeStaticResources =>
    Options.StaticResources is not null;

    /// <summary>
    /// The options for configuring Blazor integration.
    /// </summary>
    public abstract BlazorFormiumOptions Options { get; }

    /// <summary>
    /// The file provider for serving static or embedded resources.
    /// </summary>
    private IFileProvider? _fileProvider;

    /// <summary>
    /// The manager responsible for handling Blazor webview operations.
    /// </summary>
    private BlazorFormiumWebViewManager? _formiumWebViewManager;

    /// <summary>
    /// The relative path to the host page within the content root.
    /// </summary>
    private string? _relativePath;

    /// <summary>
    /// The relative path to the root folder containing the host page.
    /// </summary>
    private string? _rootFolderPath;

    /// <summary>
    /// Gets the base address of the Blazor server.
    /// </summary>
    protected string BlazorServerAddress => $"{Options.Scheme}://{Options.DomainName}";

    /// <summary>
    /// Gets or sets the path to the Blazor host page (typically index.html).
    /// </summary>
    public string HostPage { get; init; } = Path.Combine("wwwroot", "index.html");

    /// <summary>
    /// Gets the type of the root Blazor components to render.
    /// </summary>
    private RootComponentsCollection RootComponents => Options.RootComponents;

    /// <summary>
    /// Gets or sets the namespace for embedded static resources.
    /// </summary>
    public string? StaticResourcesNamespace { get; init; }

    /// <summary>
    /// Gets the application root directory.
    /// </summary>
    internal string AppRootDir
    {
        get
        {
            string appRootDir;
#pragma warning disable IL3000
            var entryAssemblyLocation = Assembly.GetEntryAssembly()?.Location;
#pragma warning restore IL3000
            if (!string.IsNullOrEmpty(entryAssemblyLocation))
            {
                appRootDir = Path.GetDirectoryName(entryAssemblyLocation)!;
            }
            else
            {
                appRootDir = AppContext.BaseDirectory;
            }

            return appRootDir;
        }
    }

    /// <summary>
    /// Gets the default file names to look for when serving directory requests.
    /// </summary>
    protected virtual string[] DefaultFileName { get; } = ["index.html", "index.htm", "default.html"];

    /// <summary>
    /// Gets the <see cref="BlazorFormiumWebViewManager"/> instance, initializing it if necessary.
    /// </summary>
    internal BlazorFormiumWebViewManager FormiumWebViewManager
    {
        get
        {
            if (_formiumWebViewManager is null)
            {
                _serviceCollection.AddBlazorWebView();

                Options.ConfigureServices?.Invoke(_serviceCollection);

                var hostPageFullPath = Path.GetFullPath(Path.Combine(AppRootDir, HostPage));
                var contentRootDirFullPath = Path.GetDirectoryName(hostPageFullPath)!;
                var contentRootRelativePath = Path.GetRelativePath(AppRootDir, contentRootDirFullPath);
                var hostPageRelativePath = Path.GetRelativePath(contentRootDirFullPath, hostPageFullPath);

                _relativePath = hostPageRelativePath;
                _rootFolderPath = contentRootRelativePath;

                List<IFileProvider> providers = new List<IFileProvider>();


                if (Options.StaticResources is not null && Options.StaticResources.Count > 0)
                {
                    foreach (var staticResource in Options.StaticResources)
                    {
                        if (staticResource.ResourcesAssembly is not null)
                        {
                            providers.Add(new EmbeddedFileProvider(staticResource.ResourcesAssembly, staticResource.BaseNamespace));
                        }
                    }
                }

                if (Directory.Exists(contentRootDirFullPath))
                {
                    providers.Add(new PhysicalFileProvider(contentRootDirFullPath));
                }

                if (providers.Count == 0)
                {
                    throw new InvalidOperationException($"The content root directory '{contentRootDirFullPath}' does not exist.");
                }

                _fileProvider = new CompositeFileProvider(providers);


                _formiumWebViewManager = new BlazorFormiumWebViewManager(this, _serviceCollection.BuildServiceProvider(), new Uri(BlazorServerAddress), _fileProvider, RootComponents.JSComponents, contentRootRelativePath, hostPageRelativePath, Options);

                foreach (var rootComponent in RootComponents)
                {

                    // Since the page isn't loaded yet, this will always complete synchronously
                    _ = rootComponent.AddToWebViewManagerAsync(_formiumWebViewManager);
                }
            }

            return _formiumWebViewManager;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorFormium"/> class.
    /// </summary>
    public BlazorFormium()
    {
        // Initialize Blazor-specific components or services here if needed
    }

    /// <summary>
    /// Handles web resource requests from the browser, serving static/embedded files or delegating to the Blazor webview manager.
    /// </summary>
    /// <param name="browser">The browser making the request.</param>
    /// <param name="frame">The frame making the request.</param>
    /// <param name="request">The web request.</param>
    /// <returns>A <see cref="WebResourceResponse"/> if handled; otherwise, the base implementation result.</returns>
    protected override WebResourceResponse? RequestWebResource(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        string RemovePossibleQueryString(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            var indexOfQueryString = url.IndexOf('?', StringComparison.Ordinal);
            return (indexOfQueryString == -1)
                ? url
                : url.Substring(0, indexOfQueryString);
        }

        var url = RemovePossibleQueryString(request.Url);

        // Check if the request is for the Blazor server address
        if (url.StartsWith(BlazorServerAddress, StringComparison.OrdinalIgnoreCase) && FormiumWebViewManager != null && _fileProvider != null)
        {
            var postData = new List<byte>();
            var uploadFiles = new List<string>();

            if (request.PostData != null)
            {
                var items = request.PostData.GetElements();

                if (items != null && items.Length > 0)
                {
                    foreach (var item in items)
                    {
                        var buffer = item.GetBytes();

                        switch (item.ElementType)
                        {
                            case CefPostDataElementType.Bytes:
                                postData.AddRange(buffer);
                                break;
                            case CefPostDataElementType.File:
                                uploadFiles.Add(item.GetFile());
                                break;
                        }
                    }
                }
            }

            var webRequest = new WebResourceRequest(new Uri(request.Url), request.Method, request.GetHeaderMap(), postData.ToArray(), uploadFiles.ToArray(), request);

            if (_isEmbdeedeStaticResources)
            {
                var namespaces = Options.StaticResources.Where(x=> !string.IsNullOrEmpty(x.BaseNamespace)).Select(x =>  x.BaseNamespace!);

                foreach (var ns in namespaces)
                {

                    var resourceName = GetResourceName(ns, webRequest.RelativePath, _rootFolderPath);
                    var fileInfo = _fileProvider.GetFileInfo(resourceName);

                    if (!fileInfo.Exists && !webRequest.HasFileName)
                    {
                        foreach (var defaultFileName in DefaultFileName)
                        {
                            resourceName = string.Join(".", resourceName, defaultFileName);
                            fileInfo = _fileProvider.GetFileInfo(resourceName);

                            if (fileInfo.Exists)
                            {
                                break;
                            }
                        }
                    }

                    if (!fileInfo.Exists && Options.OnFallback is not null)
                    {
                        var fallbackFile = Options.OnFallback.Invoke(url);

                        resourceName = GetResourceName(ns, fallbackFile, _rootFolderPath);

                        fileInfo = _fileProvider.GetFileInfo(resourceName);
                    }

                    if (fileInfo.Exists)
                    {
                        return new WebResourceResponse
                        {
                            ContentBody = new AutoCloseStream(fileInfo.CreateReadStream()),
                            ContentType = WebResourceHandler.GetMimeType(fileInfo.Name) ?? "application/octet-stream",
                            HttpStatus = WebResourceStatusCodes.Status200OK,
                        };
                    }

                }


            }

            var uri = new Uri(url);

            if (uri.PathAndQuery == "/")
            {
                url += _relativePath;
            }

            if (FormiumWebViewManager.TryGetResponseContent(url, out var statusCode, out var statusMessage, out var content, out var headers))
            {
                var response = new WebResourceResponse()
                {
                    HttpStatus = statusCode,
                    ContentBody = new AutoCloseStream(content),
                    ContentType = headers.TryGetValue("Content-Type", out var contentType) ? contentType : "application/octet-stream"
                };

                foreach (var header in headers)
                {
                    response.Headers[header.Key] = header.Value;
                }

                return response;
            }
            else
            {
                return new WebResourceResponse()
                {
                    HttpStatus = statusCode,
                };
            }
        }

        return base.RequestWebResource(browser, frame, request);
    }

    /// <summary>
    /// Gets the resource name for an embedded resource based on the relative path and root path.
    /// </summary>
    /// <param name="relativePath">The relative path to the resource.</param>
    /// <param name="rootPath">The root path, if any.</param>
    /// <returns>The fully qualified resource name.</returns>
    private string GetResourceName(string baseNamespace, string relativePath, string? rootPath = null)
    {
        var filePath = relativePath;
        if (!string.IsNullOrEmpty(rootPath))
        {
            filePath = $"{rootPath?.Trim('/', '\\')}/{filePath.Trim('/', '\\')}";
        }

        filePath = filePath.Replace('\\', '/');

        var endTrimIndex = filePath.LastIndexOf('/');

        if (endTrimIndex > -1)
        {
            // https://stackoverflow.com/questions/5769705/retrieving-embedded-resources-with-special-characters

            var path = filePath.Substring(0, endTrimIndex);
            path = path.Replace("/", ".");
            if (Regex.IsMatch(path, "\\.(\\d+)"))
            {
                path = Regex.Replace(path, "\\.(\\d+)", "._$1");
            }

            const string replacePartterns = "`~!@$%^&(),-=";

            foreach (var parttern in replacePartterns)
            {
                path = path.Replace(parttern, '_');
            }

            filePath = $"{path}{filePath.Substring(endTrimIndex)}".Trim('/');
        }

        var resourceName = $"{baseNamespace}.{filePath.Replace('/', '.')}";

        return resourceName;
    }
}
