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
            var mat = new ModMatrix(2, 2);
            mat[0, 0] = mat[0, 1] = mat[1, 0] = 1;
            mat.Build();
            var n = sc.Integer();
            ModInteger ans = 1;
            for (int i = 0; i < n; i++)
            {
                var c = sc.Long();
                ans *= ModInteger.Pow(mat.Pow(c + 1)[0, 0], ModInteger.Parse(sc.Scan()));
            }
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
#region ModMatrix
public class ModMatrix
{
    int row, col;
    ModInteger[,] mat;
    public ModInteger this[int r, int c]
    {
        get { return mat[r, c]; }
        set { mat[r, c] = value; }
    }
    public ModMatrix(int r, int c)
    {
        row = r; col = c;
        mat = new ModInteger[r, c];
    }
    public static ModMatrix operator +(ModMatrix l, ModMatrix r)
    {
        check(l, r);
        var ret = new ModMatrix(l.row, l.col);
        for (int i = 0; i < l.row; i++)
            for (int j = 0; j < l.col; j++)
                ret.mat[i, j] = l.mat[i, j] + r.mat[i, j];
        return ret;

    }
    public static ModMatrix operator *(ModMatrix l, ModMatrix r)
    {
        checkMul(l, r);
        var ret = new ModMatrix(l.row, l.col);
        for (int i = 0; i < l.row; i++)
            for (int k = 0; k < l.row; k++)
                for (int j = 0; j < l.col; j++)
                    ret.mat[i, j] += l.mat[i, k] * r.mat[k, j];
        return ret;
    }
    public static ModMatrix Pow(ModMatrix m, long n)
    {
        var ret = new ModMatrix(m.row, m.col);
        for (int i = 0; i < m.row; i++)
            ret.mat[i, i] = 1;
        for (; n > 0; m *= m, n >>= 1)
        {
            if ((n & 1) == 1)
                ret = ret * m;
        }
        return ret;

    }
    public void Build()
    {
        mats = new ModMatrix[100];
        mats[0] = this;
        for (int i = 1; i < 100; i++)
            mats[i] = mats[i - 1] * mats[i - 1];
    }
    ModMatrix[] mats;
    public ModMatrix Pow(long n)
    {
        var ret = new ModMatrix(row, col);
        for (int i = 0; i < row; i++)
            ret.mat[i, i] = 1;
        for (int i = 0; n > 0; n >>= 1, i++)
        {
            if ((n & 1) == 1)
                ret = ret * mats[i];
        }
        return ret;
    }

    [System.Diagnostics.Conditional("DEBUG")]
    static private void check(ModMatrix a, ModMatrix b)
    {
        if (a.row != b.row || a.col != b.col)
            throw new Exception("row and col have to be same.");

    }
    [System.Diagnostics.Conditional("DEBUG")]
    static private void checkMul(ModMatrix a, ModMatrix b)
    {
        if (a.col != b.row)
            throw new Exception("row and col have to be same.");

    }
}
#endregion
#region ModNumber
public partial struct ModInteger
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

#region ModPow
public partial struct ModInteger
{
    static public ModInteger Pow(ModInteger v, long k)
    {
        ModInteger ret = 1;
        var n = k;
        for (; n > 0; n >>= 1, v *= v)
        {
            if ((n & 1) == 1)
                ret = ret * v;
        }
        return ret;
    }
}
#endregion
#region StrToMod
public partial struct ModInteger
{
    static public long[] tens = new long[500];
    static ModInteger()
    {
        tens[0] = 1;
        for (int i = 1; i < 500; i++)
            tens[i] = (tens[i - 1] * 10) % (Mod - 1);
    }
    static public long Parse(string s)
    {
        long ret = 0;
        for (int i = s.Length - 1, j = 0; i >= 0; i--, j++)
            ret = (ret + (s[i] - '0') * tens[j]) % (Mod - 1);
        if (ret == 0)
            ret += Mod - 1;
        return ret;
    }

}
#endregion