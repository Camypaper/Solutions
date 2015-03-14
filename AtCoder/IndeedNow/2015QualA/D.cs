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
            var h = sc.Integer();
            var w = sc.Integer();
            var map = Enumerate(h, x => sc.Integer(w));
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            long ans = 0;
            for (int i = 1; i < h * w; i++)
                ans = ans * h * w + i;
            ans *= h * w;
            var sx = -1;
            var sy = -1;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    if (map[i][j] == 0) { sx = i; sy = j; }
            var q = new Queue<Tuple<int, int[][], int, int, int, int>>();
            q.Enqueue(new Tuple<int, int[][], int, int, int, int>(0, map, sx, sy, -1, -1));
            var used = new HashSet<long>();
            while (q.Any())
            {
                var p = q.Dequeue();
                var hash = HashCode(p.Item2);

                var d = getDistance(p.Item2);
                //Debug.WriteLine(d);
                if (d == 0)
                {
                    IO.Printer.Out.WriteLine(p.Item1);
                    return;
                }
                //foreach(var x in p.Item2)
                //  IO.Printer.Out.WriteLine(x.AsJoinedString());
                //IO.Printer.Out.WriteLine();
                // if (!used.Add(hash))
                //   continue;
                for (int i = 0; i < 4; i++)
                {
                    var nx = p.Item3 + dx[i];
                    var ny = p.Item4 + dy[i];
                    if (nx < 0 || nx >= h)
                        continue;
                    if (ny < 0 || ny >= w)
                        continue;
                    if (nx == p.Item5 && ny == p.Item6)
                        continue;
                    var mat = Enumerate(h, x => p.Item2[x].Clone() as int[]);
                    Swap(ref mat[nx][ny], ref mat[p.Item3][p.Item4]);
                    var nh = HashCode(mat);
                    var nd = getDistance(mat);
                    if (nd + p.Item1 > 24)
                        continue;
                    // if (used.Contains(nh))
                    //   continue;
                    q.Enqueue(new Tuple<int, int[][], int, int,int,int>(p.Item1 + 1, mat, nx, ny, p.Item3, p.Item4));

                }
            }
        }
        static long HashCode(int[][] map)
        {
            var h = map.Length;
            var w = map[0].Length;
            var size = h * w;
            long ret = 0;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    ret = ret * size + map[i][j];
            return ret;
        }
        static long getDistance(int[][] map)
        {
            var h = map.Length;
            var w = map[0].Length;
            var size = h * w;
            long ret = 0;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    var val = i * w + j + 1;
                    if (val == size)
                        continue;
                    for (int u = 0; u < h; u++)
                        for (int v = 0; v < w; v++)
                        {
                            if (map[u][v] == val)
                            {
                                ret += Math.Abs(u - i) + Math.Abs(v - j);
                            }
                        }

                }
            return ret;



        }
        static public void Swap<T>(ref T a, ref T b) { var c = a; a = b; b = c; }

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
