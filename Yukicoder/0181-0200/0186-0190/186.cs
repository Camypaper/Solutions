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
            var a = new BigInteger[3];
            for (long i = 0; i < 3; i++)
                a[i] = 1;
            var b = new BigInteger[3];
            var mod = new BigInteger[3];
            for (long i = 0; i < 3; i++)
            {
                b[i] = sc.Integer();
                mod[i] = sc.Integer();
            }
            var ans = MathEX.LiniearCongruence(a, b, mod);
            if (ans.Key <= 0)
                IO.Printer.Out.WriteLine(-1);
            else IO.Printer.Out.WriteLine(ans.Key);

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
static public class MathEX
{
    static public BigInteger GCD(BigInteger x, BigInteger y)
    {
        byte i = 0;
        while (x != 0 && y != 0)
        {
            if (i == 0)
                y %= x;
            else x %= y;
            i ^= 1;
        }
        return x == 0 ? y : x;
    }


    //O(log n)
    //ax+by=gcd(a,b),return x,y
    static public BigInteger ExGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        var u = new BigInteger[] { a, 1, 0 };
        var v = new BigInteger[] { b, 0, 1 };
        while (v[0] != 0)
        {
            var t = u[0] / v[0];
            for (int i = 0; i < 3; i++)
            {
                var tmp = u[i] - t * v[i];
                u[i] = v[i];
                v[i] = tmp;
            }
        }
        x = u[1];
        y = u[2];
        if (u[0] > 0)
            return u[0];
        for (long i = 0; i < 3; i++)
            u[i] = -u[i];
        return u[0];

    }
    static public BigInteger Inverse(BigInteger num, BigInteger mod)
    {
        BigInteger p, q;
        ExGCD(num, mod, out p, out q);
        return (p % mod + mod) % mod;
    }


    static public KeyValuePair<BigInteger, BigInteger> LiniearCongruence(BigInteger[] A, BigInteger[] B, BigInteger[] M)
    {
        BigInteger x = 0;
        BigInteger m = 1;
        var n = A.Length;
        for (int i = 0; i < n; i++)
        {
            var a = A[i] * m;
            var b = B[i] - A[i] * x;
            var d = GCD(M[i], a);
            if (b % d != 0) return new KeyValuePair<BigInteger, BigInteger>(0, -1);
            var t = ((b / d) * Inverse(a / d, M[i] / d)) % (M[i] / d);
            x = x + m * t;
            m *= M[i] / d;

        }
        while (x < 0)
            x += m;
        if (x % m == 0)
            x += m;
        return new KeyValuePair<BigInteger, BigInteger>(x, m);
    }
}
