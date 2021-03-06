using System;
using System.Linq;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
using System.Numerics;
using System.Diagnostics;
namespace Program
{

    public class Solver
    {
        public void Solve()
        {
            var w = sc.Integer();
            var h = sc.Integer();
            var map = Enumerate(h, x => sc.Char(w));
            var set = new DisjointSet(w * h);
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    if (map[i][j] == '.')
                    {
                        Debug.WriteLine("{0} {1}", i, j);
                        if (i + 1 < h && map[i + 1][j] == '.')
                            set.Unite(i * w + j, (i + 1) * w + j);
                        if (j + 1 < w && map[i][j + 1] == '.')
                            set.Unite(i * w + j, i * w + j + 1);
                    }
                }
            var id = -1;
            var q = new Queue<int>();
            var dist = new int[w * h];
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    if (map[i][j] == '.' && (id == -1 || set[i * w + j] == id))
                    {
                        if (id == -1)
                            id = set[i * w + j];
                        dist[i * w + j] = 0;
                        q.Enqueue(i * w + j);
                    }
                    else dist[i * w + j] = 1 << 20;
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            while (q.Any())
            {
                var p = q.Dequeue();
                var px = p / w;
                var py = p % w;
                for (int i = 0; i < 4; i++)
                {
                    var nx = px + dx[i];
                    var ny = py + dy[i];
                    if (nx < 0 || nx >= h || ny < 0 || ny >= w)
                        continue;
                    if (dist[nx * w + ny] > dist[p] + 1)
                    {
                        dist[nx * w + ny] = dist[p] + 1;
                        q.Enqueue(nx * w + ny);
                    }
                }
            }
            var ans = 1 << 20;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    if (map[i][j] == '.' && set[i * w + j] != id)
                        ans = Math.Min(dist[i * w + j], ans);

                }
            IO.Printer.Out.WriteLine(ans - 1);
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
