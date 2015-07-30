using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
public class WinterAndCandies
{
    public int getNumber(int[] type)
    {
        var h = new int[100];
        foreach (var x in type)
            h[x - 1]++;
        var sum = 0;
        var add = 1;
        foreach (var v in h)
        {
            add *= v;
            sum += add;
        }
        return sum;
    }
}
