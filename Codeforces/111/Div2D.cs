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
            var map = new SortedMap<long, List<Edge>>();
            for (int i = 0; i < m; i++)
            {
                var e = new Edge(sc.Integer() - 1, sc.Integer() - 1, i, sc.Long());
                map[e.cost].Add(e);
            }
            var set = new DisjointSet(n);
            var res = new int[m];
            foreach (var es in map)
            {
                var id = new HashMap<int, int>();
                foreach (var e in es.Value)
                {
                    var f = set[e.from];
                    var t = set[e.to];
                    if (f == t)
                        continue;

                    if (!id.ContainsKey(f))
                        id[f] = id.Count;
                    if (!id.ContainsKey(t))
                        id[t] = id.Count;
                    res[e.id] = 2;
                }
                var g = Enumerate(id.Count, x => new List<KeyValuePair<int, int>>());
                var count = new HashMap<long, int>();
                foreach (var e in es.Value)
                {
                    if (set[e.from] == set[e.to])
                        continue;
                    var f = id[set[e.from]];
                    var t = id[set[e.to]];
                    g[f].Add(new KeyValuePair<int, int>(f, t));
                    g[t].Add(new KeyValuePair<int, int>(t, f));
                    long kv = f + t * 200000L;
                    long vk = t + f * 200000L;
                    if (count.ContainsKey(kv))
                        count[kv] = count[vk] = -1;
                    else count[kv] = count[vk] = e.id;

                }
                foreach (var e in es.Value)
                    set.Unite(e.from, e.to);
                List<KeyValuePair<int, int>> bridge;
                List<List<int>> tecomp;
                Graph.GetBridge(g, out bridge, out tecomp);
                foreach (var e in bridge)
                {
                    long kv = e.Key + e.Value * 200000L;
                    if (count[kv] >= 0)
                        res[count[kv]] = 1;
                }


            }
            var strs = new string[] { "none", "any", "at least one" };
            foreach (var x in res)
                IO.Printer.Out.WriteLine(strs[x]);

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
#region Edge
public class Edge : IComparable<Edge>
{
    public int to, from, id;
    public long cost;
    public Edge(int f, int t, int p, long v) { to = t; from = f; cost = v; id = p; }
    public override string ToString() { return string.Format("{0}->{1}:{2}", from, to, cost); }
    public int CompareTo(Edge other) { return cost.CompareTo(other.cost); }
}
#endregion
#region DisjointSet
public class DisjointSet
{
    int[] par, ranks, count;
    public DisjointSet(int n)
    {
        par = new int[n];
        count = new int[n];
        for (int i = 0; i < n; i++)
        {
            par[i] = i;
            count[i] = 1;
        }
        ranks = new int[n];
    }
    public int this[int id] { get { return (par[id] == id) ? id : this[par[id]]; } }
    public bool Unite(int x, int y)
    {
        x = this[x]; y = this[y];
        if (x == y) return false;
        if (ranks[x] < ranks[y])
        {
            par[x] = y;
            count[y] += count[x];
        }
        else
        {
            par[y] = x;
            count[x] += count[y];
            if (ranks[x] == ranks[y])
                ranks[x]++;
        }
        return true;
    }
    public int Size(int x) { return count[this[x]]; }
    public bool IsUnited(int x, int y) { return this[x] == this[y]; }

}
#endregion
#region SortedMap
class SortedMap<K, V> : SortedDictionary<K, V>
    where V : new()
{
    new public V this[K i]
    {
        get
        {
            V v;
            return TryGetValue(i, out v) ? v : base[i] = new V();
        }
        set { base[i] = value; }
    }
}
#endregion
#region HashMap
class HashMap<K, V> : Dictionary<K, V>
    where V : new()
{
    new public V this[K i]
    {
        get
        {
            V v;
            return TryGetValue(i, out v) ? v : base[i] = new V();
        }
        set { base[i] = value; }
    }
}
#endregion

#region Bridge
static public partial class Graph
{
    static public void GetBridge(List<KeyValuePair<int, int>>[] g, out List<KeyValuePair<int, int>> bridge, out List<List<int>> tecomp)
    {
        var n = g.Length;
        var num = new int[n];
        var inS = new bool[n];
        var root = new Stack<int>();
        var s = new Stack<int>();
        var t = 0;
        bridge = new List<KeyValuePair<int, int>>();
        tecomp = new List<List<int>>();
        Action<int, int, List<KeyValuePair<int, int>>, List<List<int>>> dfs = null;
        dfs = (v, u, br, te) =>
            {
                num[v] = ++t;
                s.Push(v); inS[v] = true;
                root.Push(v);
                foreach (var e in g[v])
                {
                    var to = e.Value;
                    if (num[to] == 0)
                        dfs(to, v, br, te);
                    else if (u != to && inS[to])
                        while (num[root.Peek()] > num[to]) root.Pop();
                }
                if (v == root.Peek())
                {
                    br.Add(new KeyValuePair<int, int>(u, v));
                    te.Add(new List<int>());
                    while (true)
                    {
                        var w = s.Pop();
                        inS[w] = false;
                        te[te.Count - 1].Add(w);
                        if (v == w) break;
                    }
                    root.Pop();
                }
            };
        for (int u = 0; u < n; u++)
        {
            if (num[u] == 0)
            {
                dfs(u, n, bridge, tecomp);
                bridge.RemoveAt(bridge.Count - 1);
            }
        }
    }
}
#endregion
