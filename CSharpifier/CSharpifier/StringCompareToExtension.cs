using System;
using System.Collections.Generic;
using System.Text;

public static class StringCompareToExtension
{
    public static int compareTo(this String obj, object value)
    {
        return obj.CompareTo(value);
    }
}
