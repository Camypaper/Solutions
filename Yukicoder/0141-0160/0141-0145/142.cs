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
            var a = BitArray.Create();
            for (long s = sc.Integer(), x = sc.Long(), y = sc.Long(), z = sc.Long(), i = 0; i < n; i++, s = (x * s + y) % z)
                if ((s & 1) == 1) a.bits[i / 64] |= 1ul << (int)i;
            var q = sc.Integer();
            for (int _ = 0; _ < q; _++)
            {
                var s = sc.Integer() - 1;
                var t = sc.Integer();
                var u = sc.Integer() - 1;
                var v = sc.Integer();
                a.XorTo(a, s, u, t - s);
            }
            var ans = new char[n];
            for (int i = 0; i < n; i++)
                ans[i] = (a.bits[i / 64] >> i & 1) == 1 ? 'O' : 'E';
            IO.Printer.Out.WriteLine(ans.AsString());
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

public struct BitArray
{
    const int size = 100000;
    static public readonly ulong[] mask = new ulong[65];
    static BitArray()
    {
        mask[0] = 0;
        for (int i = 0; i < 64; i++)
            mask[i + 1] = (mask[i] << 1) | 1ul;
    }
    public ulong[] bits;
    public void XorTo(BitArray b, int p, int q, int len)//xor b[l,l+len)^=a[r,r+len)
    {
        var lx = p / 64;
        var ly = p % 64;
        var rx = (p + len) / 64;
        var ry = (p + len) % 64;

        var d = p - q;
        var dx = d / 64;
        var dy = d % 64;
        if (dy < 0) { dy += 64; dx--; }

        if (p > q)
        {
            if (lx == rx)
            {
                if (dy > 0 && lx - dx - 1 >= 0) b.bits[lx - dx - 1] ^= (bits[lx] & (mask[ry] & (mask[64] ^ mask[ly]))) << (64 - dy);
                b.bits[lx - dx] ^= (bits[lx] & (mask[ry] & (mask[64] ^ mask[ly]))) >> dy;
            }
            else
            {
                if (dy > 0 && lx - dx - 1 >= 0) b.bits[lx - dx - 1] ^= (bits[lx] & (mask[64] ^ mask[ly])) << (64 - dy);
                b.bits[lx - dx] ^= (bits[lx] & (mask[64] ^ mask[ly])) >> dy;
                for (var i = lx + 1; i < rx; i++)
                {
                    if (dy > 0) b.bits[i - dx - 1] ^= bits[i] << (64 - dy);
                    b.bits[i - dx] ^= bits[i] >> dy;
                }
                if (dy > 0) b.bits[rx - dx - 1] ^= (bits[rx] & (mask[ry])) << (64 - dy);
                b.bits[rx - dx] ^= (bits[rx] & (mask[ry])) >> dy;
            }
        }
        else
        {
            if (lx == rx)
            {
                b.bits[lx - dx] ^= (bits[lx] & (mask[ry] & (mask[64] ^ mask[ly]))) >> dy;
                if (dy > 0) b.bits[lx - dx - 1] ^= (bits[lx] & (mask[ry] & (mask[64] ^ mask[ly]))) << (64 - dy);
            }
            else
            {
                b.bits[rx - dx] ^= (bits[rx] & (mask[ry])) >> dy;
                if (dy > 0) b.bits[rx - dx - 1] ^= (bits[rx] & (mask[ry])) << (64 - dy);
                for (var i = rx - 1; i > lx; i--)
                {
                    b.bits[i - dx] ^= bits[i] >> dy;
                    if (dy > 0) b.bits[i - dx - 1] ^= bits[i] << (64 - dy);
                }
                b.bits[lx - dx] ^= (bits[lx] & (mask[64] ^ mask[ly])) >> dy;
                if (dy > 0 && lx - dx - 1 >= 0) b.bits[lx - dx - 1] ^= (bits[lx] & (mask[64] ^ mask[ly])) << (64 - dy);
            }
        }
    }
    public void And(BitArray other, int l, int r)//and [l,r)
    {
        for (int i = l; i < r; i++)
            bits[i] &= other.bits[i];
    }
    public void Or(BitArray other, int l, int r)//or [l,r)
    {
        for (int i = l; i < r; i++)
            bits[i] |= other.bits[i];
    }
    static public BitArray operator >>(BitArray l, int r)
    {
        var ret = Create();
        ret.bits[size - 1] = l.bits[size - 1] >> r;
        for (int i = size - 2; i >= 0; i--)
            ret.bits[i] = l.bits[i] >> r | (l.bits[i + 1] << 64 - r);
        return ret;
    }
    static public BitArray operator <<(BitArray l, int r)
    {
        var ret = Create();
        ret.bits[0] = l.bits[0] << r;
        for (int i = 1; i < size; i++)
            ret.bits[i] = l.bits[i] << r | (l.bits[i - 1] >> 64 - r);
        return ret;
    }

    static public BitArray operator &(BitArray l, BitArray r)
    {
        var ret = Create();
        for (int i = 0; i < size; i++)
            ret.bits[i] = l.bits[i] & r.bits[i];
        return ret;
    }
    static public BitArray operator |(BitArray l, BitArray r)
    {
        var ret = Create();
        for (int i = 0; i < size; i++)
            ret.bits[i] = l.bits[i] | r.bits[i];
        return ret;
    }
    static public BitArray operator ^(BitArray l, BitArray r)
    {
        var ret = Create();
        for (int i = 0; i < size; i++)
            ret.bits[i] = l.bits[i] ^ r.bits[i];
        return ret;
    }
    static public int PopCount(BitArray a)
    {
        var ret = 0; for (int i = 0; i < size; i++) ret += popCount(a.bits[i]);
        return ret;
    }
    static public BitArray Create() { return new BitArray() { bits = new ulong[size] }; }
    static int popCount(ulong i)
    {
        i = i - ((i >> 1) & 0x5555555555555555UL);
        i = (i & 0x3333333333333333ul) + ((i >> 2) & 0x3333333333333333ul);
        return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0Ful) * 0x101010101010101ul) >> 56);
    }
}
