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
            var G = Enumerate(n, x => new List<int>());
            for (int i = 0; i < n - 1; i++)
            {
                var f = sc.Integer() - 1;
                var t = sc.Integer() - 1;
                G[f].Add(t);
                G[t].Add(f);
            }
            var lcaHelper = new LCAHelper(G);
            var children = new int[n];
            Func<int, int, int> dfs = null;
            dfs = (prev, cur) =>
                {
                    var ret = 0;
                    foreach (var x in G[cur])
                        if (x == prev) continue;
                        else ret += 1 + dfs(cur, x);
                    return children[cur] = ret;
                };
            dfs(-1, 0);
            var m = sc.Integer();
            for (int i = 0; i < m; i++)
            {
                var f = sc.Integer() - 1;
                var t = sc.Integer() - 1;
                if (f == t)
                {
                    IO.Printer.Out.WriteLine(n);
                    continue;
                }
                var lca = lcaHelper.GetLCA(f, t);
                var df = lcaHelper.Depth[f] - lcaHelper.Depth[lca];
                var dt = lcaHelper.Depth[t] - lcaHelper.Depth[lca];
                var dlca = lcaHelper.Depth[lca];
                if ((df + dt) % 2 == 1)
                    IO.Printer.Out.WriteLine(0);
                else
                {
                    var d = (df + dt) / 2;
                    if (df > dt)
                    {
                        var v = f;
                        d = df - d + dlca;
                        for (int k = 0; k < 32; k++)
                            if (((lcaHelper.Depth[v] - d) >> k & 1) == 1)
                                v = lcaHelper.Parent[k, v];
                        var u = f;
                        d = lcaHelper.Depth[v];
                        for (int k = 0; k < 32; k++)
                            if (((lcaHelper.Depth[u] - (d + 1)) >> k & 1) == 1)
                                u = lcaHelper.Parent[k, u];
                        IO.Printer.Out.WriteLine(children[v] - children[u]);

                    }
                    else if (df == dt)
                    {
                        var v = f;
                        var u = t;
                        d = lcaHelper.Depth[lca];
                        for (int k = 0; k < 32; k++)
                            if (((lcaHelper.Depth[v] - (d + 1)) >> k & 1) == 1)
                                v = lcaHelper.Parent[k, v];

                        for (int k = 0; k < 32; k++)
                            if (((lcaHelper.Depth[u] - (d + 1)) >> k & 1) == 1)
                                u = lcaHelper.Parent[k, u];

                        IO.Printer.Out.WriteLine(n - children[u] - children[v] - 2);

                    }
                    else
                    {
                        var v = t;
                        d = dt - d + dlca;
                        for (int k = 0; k < 32; k++)
                            if (((lcaHelper.Depth[v] - d) >> k & 1) == 1)
                                v = lcaHelper.Parent[k, v];
                        var u = t;
                        d = lcaHelper.Depth[v];
                        for (int k = 0; k < 32; k++)
                            if (((lcaHelper.Depth[u] - (d + 1)) >> k & 1) == 1)
                                u = lcaHelper.Parent[k, u];
                        IO.Printer.Out.WriteLine(children[v] - children[u]);
                    }

                }
            }
        }
        public IO.StreamScanner sc = new IO.StreamScanner(Console.OpenStandardInput());
        static T[] Enumerate<T>(int n, Func<int, T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(i); return a; }

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

#region lca sometimes
//GetLCA O(logN)
//Make O(NlogN)
class LCAHelper
{
    public int[,] Parent;
    public int[] Depth;
    public LCAHelper(List<int>[] graph, int root = 0)
    {
        var n = graph.Length;
        Parent = new int[50, n];
        Depth = new int[n];
        dfs(root, -1, 0, graph);
        for (int k = 0; k < 32; k++)
        {
            for (int v = 0; v < n; v++)
            {
                if (Parent[k, v] < 0)
                    Parent[k + 1, v] = -1;
                else Parent[k + 1, v] = Parent[k, Parent[k, v]];
            }
        }


    }
    public int GetLCA(int u, int v)
    {
        if (Depth[u] > Depth[v])
            Swap(ref u, ref v);
        for (int k = 0; k < 32; k++)
            if (((Depth[v] - Depth[u]) >> k & 1) == 1)
                v = Parent[k, v];
        if (u == v)
            return u;
        for (int i = 32 - 1; i >= 0; i--)
        {
            if (Parent[i, u] != Parent[i, v])
            {
                u = Parent[i, u];
                v = Parent[i, v];
            }
        }

        return Parent[0, u];
    }
    private void dfs(int v, int p, int d, List<int>[] G)
    {
        Parent[0, v] = p;
        Depth[v] = d;
        foreach (var x in G[v])
        {
            if (x != p)
                dfs(x, v, d + 1, G);
        }
    }
    private static void Swap(ref int u, ref int v)
    {
        var tmp = u; u = v; v = tmp;
    }
}
#endregion
