// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides the application context for WinFormium startup, managing the main form or Formium instance.
/// </summary>
class StartupApplicationContext : ApplicationContext
{

    /// <summary>
    /// Specifies the type of main window currently running in the application context.
    /// </summary>
    internal enum RunningType
    {
        /// <summary>
        /// No main window is running.
        /// </summary>
        None,
        /// <summary>
        /// A standard WinForms Form is running.
        /// </summary>
        Form,
        /// <summary>
        /// A Formium window is running.
        /// </summary>
        Formium
    }

    Formium? _formium;

    Form? _form;

    //public Form? MainForm { get; set; }

    /// <summary>
    /// Gets the type of main window currently running in the application context.
    /// </summary>
    internal RunningType RunningOn => _formium == null && _form == null ? RunningType.None : _formium != null ? RunningType.Formium : RunningType.Form;

    /// <summary>
    /// Sets the startup form to a Formium instance and updates the main form reference.
    /// </summary>
    /// <param name="formium">The Formium instance to use as the main window.</param>
    public void UseStartupForm(Formium formium)
    {
        //UnregisterStartupFormEvents();

        _form = null;
        _formium = formium;
        MainForm = formium.HostWindow;

        //RegisterStartupFormEvents();
    }

    /// <summary>
    /// Sets the startup form to a standard Form and updates the main form reference.
    /// </summary>
    /// <param name="form">The Form instance to use as the main window.</param>
    public void UseStartupForm(Form form)
    {
        //UnregisterStartupFormEvents();
        _formium = null;
        _form = form;
        MainForm = form;

        //RegisterStartupFormEvents();
    }

    /// <summary>
    /// Unregisters the FormClosed event handler from the current main form.
    /// </summary>
    private void UnregisterStartupFormEvents()
    {
        if (MainForm is not null)
        {
            MainForm.FormClosed -= OnMainFormClosed;
        }
    }

    /// <summary>
    /// Registers the FormClosed event handler to the current main form.
    /// </summary>
    private void RegisterStartupFormEvents()
    {
        if (MainForm is not null)
        {
            MainForm.FormClosed += OnMainFormClosed;
        }
    }

    /// <summary>
    /// Gets the main window handle as an <see cref="IWin32Window"/> for the current main form or Formium instance.
    /// </summary>
    public IWin32Window? MainWindowHandle => RunningOn switch
    {
        RunningType.Form => _form,
        RunningType.Formium => _formium,
        _ => null
    };

    //internal protected virtual void OnMainFormClosed(object? sender, EventArgs e)
    //{
    //    //base.OnMainFormClosed(sender, e);

    //    for (var i = Application.OpenForms.Count - 1; i != 0; i--)
    //    {
    //        var form = Application.OpenForms[i];
    //        if (form is null || form.IsDisposed) continue;
    //        form.Close();
    //    }

    //    CefRuntime.QuitMessageLoop();
    //}
}
