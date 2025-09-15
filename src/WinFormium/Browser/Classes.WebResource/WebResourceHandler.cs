// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Abstract base class for handling web resource requests in WinFormium.
/// Inherits from <see cref="CefResourceHandler"/> and provides CORS, range, and custom response support.
/// </summary>
public abstract class WebResourceHandler
    : CefResourceHandler
{
    /// <summary>
    /// HTTP header for allowed CORS headers.
    /// </summary>
    private const string ACCESS_CONTROL_ALLOW_HEADERS = "Access-Control-Allow-Headers";
    /// <summary>
    /// HTTP header for allowed CORS methods.
    /// </summary>
    private const string ACCESS_CONTROL_ALLOW_METHODS = "Access-Control-Allow-Methods";
    /// <summary>
    /// HTTP header for allowed CORS origins.
    /// </summary>
    private const string ACCESS_CONTROL_ALLOW_ORIGIN = "Access-Control-Allow-Origin";
    /// <summary>
    /// HTTP header for CORS max age.
    /// </summary>
    private const string ACCESS_CONTROL_MAX_AGE = "Access-Control-Max-Age";
    /// <summary>
    /// HTTP header for X-Frame-Options.
    /// </summary>
    private const string X_FRAME_OPTIONS = "X-Frame-Options";
    /// <summary>
    /// HTTP header for X-Powered-By.
    /// </summary>
    private const string X_POWERED_BY = "X-Powered-By";

    /// <summary>
    /// Gets a value indicating whether CORS policy headers should be enabled for responses.
    /// </summary>
    protected virtual bool EnableCORSPolicy
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Handle to prevent garbage collection during request processing.
    /// </summary>
    private GCHandle _gcHandle;
    /// <summary>
    /// Current offset in the response stream for reading.
    /// </summary>
    private int _readStreamOffset;
    /// <summary>
    /// Start position for range requests, if specified.
    /// </summary>
    private int? _buffStartPostition = null;
    /// <summary>
    /// End position for range requests, if specified.
    /// </summary>
    private int? _buffEndPostition = null;
    /// <summary>
    /// Indicates if the current request is a partial content (range) request.
    /// </summary>
    private bool _isPartContent = false;
    /// <summary>
    /// Cancellation token source for async operations.
    /// </summary>
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    /// <summary>
    /// The response object for the current request.
    /// </summary>
    private WebResourceResponse? _resourceResponse;

    /// <summary>
    /// When implemented in a derived class, returns the response for the given request.
    /// </summary>
    /// <param name="request">The web resource request.</param>
    /// <returns>A <see cref="WebResourceResponse"/> representing the response.</returns>
    abstract protected WebResourceResponse GetResourceResponse(WebResourceRequest request);

    /// <summary>
    /// Gets the scheme handler options for this resource handler.
    /// </summary>
    protected virtual WebResourceSchemeHandlerOptions SchemeOptions { get; } = new WebResourceSchemeHandlerOptions();

    /// <summary>
    /// Gets the MIME type for the specified file name.
    /// </summary>
    /// <param name="fileName">The file name to get the MIME type for.</param>
    /// <returns>The MIME type string.</returns>
    public static string GetMimeType(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.Trim('.') ?? string.Empty;

        if (string.IsNullOrEmpty(ext))
        {
            return "application/octet-stream";
        }

        return CefRuntime.GetMimeType(ext);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebResourceHandler"/> class.
    /// </summary>
    public WebResourceHandler()
    {
        _gcHandle = GCHandle.Alloc(this);
    }

    /// <summary>
    /// Skips the specified number of bytes in the response stream.
    /// </summary>
    /// <param name="bytesToSkip">The number of bytes to skip.</param>
    /// <param name="bytesSkipped">The actual number of bytes skipped.</param>
    /// <param name="callback">The skip callback.</param>
    /// <returns>True if the skip was successful; otherwise, false.</returns>
    protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
    {
        bytesSkipped = bytesToSkip;
        return true;
    }

    /// <summary>
    /// Sets the response headers for the current request.
    /// </summary>
    /// <param name="response">The response object to set headers on.</param>
    /// <param name="responseLength">The length of the response content.</param>
    /// <param name="redirectUrl">The redirect URL, if any.</param>
    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    {
        var statusCode = _resourceResponse?.HttpStatus ?? WebResourceStatusCodes.Status400BadRequest;

        if (_resourceResponse != null)
        {
            response.SetHeaderMap(_resourceResponse.Headers);
        }

        response.Status = statusCode;
        redirectUrl = string.Empty;

        if (statusCode == WebResourceStatusCodes.Status200OK && _resourceResponse != null)
        {
            responseLength = _resourceResponse.Length;
            response.MimeType = _resourceResponse.ContentType ?? string.Empty;
            response.Url = _resourceResponse.Url ?? string.Empty;

            if (_isPartContent)
            {
                response.SetHeaderByName("Accept-Ranges", "bytes", true);

                var startPos = 0;
                var endPos = _resourceResponse.Length - 1;

                if (_buffStartPostition.HasValue && _buffEndPostition.HasValue)
                {
                    startPos = _buffStartPostition.Value;
                    endPos = _buffStartPostition.Value;
                }
                else if (!_buffEndPostition.HasValue && _buffStartPostition.HasValue)
                {
                    startPos = _buffStartPostition.Value;
                }

                response.SetHeaderByName("Content-Range", $"bytes {startPos}-{endPos}/{_resourceResponse.Length}", true);
                response.SetHeaderByName("Content-Length", $"{endPos - startPos + 1}", true);

                response.Status = 206;
            }

            response.SetHeaderByName("Content-Type", response.MimeType, true);
            response.Charset = _resourceResponse.Headers.Get("charset") ?? string.Empty;
            response.SetHeaderByName(X_POWERED_BY, $"{nameof(WinFormium)}/{Assembly.GetExecutingAssembly().GetName().Version}", true);
        }
        else
        {
            responseLength = 0;
        }
    }

    /// <summary>
    /// Opens the resource for the given request and prepares the response asynchronously.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="handleRequest">Indicates whether the request is handled immediately.</param>
    /// <param name="callback">The callback to continue or cancel the request.</param>
    /// <returns>True if the request is being handled; otherwise, false.</returns>
    protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
    {
        var uri = new Uri(request.Url);
        var headers = request.GetHeaderMap();

        if (!string.IsNullOrEmpty(headers.Get("range")))
        {
            var rangeString = headers?.Get("range") ?? string.Empty;
            var group = Regex.Match(rangeString, @"(?<start>\d+)-(?<end>\d*)")?.Groups;
            if (group != null)
            {
                if (!string.IsNullOrEmpty(group["start"].Value) && int.TryParse(group["start"].Value, out var startPos))
                {
                    _buffStartPostition = startPos;
                }

                if (!string.IsNullOrEmpty(group["end"].Value) && int.TryParse(group["end"].Value, out var endPos))
                {
                    _buffEndPostition = endPos;
                }
            }
            _isPartContent = true;
        }

        _readStreamOffset = 0;

        if (_buffStartPostition.HasValue)
        {
            _readStreamOffset = _buffStartPostition.Value;
        }

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

        var method = request.Method;

        var resourceRequest = new WebResourceRequest(uri, method, headers, postData.ToArray(), uploadFiles.ToArray(), request);

        handleRequest = false;

        Task.Run(() =>
        {
            try
            {
                _resourceResponse = GetResourceResponse(resourceRequest);

                _resourceResponse.Url = request.Url;

                if (EnableCORSPolicy)
                {
                    _resourceResponse.Headers.Set(ACCESS_CONTROL_ALLOW_HEADERS, "*");
                    _resourceResponse.Headers.Set(ACCESS_CONTROL_ALLOW_METHODS, "*");
                    _resourceResponse.Headers.Set(X_FRAME_OPTIONS, "ALLOWALL");

                    _resourceResponse.Headers.Set(ACCESS_CONTROL_MAX_AGE, "3600");

                    if (!string.IsNullOrEmpty(request.GetHeaderByName("origin")))
                    {
                        _resourceResponse.Headers.Set(ACCESS_CONTROL_ALLOW_ORIGIN, request.GetHeaderByName("origin"));
                    }
                    else
                    {
                        _resourceResponse.Headers.Set(ACCESS_CONTROL_ALLOW_ORIGIN, "*");
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Instance.Log.LogError(ex);

                Console.WriteLine(ex);

                callback.Cancel();
            }

        }, _cancellationTokenSource.Token).ContinueWith(t => callback.Continue());

        return true;
    }

    /// <summary>
    /// Cancels the current request and any ongoing asynchronous operations.
    /// </summary>
    protected override void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Reads response data into the provided buffer.
    /// </summary>
    /// <param name="dataOut">Pointer to the output buffer.</param>
    /// <param name="bytesToRead">Number of bytes to read.</param>
    /// <param name="bytesRead">Number of bytes actually read.</param>
    /// <param name="callback">The read callback.</param>
    /// <returns>True if data was read; false if the end of the stream is reached.</returns>
    protected override bool Read(nint dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
    {
        if (_resourceResponse?.ContentBody == null)
        {
            bytesRead = 0;
            return false;
        }

        var total = _resourceResponse.Length;

        var bytesToCopy = (int)(total - _readStreamOffset);

        if (total == 0 || bytesToCopy <= 0)
        {
            bytesRead = 0;
            return false;
        }

        bytesToCopy = Math.Min(bytesToCopy, bytesToRead);

        //var buff = new byte[bytesToCopy];
        Span<byte> buff = stackalloc byte[bytesToCopy];

        _resourceResponse.ContentBody.Position = _readStreamOffset;

        //_resourceResponse.ContentBody.Read(buff, 0, bytesToCopy);

        _resourceResponse.ContentBody.ReadExactly(buff);

        Marshal.Copy(buff.ToArray(), 0, dataOut, bytesToCopy);

        _readStreamOffset += bytesToCopy;

        bytesRead = bytesToCopy;

        if (_readStreamOffset == _resourceResponse.Length)
        {
            _resourceResponse.Dispose();
            _gcHandle.Free();
        }

        return true;
    }
}
