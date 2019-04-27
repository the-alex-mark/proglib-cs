using System;

namespace System
{
	public static class StringExtension
	{
		internal static string TrimWithBOM(this string str)
		{
			return str.Trim().Trim(new char[]
			{
				'﻿',
				'​'
			});
		}
	}
}
