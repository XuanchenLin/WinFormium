// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue.Interop;
[StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
	internal unsafe struct cef_range_t
	{
		public int from;
		public int to;

		public cef_range_t(int from, int to)
		{
			this.from = from;
			this.to = to;
		}
	}
