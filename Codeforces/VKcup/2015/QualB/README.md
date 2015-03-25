#VKCup 2015 QualBまとめ

##[A:Rotate, Flip and Zoom](http://codeforces.com/contest/523/problem/A)
+ タグ: Implementation
+ 概要:  
h*wの行列を90°回転させ，反転させ，2倍に拡大せよ．
+ 制約:  
1<=w,h<=100
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法  
ans[i \* 2 + k][j \* 2 + t] = str[j][i]すればよい．(0<=k,t<=1)
+ コメント:  
問題文はわかりにくいがタイトルどおり．

##[B:Mean Requests](http://codeforces.com/contest/523/problem/B)
+ タグ: Implementation Cumulative_Sum
+ 概要:  
N個の数からなるデータが与えられる．また，M個の時系列クエリが与えられる．時間窓Tのときにおいて，各クエリに対し，問題文中で与えられる，real,approx,errorを求めよ．
+ 制約:  
1<=N<=20,000  
1<=T<=N  
1<=M<=N  
1<=a_i<=1,000,000  
+ 時間制限: 4sec
+ メモリ制限: 256MB
+ 解法  
問題文中で与えられる数式を累積和を用いて計算すればよい．
+ コメント:  
問題文が分かりにくい．

##[C:Name Quest](http://codeforces.com/contest/523/problem/C)
+ タグ: Implementation String
+ 概要:  
英小文字のみからなる文字列SとTが与えられる．Tを2つに割ったときにそのどちらにも部分列としてSが含まれるような割り方は何通りあるか．
+ 制約:  
1<=S<=1,000  
1<=T<=1,000,000  
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法  
前からSを作ったときの始点をl，後ろからSを作ったときの始点をrとする．求める答えはmax(0,r-l)である．
+ コメント:  
C問題にしては簡単．

##[D:Statistics of Recompressing Videos](http://codeforces.com/contest/523/problem/D)
+ タグ: Sorting Data_Strucuture
+ 概要:  
k個のスレッドからなるCPUにタスクをN個順に突っ込む．各タスクを突っ込む時刻s_iと，かかる時間m_iが与えられる．最終的な終了時刻を求めよ．
+ 制約:  
1<=k,N<=500,000  
1<=s\_i,m\_i<=10^9  
+ 時間制限: 3sec
+ メモリ制限: 256MB
+ 解法  
優先度付きキューを使ってタスクを管理する．最初のk個はそのまま突っ込む，残りは取り出した時刻をtとしたときにmax(s_i,t)+m_iに終了するように突っ込む．
+ コメント:  
D問題にしては簡単かも．QualAより全体的に簡単かも？
