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
            var G = new HeavyLightDecomposition(n);
            for (int i = 0; i < n - 1; i++)
            {
                var a = sc.Integer();
                var b = sc.Integer();
                G.AddEdge(a, b);
            }
            G.Build(0);
            for (int i = 0; i < m; i++)
            {
                if (sc.Integer() == 0)
                {
                    var a = sc.Integer();
                    var b = sc.Integer();
                    var lca = G.GetLCA(a, b);
                    long ans = G.Query(0, a) + G.Query(0, b) - 2 * G.Query(0, lca);
                    IO.Printer.Out.WriteLine(ans);
                }
                else
                {
                    var v = sc.Integer();
                    var x = sc.Integer();
                    G.Add(v, x);
                }
            }
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
        static Printer() { Out = new Printer(Console.OpenStandardOutput()) { AutoFlush = false}; }
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



#region HLDecomposition
public struct Edge
{
    public int from, to;
    public long cost;
    public Edge(int f, int t)
    {
        from = f; to = t; cost = 0;
    }
    public override string ToString()
    {
        return string.Format("{0}->{1}", from, to);
    }
}
public class HeavyLightDecomposition
{
    public void Add(int from, long val)
    {
        var f = go[from];
        if (pos[from] + 2 <= f.heavy.Count + 1)
        {
            f.seg.Add(pos[from] + 1, val);
            f.rng.Add(pos[from] + 2, f.heavy.Count + 1, val);
        }
    }
    public long Query(int from, int to)
    {
        var f = go[from];
        var t = go[to];
        if (f.id == t.id)
        {
            return f.rng[pos[from] + 2, pos[to] + 1];
        }
        else
        {
            var ch = GetAncestorAt(t, f.depth + 1);
            var med = H[ch];
            long sum = f.rng[pos[from] + 2, pos[med.light.from] + 1];
            var add = f.seg[pos[med.light.from] + 1];
            sum += (depth[to] - depth[med.light.from]) * add;
            sum += Query(med.light.to, to);
            return sum;
        }
    }
    List<Edge>[] G;
    List<Node> H = new List<Node>();
    int[] subTreeSize;
    int[] par;
    int[] pos;
    int[,] parent;
    int[] depth;

