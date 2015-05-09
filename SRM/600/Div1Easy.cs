using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
public class ORSolitaire
{
    public int getMinimum(int[] numbers, int goal)
    {
        var n=numbers.Length;
        var a = new long[n];
        for (int i = 0; i < n; i++)
            a[i] = numbers[i];
        var cnt = new int[64];
        long v = goal;
        foreach (var x in a)
        {
            var pass = false;
            for (int i = 0; i < 64; i++)
                if ((v >> i & 1) == 0 && (x >> i & 1) == 1)
                   pass = true;
            if (pass)
                continue;
            for (int i = 0; i < 64; i++)
                if ((x >> i & 1) == 1) cnt[i]++;
        }
        var ans = int.MaxValue;
        for (int i = 0; i < 64; i++)
            if ((v >> i & 1) == 1) ans = Math.Min(cnt[i], ans);
        return ans;
    }

}
