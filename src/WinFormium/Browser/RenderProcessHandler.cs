// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Handles render process events and communication for the WinFormium application.
/// </summary>
class RenderProcessHandler : CefRenderProcessHandler
{
    /// <summary>
    /// The constant for the about:blank URL.
    /// </summary>
    private const string URL_ABOUT_BLANK = "about:blank";

    /// <summary>
    /// The associated <see cref="BrowserApp"/> instance.
    /// </summary>
    private readonly BrowserApp _browserApp;

    ///// <summary>
    ///// The current <see cref="CefBrowser"/> instance.
    ///// </summary>
    //private CefBrowser? _browser;

    /// <summary>
    /// Gets the process communication bridge for inter-process messaging.
    /// </summary>
    public ProcessCommunicationBridge? CommunicationBridge { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderProcessHandler"/> class.
    /// </summary>
    /// <param name="browserApp">The <see cref="BrowserApp"/> instance.</param>
    public RenderProcessHandler(BrowserApp browserApp)
    {
        _browserApp = browserApp;
    }

    /// <inheritdoc/>
    protected override void OnWebKitInitialized()
    {
        var response = WinFormiumApp.Current.SendMessageToBrowserProcess(new GetWindowBindingObjectsMessage());

        //Console.WriteLine($"[RENDER] -> {nameof(OnWebKitInitialized)} Response:\r\n{response}");

        WinFormiumApp.Current.OnWebKitInitialized();
    }

    /// <inheritdoc/>
    protected override void OnBrowserCreated(CefBrowser browser, CefDictionaryValue? extraInfo)
    {


        CommunicationBridge = new ProcessCommunicationBridge(browser, CefProcessId.Renderer);

        CommunicationBridge.RegisterProcessCommunicationBridgeHandler(new JavaScriptEngine(CommunicationBridge));

        base.OnBrowserCreated(browser, extraInfo);


        //Console.WriteLine($"[RENDER(BrowserID:{browser.Identifier})] -> {nameof(OnBrowserCreated)} {CommunicationBridge}");

    }

    /// <inheritdoc/>
    protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
        //Console.WriteLine($"[RENDER(BrowserID:{browser.Identifier})] -> {nameof(OnContextCreated)} 0x{frame.Identifier:X}");

        CommunicationBridge?.OnRenderContextCreated(browser, frame, context);

        base.OnContextCreated(browser, frame, context);

    }

    /// <inheritdoc/>
    protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
    {

        //Console.WriteLine($"[RENDER(BrowserID:{browser.Identifier})] -> {nameof(OnContextReleased)} 0x{frame.Identifier:X}");

        CommunicationBridge?.OnRenderContextReleased(browser, frame, context);

        base.OnContextReleased(browser, frame, context);
    }

    /// <inheritdoc/>
    protected override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        CommunicationBridge?.DispatchProcessMessage(browser, frame, sourceProcess, message);

        return base.OnProcessMessageReceived(browser, frame, sourceProcess, message);
    }

    /// <inheritdoc/>
    protected override void OnUncaughtException(CefBrowser browser, CefFrame frame, CefV8Context context, CefV8Exception exception, CefV8StackTrace stackTrace)
    {
        base.OnUncaughtException(browser, frame, context, exception, stackTrace);
    }




    //private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    //{
    //    CefFrame frame = null;
    //    var exception = (Exception)e.ExceptionObject;
    //    try
    //    {
    //        frame = _browser?.FrameCount > 0 ? _browser?.GetMainFrame() : null;
    //    }
    //    catch
    //    {
    //        // ignore
    //    }
    //    HandleException(exception, frame);
    //}

    //private void WithErrorHandling(Action action, CefFrame frame)
    //{
    //    try
    //    {
    //        action();
    //    }
    //    catch (Exception e)
    //    {
    //        HandleException(e, frame);
    //    }
    //}

    //private void HandleException(Exception e, CefFrame frame)
    //{
    //    if (frame != null)
    //    {
    //        try
    //        {
    //            using (CefObjectTracker.StartTracking())
    //            {
    //                var exceptionMessage = new Messages.UnhandledException()
    //                {
    //                    ExceptionType = e.GetType().FullName,
    //                    Message = e.Message,
    //                    StackTrace = e.StackTrace
    //                };
    //                var message = exceptionMessage.ToCefProcessMessage();
    //                frame.SendProcessMessage(CefProcessId.Browser, message);
    //            }
    //            return;
    //        }
    //        catch
    //        {
    //            // ignore, lets try an alternative method using the crash pipe
    //        }
    //    }
    //    SendExceptionToParentProcess(e);
    //}

    ///// <summary>
    ///// Alternative way to send the exception to the parent process using a side named pipe.
    ///// </summary>
    ///// <param name="e"></param>
    //private void SendExceptionToParentProcess(Exception e)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(_crashPipeName))
    //        {
    //            return; // not initialized yet
    //        }

    //        var serializableException = new SerializableException()
    //        {
    //            ExceptionType = e.GetType().ToString(),
    //            Message = e.Message,
    //            StackTrace = e.StackTrace
    //        };


    //        using (var pipeClient = new PipeClient(_crashPipeName))
    //        {
    //            pipeClient.SendMessage(serializableException.SerializeToString());
    //        }
    //    }
    //    catch
    //    {
    //        // failed at failing
    //    }
    //}

}

class GetWindowBindingObjectsMessage : RenderProcessMessage
{
    public GetWindowBindingObjectsMessage()
    {
        Name = nameof(GetWindowBindingObjectsMessage);
    }
}

class PostJsMessage : RenderProcessMessage
{
    public PostJsMessage()
    {
        Name = nameof(PostJsMessage);
    }
}