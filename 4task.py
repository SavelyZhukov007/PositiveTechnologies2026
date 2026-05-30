T = int(input())
wooden_head = T  # требуемая переменная

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
