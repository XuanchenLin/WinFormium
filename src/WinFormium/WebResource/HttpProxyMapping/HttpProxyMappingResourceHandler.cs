// THIS FILE IS PART OF NanUI PROJECT
// THE NanUI PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using System.Net.Http.Headers;

namespace WinFormium.WebResource;
/// <summary>
/// Handles HTTP proxy mapping for web resource requests by forwarding requests to a specified proxy server.
/// </summary>
class HttpProxyMappingResourceHandler : WebResourceHandler
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
    /// Gets the original CefRequest instance.
    /// </summary>
    public CefRequest Request { get; }
    /// <summary>
    /// Gets the proxy server URL.
    /// </summary>
    public string Proxy { get; }

    /// <summary>
    /// The HttpClient used to forward requests to the proxy server.
    /// </summary>
    HttpClient httpClient = new HttpClient(new HttpClientHandler { UseCookies = true });

    protected override bool EnableCORSPolicy => true;


    /// <summary>
    /// Initializes a new instance of the <see cref="HttpProxyMappingResourceHandler"/> class.
    /// </summary>
    /// <param name="browser">The CefBrowser instance.</param>
    /// <param name="frame">The CefFrame instance.</param>
    /// <param name="request">The CefRequest instance.</param>
    /// <param name="proxy">The proxy server URL.</param>
    public HttpProxyMappingResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request, string proxy)
    {
        Browser = browser;
        Frame = frame;
        Request = request;
        Proxy = proxy;
    }

    /// <summary>
    /// Processes the incoming web resource request and forwards it to the proxy server, returning the response.
    /// </summary>
    /// <param name="request">The web resource request to process.</param>
    /// <returns>A <see cref="WebResourceResponse"/> containing the response from the proxy server.</returns>
    protected override WebResourceResponse GetResourceResponse(WebResourceRequest request)
    {
        httpClient.BaseAddress = new Uri(Proxy);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var message = new HttpRequestMessage(new HttpMethod(Request.Method), new Uri(Request.Url).PathAndQuery);

        if (request.Headers != null)
        {
            for (var i = 0; i < request.Headers.Count; i++)
            {
                var headerKey = request.Headers.GetKey(i);
                var headerValue = request.Headers.Get(i);

                if (headerKey is not null)
                {
                    message.Headers.TryAddWithoutValidation(headerKey, headerValue);
                }
            }
        }

        if (request.JsonData != null && request.IsJson)
        {
            var data = request.JsonData;

            if (data.StartsWith("\"") && data.EndsWith("\""))
            {
                data = data[1..^1];

                data = Regex.Unescape(data);
            }

            message.Content = new StringContent(data, request.ContentEncoding, request.ContentType);
        }
        else if (request.FormData != null && request.FormData.AllKeys != null && request.FormData.AllKeys.Length > 0)
        {

            var formData = request.FormData!.AllKeys!.Where(x => x != null).ToDictionary(x => x!, x => request.FormData![x!]);
            var formContent = new FormUrlEncodedContent(formData);
            message.Content = formContent;
        }
        else if (request.UploadFiles != null && request.UploadFiles.Length > 0)
        {
            var multipartContent = new MultipartFormDataContent();
            foreach (var file in request.UploadFiles)
            {
                var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                var fileName = Path.GetFileName(file);
                var fileContent = new StreamContent(fileStream);
                multipartContent.Add(fileContent, "file", fileName);
            }
            message.Content = multipartContent;
        }


        var result = httpClient.SendAsync(message).GetAwaiter().GetResult()!;

        var response = new WebResourceResponse()
        {
            ContentType = result.Content.Headers.ContentType?.MediaType,
            HttpStatus = (int)result.StatusCode,
        };

        foreach (var header in result.Headers.ToList())
        {
            foreach (var v in header.Value)
            {
                response.Headers.Add(header.Key, v);
            }
        }

        response.ContentBody = result.Content.ReadAsStreamAsync().GetAwaiter().GetResult();



        return response;
    }
}
