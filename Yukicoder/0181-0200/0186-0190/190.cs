using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
static public class Program
{
    static public void Main()
    {
        var n = readInteger().Validate(x => 1 <= x && x <= 100000);
        var A = readInteger(2 * n).ValidateArray(x => -100000 <= x && x <= 100000);
        Array.Sort(A);
        readEOF();
        var neg = new List<int>();
        var pos = new List<int>();
        var zero = new List<int>();
        foreach (var x in A)
            if (x > 0) pos.Add(x);
            else if (x == 0) zero.Add(x);
            else neg.Add(-x);
        neg.Sort();
        pos.Sort();
        int dry = 0, wet = 0, moist = 0;
        {
            var p = 0;
            var q = 0;
            var a = zero.Concat(pos).ToArray();
            var rem = 0;
            while (p < neg.Count && q < a.Length)
            {
                if (neg[p] > a[q])
                {
                    //Console.WriteLine("{0} {1}", neg[p], a[q]);
                    dry++; p++; q++;
                }
                else { p++; rem++; }
            }
            dry += (neg.Count + rem - p) / 2;
        }
        {
            var p = 0;
            var q = 0;
            var a = zero.Concat(neg).ToArray();
            var rem = 0;
            while (p < pos.Count && q < a.Length)
            {
                if (pos[p] > a[q])
                {
                    wet++; p++; q++;
                }
                else { p++; rem++; }
            }
            wet += (pos.Count - p + rem) / 2;
        }
        {
            var p = 0;
            var q = 0;
            while (p < pos.Count && q < neg.Count)
            {
                if (pos[p] > neg[q]) q++;
                else if (pos[p] < neg[q]) p++;
                else { moist++; p++; q++; }
            }
            moist += zero.Count / 2;
        }
        Console.WriteLine("{0} {1} {2}", dry, wet, moist);
    }
    static public void Swap<T>(ref T a, ref T b) { var tmp = a; a = b; b = tmp; }
    static int readInteger()
    {
        var s = Console.ReadLine();
        return int.Parse(s);
    }
    static int[] readInteger(int n)
    {
        var s = Console.ReadLine().Split(' ');
        if (s.Length != n)
            throw new Exception(string.Format("invalid input, expected{0} actual{1}", n, s.Length));
        var ret = new int[n];
        for (int i = 0; i < n; i++)
            ret[i] = int.Parse(s[i]);
        return ret;
    }
    static string[] readString(int n)
    {
        var s = Console.ReadLine().Split(' ');
        if (s.Length != n)
            throw new Exception(string.Format("invalid input, expected{0} actual{1}", n, s.Length));
        return s;
    }
    static void readEOF()
    {
        if (Console.In.Peek() >= 0)
            throw new Exception("invalid input too long input file");
    }

}
static public class Ex
{
    static public T Validate<T>(this T input, Func<T, bool> f)
    {
        if (!f(input))
            throw new Exception("invalid input");
        return input;
    }
    static public T[] ValidateArray<T>(this T[] input, Func<T, bool> f)
    {
        foreach (var x in input)
            if (!f(x))
                throw new Exception("invalid input");
        return input;
    }
}
