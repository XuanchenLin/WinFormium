// THIS FILE IS PART OF NanUI PROJECT
// THE NanUI PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.WebResource;
/// <summary>
/// Handles web resource requests by mapping them to embedded files within an assembly.
/// </summary>
class EmbeddedFileMappingResourceHandler : WebResourceHandler
{
    /// <summary>
    /// Gets the associated CefBrowser instance.
    /// </summary>
    public CefBrowser Browser { get; }
    /// <summary>
    /// Gets the associated CefFrame instance.
    /// </summary>
    public CefFrame Frame { get; }
    /// <summary>
    /// Gets the CefRequest for the current resource request.
    /// </summary>
    public CefRequest Request { get; }
    /// <summary>
    /// Gets the options for embedded file mapping.
    /// </summary>
    public EmbeddedFileMappingOptions Options { get; }
    /// <summary>
    /// Gets the assembly containing the embedded resources.
    /// </summary>
    public Assembly ResourceAssembly => Options.ResourceAssembly;

    /// <summary>
    /// Gets the default namespace used for embedded resources.
    /// </summary>
    public string DefaultNamespace => Options.DefaultNamespace ?? ResourceAssembly.EntryPoint?.DeclaringType?.Namespace ?? ResourceAssembly.GetName().Name!;

    /// <summary>
    /// Gets a value indicating whether CORS policy is enabled.
    /// </summary>
    protected override bool EnableCORSPolicy => true;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedFileMappingResourceHandler"/> class.
    /// </summary>
    /// <param name="browser">The CefBrowser instance.</param>
    /// <param name="frame">The CefFrame instance.</param>
    /// <param name="request">The CefRequest instance.</param>
    /// <param name="options">The embedded file mapping options.</param>
    public EmbeddedFileMappingResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request, EmbeddedFileMappingOptions options)
    {
        Browser = browser;
        Frame = frame;
        Request = request;
        Options = options;
    }

    /// <summary>
    /// Gets the resource name for an embedded resource based on the relative path and root path.
    /// </summary>
    /// <param name="relativePath">The relative path of the resource.</param>
    /// <param name="rootPath">The root directory within the embedded resources.</param>
    /// <returns>The fully qualified resource name.</returns>
    private string GetResourceName(string relativePath, string? rootPath)
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

        var resourceName = $"{DefaultNamespace}.{filePath.Replace('/', '.')}";

        return resourceName;
    }

    /// <summary>
    /// Gets the web resource response for the specified request by mapping it to an embedded resource.
    /// </summary>
    /// <param name="request">The web resource request.</param>
    /// <returns>A <see cref="WebResourceResponse"/> containing the response data or a 404 status if not found.</returns>
    protected override WebResourceResponse GetResourceResponse(WebResourceRequest request)
    {
        var requestUrl = request.RequestUrl;

        var mainAssembly = ResourceAssembly;

        var response = new WebResourceResponse();

        if (request.Method != WebResourceRequestMethod.GET || mainAssembly is null)
        {
            response.HttpStatus = WebResourceStatusCodes.Status404NotFound;

            return response;
        }

        var resourceName = GetResourceName(request.RelativePath, Options.EmbeddedResourceDirectoryName);

        Assembly? satelliteAssembly = null;

        try
        {
            var assemblyLocation = string.IsNullOrEmpty(mainAssembly.Location) ? Application.ExecutablePath : mainAssembly.Location;
            var fileInfo = new FileInfo(new Uri(assemblyLocation).LocalPath);

            var satelliteFilePath = Path.Combine(fileInfo.DirectoryName ?? string.Empty, $"{Thread.CurrentThread.CurrentCulture}", $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}.resources.dll");

            if (File.Exists(satelliteFilePath))
            {
                satelliteAssembly = mainAssembly.GetSatelliteAssembly(Thread.CurrentThread.CurrentCulture);
            }
        }
        catch
        {

        }

        var embeddedResources = mainAssembly.GetManifestResourceNames().Select(x => new { Target = mainAssembly, Name = x, ResourceName = x, IsSatellite = false });

        if (satelliteAssembly != null)
        {
            static string ProcessCultureName(string filename) => $"{Path.GetFileNameWithoutExtension(Path.GetFileName(filename))}.{Thread.CurrentThread.CurrentCulture.Name}{Path.GetExtension(filename)}";

            embeddedResources = embeddedResources.Union(satelliteAssembly.GetManifestResourceNames().Select(x => new { Target = satelliteAssembly, Name = ProcessCultureName(x), ResourceName = ProcessCultureName(x), IsSatellite = true }));
        }

        var namespaces = mainAssembly.DefinedTypes.Select(x => x.Namespace).Distinct().ToArray();


        string ChangeResourceName(string rawName)
        {
            var targetName = namespaces.Where(x => x != null && !string.IsNullOrEmpty(x) && rawName.StartsWith(x!)).OrderByDescending(x => x!.Length).FirstOrDefault();

            if (targetName == null)
            {
                targetName = DefaultNamespace;
            }

            return $"{DefaultNamespace}{rawName.Substring($"{targetName}".Length)}";
        }

        embeddedResources = embeddedResources.Select(x =>
        new
        {
            x.Target,
            //Name = $"{DefaultNamespace}{x.Name.Substring($"{DefaultNamespace}".Length)}",
            Name = ChangeResourceName(x.Name),
            x.ResourceName,
            x.IsSatellite
        });


        var resource = embeddedResources.SingleOrDefault(x => x.Name.Equals(resourceName, StringComparison.CurrentCultureIgnoreCase));


        if (resource == null && !request.HasFileName)
        {
            foreach (var defaultFileName in SchemeOptions.DefaultFileName)
            {

                resourceName = string.Join(".", resourceName, defaultFileName);

                resource = embeddedResources.SingleOrDefault(x => x.Name.Equals(resourceName, StringComparison.CurrentCultureIgnoreCase));

                if (resource != null)
                {
                    break;
                }
            }
        }

        if (resource == null && Options.OnFallback != null)
        {
            var fallbackFile = Options.OnFallback.Invoke(requestUrl);

            resourceName = GetResourceName(fallbackFile, Options.EmbeddedResourceDirectoryName);

            resource = embeddedResources.SingleOrDefault(x => x.Name.Equals(resourceName, StringComparison.CurrentCultureIgnoreCase));
        }

        //System.Diagnostics.Debug.WriteLine($"Resource: {resourceName}");
        //var names = embeddedResources.Select(x => x.Name).ToArray();
        //System.Diagnostics.Debug.WriteLine($"Resources: {string.Join("\r\n", names)}");

        if (resource != null)
        {
            var manifestResourceName = resource.ResourceName;

            if (resource.IsSatellite)
            {
                manifestResourceName = $"{Path.GetFileNameWithoutExtension(Path.GetFileName(manifestResourceName))}{Path.GetExtension(manifestResourceName)}";
            }

            var contenStream = resource?.Target?.GetManifestResourceStream(manifestResourceName);

            if (contenStream != null)
            {

                response.ContentBody = contenStream;
                response.ContentType = CefRuntime.GetMimeType(Path.GetExtension(resourceName).Trim('.')) ?? "text/plain";
                return response;
            }
        }

        response.HttpStatus = WebResourceStatusCodes.Status404NotFound;

        return response;
    }
}
