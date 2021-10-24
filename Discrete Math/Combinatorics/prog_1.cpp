#include <iostream>
#include <locale.h>
#include <vector>

const int n = 5;
int count = 1;
std::vector<int> a;
std::vector<bool> used;
std::string s = "крыша";

void out(std::vector<int> a)
{
    int k;
    std::cout << count << ".   ";
    if (count > 9) std::cout << '\b';
    if (count > 99) std::cout << '\b';
    for (int i = 0; i < n; ++i) {
        k = a[i] - 1;
        std::cout << s[k];
    }
    std::cout << std::endl;
    ++count;
}

void rec(int idx)
{
    if (idx == n)
    {
        out(a);
        return;
    }
    for (int i = 1; i <= n; ++i)
    {
        if (used[i]) continue;
        a[idx] = i;
        used[i] = true;
        rec(idx + 1);
        used[i] = false;
    }
}

int main()
{
    setlocale(0, "Rus");     
    used = std::vector<bool>(n + 1, false);
    a.resize(n);
    rec(0);
    std::cout << "Выполнил Сагиров А.И. студент группы 4109" << std::endl;
    return 0;
}

