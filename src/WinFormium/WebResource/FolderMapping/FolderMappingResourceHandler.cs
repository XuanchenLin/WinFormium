// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;
/// <summary>
/// Handles web resource requests by mapping them to files in a local folder.
/// Supports CORS policy and fallback logic for missing files.
/// </summary>
class FolderMappingResourceHandler : WebResourceHandler
{
    /// <summary>
    /// Gets the associated CefBrowser instance for the current request.
    /// </summary>
    public CefBrowser Browser { get; }
    /// <summary>
    /// Gets the CefFrame in which the request was made.
    /// </summary>
    public CefFrame Frame { get; }
    /// <summary>
    /// Gets the CefRequest object representing the original request.
    /// </summary>
    public CefRequest Request { get; }

    /// <summary>
    /// Gets the options used for folder mapping, including folder path and fallback logic.
    /// </summary>
    public FolderMappingOptions Options { get; }

    /// <summary>
    /// Indicates whether CORS policy is enabled for this handler.
    /// </summary>
    protected override bool EnableCORSPolicy => true;

    /// <summary>
    /// Initializes a new instance of the <see cref="FolderMappingResourceHandler"/> class.
    /// </summary>
    /// <param name="browser">The CefBrowser instance associated with the request.</param>
    /// <param name="frame">The CefFrame in which the request was made.</param>
    /// <param name="request">The CefRequest object representing the request.</param>
    /// <param name="options">The folder mapping options.</param>
    public FolderMappingResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request, FolderMappingOptions options)
    {
        Browser = browser;
        Frame = frame;
        Request = request;
        Options = options;
    }

    /// <summary>
    /// Gets the web resource response for the specified request by mapping the request to a file in the local folder.
    /// Handles default file names and fallback logic if the file does not exist.
    /// </summary>
    /// <param name="request">The web resource request.</param>
    /// <returns>A <see cref="WebResourceResponse"/> containing the file content or a 404 status if not found.</returns>
    protected override WebResourceResponse GetResourceResponse(WebResourceRequest request)
    {
        var requestUrl = request.RequestUrl;

        var response = new WebResourceResponse();

        if (request.Method != WebResourceRequestMethod.GET)
        {
            response.HttpStatus = WebResourceStatusCodes.Status404NotFound;

            return response;
        }

        var filePath = request.RelativePath;

        var physicalFilePath = Path.Combine(Options.FolderPath, filePath);

        if (!request.HasFileName)
        {
            foreach (var defaultFileName in SchemeOptions.DefaultFileName)
            {
                physicalFilePath = Path.Combine(physicalFilePath, defaultFileName);

                if (File.Exists(physicalFilePath))
                {
                    break;
                }
            }
        }

        if (!File.Exists(physicalFilePath) && Options.OnFallback != null)
        {
            var fallbackFile = Options.OnFallback.Invoke(requestUrl);

            physicalFilePath = Path.GetFullPath(Path.Combine(Options.FolderPath, fallbackFile));
        }

        if (File.Exists(physicalFilePath))
        {
            response.ContentBody = File.Open(physicalFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); //File.OpenRead(physicalFilePath);
            response.ContentType = CefRuntime.GetMimeType(Path.GetExtension(physicalFilePath).Trim('.')) ?? "text/plain";
        }
        else
        {
            response.HttpStatus = WebResourceStatusCodes.Status404NotFound;
        }

        return response;
    }
}
