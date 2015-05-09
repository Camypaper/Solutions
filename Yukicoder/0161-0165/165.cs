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
            var b = sc.Integer();
            var p = new Triplet<long>[n];
            var xs = new List<long>();
            var xp = new List<long>();
            var ys = new List<long>();
            var yp = new List<long>();
            for (int i = 0; i < n; i++)
            {
                p[i] = new Triplet<long>(sc.Long() * 2, sc.Long() * 2, sc.Long());
                xp.Add(p[i].I);
                yp.Add(p[i].J);
            }
            xp.Sort(); yp.Sort();
            foreach (var x in xp)
                xs.Add(x - 1);
            xs.Add(xp[xp.Count - 1] + 1);
            foreach (var y in yp)
                ys.Add(y - 1);
            ys.Add(yp[yp.Count - 1] + 1);
            xp.Add(long.MinValue);
            yp.Add(long.MinValue);
            var zipX = xs.Concat(xp).Distinct().ToArray();
            var zipY = ys.Concat(yp).Distinct().ToArray();
            Array.Sort(zipX); Array.Sort(zipY);
            var Xsize = zipX.Length;
            var Ysize = zipY.Length;
            var sum = new long[Xsize + 1, Ysize + 1];
            var cnt = new int[Xsize + 1, Ysize + 1];
            for (int i = 0; i < n; i++)
            {
                var px = Array.BinarySearch(zipX, p[i].I);
                var py = Array.BinarySearch(zipY, p[i].J);
                sum[px, py] += p[i].K;
                cnt[px, py]++;
            }
            for (int i = 0; i <= Xsize; i++)
                for (int j = 1; j <= Ysize; j++)
                {
                    sum[i, j] += sum[i, j - 1];
                    cnt[i, j] += cnt[i, j - 1];
                }
            for (int i = 1; i <= Xsize; i++)
                for (int j = 0; j <= Ysize; j++)
                {
                    sum[i, j] += sum[i - 1, j];
                    cnt[i, j] += cnt[i - 1, j];
                }
            var pos = xs.Distinct().ToArray();
            var Z = ys.Distinct().ToArray();
            Array.Sort(pos); Array.Sort(Z);
            for (int i = 0; i < Z.Length; i++)
                Z[i] = Array.BinarySearch(zipY, Z[i]);
            var max = 0;
            for (int i = 0; i < pos.Length; i++)
                for (int j = i + 1; j < pos.Length; j++)
                {
                    var from = Array.BinarySearch(zipX, pos[i]);
                    var to = Array.BinarySearch(zipX, pos[j]);
                    int l = 0, r = 0;
                    while (l < Z.Length)
                    {
                        long val = 0;
                        while (r < Z.Length)
                        {
                            val = sum[to, Z[r]] + sum[from - 1, Z[l] - 1] - sum[from - 1, Z[r]] - sum[to, Z[l] - 1];
                            if (val > b) break;
                            else max = Math.Max(max, cnt[to, Z[r]] + cnt[from - 1, Z[l] - 1] - cnt[from - 1, Z[r]] - cnt[to, Z[l] - 1]);
                            r++;
                        }
                        if (r == Z.Length)
                            break;
                        while (l < r)
                        {
                            val = sum[to, Z[r]] + sum[from - 1, Z[l] - 1] - sum[from - 1, Z[r]] - sum[to, Z[l] - 1];
                            if (val <= b) { max = Math.Max(max, cnt[to, Z[r]] + cnt[from - 1, Z[l] - 1] - cnt[from - 1, Z[r]] - cnt[to, Z[l] - 1]); break; }
                            l++;
                        }
                    }
                }
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
#region Triplet<T>

public struct Triplet<T>
{
    public T I, J, K;
    public Triplet(T i, T j, T k) : this() { I = i; J = j; K = k; }
    public Triplet(params T[] arg) : this(arg[0], arg[1], arg[2]) { }
    public override string ToString() { return string.Format("{0} {1} {2}", I, J, K); }
}
#endregion
