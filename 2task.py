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