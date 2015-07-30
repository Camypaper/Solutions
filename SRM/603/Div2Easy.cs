using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
public class MiddleCode
{
    public string encode(string str)
    {
        var a = str.ToList();
        var l = new List<char>();
        while (a.Any())
        {
            if (a.Count % 2 == 1)
            {
                var m = a.Count / 2;
                l.Add(a[m]);
                a.RemoveAt(m);
            }
            else
            {
                var m = a.Count / 2;
                if (a[m - 1] < a[m]) m--;

                l.Add(a[m]);
                a.RemoveAt(m);
            }
        }
        return l.AsString();
    }

}
static public class EnumerableEX
{
    static public string AsString(this IEnumerable<char> e) { return new string(e.ToArray()); }
}