    Node[] go;
    int[,] hlpar;
    public HeavyLightDecomposition(int n)
    {
        G = Enumerate(n, x => new List<Edge>());
        subTreeSize = new int[n];
        pos = new int[n];
        depth = new int[n];
        par = new int[n];
        parent = new int[32, n];
        go = new Node[n];
    }
    public void AddEdge(int f, int t)
    {
        G[f].Add(new Edge(f, t));
        G[t].Add(new Edge(t, f));
    }
    #region impl
    public void Build(int root)
    {
        ComputeSubTreeSize(root);
        Decomposite(new Edge(-1, root), -1, 0, 0);
        BuildLCA();
        BuildHLLCA();

    }
    public void ComputeSubTreeSize(int root)
    {
        var stack = new Stack<KeyValuePair<int, int>>();
        stack.Push(new KeyValuePair<int, int>(root, -1));
        var visit = new bool[G.Length];
        while (stack.Any())
        {
            var p = stack.Pop();
            var cur = p.Key;
            var prev = p.Value;
            if (visit[p.Key])
            {
                foreach (var next in G[cur])
                {
                    var to = To(next);
                    if (to != prev)
                        subTreeSize[cur] += subTreeSize[to];
                }
            }
            else
            {
                visit[cur] = true;
                stack.Push(new KeyValuePair<int, int>(cur, prev));
                subTreeSize[cur]++;
                foreach (var next in G[cur])
                {
                    var to = To(next);
                    if (to != prev) stack.Push(new KeyValuePair<int, int>(to, cur));
                }
            }
        }
    }
    public void Decomposite(Edge light, int prevId, int d, int lv)
    {
        var node = new Node();
        node.light = light;
        node.id = H.Count;
        node.par = prevId;
        node.depth = lv;
        H.Add(node);
        var prev = light.from;
        var cur = light.to;
        while (cur != prev)
        {
            var next = cur;
            var max = 0;
            depth[cur] = d;
            par[cur] = prev;
            go[cur] = node;
            pos[cur] = node.heavy.Count;
            foreach (var to in G[cur])
            {
                var t = To(to);
                if (t != prev) max = Math.Max(max, subTreeSize[t]);
            }
            foreach (var to in G[cur])
            {
                var t = To(to);
                if (t == prev) continue;
                if (max == subTreeSize[t])
                {
                    //Debug.WriteLine("{0}->{1}", cur, t);
                    max = 1 << 30;
                    next = t;
                    node.heavy.Add(to);
                }
                else Decomposite(to, node.id, d + 1, lv + 1);
            }
            prev = cur;
            cur = next;
            d++;
        }
        node.init();

    }
    public void BuildLCA()
    {
        for (int k = 0; k < 31; k++)
            for (int v = 0; v < G.Length; v++)
                parent[k, v] = -1;
        for (int i = 0; i < G.Length; i++)
            parent[0, i] = par[i];
        for (int k = 0; k < 31; k++)
        {
            for (int v = 0; v < G.Length; v++)
                if (parent[k, v] < 0) parent[k + 1, v] = -1;
                else parent[k + 1, v] = parent[k, parent[k, v]];
        }

    }
    public void BuildHLLCA()
    {
        hlpar = new int[32, H.Count];
        for (int k = 0; k < 31; k++)
            for (int v = 0; v < H.Count; v++)
                hlpar[k, v] = -1;
        for (int i = 0; i < H.Count; i++)
            hlpar[0, i] = H[i].par;
        for (int k = 0; k < 31; k++)
        {
            for (int v = 0; v < H.Count; v++)
                if (hlpar[k, v] < 0) hlpar[k + 1, v] = -1;
                else hlpar[k + 1, v] = hlpar[k, hlpar[k, v]];
        }

    }
    public int GetAncestorAt(Node v, int d)
    {
        if (v.depth < d)
            throw new Exception();
        for (int i = 0; i < 32; i++)
            if ((((v.depth - d) >> i) & 1) == 1)
                v = H[hlpar[i, v.id]];
        return v.id;
    }
    public int To(Edge t) { return t.to; }
    public int GetLCA(int u, int v)
    {
        if (depth[u] > depth[v]) { var tmp = u; u = v; v = tmp; }
        for (int k = 0; k < 32; k++)
            if ((((depth[v] - depth[u]) >> k) & 1) == 1)
                v = parent[k, v];
        if (u == v)
            return u;
        for (int i = 31; i >= 0; i--)
            if (parent[i, u] != parent[i, v])
            {
                u = parent[i, u];
                v = parent[i, v];
            }
        return parent[0, u];
    }
    public int GetAncestor(int v, int k)
    {
        var to = depth[v] - k;
        for (int i = 0; i < 32; i++)
            if ((((depth[v] - to) >> i) & 1) == 1)
                v = parent[i, v];
        return v;
    }
    public int GetAncestorAt(int v, int d)
    {
        if (depth[v] < d)
            throw new Exception();
        for (int i = 0; i < 32; i++)
            if ((((depth[v] - d) >> i) & 1) == 1)
                v = parent[i, v];
        return v;
    }
    static T[] Enumerate<T>(int n, Func<int, T> f) { var a = new T[n]; for (int i = 0; i < n; ++i) a[i] = f(i); return a; }
    #endregion
}
public class Node
{
    public Edge light;
    public List<Edge> heavy = new List<Edge>();
    public int par;
    public int depth;
    public int id;
    public FenwickTree seg;
    public RangeAddFenwickTree rng;
    public void init()
    {
        seg = new FenwickTree(heavy.Count + 1);
        rng = new RangeAddFenwickTree(heavy.Count + 1);
    }
    public override string ToString()
    {
        var s = new List<int>();
        s.Add(light.from);
        s.Add(light.to);
        foreach (var e in heavy)
            s.Add(e.to);
        return s.AsJoinedString("->");
    }
}
#endregion

#region FenwickTree
[System.Diagnostics.DebuggerDisplay("Data={ToString()}")]
public class FenwickTree
{
    int n;
    long[] bit;
    int max = 1;
    public FenwickTree(int size)
    {
        n = size; bit = new long[n + 1];
        while ((max << 1) <= n) max <<= 1;
    }
    /// <summary>sum[a,b]</summary>
    public long this[int i, int j] { get { return this[j] - this[i - 1]; } }
    /// <summary>sum[0,i]</summary>
    public long this[int i] { get { long s = 0; for (; i > 0; i -= i & -i) s += bit[i]; return s; } }
    public int LowerBound(long w)
    {
        if (w <= 0) return 0;
        int x = 0;
        for (int k = max; k > 0; k >>= 1)
            if (x + k <= n && bit[x + k] < w)
            {
                w -= bit[x + k];
                x += k;
            }
        return x + 1;
    }
    /// <summary>add v to bit[i]</summary>
    public void Add(int i, long v)
    {
        if (i == 0) System.Diagnostics.Debug.Fail("BIT is 1 indexed");
        for (; i <= n; i += i & -i) bit[i] += v;
    }
    public override string ToString() { return string.Join(",", Enumerable.Range(0, n + 1).Select(i => this[i, i])); }
}
#endregion
#region RangeAddFenwickTree
public class RangeAddFenwickTree
{
    int n;
    FenwickTree a, b;
    public RangeAddFenwickTree(int n)
    {
        this.n = n;
        a = new FenwickTree(n);
        b = new FenwickTree(n);
    }
    /// <summary>Add V to[i,j]</summary>
    public void Add(int i, int j, long v)
    {
        a.Add(i, -(i - 1) * v); a.Add(j + 1, j * v);
        b.Add(i, v); b.Add(j + 1, -v);
    }
    /// <summary>Sum [0,i]</summary>
    public long this[int i] { get { return a[i] + b[i] * i; } }
    /// <summary>Sum [i,j]</summary>
    public long this[int i, int j] { get { return this[j] - this[i - 1]; } }
    public override string ToString() { return string.Join(",", Enumerable.Range(0, n + 1).Select(i => this[i, i])); }

}
#endregion
