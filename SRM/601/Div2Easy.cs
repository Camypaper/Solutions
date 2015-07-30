using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
public class WinterAndMandarins
{
    public int getNumber(int[] bags, int K)
    {
        Array.Sort(bags);
        var n=bags.Length;
        var min = int.MaxValue;
        for (int i = 0; i < n; i++)
        {
            if (i + K-1 >= n)
                break;
            min = Math.Min(min, bags[i + K - 1] - bags[i]);
        }
        return min;
    }
}
