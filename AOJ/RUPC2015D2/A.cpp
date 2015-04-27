#include <iostream>
#include <map>
using namespace std;
int n, m, a[200];
int x, y, p;
map<int, int>dic;
int main()
{
	cin >> n >> m;
	for (int i = 0; i < m; i++)	{
		cin >> x >> y;
		for (int j = 0; j < y; j++)	{
			p = (x + j) % n;
			a[p] = a[p + n] = 1;
		}
	}
	x = 1;
	for (int i = 0; i < n; i++)	{
		if (a[i])
			continue;
		x = 0;
		break;
	}
	if (x)	{
		cout << n << ' ' << 1 << endl;
		return 0;
	}

	int f = a[0];
	for (int i = 0; i <= n;){

		if (a[i]){
			if (f){
				i++; continue;
			}
			for (int j = i; j < 2 * n; j++)	{
				if (a[j])continue;
				else {
					dic[j - i]++;
					i = j; break;
				}
			}
		}
		else f = 0;
		i++;
	}
	for (auto it = dic.rbegin(); it != dic.rend(); ++it){
		auto p = *it;
		cout << p.first << ' ' << p.second << endl;
	}

	return 0;
}
