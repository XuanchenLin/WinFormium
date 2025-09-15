// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using WinFormium.Browser;

namespace WinFormium.BlazorHybrid;

/// <summary>
/// Delegate to configure services for Blazor Hybrid applications.
/// </summary>
/// <param name="services">
/// The service collection to configure. 
/// </param>
public delegate void ConfigureServices(ServiceCollection services);

/// <summary>
/// Options for configuring a <see cref="BlazorFormium"/> instance.
/// </summary>
public sealed class BlazorFormiumOptions : WebResourceOptions
{
    /// <summary>
    /// Gets the root component type for the Blazor application.
    /// </summary>
    public RootComponentsCollection RootComponents { get; } = new();

    /// <summary>
    /// Gets the host path to the main HTML file (typically "wwwroot/index.html").
    /// </summary>
    public string HostPage { get; init; } = Path.Combine("wwwroot", "index.html");



    /// <summary>
    /// Gets the parameters to pass to the root component.
    /// </summary>
    public Dictionary<string, object?>? Parameters { get; init; } = null;

    /// <summary>
    /// Gets the delegate to configure additional services for the Blazor application.
    /// </summary>
    public ConfigureServices? ConfigureServices { get; init; } = null;


    /// <summary>
    /// Gets the collection of assemblies containing static resources (e.g., CSS, JS, images) to be served by the Blazor application.
    /// </summary>
    public ICollection<BlazorFormiumAssemblyResources> StaticResources { get; } = new Collection<BlazorFormiumAssemblyResources>();

}
