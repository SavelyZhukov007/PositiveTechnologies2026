using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Program
{
    const int MAX_DAY_DISTANCE = 20000;

    static int n, m, k;

    static int[] head;
    static int[] to;
    static int[] next;
    static int[] weight;
    static int edgePtr = 0;

    static int[] dist;
    static int[] parent;
    static int[] seen;
    static int stamp = 0;

    static int[] activeSightCount;

    static LongHeap heap = new LongHeap();

    static long MakeKey(int d, int v)
    {
        return ((long)d << 32) | (uint)v;
    }

    static int KeyDist(long key)
    {
        return (int)(key >> 32);
    }

    static int KeyVertex(long key)
    {
        return (int)(uint)key;
    }

    static void AddEdge(int a, int b, int w)
    {
        to[edgePtr] = b;
        weight[edgePtr] = w;
        next[edgePtr] = head[a];
        head[a] = edgePtr++;
    }

    static bool FindNearestSight(int start, int limit, out int target, out int targetDist)
    {
        target = -1;
        targetDist = -1;

        stamp++;

        if (stamp == int.MaxValue)
        {
            Array.Clear(seen, 0, seen.Length);
            stamp = 1;
        }

        heap.Clear();

        seen[start] = stamp;
        dist[start] = 0;
        parent[start] = -1;

        heap.Push(MakeKey(0, start));

        while (heap.Count > 0)
        {
            long key = heap.Pop();

            int curDist = KeyDist(key);
            int v = KeyVertex(key);

            if (seen[v] != stamp || dist[v] != curDist)
                continue;

            if (curDist > limit)
                break;

            if (v != start && activeSightCount[v] > 0)
            {
                target = v;
                targetDist = curDist;
                return true;
            }

            for (int e = head[v]; e != -1; e = next[e])
            {
                int u = to[e];
                int nd = curDist + weight[e];

                if (nd > limit)
                    continue;

                if (seen[u] != stamp || nd < dist[u])
                {
                    seen[u] = stamp;
                    dist[u] = nd;
                    parent[u] = v;
                    heap.Push(MakeKey(nd, u));
                }
            }
        }

        return false;
    }

    public static void Main()
    {
        FastScanner fs = new FastScanner(Console.OpenStandardInput());

        n = fs.NextInt();
        m = fs.NextInt();
        k = fs.NextInt();

        int verticesLimit = n + 1;

        head = new int[verticesLimit];
        for (int i = 0; i < verticesLimit; i++)
            head[i] = -1;

        to = new int[2 * m];
        next = new int[2 * m];
        weight = new int[2 * m];

        for (int i = 0; i < n; i++)
        {
            fs.SkipToken();
            fs.SkipToken();
        }

        for (int i = 0; i < m; i++)
        {
            int a = fs.NextInt();
            int b = fs.NextInt();
            int w = fs.NextInt();

            if ((uint)a < (uint)verticesLimit && (uint)b < (uint)verticesLimit)
            {
                AddEdge(a, b, w);
                AddEdge(b, a, w);
            }
        }

        int[] sights = new int[k];
        activeSightCount = new int[verticesLimit];

        for (int i = 0; i < k; i++)
        {
            int v = fs.NextInt();
            sights[i] = v;

            if ((uint)v < (uint)verticesLimit)
                activeSightCount[v]++;
        }

        dist = new int[verticesLimit];
        parent = new int[verticesLimit];
        seen = new int[verticesLimit];

        List<int[]> routes = new List<int[]>();
        List<int> route = new List<int>(4096);
        List<int> path = new List<int>(4096);

        int visited = 0;
        int startPtr = 0;

        while (visited < k)
        {
            while (startPtr < k && activeSightCount[sights[startPtr]] == 0)
                startPtr++;

            if (startPtr == k)
                break;

            int current = sights[startPtr];
            int remaining = MAX_DAY_DISTANCE;

            route.Clear();
            route.Add(current);

            if (activeSightCount[current] > 0)
            {
                visited += activeSightCount[current];
                activeSightCount[current] = 0;
            }

            while (remaining > 0 && visited < k)
            {
                int target;
                int d;

                if (!FindNearestSight(current, remaining, out target, out d))
                    break;

                path.Clear();

                int x = target;

                while (x != current && x != -1)
                {
                    path.Add(x);
                    x = parent[x];
                }

                if (x == -1)
                    break;

                remaining -= d;

                for (int i = path.Count - 1; i >= 0; i--)
                {
                    int v = path[i];

                    route.Add(v);

                    if (activeSightCount[v] > 0)
                    {
                        visited += activeSightCount[v];
                        activeSightCount[v] = 0;
                    }
                }

                current = target;
            }

            routes.Add(route.ToArray());
        }

        StreamWriter output = new StreamWriter(Console.OpenStandardOutput(), Encoding.ASCII, 1 << 20);

        output.WriteLine(routes.Count);

        foreach (int[] r in routes)
        {
            output.WriteLine(r.Length);

            for (int i = 0; i < r.Length; i++)
            {
                if (i > 0)
                    output.Write(' ');

                output.Write(r[i]);
            }

            output.WriteLine();
        }

        output.Flush();
    }

    sealed class LongHeap
    {
        long[] data = new long[1024];

        public int Count { get; private set; }

        public void Clear()
        {
            Count = 0;
        }

        public void Push(long x)
        {
            if (Count == data.Length)
                Array.Resize(ref data, data.Length << 1);

            int i = Count++;

            while (i > 0)
            {
                int p = (i - 1) >> 1;

                if (data[p] <= x)
                    break;

                data[i] = data[p];
                i = p;
            }

            data[i] = x;
        }

        public long Pop()
        {
            long result = data[0];
            long x = data[--Count];

            if (Count > 0)
            {
                int i = 0;

                while (true)
                {
                    int c = i * 2 + 1;

                    if (c >= Count)
                        break;

                    int r = c + 1;

                    if (r < Count && data[r] < data[c])
                        c = r;

                    if (data[c] >= x)
                        break;

                    data[i] = data[c];
                    i = c;
                }

                data[i] = x;
            }

            return result;
        }
    }

    sealed class FastScanner
    {
        readonly Stream stream;
        readonly byte[] buffer = new byte[1 << 16];

        int len = 0;
        int ptr = 0;

        public FastScanner(Stream stream)
        {
            this.stream = stream;
        }

        int Read()
        {
            if (ptr >= len)
            {
                len = stream.Read(buffer, 0, buffer.Length);
                ptr = 0;

                if (len <= 0)
                    return -1;
            }

            return buffer[ptr++];
        }

        public int NextInt()
        {
            int c;

            do
            {
                c = Read();
            }
            while (c <= 32 && c >= 0);

            int sign = 1;

            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            int x = 0;

            while (c > 32)
            {
                x = x * 10 + c - '0';
                c = Read();
            }

            return x * sign;
        }

        public void SkipToken()
        {
            int c;

            do
            {
                c = Read();
            }
            while (c <= 32 && c >= 0);

            while (c > 32)
                c = Read();
        }
    }
}