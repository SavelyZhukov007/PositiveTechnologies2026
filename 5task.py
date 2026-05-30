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

    # Первый слой: квадраты 2x2.
    # Ошибка равна 1 только если все 4 пикселя одинаковые, иначе 0.
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

    area = 4  # площадь текущего дочернего квадрата

    while dim > 1:
        nd = dim >> 1
        ndp = [0] * (nd * nd)
        nones = [0] * (nd * nd)

        out = 0
        ar = area
        old_dp = dp
        old_ones = ones

        for i in range(nd):
            p = (i << 1) * dim
            end = p + dim

            while p < end:
                p2 = p + dim

                d0 = old_dp[p]
                d1 = old_dp[p + 1]
                d2 = old_dp[p2]
                d3 = old_dp[p2 + 1]

                o0 = old_ones[p]
                o1 = old_ones[p + 1]
                o2 = old_ones[p2]
                o3 = old_ones[p2 + 1]

                total = d0 + d1 + d2 + d3

                # delta, если квадрат сделать полностью белым
                a0 = o0 - d0
                a1 = o1 - d1
                a2 = o2 - d2
                a3 = o3 - d3

                # delta, если квадрат сделать полностью черным
                b0 = ar - o0 - d0
                b1 = ar - o1 - d1
                b2 = ar - o2 - d2
                b3 = ar - o3 - d3

                best = a0 + b1

                v = a0 + b2
                if v < best:
                    best = v
                v = a0 + b3
                if v < best:
                    best = v

                v = a1 + b0
                if v < best:
                    best = v
                v = a1 + b2
                if v < best:
                    best = v
                v = a1 + b3
                if v < best:
                    best = v

                v = a2 + b0
                if v < best:
                    best = v
                v = a2 + b1
                if v < best:
                    best = v
                v = a2 + b3
                if v < best:
                    best = v

                v = a3 + b0
                if v < best:
                    best = v
                v = a3 + b1
                if v < best:
                    best = v
                v = a3 + b2
                if v < best:
                    best = v

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
