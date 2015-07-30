using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
public class TypoCoderDiv2
{
    public int count(int[] rating)
    {
        var n = rating.Length;
        const int v = 1200;
        int now = 500;
        var cnt = 0;
        foreach (var x in rating)
        {
            if (now < v && v <= x)
                cnt++;
            if (now >= v && v > x)
                cnt++;
            now = x;
        }
        return cnt;
    }
}
