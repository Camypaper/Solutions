using System;
using System.Collections.Generic;
using System.Linq;

static public class Program
{
    static public void Main()
    {
        var n = readInteger().Validate(x => 1 <= x && x <= (int)1e5);
        var a = readInteger(n).ValidateArray(x => 1 <= x && x <= (int)1e9);
        var dic = new Dictionary<int, int>();
        foreach (var x in a)
        {
            if (dic.ContainsKey(x))
                dic[x]++;
            else dic[x] = 1;
        }
        var cnt = dic.Count(x => x.Value == 1);
        Console.WriteLine(cnt);
    }

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


