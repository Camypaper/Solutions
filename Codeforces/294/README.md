#CFRound294まとめ
##[Div2A:A and B and Chess](http://codeforces.com/contest/519/problem/A)
+ タグ: Implementation
+ 概要:  
チェス盤が与えられる．それぞれの駒はある得点を持つ．黒と白どちらが優位か判定せよ．
+ 時間制限: 1sec
+ メモリ制限: 256MB
+ 解法:  
愚直にシミュレーションする．
+ コメント:  
やるだけ．

##[Div2B:A and B and Compilation Errors](http://codeforces.com/contest/519/problem/B)
+ タグ: Implementation
+ 概要:  
**N**個の数からなる数列**a**が与えられる．次に**a**から一つ数を取り除いた数列**b**が，その次に**b**から一つ数を取り除いた数列**c**が与えられる．**a**に含まれており，**b**に含まれない数，**b**に含まれており，**c**に含まれない数を出力せよ．
+ 制約:  
3<=**N**<=100,000
1<=**a**_i<=1e9
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法:  
ハッシュテーブルに出現回数を突っ込む．差し引いたときに残ったものが答え．
+ コメント:  
やるだけ．

##[Div2C:A and B and Team Training](http://codeforces.com/contest/519/problem/C)
+ タグ: Math
+ 概要:  
**N**人の経験者と**M**人の初心者がいる．2人の経験者と1人の初心者からなるチームか，1人の経験者と2人の初心者からなるチームのどちらかを組むことができる．作ることができるチームの最大数を答えよ．
+ 制約:  
0<=**N**<=500,000  
0<=**M**<=500,000
+ 時間制限: 1sec
+ メモリ制限: 256MB
+ 解法:  
どちらかのチーム数を決め打つと，もう片方のチーム数も定まる．全探索してやって_O(N)_．
+ コメント:  
Cで本当に全探索するだけみたいの出るの珍しい気がする．

##[Div2D:A and B and Interesting Substrings](http://codeforces.com/contest/519/problem/D)
+ タグ: Cumlative_Sum
+ 概要:  
英小文字だけからなる文字列**S**とアルファベットが持つ得点**a**が与えられる．次の条件を満たす部分文字列の数を答えよ．  
 1. 両端のアルファベットが同じ．
 2. 両端を取り除いた部分文字列の得点の総和が0
+ 制約:  
1<=|**S**|<=100,000  
-100,000<=**a**_i<=100,000
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法:  
とりあえず累積和を取って部分文字列の得点を計算できるようにする．今_i_番目の文字を見ているとき，1番目から_i_-1番目までの部分文字列の得点を_p_\_i,1番目から_i_番目までの部分文字列の得点を_q_\_iとする．_i_番目の文字とペアになるものはS[_i_]==S[_j_] かつ _p_\_i==_q_\_kかつ_i_>_j_を満たすものである．よって，アルファベットごとに値の出現回数を管理してやればよい．これはハッシュテーブルを用いて実現可能．全体として_O(|S|)_でできる．
+ コメント:  
この回，全体的に簡単だなぁ．

##[Div2E:A and B and Lecture Rooms](http://codeforces.com/contest/519/problem/E)
+ タグ: Graph Doubling LCA
+ 概要:  
**N**個の頂点からなる根付き木が与えられる．**M**個のクエリに答えよ．各クエリは_i_番の頂点と_j_番の距離が等しいものの答えを出力せよ．
+ 制約:  
1<=**N**<=100,000  
1<=**M**<=100,000
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法:  
LCA(Lowest common ancestor)を前もって求めておく．i番の頂点の根からの深さを_depth(i)_とする．_i_==_j_のとき**N**，_depth(i)_≠_depth(j)_のときは_depth(k)_==(_depth(i)_+_depth(j)_)/2となるような頂点の子孫であって，_i_にも_j_にも向かわないものを選ぶ．_depth(i)_==_depth(j)_のときはLCAから根にも向かう方向にも数える．距離が奇数のときは存在しない．
+ コメント:  
解法の方針はすぐ分かったのにバグらせまくってつらい．LCAとかダブリングはもうちょい練習しておきたい．
