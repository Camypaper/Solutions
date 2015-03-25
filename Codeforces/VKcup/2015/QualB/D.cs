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
            var k = sc.Integer();
            var pq = new PriorityQueue<long>();
            var ans = new long[n];
            for (int i = 0; i < n; i++)
            {
                long s = sc.Long();
                long span =sc.Long();
                if (pq.Count == k)
                {
                    s = Math.Max(s, pq.Dequeue());
                    pq.Enqueue(s + span);
                }
                else
                    pq.Enqueue(s + span);
                ans[i] = s + span;
            }
            foreach(var x in ans)
                IO.Printer.Out.WriteLine(x);
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


#region PriorityQueue and PairingHeap
public class PriorityQueue<T>
{
    PairingHeap<T> top;
    Comparison<T> compare;
    int size;
    public int Count { get { return size; } }
    public PriorityQueue() : this(Comparer<T>.Default) { }
    public PriorityQueue(Comparison<T> comparison) { compare = comparison; }
    public PriorityQueue(IComparer<T> comparer) { compare = comparer.Compare; }
    public void Enqueue(T item)
    {
        var heap = new PairingHeap<T>(item);
        top = PairingHeap<T>.Merge(top, heap, compare);
        size++;
    }
    public T Dequeue()
    {
        var ret = top.Key;
        size--;
        top = PairingHeap<T>.Pop(top, compare);
        return ret;
    }
    public bool Any() { return size > 0; }
    public T Peek() { return top.Key; }
}

#region PairingHeap
public class PairingHeap<T>
{
    public PairingHeap(T k) { key = k; }
    private readonly T key;
    public T Key { get { return key; } }
    private PairingHeap<T> head;
    private PairingHeap<T> next;
    static public PairingHeap<T> Pop(PairingHeap<T> s, Comparison<T> compare)
    {
        return MergeLst(s.head, compare);
    }
    static public PairingHeap<T> Merge(PairingHeap<T> l, PairingHeap<T> r, Comparison<T> compare)
    {
        if (l == null || r == null) return l == null ? r : l;
        if (compare(l.key, r.key) > 0) Swap(ref l, ref r);
        r.next = l.head;
        l.head = r;
        return l;
    }
    static public PairingHeap<T> MergeLst(PairingHeap<T> s, Comparison<T> compare)
    {
        var n = new PairingHeap<T>(default(T));
        while (s != null)
        {
            PairingHeap<T> a = s, b = null;
            s = s.next; a.next = null;
            if (s != null) { b = s; s = s.next; b.next = null; }
            a = Merge(a, b, compare); a.next = n.next;
            n.next = a;
        }
        while (n.next != null)
        {
            var j = n.next;
            n.next = n.next.next;
            s = Merge(j, s, compare);
        }
        return s;
    }
    static void Swap(ref PairingHeap<T> l, ref PairingHeap<T> r) { var t = l; l = r; r = t; }
}
#endregion
#endregion
