#include <iostream>
#include <locale.h>
#include <vector>
#include <string>
#include <fstream>

const int n = 9;
int count = 1;
std::vector<int> a;
std::vector<bool> used;
std::string s = "основания";
std::string s_mem = "start";

std::ofstream fout("combinations.txt");

void out(std::vector<int> a)
{
    int k;
    std::string s_temp;
    for (int i = 0; i < n; ++i) {
        k = a[i] - 1;
        s_temp += s[k];
    }
    std::string::size_type check = s_mem.find(s_temp);
    if (check != 4294967295) return;
    std::cout << count << ". ";    
    s_mem += ' ';
    s_mem += s_temp;
    std::cout << s_temp; 
    fout << count << ". " << s_temp << '\n';
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
    fout << "Выполнил Сагиров А.И. студент группы 4109" << '\n';
    fout.close();
    return 0;
}
