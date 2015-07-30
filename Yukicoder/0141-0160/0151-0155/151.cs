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
            var q = sc.Integer();
            RBSTNode left = null;
            RBSTNode right = null;
            for (int i = 0; i < n; i++)
            {
                left = RBSTNode.Insert(left, 0, new RBSTNode(0));
                right = RBSTNode.Insert(right, 0, new RBSTNode(0));
            }
            for (int _ = 0; _ < q; _++)
            {
                var rl1 = RBSTNode.Split(left, 1);
                var rl2 = RBSTNode.Split(right, n - 1);
                left = RBSTNode.Merge(rl1.rst, rl2.rst);
                right = RBSTNode.Merge(rl1.lst, rl2.lst);
                var t = sc.Char();
                var x = sc.Integer();
                var y = sc.Integer();
                if (t == 'L')
                    RBSTNode.Add(left, x, x + 1, y);
                else if (t == 'R')
                    RBSTNode.Add(right, x, x + 1, y);
                else
                {
                    var l = RBSTNode.Sum(left, x, y);
                    var r = RBSTNode.Sum(right, x, y);
                    IO.Printer.Out.WriteLine(l + r);
                }
            }

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

#region RBSTNode

public class RBSTNode
{
    public long val;
    public RBSTNode lst, rst;
    public int cnt;
    public long add;
    public long sum;
    // public long max;
    public RBSTNode(long v) { val = v; Update(this); }
    private RBSTNode(RBSTNode l, RBSTNode r) { lst = l; rst = r; }

    static public RBSTNode Build(long[] a, int b, int e)
    {
        var n = e - b;
        if (n == 0) return null;
        var m = b + (n >> 1);
        var l = Build(a, b, m);
        var r = Build(a, m + 1, e);
        var ret = new RBSTNode(a[m]) { lst = l, rst = r };
        return Update(ret);
    }
    static public long[] ToArray(RBSTNode n)
    {
        var a = new long[Count(n)];
        CopyTo(n, a, 0, 0);
        return a;
    }

    static public int CopyTo(RBSTNode n, long[] a, int it, long add)
    {
        if (n == null)
            return it;
        add += n.add;
        it = CopyTo(n.lst, a, it, add);
        a[it++] = n.val + add;
        it = CopyTo(n.rst, a, it, add);
        return it;
    }
    static public int Count(RBSTNode n) { return n == null ? 0 : n.cnt; }
    static public long Sum(RBSTNode n, int l, int r)
    {
        if (n == null || r <= 0 || Count(n) <= l) return 0;
        if (l <= 0 && Count(n) <= r)
            return n.sum;
        long ret = 0;
        if (0 < r && l < Count(n.lst)) ret += Sum(n.lst, l, r);
        if (Count(n.lst) + 1 < r && l < Count(n)) ret += Sum(n.rst, l - Count(n.lst) - 1, r - Count(n.lst) - 1);
        if (l <= Count(n.lst) && Count(n.lst) < r) ret += n.val;
        ret += n.add * (Math.Min(r, Count(n)) - Math.Max(l, 0));
        return ret;
    }
    /* StarySkyTreeMode

            static public long Max(ImmutableNode n, int l, int r)
            {
                if (n == null || r <= 0 || Count(n) <= l) return long.MinValue;
                if (l <= 0 && Count(n) <= r)
                    return n.max;
                long ret = long.MinValue;
                if (0 < r && l < Count(n.lst)) ret = Math.Max(ret, Max(n.lst, l, r) + n.add);
                if (Count(n.lst) + 1 < r && l < Count(n)) ret = Math.Max(ret, Max(n.rst, l - Count(n.lst) - 1, r - Count(n.lst) - 1) + n.add);
                if (l <= Count(n.lst) && Count(n.lst) < r) ret = Math.Max(ret, n.val + n.add);
                return ret;
            }
    //*/
    static RBSTNode Update(RBSTNode n)
    {
        if (n == null) return null;
        n.cnt = 1;
        if (n.lst != null) n.cnt += n.lst.cnt;
        if (n.rst != null) n.cnt += n.rst.cnt;

        n.sum = n.val + Count(n) * n.add;
        if (n.lst != null) n.sum += n.lst.sum;
        if (n.rst != null) n.sum += n.rst.sum;

        /* StarySkyTreeMode
        n.max = n.val + n.add;
        if (n.lst != null) n.max = Math.Max(n.max, n.add + n.lst.max);
        if (n.rst != null) n.max = Math.Max(n.max, n.add + n.rst.max);
        //*/

        return n;
    }

