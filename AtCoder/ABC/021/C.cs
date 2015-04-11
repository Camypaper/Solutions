using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
using System.Numerics;
using System.Threading.Tasks;
namespace Program
{
    public class Solver
    {
        public void Solve()
        {
            var n = sc.Integer();
            var a = sc.Integer() - 1;
            var b = sc.Integer() - 1;
            var m = sc.Integer();
            var G = Enumerate(n, x => new List<int>());
            var d = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    d[i, j] = 1 << 20;
            for (int i = 0; i < n; i++)
                d[i, i] = 0;
            for (int i = 0; i < m; i++)
            {
                var f = sc.Integer() - 1;
                var g = sc.Integer() - 1;
                G[f].Add(g);
                G[g].Add(f);
                d[f, g] = d[g, f] = 1;

            }
            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        d[i, j] = Math.Min(d[i, j], d[i, k] + d[k, j]);

            var dp = new ModInteger[n];
            dp[a] = 1;
            var q = new Queue<Edge>();
            q.Enqueue(new Edge(-1, a, 0));
            var used = new bool[n];
            while (q.Any())
            {
                var p = q.Dequeue();
                Debug.WriteLine("{0} {1} {2}", p.from, p.to, p.dist);
                if (p.from != -1)
                    dp[p.to] += dp[p.from];
                if (used[p.to])
                    continue;
                used[p.to] = true;

                foreach (var next in G[p.to])
                {
                    if (d[a, next] == p.dist + 1)
                        q.Enqueue(new Edge(p.to, next, p.dist + 1));
                }
            }
            IO.Printer.Out.WriteLine(dp[b]);
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


#region ModNumber
public struct ModInteger
{
    public const long Mod = (long)1e9 + 7;
    public long num;
    public ModInteger(long n) : this() { num = n % Mod; }
    public override string ToString() { return num.ToString(); }
    public static ModInteger operator +(ModInteger l, ModInteger r) { var n = l.num + r.num; if (n >= Mod)n -= Mod; return new ModInteger(n); }
    public static ModInteger operator -(ModInteger l, ModInteger r) { var n = l.num - r.num; if (n < 0)n += Mod; return new ModInteger(n); }
    public static ModInteger operator *(ModInteger l, ModInteger r) { return new ModInteger((l.num * r.num) % Mod); }
    public static implicit operator ModInteger(long n) { return new ModInteger(n); }
}
#endregion
