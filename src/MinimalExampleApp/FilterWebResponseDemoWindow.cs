// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Text;

using WinFormium;
using WinFormium.Browser;
using WinFormium.Browser.CefGlue;

namespace MinimalExampleApp;

class FilterWebResponseDemoWindow : Formium
{
    public FilterWebResponseDemoWindow()
    {

        MinimumSize = Size = new Size(1280, 720);

        Url = "https://www.bing.com";
    }
    protected override WebResponseFilterHandlerDelegate? FilterWebResponse(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
    {
        if ((request.Url.StartsWith( "https://www.bing.com") || request.Url.StartsWith( "https://cn.bing.com")) && request.Method=="GET" && response.GetHeaderMap()["content-type"] is var contentType && contentType is not null && contentType.Contains("text/html", StringComparison.InvariantCultureIgnoreCase) && frame.IsMain)
        {
            return new WebResponseFilterHandlerDelegate((origin) =>
            {
                var html = Encoding.UTF8.GetString(origin);
                html = html.Replace("</body>", """<div style="position: fixed; left: 50%; top: 40%; padding: 20px; background: #cc0000af; transform: translateX(-50%) translateY(-100%); color: white; font-weight: 600; font-size: 2em;border-radius:15px;backdrop-filter:blur(10px);box-shadow: 1px 1px 9px #333333ae;z-index:9999" data-bm="18">!!!INJECTED!!! by WinFormium</div></body>""");
                return Encoding.UTF8.GetBytes(html);
            });
        }
        return base.FilterWebResponse(browser, frame, request, response);
    }
    //protected override WebResourceResponse? RequestWebResource(CefBrowser browser, CefFrame frame, CefRequest request)
    //{

    //    if (request.Url.StartsWith("https://cn.bing.com"))
    //    {
    //        var response = new WebResourceResponse(Encoding.UTF8.GetBytes($"Filtered by {nameof(FilterWebResponse)}"), "text/plain");
    //        return response;
    //    }

    //    return base.RequestWebResource(browser, frame, request);
    //}
}