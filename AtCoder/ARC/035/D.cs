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
            var x = new int[n];
            var y = new int[n];
            var perm = new double[1000050];
            var count = new long[1000050];
            perm[0] = 1.0;
            for (int i = 1; i < 1000050; i++)
            {
                perm[i] = perm[i - 1] * i;
                count[i] = count[i - 1];
                while (perm[i] >= 2.0)
                {
                    perm[i] /= 2;
                    count[i]++;
                }
            }
            for (int i = 0; i < n; i++)
            {
                x[i] = sc.Integer();
                y[i] = sc.Integer();
            }
            var tree = new SegmentTree(n - 1);
            for (int i = 0; i < n - 1; i++)
            {
                var dx = x[i + 1] - x[i];
                var dy = y[i + 1] - y[i];
                var all = count[dx + dy];
                var div = count[dx] + count[dy];
                var allv = perm[dx + dy];
                var divv = perm[dx] * perm[dy];
                var cnt = all - div;
                var val = allv / divv;
                if (val >= 2.0) { cnt++; val /= 2.0; }
                else if (val < 1.0) { cnt--; val *= 2.0; }
                tree.Update(i, cnt, val);
            }
            var q = sc.Integer();
            for (int i = 0; i < q; i++)
            {
                var t = sc.Integer();
                if (t == 1)
                {
                    var k = sc.Integer() - 1;
                    var u = sc.Integer();
                    var v = sc.Integer();
                    x[k] = u;
                    y[k] = v;
                    if (k < n - 1)
                    {
                        var dx = x[k + 1] - x[k];
                        var dy = y[k + 1] - y[k];
                        var all = count[dx + dy];
                        var div = count[dx] + count[dy];
                        var allv = perm[dx + dy];
                        var divv = perm[dx] * perm[dy];
                        var cnt = all - div;
                        var val = allv / divv;
                        if (val >= 2.0) { cnt++; val /= 2.0; }
                        else if (val < 1.0) { cnt--; val *= 2.0; }
                        tree.Update(k, cnt, val);
                    }
                    if (k > 0)
                    {
                        var dx = x[k] - x[k - 1];
                        var dy = y[k] - y[k - 1];
                        var all = count[dx + dy];
                        var div = count[dx] + count[dy];
                        var allv = perm[dx + dy];
                        var divv = perm[dx] * perm[dy];
                        var cnt = all - div;
                        var val = allv / divv;
                        if (val >= 2.0) { cnt++; val /= 2.0; }
                        else if (val < 1.0) { cnt--; val *= 2.0; }
                        tree.Update(k - 1, cnt, val);
                    }



                }
                else
                {
                    var l = sc.Integer() - 1;
                    var r = sc.Integer() - 1;
                    var u = sc.Integer() - 1;
                    var v = sc.Integer() - 1;
                    var first = tree.Query(l, r);
                    var sec = tree.Query(u, v);
                    if (first.Key > sec.Key)
                        IO.Printer.Out.WriteLine("FIRST");
                    else
                        IO.Printer.Out.WriteLine("SECOND");

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
#region SegTree
[System.Diagnostics.DebuggerDisplay("Data={ToString()}")]
public class SegmentTree
{
    int n;
    long[] cnt;
    double[] val;
    public SegmentTree(int size)
    {
        n = 1;
        while (n < size)
            n <<= 1;
        cnt = new long[n << 1];
        val = new double[n << 1];
        for (int i = 0; i < val.Length; i++)
            val[i] = 1.0;
    }

    // turn kth index value into v
    public void Update(int k, long v, double rem)
    {
        k += n;
        cnt[k] = v;
        val[k] = rem;
        k >>= 1;
        while (k > 0)
        {
            cnt[k] = cnt[k << 1] + cnt[(k << 1) + 1];
            val[k] = val[k << 1] * val[(k << 1) + 1];
            if (val[k] >= 2.0)
            {
                cnt[k]++;
                val[k] /= 2.0;
            }
            else if (val[k] < 1)
            {
                cnt[k]--;
                val[k] *= 2.0;
            }
            k >>= 1;
        }

    }
    public KeyValuePair<long, double> Query(int a, int b) { return Query(a, b, 1, 0, n); }
    private KeyValuePair<long, double> Query(int a, int b, int k, int l, int r)
    {
        if (r <= a || b <= l)
            return new KeyValuePair<long, double>(0, 1);
        if (a <= l && r <= b)
            return new KeyValuePair<long, double>(cnt[k], val[k]);
        else
        {
            var vl = Query(a, b, k << 1, l, (l + r) >> 1);
            var vr = Query(a, b, (k << 1) + 1, (l + r) >> 1, r);
            var t = vl.Key + vr.Key;
            var v = vl.Value * vr.Value;
            if (v >= 2.0) { t++; v /= 2; }
            else if (v < 1.0) { t--; v *= 2.0; }
            return new KeyValuePair<long, double>(t, v);
        }
    }

}
#endregion
