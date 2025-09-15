// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;

/// <summary>
/// Handles web resource requests for the <see cref="WebViewCore"/> instance.
/// Provides custom cookie access filtering, resource handler selection, and response filtering.
/// </summary>
class WebResourceRequestHandler : CefResourceRequestHandler
{
    private WebViewCore _webViewCore;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebResourceRequestHandler"/> class.
    /// </summary>
    /// <param name="webViewCore">The associated <see cref="WebViewCore"/> instance.</param>
    public WebResourceRequestHandler(WebViewCore webViewCore)
    {
        _webViewCore = webViewCore;
    }

    /// <summary>
    /// Provides a custom implementation for filtering cookie access.
    /// </summary>
    class CustomCookieAccessFilter : CefCookieAccessFilter
    {
        /// <inheritdoc/>
        protected override bool CanSaveCookie(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response, CefCookie cookie)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override bool CanSendCookie(CefBrowser browser, CefFrame frame, CefRequest request, CefCookie cookie)
        {
            return true;
        }
    }

    /// <summary>
    /// A resource handler that returns a predefined <see cref="WebResourceResponse"/>.
    /// </summary>
    class WebResponseFilterResourceHandler : WebResourceHandler
    {
        private readonly WebResourceResponse _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebResponseFilterResourceHandler"/> class.
        /// </summary>
        /// <param name="response">The response to return for the resource request.</param>
        public WebResponseFilterResourceHandler(WebResourceResponse response)
        {
            _response = response;
        }

        /// <inheritdoc/>
        protected override WebResourceResponse GetResourceResponse(WebResourceRequest request)
        {
            return _response;
        }
    }

    /// <summary>
    /// Implements a response filter for web resources, allowing content modification.
    /// </summary>
    class WebResponseFilter : CefResponseFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebResponseFilter"/> class.
        /// </summary>
        /// <param name="browser">The browser instance.</param>
        /// <param name="frame">The frame instance.</param>
        /// <param name="request">The resource request.</param>
        /// <param name="response">The resource response.</param>
        public WebResponseFilter(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
        {
            Browser = browser;
            Frame = frame;
            Request = request;
            Response = response;
        }

        /// <summary>
        /// Gets the browser associated with this filter.
        /// </summary>
        public CefBrowser Browser { get; }

        /// <summary>
        /// Gets the frame associated with this filter.
        /// </summary>
        public CefFrame Frame { get; }

        /// <summary>
        /// Gets the request associated with this filter.
        /// </summary>
        public CefRequest Request { get; }

        /// <summary>
        /// Gets the response associated with this filter.
        /// </summary>
        public CefResponse Response { get; }

        /// <summary>
        /// Gets or sets the delegate used to filter the response content.
        /// </summary>
        public required WebResponseFilterHandlerDelegate ContentFilterHandler { get; init; }

        List<byte> _buffer = new();

        bool _contentHandled = false;

        /// <inheritdoc/>
        protected override CefResponseFilterStatus Filter(UnmanagedMemoryStream dataIn, long dataInSize, out long dataInRead, UnmanagedMemoryStream dataOut, long dataOutSize, out long dataOutWritten)
        {
            /*
            调用此函数过滤一大块数据。预期用法如下：
            1. 从 |data_in| 读取输入数据，并将 |data_in_read| 设置为已读取的字节数，最大为 |data_in_size|。如果 |data_in_size| 为零，则 |data_in| 将为 NULL。
            2. 将过滤后的输出数据写入 |data_out|，并将 |data_out_written| 设置为已写入的字节数，最大为 |data_out_size|。如果没有写入输出数据，则必须从 |data_in| 读取所有数据（用户必须设置 |data_in_read| = |data_in_size|）。
            3. 如果所有输出数据都已写入，则返回 RESPONSE_FILTER_DONE；如果输出数据仍处于待处理状态，则返回 RESPONSE_FILTER_NEED_MORE_DATA。此方法将被重复调用，直到输入缓冲区已完全读取（用户设置 data_in_read| = |data_in_size|）且没有其他输入数据需要过滤（资源响应已完成）。
            如果用户已填充输出缓冲区（设置 |data_out_written| = |data_out_size|）并返回 RESPONSE_FILTER_NEED_MORE_DATA 以指示输出数据仍待处理，则可以使用空输入缓冲区再次调用此方法。
            当满足以下条件之一时，此方法的调用将停止：
            1. 没有其他输入数据需要过滤（资源响应已完成），并且用户设置 |data_out_written| = 0 或返回 RESPONSE_FILTER_DONE 以指示所有数据已写入；或者；
            2. 用户返回 RESPONSE_FILTER_ERROR 以指示发生错误。请勿保留对传递给此方法的缓冲区的引用。
            */
            if (dataIn is null || dataInSize == 0)
            {
                if (!_contentHandled && _buffer.Count > 0)
                {
                    var result = ContentFilterHandler.Invoke(_buffer.ToArray());

                    if (result is not null)
                    {
                        _buffer.Clear();
                        _buffer.AddRange(result);
                    }
                    _contentHandled = true;
                }

                dataInRead = 0;

                if (_buffer.Count > dataOutSize)
                {
                    dataOutWritten = dataOutSize;

                    var writtenBuff = _buffer.Take((int)dataOutWritten);
                    dataOut.Write(writtenBuff.ToArray(), 0, (int)dataOutWritten);

                    _buffer.RemoveRange(0, (int)dataOutWritten);

                    return CefResponseFilterStatus.NeedMoreData;
                }
                else
                {

                    dataOutWritten = _buffer.Count;
                    dataOut.Write(_buffer.ToArray(), 0, _buffer.Count);
                    return CefResponseFilterStatus.Done;
                }
            }


            dataInRead = dataInSize;
            dataOutWritten = 0;


            Span<byte> buff = stackalloc byte[(int)dataInSize];
            var retval = dataIn.Read(buff);

            _buffer.AddRange(buff.ToArray());

            return CefResponseFilterStatus.NeedMoreData;
        }

        /// <inheritdoc/>
        protected override bool InitFilter()
        {
            return true;
        }
    }

