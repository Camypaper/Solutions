using System;
using System.Linq;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
//using System.Numerics;
namespace Program
{
    public class Solver
    {
        public void Solve()
        {
            var n = sc.Integer();
            var val = new double[n];
            var G = Enumerate(n, x => new List<int>());
            var RG = Enumerate(n, x => new List<int>());
            for (int i = 0; i < n; i++)
            {
                val[i] = sc.Integer();
                var from = sc.Integer() - 1;
                if (from == i) continue;
                G[from].Add(i);
                RG[i].Add(from);
            }
            int sz;
            var scc = DSCCHelper.SCC(G, RG, out sz);
            var l = Enumerate(sz, x => new List<int>());
            for (int i = 0; i < n; i++) l[scc[i]].Add(i);
            var ans = 0.0;
            for (int i = 0; i < sz; i++)
            {
                var min = double.MaxValue;
                foreach (var x in l[i])
                {
                    if (val[x] < min) min = val[x];
                    ans += val[x] / 2;
                }
                ans += min / 2;
                foreach (var x in l[i])
                    foreach (var to in G[x]) val[to] /= 2;
            }
            IO.Printer.Out.WriteLine("{0:F1}", ans);
        }
        internal IO.StreamScanner sc = new IO.StreamScanner(Console.OpenStandardInput());
        static T[] Enumerate<T>(int n, Func<T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(); return a; }
        static T[] Enumerate<T>(int n, Func<int, T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(i); return a; }
    }
}

#region Ex
namespace Program.IO
{
    using System.IO;
    using System.Linq;
    public class Printer : StreamWriter
    {
        static Printer()
        {
            Out = new Printer(Console.OpenStandardOutput()) { AutoFlush = false };
        }
        public static Printer Out { get; set; }
        public override IFormatProvider FormatProvider { get { return System.Globalization.CultureInfo.InvariantCulture; } }
        public Printer(System.IO.Stream stream) : base(stream, new System.Text.UTF8Encoding(false, true)) { }
        public void Write<T>(string format, IEnumerable<T> source) { base.Write(format, source.OfType<object>().ToArray()); }
        public void WriteLine<T>(string format, IEnumerable<T> source) { base.WriteLine(format, source.OfType<object>().ToArray()); }
    }
    public class StreamScanner
    {
        public StreamScanner(Stream stream) { str = stream; }
        private readonly Stream str;
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
        public char[] Char(int n) { var a = new char[n]; for (int i = 0; i < n; i++) a[i] = Char(); return a; }
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
        public double Double() { return double.Parse(Scan(), System.Globalization.CultureInfo.InvariantCulture); }
        private T[] enumerate<T>(int n, Func<T> f)
        {
            var a = new T[n];
            for (int i = 0; i < n; ++i) a[i] = f();
            return a;
        }

        public string[] Scan(int n) { return enumerate(n, Scan); }
        public double[] Double(int n) { return enumerate(n, Double); }
        public int[] Integer(int n) { return enumerate(n, Integer); }
        public long[] Long(int n) { return enumerate(n, Long); }
        public void Flush() { str.Flush(); }

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
#region DecomposiitonOfStronglyConnectedComponents
static public class DSCCHelper
{
    static public int[] SCC(List<int>[] G, List<int>[] RG, out int k)
    {
        var n = G.Length;
        var used = new bool[n];
        var vs = new List<int>(n);
        var cmp = new int[n];
        Action<int> dfs = null;
        dfs = v =>
        {
            used[v] = true;
            foreach (var to in G[v])
                if (!used[to]) dfs(to);
            vs.Add(v);
        };
        Action<int, int> rdfs = null;
        rdfs = (v, t) =>
        {
            used[v] = true;
            cmp[v] = t;
            foreach (var to in RG[v])
                if (!used[to]) rdfs(to, t);
        };

        for (int i = 0; i < used.Length; i++)
            if (!used[i]) dfs(i);
        used = new bool[n];
        k = 0;
        for (int i = vs.Count - 1; i >= 0; i--)
            if (!used[vs[i]]) rdfs(vs[i], k++);
        return cmp;

    }
}
#endregion