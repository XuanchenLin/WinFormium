// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// XML node types.
/// </summary>
public enum CefXmlNodeType
{
    Unsupported = 0,
    ProcessingInstruction,
    DocumentType,
    ElementStart,
    ElementEnd,
    Attribute,
    Text,
    CData,
    EntityReference,
    WhiteSpace,
    Comment,
}
