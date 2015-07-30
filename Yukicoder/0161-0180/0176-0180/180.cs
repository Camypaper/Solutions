using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            var a = new int[n];
            var b = new long[n];
            for (int i = 0; i < n; i++)
            {
                b[i] = sc.Integer();
                a[i] = sc.Integer();
            }
            var y = (long)Algorithm.GoldenSectionSearch(1.0, 1000000000000000000, x =>
            {
                var max = double.MinValue;
                var min = double.MaxValue;
                for (int i = 0; i < n; i++)
                {
                    max = Math.Max(a[i] * x + b[i], max);
                    min = Math.Min(a[i] * x + b[i], min);
                }
                return (double)(max - min);
            });
            var y2 = (long)Algorithm.GoldenSectionSearch(1.0, y, x =>
            {
                var max = double.MinValue;
                var min = double.MaxValue;
                for (int i = 0; i < n; i++)
                {
                    max = Math.Max(a[i] * x + b[i], max);
                    min = Math.Min(a[i] * x + b[i], min);
                }
                return (double)(max - min);
            });
            var ans = long.MaxValue;
            long ansX = 1;
            {
                var max = long.MinValue;
                var min = long.MaxValue;
                for (int i = 0; i < n; i++)
                {
                    max = Math.Max(a[i] * 1 + b[i], max);
                    min = Math.Min(a[i] * 1 + b[i], min);
                }
                ans = max - min;
               
            }

            for (int j = -50000; j < 50000; j++)
            {
                var x = j + y;
                if (x < 1)
                    continue;
                var max = long.MinValue;
                var min = long.MaxValue;
                for (int i = 0; i < n; i++)
                {
                    max = Math.Max(a[i] * x + b[i], max);
                    min = Math.Min(a[i] * x + b[i], min);
                }
                if (ans > max - min)
                {
                    ans = max - min;
                    ansX = x;

                }
                else if (ans == max - min && ansX > x)
                {
                    ansX = x;
                }
            } for (int j = -50000; j < 50000; j++)
            {
                var x = j + y2;
                if (x < 1)
                    continue;
                var max = long.MinValue;
                var min = long.MaxValue;
                for (int i = 0; i < n; i++)
                {
                    max = Math.Max(a[i] * x + b[i], max);
                    min = Math.Min(a[i] * x + b[i], min);
                }
                if (ans > max - min)
                {
                    ans = max - min;
                    ansX = x;

                }
                else if (ans == max - min && ansX > x)
                {
                    ansX = x;
                }
            }
            IO.Printer.Out.WriteLine(ansX);

        }


        public IO.StreamScanner sc;
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
        solver.sc = new Program.IO.StreamScanner(Console.OpenStandardInput());
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
        for (int i = 0; i < 1000; i++)
        {
            //if (fc < fd)//凸
            if (fc > fd)//凹
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