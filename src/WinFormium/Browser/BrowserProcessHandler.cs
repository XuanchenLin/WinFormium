// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Handles browser process events and communication for the WinFormium application.
/// </summary>
class BrowserProcessHandler : CefBrowserProcessHandler
{
    /// <summary>
    /// The associated <see cref="BrowserApp"/> instance.
    /// </summary>
    private readonly BrowserApp _browserApp;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserProcessHandler"/> class.
    /// </summary>
    /// <param name="browserApp">The <see cref="BrowserApp"/> instance.</param>
    public BrowserProcessHandler(BrowserApp browserApp)
    {
        _browserApp = browserApp;

        WinFormiumApp.Current.RegisterRendererProcessMessageHandler(nameof(GetWindowBindingObjectsMessage), ProcessWindowBindingObjectsMessage);

    }

    /// <inheritdoc/>
    protected override void OnBeforeChildProcessLaunch(CefCommandLine commandLine)
    {
        base.OnBeforeChildProcessLaunch(commandLine);

        commandLine.AppendSwitch("host-process-id", $"{_browserApp.AppContext.BrowserProcessId}");

        System.Diagnostics.Debug.WriteLine($"[{_browserApp.GetProcessTypeName().ToUpper()} PROCESS] Launched:\r\n{commandLine}");

    }

    /// <inheritdoc/>
    protected override void OnContextInitialized()
    {

    }

    /// <inheritdoc/>
    protected override void OnRegisterCustomPreferences(CefPreferencesType type, CefPreferenceRegistrar registrar)
    {
        base.OnRegisterCustomPreferences(type, registrar);
    }

    /// <summary>
    /// Processes the message to retrieve window binding objects structure data.
    /// </summary>
    /// <param name="_">The message payload (not used).</param>
    /// <returns>The result of processing the message.</returns>
    private string? ProcessWindowBindingObjectsMessage(string? _)
    {
        //TODO:从WindowBindingObjects中获取对象的结构数据
        return "OK";
    }

}

