#yukicoder175-177まとめ

##[No.175 simpleDNA](http://yukicoder.me/problems/415)
+ タグ: Math
+ 概要:  
+ 解法:
2^(l-1)\*nする。
+ コメント:  
ハイパー読解問題

##[No.176 2種類の切手](http://yukicoder.me/problems/457)
+ タグ: Math
+ 概要:  
A円の切手とB円の切手を組み合わせてT円以上にしたい．最小の金額はいくらか．
+ 制約:  
1<=A<B,T<=1e9
+ 解法:  
10^7個ぐらいまで片方の切手を買ったとき、残りをいくつ買うかを全探索する。
+ コメント:  
似たようなのこどふぉで見た記憶があるけど思い出せない。

##[No.177 制作進行の宮森あおいです!](http://yukicoder.me/problems/177)
+ タグ: Graph Flow
+ 概要:  
N人の原画マンがいる。仕事をA\_iだけこなせる．M人の作画監督に仕事を渡す．仕事をB\_iだけこなせる．相性が悪い人には渡せない．Wの仕事を終了させられるか
+ 制約:  
1≤W≤10000 : 宮森あおいが抱えてるカットの数
1≤N≤50 : 原画マンの数
1≤M≤50 : 作画監督の数
0≤Ji≤10000 : i番目の原画マンが1日に描ける量
0≤Ci≤10000 : i番目の作画監督が1日に仕上げられる量
0≤Qi≤N : i番目の作画監督があわない原画マンの人数
1≤Xi,j≤N : Xi,j=kならi番目の作画監督と,k番目の原画マンと作画があわないということ
Xi,j<Xi,j+1
+ 解法:  
最大流を流すだけ．始点->原画マン->作画監督入力->作画監督出力->終点にエッジを張った．
+ コメント:  
冷静に考えて，作画監督入力->出力はいらない(つらい)
直したやつをはっておいた
