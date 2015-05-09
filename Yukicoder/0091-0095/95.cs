using System;
using System.Linq;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
using System.Numerics;
namespace Program
{

    public class Solver
    {
        public void Solve()
        {
            var n = sc.Integer();
            var m = sc.Integer();
            var k = sc.Integer();
            var G = Enumerate(n, x => new List<int>());
            var d = Enumerate(n, x => Enumerate(n, y => 100));
            for (int i = 0; i < n; i++)
                d[i][i] = 0;
            for (int i = 0; i < m; i++)
            {
                var a = sc.Integer() - 1;
                var b = sc.Integer() - 1;
                G[a].Add(b); G[b].Add(a); d[a][b] = d[b][a] = 1;
            }
            for (int t = 0; t < n; t++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        d[i][j] = Math.Min(d[i][t] + d[t][j], d[i][j]);
            var s = new List<int>() { 0 };
            for (int _ = n - 1; _ > 0 && s.Count <= k; _--)
            {
                s.Add(_);
                var dp = new int[s.Count, 1 << s.Count];
                for (int i = 0; i < s.Count; i++)
                    for (int j = 0; j < 1 << s.Count; j++)
                        dp[i, j] = 1000;
                dp[0, 1] = 0;
                for (int i = 1; i < 1 << s.Count; i++)
                {
                    for (int j = 0; j < s.Count; j++)
                    {
                        if ((i >> j & 1) == 0)
                            continue;
                        if (dp[j, i] > k)
                            continue;
                        for (int t = 0; t < s.Count; t++)
                        {
                            if ((i >> t & 1) == 1)
                                continue;
                            var e = d[s[j]][s[t]];
                            dp[t, i | 1 << t] = Math.Min(dp[j, i] + e, dp[t, i | 1 << t]);
                        }
                    }
                }
                var ok = false;
                for (int i = 0; i < s.Count; i++)
                    if (dp[i, (1 << s.Count) - 1] <= k) { ok = true; break; }
                if (!ok) s.RemoveAt(s.Count - 1);
            }
            var max = 0L;
            foreach (var x in s)
                max += (1L << x) - 1;
            IO.Printer.Out.WriteLine(max);
        }
        public IO.StreamScanner sc = new IO.StreamScanner(Console.OpenStandardInput());
        static T[] Enumerate<T>(int n, Func<int, T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(i); return a; }
        static public void Swap<T>(ref T a, ref T b) { var tmp = a; a = b; b = tmp; }
    }
}

#region Ex
namespace Program.IO
{
    using System.IO;
    using System.Text;
    using System.Globalization;
    public class Printer : StreamWriter
    {
        static Printer() { Out = new Printer(Console.OpenStandardOutput()) { AutoFlush = false }; }
        public static Printer Out { get; set; }
        public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }
        public Printer(System.IO.Stream stream) : base(stream, new UTF8Encoding(false, true)) { }
        public Printer(System.IO.Stream stream, Encoding encoding) : base(stream, encoding) { }
        public void Write<T>(string format, IEnumerable<T> source) { base.Write(format, source.OfType<object>().ToArray()); }
        public void WriteLine<T>(string format, IEnumerable<T> source) { base.WriteLine(format, source.OfType<object>().ToArray()); }
    }
    public class StreamScanner
    {
        public StreamScanner(Stream stream) { str = stream; }
        public readonly Stream str;
        private readonly byte[] buf = new byte[1024];
        private int len, ptr;
        public bool isEof = false;
        public bool IsEndOfStream { get { return isEof; } }
        private byte read()
        {
            if (isEof) return 0;
            if (ptr >= len) { ptr = 0; if ((len = str.Read(buf, 0, 1024)) <= 0) { isEof = true; return 0; } }
            return buf[ptr++];
        }
        public char Char() { byte b = 0; do b = read(); while (b < 33 || 126 < b); return (char)b; }

        public string Scan()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 33 && b <= 126; b = (char)read())
                sb.Append(b);
            return sb.ToString();
        }
        public long Long()
        {
            if (isEof) return long.MinValue;
            long ret = 0; byte b = 0; var ng = false;
            do b = read();
            while (b != '-' && (b < '0' || '9' < b));
            if (b == '-') { ng = true; b = read(); }
            for (; true; b = read())
            {
                if (b < '0' || '9' < b)
                    return ng ? -ret : ret;
                else ret = ret * 10 + b - '0';
            }
        }
        public int Integer() { return (isEof) ? int.MinValue : (int)Long(); }
        public double Double() { return double.Parse(Scan(), CultureInfo.InvariantCulture); }
        private T[] enumerate<T>(int n, Func<T> f)
        {
            var a = new T[n];
            for (int i = 0; i < n; ++i) a[i] = f();
            return a;
        }

        public char[] Char(int n) { return enumerate(n, Char); }
        public string[] Scan(int n) { return enumerate(n, Scan); }
        public double[] Double(int n) { return enumerate(n, Double); }
        public int[] Integer(int n) { return enumerate(n, Integer); }
        public long[] Long(int n) { return enumerate(n, Long); }
    }
}
static class Ex
{
    static public string AsString(this IEnumerable<char> ie) { return new string(System.Linq.Enumerable.ToArray(ie)); }
    static public string AsJoinedString<T>(this IEnumerable<T> ie, string st = " ") { return string.Join(st, ie); }
    static public void Main()
    {
        var solver = new Program.Solver();
        solver.Solve();
        Program.IO.Printer.Out.Flush();
    }
}
#endregion
