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
            var a = sc.Integer(n);
            var dic = new Dictionary<int, int>();
            var rmq = new SegmentTree(n);
            var ev = Enumerate(n, x => new List<KeyValuePair<int, int>>());
            for (int i = 0; i < m; i++)
            {
                var l = sc.Integer() - 1;
                var r = sc.Integer() - 1;
                ev[r].Add(new KeyValuePair<int, int>(i, l));
            }
            var ans = new long[m];
            for (int i = 0; i < n; i++)
            {
                var v = a[i];
                if(dic.ContainsKey(v))
                    rmq.Update(dic[v], i - dic[v]);
                dic[v] = i;
                foreach (var kv in ev[i])
                {
                    var l = kv.Value;
                    var id = kv.Key;
                    var min = rmq.Query(l, i + 1);
                    if (min == SegmentTree.Max)
                        min = -1;
                    ans[id] = min;
                }

            }
            foreach(var v in ans)
                IO.Printer.Out.WriteLine(v);
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
    public const long Max = 1L << 60;
    int n;
    long[] data;
    public SegmentTree(int size)
    {
        n = 1;
        while (n < size)
            n <<= 1;
        data = new long[n << 1];
        for (int i = 0; i < data.Length; i++)
            data[i] = Max;
    }

    // turn kth index value into v
    public void Update(int k, long v)
    {
        k += n;
        data[k] = v;
        k >>= 1;
        while (k > 0)
        {
            data[k] = Math.Min(data[k << 1], data[(k << 1) + 1]);
            k >>= 1;
        }

    }
    public long Query(int a, int b) { return Query(a, b, 1, 0, n); }
    private long Query(int a, int b, int k, int l, int r)
    {
        if (r <= a || b <= l)
            return Max;
        if (a <= l && r <= b)
            return data[k];
        else
        {
            var vl = Query(a, b, k << 1, l, (l + r) >> 1);
            var vr = Query(a, b, (k << 1) + 1, (l + r) >> 1, r);
            return Math.Min(vl, vr);
        }
    }

}
#endregion
