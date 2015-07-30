using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
static public class Program
{
    static public void Main()
    {
        var max = BigInteger.Pow(10, 200);
        const long mod = (long)1e9 + 9;
        var S = readString(2).ValidateArray(x =>
        {
            var v = BigInteger.Parse(x);
            return 1 <= v && v <= max;
        });
        readEOF();
        var a = func(S[0]);
        var b = func(S[1]);
        long ans = 0;
        for (int i = 1; i < Math.Min(a.Length, b.Length); i++)
            ans = (ans + a[i] * b[i]) % mod;
        Console.WriteLine(ans);
    }
    static long[] func(string str)
    {
        const long mod = (long)1e9 + 9;
        var len = str.Length;
        var max = len * 9;
        var dp = new long[2, len + 1, max + 10];
        dp[1, 0, 0] = 1;
        for (int i = 0; i < len; i++)
            for (int j = 0; j <= max; j++)
                for (int k = 0; k < 10; k++)
                {
                    if (k == str[i] - '0')
                        dp[1, i + 1, j + k] = (dp[1, i + 1, j + k] + dp[1, i, j]) % mod;
                    else if (k < str[i] - '0') dp[0, i + 1, j + k] = (dp[0, i + 1, j + k] + dp[1, i, j]) % mod;
                    dp[0, i + 1, j + k] = (dp[0, i + 1, j + k] + dp[0, i, j]) % mod;
                }
        var ret = new long[max + 10];
        for (int i = 0; i <= max; i++)
            ret[i] = (dp[0, len, i] + dp[1, len, i]) % mod;
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
    static public T[] ValidateArray<T>(this T[] input, Func<T, bool> f)
    {
        foreach (var x in input)
            if (!f(x))
                throw new Exception("invalid input");
        return input;
    }
}
