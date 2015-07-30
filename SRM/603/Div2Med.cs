using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
public class SplitIntoPairs
{
    public int makepairs(int[] A, int X)
    {
        Array.Sort(A);
        var n = A.Length;
        var cnt = 0;
        var used = new bool[n];
        for (int i = 0; i < n; i++)
        {
            if (used[i])
                continue;
            if (A[i] < 0)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (used[j]) continue;
                    if (Math.BigMul(A[i], A[j]) >= X)
                    {
                        cnt++;
                        used[i] = used[j] = true; break;
                    }
                }
            }
            else
            {
                for (int j = n - 1; j > i; j--)
                {
                    if (used[j]) continue;
                    if (Math.BigMul(A[i], A[j]) >= X)
                    {
                        cnt++;
                        used[i] = used[j] = true; break;
                    }
                }
            }
        }
        return cnt;
    }

}
