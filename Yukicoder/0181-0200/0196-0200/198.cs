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
            var b = sc.Long();
            var n = sc.Integer();
            var c = sc.Long(n);
            Array.Sort(c);
            var min = long.MaxValue;
            var sum = c.Sum() + b;
            var p = Algorithm.GoldenSectionSearch(0, 2e9, x =>
                {
                    if (x * n > sum)
                        return double.MaxValue;
                    double val = 0;
                    for (int i = 0; i < n; i++)
                    {
                        val += Math.Abs(x - c[i]);
                    }
                    return val;
                });
            for (int i = -10000; i < 100000; i++)
            {
                long val = 0;
                long v = (long)p + i;
                if (v < 0)
                    continue;
                if (v * n > sum)
                    continue;
                for (int j = 0; j < n; j++)
                    val += Math.Abs(v - c[j]);
                min = Math.Min(val, min);
            }
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
#region GoldenSectionSearch
static public partial class Algorithm
{
    static public double GoldenSectionSearch(double a, double b, Func<double, double> function)
    {
        var r = 2 / (3 + Math.Sqrt(5));
        var c = a + r * (b - a);
        var d = b - r * (b - a);
        var fc = function(c);
        var fd = function(d);
        for (int i = 0; i < 100; i++)
        {
            //if (fc < fd)//凸
            if(fc>fd)//凹
            {
                a = c; c = d; d = b - r * (b - a);
                fc = fd; fd = function(d);
            }
            else
            {
                b = d; d = c; c = a + r * (b - a);
                fd = fc; fc = function(c);
            }
        }
        return c;
    }
}
#endregion