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
            var w = sc.Integer();
            var h = sc.Integer();
            var map = Enumerate(h, x => sc.Integer(w));
            var dp = new int[w * h, w * h];//iから来て現在j
            for (int i = 0; i < w * h; i++)
                for (int j = 0; j < w * h; j++)
                    dp[i, j] = 1 << 20;
            var q = new Queue<state>();
            q.Enqueue(new state(-1, 0));
            int[] dx = { 1, -1, 0, 0};
            int[] dy = { 0, 0, 1, -1};
            var min = 1 << 20;

            Func<int, int, int, bool> ok = (x, y, z) =>
                {
                    if (map[x / w][x % w] == map[y / w][y % w] || map[x / w][x % w] == map[z / w][z % w] || map[y / w][y % w] == map[z / w][z % w])
                        return false;
                    if (map[x / w][x % w] < map[y / w][y % w] && map[y / w][y % w] > map[z / w][z % w])
                        return true;
                    if (map[x / w][x % w] > map[y / w][y % w] && map[y / w][y % w] < map[z / w][z % w])
                        return true;
                    return false;
                };
            while (q.Any())
            {
                var p = q.Dequeue();
                if (p.cur == w * h - 1)
                    min = Math.Min(min, dp[p.prev, p.cur]);
                var x = p.cur / w;
                var y = p.cur % w;
                var px = p.prev / w;
                var py = p.prev % w;
                Debug.WriteLine("{0} {1}", p.prev, p.cur);
                for (int i = 0; i < 4; i++)
                {
                    var nx = x + dx[i];
                    var ny = y + dy[i];
                    if (nx < 0 || nx >= h || ny < 0 || ny >= w)
                        continue;
                    if (p.prev == -1)
                    {
                        dp[p.cur, nx * w + ny] = 1;
                        q.Enqueue(new state(p.cur, nx * w + ny));
                    }
                    else if (ok(p.prev, p.cur, nx * w + ny))
                    {
                        if (dp[p.prev, p.cur] + 1 < dp[p.cur, nx * w + ny])
                        {
                            dp[p.cur, nx * w + ny] = dp[p.prev, p.cur] + 1;
                            q.Enqueue(new state(p.cur, nx * w + ny));
                        }

                    }
                }
            }
            if (min >= 1 << 20) min = -1;
            IO.Printer.Out.WriteLine(min);

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
        public char Char() { byte b = 0; do b = read(); while ((b < 33 || 126 < b) && !isEof); return (char)b; }

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
public struct state
{
    public int prev, cur;
    public state(int q, int p) : this() { prev = q; cur = p; }
}
