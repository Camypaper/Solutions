#include <iostream>
#include <vector>
using namespace std;
int n, st, ans[5], x, y;
vector<int>h, cnt;
int main()
{
	cin >> n;
	for (int i = 0; i < n; i++){
		cin >> x;
		if (h.size() == 0){
			h.push_back(x);
			cnt.push_back(1);
		}
		else if (h.back() != x){
			h.push_back(x);
			cnt.push_back(1);
		}
		else cnt[cnt.size() - 1]++;
	}
	for (int i = 1; i < h.size() - 1; i++)
	{
		if (h[i - 1]<h[i] && h[i]>h[i + 1])
		{
			if (cnt[i] == 1)
				ans[3]++;
			else ans[0]++;
		}
		if (h[i - 1] > h[i] && h[i] < h[i + 1]){
			if (cnt[i] == 1)
				ans[4]++;
			else ans[1]++;
		}
		if (h[i - 1] < h[i] && h[i] < h[i + 1] && cnt[i]>1)
			ans[2]++;
		if (h[i - 1] > h[i] && h[i] > h[i + 1] && cnt[i] > 1)
			ans[2]++;

	}

	for (int i = 0; i < 5; i++)	{
		cout << ans[i];
		if (i == 4)
			cout << endl;
		else cout << ' ';
	}
	return 0;
}
