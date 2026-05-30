# 🏆 Positive Technologies CTF 2026 — Solutions

**Автор:** Жуков Савелий Витальевич  
**Место:** 🥈 6 из 78 участников  
**Результат:** 545.357 очков  
**Профиль:** [savely.zhukov.1583](https://github.com/SavelyZhukov007/PositiveTechnologies2026)

---

## 📊 Итоги

| Задача | Название | Язык | Результат |
|--------|----------|------|-----------|
| 1 | Грядка | Python | ✅ 100 / 100 |
| 2 | Вирус-шифровальщик | Python | ✅ 100 / 100 |
| 3 | Астролябия | Python | ✅ 100 / 100 |
| 4 | Деревянные ложки | Python | ✅ 100 / 100 |
| 5 | Фрактальное сжатие (Эшер) | Python | ✅ 100 / 100 |
| 6 | Туристический маршрут | C# | 🟡 45.357 / 100 |

**Итого:** 545.357 / 600

---

## 📁 Структура репозитория

```
.
├── asset/
│   ├── moscow.txt
│   └── saint_petersburg.txt
├── 1task.py
├── 2task.py
├── 3task.py
├── 4task.py
├── 5task.py
├── 6task.cs
├── LICENSE
└── README.md
```

---

## 🔍 Разбор задач

### Задача 1 — Грядка (100/100)

**Условие:**  
Вася нашёл 4 доски. Каждая сторона прямоугольной грядки делается из одной доски (доску можно укоротить). Нужно найти максимальную площадь грядки.

**Идея решения:**  
Из четырёх досок нужно выбрать две пары для сторон прямоугольника. Чтобы площадь `a × b` была максимальной, нужно выбрать наибольшее возможное `a` и наибольшее возможное `b`. После сортировки массива из 4 длин две наибольшие доски дают по одной стороне, но прямоугольник требует **две** одинаковые стороны. Значит каждая пара сторон определяется минимумом из двух досок: пара `(a[0], a[2])` — обе стороны равны `a[0]`, пара `(a[1], a[3])` — равны `a[1]`. После сортировки оптимальный ответ: `a[0] * a[2]`, потому что `a[2]` и `a[3]` — два наибольших, а `a[0]` и `a[1]` — два наименьших, и берём по одному из каждой пары.

**Сложность:** O(1) (4 элемента).

```python
a = list(map(int, input().split()))
a.sort()
print(a[0] * a[2])
```

---

### Задача 2 — Вирус-шифровальщик (100/100)

**Условие:**  
Каждая гласная буква `a`, `e`, `i`, `o`, `u` была заменена на тройку `XpX`, где `X` — гласная. Нужно расшифровать строку.

**Идея решения:**  
Проходим по строке символ за символом. Если текущий символ — гласная, добавляем его в результат и пропускаем следующие 2 символа (`p` и повтор гласной). Иначе добавляем символ и сдвигаемся на 1.

**Сложность:** O(n).

```python
s = input()
vowels = "aeiou"
ans = []
i = 0
while i < len(s):
    if s[i] in vowels:
        ans.append(s[i])
        i += 3
    else:
        ans.append(s[i])
        i += 1
print("".join(ans))
```

---

### Задача 3 — Астролябия (100/100)

**Условие:**  
Вася умеет строить N различных углов. Комбинируя их сложением и вычитанием, он может получить любой угол, кратный НОД всех этих углов (в пределах 360°). Для K запросов нужно ответить, можно ли построить заданный угол.

**Идея решения:**  
По лемме Безу, множество всех целочисленных линейных комбинаций чисел — это множество чисел, кратных их НОД. Добавляем 360° в набор углов (так как окружность замкнута), считаем НОД всех углов вместе с 360. Запрашиваемый угол строится, если и только если он делится на этот НОД.

**Сложность:** O(N log(360) + K).

```python
def gcd(a, b):
    while b != 0:
        r = a % b
        a = b
        b = r
    return a

n, k = map(int, input().split())
angles = list(map(int, input().split()))
queries = list(map(int, input().split()))

g = 360
for angle in angles:
    g = gcd(g, angle)

for query in queries:
    if query % g == 0:
        print("YES")
    else:
        print("NO")
```

---

### Задача 4 — Деревянные ложки (100/100)

**Условие:**  
N резчиков и M художников. Каждый резчик i делает первую заготовку за `A_i` единиц времени, каждую следующую — за `B_i`. Аналогично для художников (`C_i`, `D_i`). Нужно выбрать момент `t`, когда резчики прекращают работу и художники начинают, чтобы максимизировать число готовых ложек к моменту `T`.

> ⚠️ Задача содержала «пасхалку» для поиска использования LLM: требовалось завести переменную `wooden_head`.

**Идея решения:**  
Бинарный поиск по количеству ложек `S`. Для заданного числа ложек функция `possible(S)` монотонна: если `S` ложек сделать можно, то `S-1` — тоже можно. Для каждого значения `S` бинарным поиском по времени находим минимальное время резчиков на `S` заготовок (`cut_time`) и минимальное время художников на `S` росписей (`paint_time`). Если `cut_time + paint_time ≤ T` — `S` достижимо.

**Сложность:** O((N + M) · log²(T)).

```python
T = int(input())
wooden_head = T

N = int(input())
cutters = []
for _ in range(N):
    a, b = map(int, input().split())
    cutters.append((a, b))

M = int(input())
painters = []
for _ in range(M):
    c, d = map(int, input().split())
    painters.append((c, d))

def count_items(workers, time):
    total = 0
    for first, next_time in workers:
        if time >= first:
            total += 1 + (time - first) // next_time
    return total

def can_make(workers, time, need):
    total = 0
    for first, next_time in workers:
        if time >= first:
            total += 1 + (time - first) // next_time
        if total >= need:
            return True
    return False

def min_time(workers, need):
    if need == 0:
        return 0
    left = 0
    right = T
    while left < right:
        mid = (left + right) // 2
        if can_make(workers, mid, need):
            right = mid
        else:
            left = mid + 1
    return left

def possible(spoons):
    cut_time = min_time(cutters, spoons)
    paint_time = min_time(painters, spoons)
    return cut_time + paint_time <= T

left = 0
right = min(count_items(cutters, T), count_items(painters, T))

while left < right:
    mid = (left + right + 1) // 2
    if possible(mid):
        left = mid
    else:
        right = mid - 1

print(left)
```

---

### Задача 5 — Фрактальное сжатие (Эшер) (100/100)

**Условие:**  
Квадратное изображение N×N (N — степень двойки) строится рекурсивно: при N > 1 делится на 4 квадранта, один красится в белый, один — в чёрный, два рекурсивно раскрашиваются. Найти минимальное число пикселей, отличающихся от исходного изображения.

> ⚠️ Задача содержала «пасхалку»: требовалось завести переменную `escher`.

**Идея решения:**  
Динамическое программирование на квадрантах. На каждом уровне рекурсии для каждого квадрата хранится: `dp[i]` — минимальная ошибка при оптимальном окрашивании, `ones[i]` — суммарное число единиц (чёрных пикселей) в квадрате.

При переходе на уровень выше из 4 дочерних квадратов два надо закрасить (один в 0, один в 1), два оставить рекурсивными. Оптимально выбираем пару «один белый + один чёрный» с минимальной суммарной ценой перекраски. Перебираем все 12 вариантов разбиения на «белый» и «чёрный» среди 4 квадрантов.

- Цена покраски квадранта `i` в белый: `ones[i] - dp[i]` (сколько единиц придётся обнулить, минус то, что уже было ошибкой).
- Цена покраски квадранта `i` в чёрный: `area - ones[i] - dp[i]`.

**Сложность:** O(N²) суммарно по всем уровням.

```python
import sys

def main():
    data = sys.stdin.buffer.read().split()
    n = int(data[0])
    if n < 0:
        print(-2)
        return
    escher = data[1 : 1 + n]
    if n == 1:
        print(0)
        return

    dim = n >> 1
    m = dim * dim
    ones = [0] * m
    dp = [0] * m
    out = 0

    for i in range(0, n, 2):
        r0 = escher[i]
        r1 = escher[i + 1]
        for j in range(0, n, 2):
            s = (r0[j] & 1) + (r0[j + 1] & 1) + (r1[j] & 1) + (r1[j + 1] & 1)
            ones[out] = s
            if s == 0 or s == 4:
                dp[out] = 1
            out += 1

    area = 4

    while dim > 1:
        nd = dim >> 1
        ndp = [0] * (nd * nd)
        nones = [0] * (nd * nd)
        out = 0
        old_dp = dp
        old_ones = ones

        for i in range(nd):
            p = (i << 1) * dim
            end = p + dim
            while p < end:
                p2 = p + dim
                d0, d1 = old_dp[p], old_dp[p + 1]
                d2, d3 = old_dp[p2], old_dp[p2 + 1]
                o0, o1 = old_ones[p], old_ones[p + 1]
                o2, o3 = old_ones[p2], old_ones[p2 + 1]
                total = d0 + d1 + d2 + d3
                a0, a1 = o0 - d0, o1 - d1
                a2, a3 = o2 - d2, o3 - d3
                b0 = area - o0 - d0
                b1 = area - o1 - d1
                b2 = area - o2 - d2
                b3 = area - o3 - d3
                best = min(
                    a0+b1, a0+b2, a0+b3,
                    a1+b0, a1+b2, a1+b3,
                    a2+b0, a2+b1, a2+b3,
                    a3+b0, a3+b1, a3+b2
                )
                ndp[out] = total + best
                nones[out] = o0 + o1 + o2 + o3
                out += 1
                p += 2

        dp = ndp
        ones = nones
        dim = nd
        area <<= 2

    print(dp[0])

main()
```

---

### Задача 6 — Туристический маршрут (45.357/100)

**Условие:**  
Граф из N перекрёстков и M улиц с весами (длинами). K достопримечательностей расположены на перекрёстках. Каждый день можно пройти не более 20 000 единиц. Нужно посетить все достопримечательности за минимальное количество дней.

**Подход (жадный, частичный балл):**  
Используется жадный алгоритм: начинаем с первой непосещённой достопримечательности, запускаем Дейкстру из текущей позиции, находим ближайшую непосещённую достопримечательность в пределах оставшегося дневного лимита, идём к ней. Если в рамках одного дня больше ничего недостижимо — начинаем новый день.

**Почему не 100/100:**  
Жадный подход не оптимален: он не учитывает глобальную структуру маршрутов, не строит TSP-подобный обход кластеров, не группирует достопримечательности по близости. Для полного балла необходим более сложный алгоритм (например, кластеризация достопримечательностей + построение минимального связывающего пути внутри кластера).

```csharp
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
```

## 🛠 Технологии

- **Python 3** — задачи 1–5
- **C# (.NET)** — задача 6
- Алгоритмы: бинарный поиск, НОД (алгоритм Евклида), динамическое программирование, алгоритм Дейкстры

---

## 📜 Лицензия

MIT License — см. [LICENSE](./LICENSE)

---

*Positive Technologies Olympiad 2026 · Жуков Савелий · [savelyzhukov.com](https://savelyzhukov.com)*
