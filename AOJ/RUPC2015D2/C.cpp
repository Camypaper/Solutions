#include <iostream>
#include <queue>
using namespace std;

template<typename T, typename U> inline void amin(T &x, U y) { if (y < x) x = y; }

priority_queue<pair<long long, int>>pq;
int n, g, d[16], c[16], p, np;
long long dist[200050], e, ne;
const long long INF = (long long)1e16;
int main()
{
	cin >> n >> g;
	for (int i = 0; i < n; i++)
		cin >> d[i] >> c[i];
	for (int i = 0; i < 200000; i++)
		dist[i] = INF;
	dist[g] = 0;
	pq.emplace(0, g);
	while (!pq.empty())
	{
		p = pq.top().second;
		e = -pq.top().first;
		pq.pop();
		if (dist[p] < e)
			continue;
		for (int i = 0; i < n; i++)
		{
			np = abs(p + d[i]);
			ne = e + c[i];
			if (np >= 200050)
				continue;
			if (dist[np]>ne)
			{
				dist[np] = ne;
				pq.emplace(-ne, np);
			}
		}
		for (int i = 0; i < n; i++)
		{
			np = abs(p - d[i]);
			ne = e + c[i];
			if (np >= 200050)
				continue;
			if (dist[np]>ne)
			{
				dist[np] = ne;
				pq.emplace(-ne, np);
			}
		}
	}
	if (dist[0] >= INF)dist[0] = -1;
	cout << dist[0] << endl;

	return 0;
}
