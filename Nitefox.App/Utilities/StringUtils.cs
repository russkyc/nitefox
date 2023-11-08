// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using System.Text.RegularExpressions;

namespace Nitefox.App.Utilities;

public static class StringUtils
{
    public static bool IsNullOrEmpty(this string input)
        => string.IsNullOrEmpty(input);
    
    public static bool IsNullOrWhiteSpace(this string input)
        => string.IsNullOrWhiteSpace(input);
    
    public static string ToPathSafeString(this string input)
        => Regex.Replace(input,"[\\/:*?\"<>|]*", "");

    public static string ToDelimitedString(this IEnumerable<string> collection)
        => string.Join(", ", collection);
    
    public static string ToCleanQueryString(this string input)
    => Regex.Replace(input, "^.*:\\s*", "");
}