    static public RBSTNode Add(RBSTNode n, int l, int r, long v)
    {
        if (n == null || r <= 0 || Count(n) <= l) return n;
        FallDown(n);
        if (l <= 0 && Count(n) <= r)
        {
            n.add += v;
            return Update(n);
        }
        else
        {
            if (0 < r && l < Count(n.lst))
                n.lst = Add(n.lst, l, r, v);
            if (Count(n.lst) + 1 < r && l < Count(n))
                n.rst = Add(n.rst, l - Count(n.lst) - 1, r - Count(n.lst) - 1, v);
            if (l <= Count(n.lst) && Count(n.lst) < r)
                n.val += v;
            return Update(n);
        }
    }


    static public RBSTNode GetNode(RBSTNode n, int k)
    {
        while (n != null)
        {
            if (k < Count(n.lst))
                n = n.lst;
            else if (k == Count(n.lst))
                break;
            else
            {
                k -= Count(n.lst) + 1;
                n = n.rst;
            }
        }
        return n;
    }
    static public RBSTNode FallDown(RBSTNode n)
    {
        if (n == null) return null;
        if (n.add == 0) return n;
        if (n.lst != null)
        {
            n.lst.add += n.add;
            Update(n.lst);
        }
        if (n.rst != null)
        {
            n.rst.add += n.add;
            Update(n.rst);
        }
        n.val += n.add;
        n.add = 0;
        return Update(n);
    }

    static Random rand = new Random();
    static public RBSTNode Merge(RBSTNode a, RBSTNode b)
    {
        if (a == null || b == null)
            return a ?? b;
        if (rand.Next() % (Count(a) + Count(b)) < Count(a))
        {
            var acopy = FallDown(a);
            acopy.rst = Merge(acopy.rst, b);
            return Update(acopy);
        }
        else
        {
            var bcopy = FallDown(b);
            bcopy.lst = Merge(a, bcopy.lst);
            return Update(bcopy);
        }
    }
    static public RBSTNode Merge(RBSTNode l, RBSTNode m, RBSTNode r)
    {
        return Merge(Merge(l, m), r);
    }
    static public RBSTNode Split(RBSTNode n, int k)
    {
        if (n == null) return new RBSTNode(null, null);
        var copy = FallDown(n);
        if (k <= Count(n.lst))
        {
            var s = Split(copy.lst, k);
            copy.lst = s.rst;
            s.rst = Update(copy);
            return s;
        }
        else
        {
            var s = Split(copy.rst, k - Count(n.lst) - 1);
            copy.rst = s.lst;
            s.lst = Update(copy);
            return s;
        }
    }
    static public RBSTNode[] Split(RBSTNode n, int l, int r)
    {
        var p1 = RBSTNode.Split(n, r);
        var q1 = RBSTNode.Split(p1.lst, l);
        return new RBSTNode[] { q1.lst, q1.rst, p1.rst };
    }
    static public RBSTNode Insert(RBSTNode n, int k, RBSTNode item)
    {
        if (n == null) return item;
        if (rand.Next() % (Count(n) + Count(item)) < Count(n))
        {
            if (k <= Count(n.lst))
                n.lst = Insert(n.lst, k, item);
            else n.rst = Insert(n.rst, k - Count(n.lst) - 1, item);
            return Update(n);
        }
        else
        {
            var s = Split(n, k);
            item.lst = s.lst; item.rst = s.rst;
            return Update(item);
        }
    }
    static public RBSTNode RemoveAt(RBSTNode n, int k)
    {
        if (n == null) return null;
        if (k < Count(n.lst))
        {
            n.lst = RemoveAt(n.lst, k);
            return Update(n);
        }
        else if (k == Count(n.lst))
        {
            var nn = Merge(n.lst, n.rst);
            Isolate(n);
            return nn;
        }
        else
        {
            n.rst = RemoveAt(n.rst, k - Count(n.rst) - 1);
            return Update(n);
        }
    }
    static public RBSTNode Isolate(RBSTNode n)
    {
        if (n == null) return null;
        n.lst = n.rst = null;
        return Update(n);
    }
}


#endregion
