// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


namespace WinFormium.BlazorHybrid;

class BlazorFormiumWebViewManager : WebViewManager
{
    public BlazorFormium BlazorFormium { get; }

    private readonly Uri _appBaseUri;
    private readonly string _contentRootRelativePath;
    private readonly string _hostPagePathWithinFileProvider;
    private readonly BlazorFormiumOptions _options;

    internal const string initScript = """"
            window.external = {
            	sendMessage: message => {
            		window.formium.webview.postMessage(message);
            	},
            	receiveMessage: callback => {
            		window.formium.webview.addEventListener('message', e => {
                        const { data } = e;
                        if (typeof data === 'string') {
                            callback(data);
                        }
                    });
            	}
            };
            """";

    public BlazorFormiumWebViewManager(BlazorFormium blazorFormium, IServiceProvider services, Uri appBaseUri, IFileProvider fileProvider, JSComponentConfigurationStore jSComponent, string contentRootRelativePath, string hostPagePathWithinFileProvider, BlazorFormiumOptions options) : base(services, Dispatcher.CreateDefault(), appBaseUri, fileProvider, new JSComponentConfigurationStore(), hostPagePathWithinFileProvider)
    {
        BlazorFormium = blazorFormium;

        _appBaseUri = appBaseUri;
        _contentRootRelativePath = contentRootRelativePath;
        _hostPagePathWithinFileProvider = hostPagePathWithinFileProvider;
        _options = options;



        //Dispatcher.InvokeAsync(async () =>
        //{
        //    await AddRootComponentAsync(blazorFormium.RootComponent, blazorFormium.Selector, blazorFormium.Parameters is null ? ParameterView.Empty : ParameterView.FromDictionary(blazorFormium.Parameters));
        //});




        blazorFormium.BrowserDocumentAvailable += (_, args) =>
        {
            InitScript(blazorFormium);
        };

        Navigate("/");


    }

    private void InitScript(BlazorFormium webview)
    {
        webview.AddScriptToExecuteOnDocumentCreated(initScript);

        webview.WebMessageReceived += (_, args) =>
        {
            string message = args.TryGetWebMessageAsString() ?? string.Empty;
            //Console.WriteLine($"[BlazorFormiumWebViewManager] WebMessageReceived: {args.Source} - {message}");

            MessageReceived(new Uri(args.Source), message);
        };
    }

    public bool TryGetResponseContent(string url, out int statusCode, out string statusMessage, out Stream content, out IDictionary<string, string> headers)
    {
        return TryGetResponseContent(url, false, out statusCode, out statusMessage, out content, out headers);
    }

    protected override void NavigateCore(Uri absoluteUri)
    {
        BlazorFormium.Url = absoluteUri.ToString();
    }

    protected override void SendMessage(string message)
    {
        //Console.WriteLine($"[BlazorFormiumWebViewManager] Sending message: {message}");

        BlazorFormium.PostWebMessageAsString(message);
    }
}