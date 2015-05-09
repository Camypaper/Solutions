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
            var table = new CombinationTable(2500);
            Func<int, int, long> nhm = (n, m) => table[n + m - 1, m];

            var h = new int[26];
            foreach (var x in "helloworld")
                h[x - 'a']++;
            var c = sc.Integer(26);
            for (int i = 0; i < 26; i++)
                if (c[i] < h[i])
                {
                    IO.Printer.Out.WriteLine(0);
                    return;
                }
            var str = "helloworld".Distinct().ToArray();
            var sum = c.Sum();
            var len = 0;
            foreach (var x in str)
                len += c[x - 'a'];
            long ans = 1;
            ans *= table[c['h' - 'a'], 1];
            ans *= table[c['e' - 'a'], 1];
            long max = 0;
            for (int i = 2; i < c['l' - 'a']; i++)
                max = Math.Max(max, table[i, 2] * (c['l' - 'a'] - i));
            ans *= max;
            ans *= table[c['o' - 'a'] / 2, 1] * (c['o' - 'a'] - c['o' - 'a'] / 2);
            ans *= table[c['w' - 'a'], 1];
            ans *= table[c['r' - 'a'], 1];
            ans *= table[c['d' - 'a'], 1];
            //if (sum > len)
              //  ans *= nhm(sum - len, len + 1);
            IO.Printer.Out.WriteLine(ans);


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

#region CombinationTable
public class CombinationTable
{
    long[][] nCr;
    public CombinationTable(int n)
    {
        nCr = new long[n + 1][];
        for (int i = 0; i <= n; i++)
            nCr[i] = new long[i + 2];
        nCr[0][0] = 1;
        for (int i = 1; i <= n; i++)
            for (int j = 0; j <= i; j++)
                if (j > 0)
                    nCr[i][j] = nCr[i - 1][j - 1] + nCr[i - 1][j];
                else nCr[i][j] = 1;
    }
    public long this[int n, int r]
    {
        get
        {
            if (n <= 0 || n > nCr.Length)
                throw new IndexOutOfRangeException("n<=0 || n>N ");
            if (r <= 0 || r > n)
                throw new IndexOutOfRangeException("r<=0 ||r>n");
            return nCr[n][r];
        }
    }
}
#endregion
