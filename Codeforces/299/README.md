#CF299まとめ

##[Div1 A: Tavas and Karafs](http://codeforces.com/contest/536/problem/A)
+ タグ: Binary_Search Math
+ 概要:  
A+(i-1)*Bで表せる木が無限に並んでいる．相異なる木を最大m個選んで高さを1減らす操作をt回行うとき，左端lから右端rまで全て食べきれるようなrを求めよ．
+ 解法:  
高さtまでの木しか高さを0にできない．さらに最大でm*tしか高さを減らせない．よって高さと累積和を持って二分探索すればよい．
+ コメント:  
読解が困難．

##[Div1 B: Tavas and Malekas ](http://codeforces.com/contest/536/problem/B)
+ タグ: String
+ 概要:  
長さNの文字列Sと，文字列pを考える．x1,x2,..xkにおいてS_x1..X_x1+pがpと一致するとき，Sは何通りあるか．
+ 解法:  
実際に当てはめてみる．そして，一致しなければ0，そうでなければ26^(未確定の位置)である．判定はZ algorithmを用いてO(N)でできる．
