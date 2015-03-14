using System;
using System.Linq;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;
using StringBuilder = System.Text.StringBuilder;
//using System.Numerics;
namespace Program
{
    public class Solver
    {
        public void Solve()
        {
            var n = sc.Integer();
            var a = sc.Integer(n);
            Array.Sort(a);
            var cum = new long[n + 1];
            for (int i = 0; i < n; i++)
                cum[i + 1] = cum[i] + a[i];
            var m = sc.Integer();
            var b = sc.Integer(m);
            var c = sc.Long(m);
            for (int i = 0; i < m; i++)
            {
                var p = a.UpperBound(b[i]);
                Debug.WriteLine(cum[p]);
                if (cum[p] >= c[i])
                    IO.Printer.Out.WriteLine("Yes");
                else IO.Printer.Out.WriteLine("No");

            }

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
    //static public string AsString(this IEnumerable<char> ie) { return new string(System.Linq.Enumerable.ToArray(ie)); }
    //static public string AsJoinedString<T>(this IEnumerable<T> ie, string st = " ") { return string.Join(st, ie); }
    static public void Main()
    {
        var solver = new Program.Solver();
        solver.Solve();
        Program.IO.Printer.Out.Flush();
    }
}
#endregion


#region BinarySearch for Array
static public partial class Algorithm
{

    static private int binarySearch<T>(this T[] a, int idx, int len, T v, Comparison<T> cmp, bool islb)
    {
        int l = idx;
        int h = idx + len - 1;
        while (l <= h)
        {
            int i = l + ((h - l) >> 1);
            int ord;
            if (a[i] == null || v == null) return -1;
            else ord = cmp(a[i], v);
            if (ord < 0) l = i + 1;
            else if (ord == 0)
            {
                if (!islb) l = i + 1;
                else h = i - 1;
            }
            else h = i - 1;
        }

        return l;
    }
    static public int UpperBound<T>(this T[] a, int idx, int len, T v, Comparison<T> cmp) { return binarySearch(a, idx, len, v, cmp, false); }
    static public int UpperBound<T>(this T[] a, int idx, int len, T v) where T : IComparable<T> { return UpperBound(a, idx, len, v, Comparer<T>.Default.Compare); }
    static public int UpperBound<T>(this T[] a, T v) where T : IComparable<T> { return UpperBound(a, 0, a.Length, v, Comparer<T>.Default.Compare); }
    static public int UpperBound<T>(this T[] a, T v, IComparer<T> cmp) { return UpperBound(a, 0, a.Length, v, cmp.Compare); }
    static public int UpperBound<T>(this T[] a, T v, Comparison<T> cmp) { return UpperBound(a, 0, a.Length, v, cmp); }

    static public int LowerBound<T>(this T[] a, int idx, int len, T value, Comparison<T> cmp) { return binarySearch(a, idx, len, value, cmp, true); }
    static public int LowerBound<T>(this T[] a, int idx, int len, T value) where T : IComparable<T> { return LowerBound(a, idx, len, value, Comparer<T>.Default.Compare); }
    static public int LowerBound<T>(this T[] a, T val) where T : IComparable<T> { return LowerBound(a, 0, a.Length, val, Comparer<T>.Default.Compare); }
    static public int LowerBound<T>(this T[] a, T val, IComparer<T> cmp) { return LowerBound(a, 0, a.Length, val, cmp.Compare); }
    static public int LowerBound<T>(this T[] a, T v, Comparison<T> cmp) { return LowerBound(a, 0, a.Length, v, cmp); }
}
#endregion
