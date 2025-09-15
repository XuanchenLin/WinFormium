// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// DOM node types.
/// </summary>
public enum CefDomNodeType
{
   Unsupported = 0,
   Element,
   Attribute,
   Text,
   CDataSection,
   ProcessingInstruction,
   Comment,
   Document,
   DocumentType,
   DocumentFragment,
}
