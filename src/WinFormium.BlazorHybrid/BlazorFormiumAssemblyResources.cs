// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace WinFormium.BlazorHybrid;

/// <summary>
/// Represents an assembly containing static resources for a Blazor Hybrid application.
/// </summary>
public sealed class BlazorFormiumAssemblyResources
{
    /// <summary>
    /// Gets the assembly containing the static resources.
    /// </summary>
    public Assembly ResourcesAssembly { get; }

    /// <summary>
    /// Gets or sets the base namespace within the assembly where the static resources are located.
    /// </summary>
    public string? BaseNamespace { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorFormiumAssemblyResources"/> class with the specified assembly.
    /// </summary>
    /// <param name="resourcesAssembly">
    /// The assembly containing the static resources.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resourcesAssembly"/> is <c>null</c>.
    /// </exception>

    public BlazorFormiumAssemblyResources(Assembly resourcesAssembly)
    {
        ResourcesAssembly = resourcesAssembly ?? throw new ArgumentNullException(nameof(resourcesAssembly));

        BaseNamespace = resourcesAssembly.EntryPoint?.DeclaringType?.Namespace ?? resourcesAssembly.GetName().Name!;
    }
}