    /// <inheritdoc/>
    protected override CefCookieAccessFilter GetCookieAccessFilter(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        return new CustomCookieAccessFilter();
    }

    /// <inheritdoc/>
    protected override CefResourceHandler? GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
    {
        var targetUri = new Uri(request.Url);

        var handlers = _webViewCore.WebResourceHandlers.Select(x => new { Uri = new Uri(x.Key), HandlerCreationAction = x.Value });

        var matchedHandlers = handlers.Where(x => x.Uri.Scheme == targetUri.Scheme && x.Uri.Port == targetUri.Port && x.Uri.Host == targetUri.Host);

        matchedHandlers = matchedHandlers.Where(x => targetUri.AbsolutePath.StartsWith(x.Uri.AbsolutePath));

        var targetHandler = matchedHandlers.OrderBy(x => x.Uri.AbsolutePath.Length).FirstOrDefault();


        if (targetHandler is not null)
        {
            return targetHandler.HandlerCreationAction.Invoke(browser, frame, request);
        }

        var response = _webViewCore.OnWebResourceRequesting(browser, frame, request);

        if (response is null)
        {
            return base.GetResourceHandler(browser, frame, request);
        }
        else
        {
            return new WebResponseFilterResourceHandler(response);
        }

    }

    /// <inheritdoc/>
    protected override CefResponseFilter? GetResourceResponseFilter(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
    {
        var filter = _webViewCore.OnWebResourceResponseFilter(browser, frame, request, response);

        if (filter is null)
        {
            return base.GetResourceResponseFilter(browser, frame, request, response);
        }
        else
        {
            return new WebResponseFilter(browser, frame, request, response)
            {
                ContentFilterHandler = filter
            };
        }
    }
}



