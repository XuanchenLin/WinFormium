// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;
internal static class StringHelper
{
		public static bool IsNullOrEmpty(string value)
		{
			return value == null || value.Length == 0;
		}

		public static bool IsNullOrWhiteSpace(string value)
		{
			if (value == null) return true;

			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i])) return false;
			}

			return true;
		}
}
