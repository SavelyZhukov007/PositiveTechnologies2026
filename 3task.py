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