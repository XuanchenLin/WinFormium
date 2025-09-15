// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Represents an action for creating and configuring the startup form or Formium instance
/// in the application context during application initialization.
/// </summary>
public sealed class AppCreationAction
{
    /// <summary>
    /// Gets or sets the standard Windows Form to be used as the startup form.
    /// </summary>
    internal Form? Form { get; set; }

    /// <summary>
    /// Gets or sets the Formium instance to be used as the startup form.
    /// </summary>
    internal Formium? Formium { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppCreationAction"/> class.
    /// </summary>
    public AppCreationAction()
    {
    }

    /// <summary>
    /// Configures the specified <see cref="StartupApplicationContext"/> by setting its startup form
    /// to either the <see cref="Formium"/> instance or the standard <see cref="Form"/>, if available.
    /// </summary>
    /// <param name="appContext">The application context to configure.</param>
    internal void ConfigureAppContext(StartupApplicationContext appContext)
    {
        if (Formium != null)
        {
            appContext.UseStartupForm(Formium);
        }
        else if (Form != null)
        {
            appContext.UseStartupForm(Form);
        }

        //if (appContext.MainForm is not null)
        //{
        //    appContext.MainForm.Show();
        //}


        //CefRuntime.RunMessageLoop();
    }
}
