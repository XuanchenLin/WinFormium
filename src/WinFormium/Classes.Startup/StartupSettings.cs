// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;


/// <summary>
/// Provides startup configuration options for the WinFormium application, including
/// methods to specify the main window or to start without a window.
/// </summary>
public sealed class StartupSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupSettings"/> class.
    /// </summary>
    public StartupSettings()
    {


    }

    /// <summary>
    /// Specifies a <see cref="Formium"/> instance to use as the application's main window.
    /// </summary>
    /// <param name="formium">The <see cref="Formium"/> instance to use as the main window.</param>
    /// <returns>An <see cref="AppCreationAction"/> configured with the specified <see cref="Formium"/>.</returns>
    public AppCreationAction UseMainWindow(Formium formium)
    {
        return new AppCreationAction()
        {
            Formium = formium
        };
    }

    /// <summary>
    /// Specifies a standard <see cref="Form"/> to use as the application's main window.
    /// </summary>
    /// <param name="form">The <see cref="Form"/> instance to use as the main window.</param>
    /// <returns>An <see cref="AppCreationAction"/> configured with the specified <see cref="Form"/>.</returns>
    public AppCreationAction UseMainWindow(Form form)
    {
        return new AppCreationAction()
        {
            Form = form
        };
    }

    /// <summary>
    /// Configures the application to start without displaying any main window.
    /// </summary>
    /// <returns>An <see cref="AppCreationAction"/> representing a windowless startup.</returns>
    public AppCreationAction StartWitoutWindow()
    {
        return new AppCreationAction() { };
    }
}
