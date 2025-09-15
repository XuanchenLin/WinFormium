// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Represents a setting used during application creation, encapsulating a value and its type.
/// </summary>
/// <param name="Value">The value of the setting.</param>
/// <param name="ValueType">The type of the value.</param>
public sealed record AppCreationSetting(object Value, Type ValueType);

