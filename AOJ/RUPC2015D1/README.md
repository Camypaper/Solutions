#RUPC2015 Day1まとめ
##[A:Soccer](http://judge.u-aizu.ac.jp/onlinejudge/cdescription.jsp?cid=RitsCamp15Day1&pid=A)
+ タグ: Implementation
+ 概要:
サッカー中のあるフレームのボールを持っている選手が与えられる．各チームのパスの最大距離とそれにかかった時間を求めよ．
+ 制約
1<=N<=100  
0<=fi<fi+1<=324 000  
1<=a_i<=11  
t_i=0,1  
0<=xi<=120  
0<=yi<=90  
+ 時間制限: 1sec
+ メモリ制限: 65536KB
+ 解法:  
i番目とi+1番目のチームが同じかつユニフォーム番号が違えばパスとして全探索．_O(N)_．

##[B:RUPC](http://judge.u-aizu.ac.jp/onlinejudge/cdescription.jsp?cid=RitsCamp15Day1&pid=B)
+ タグ: Sorting Binary_Search Cumlative_Sum
+ 概要:
N問からなるコンテストがある．各問の得点はa_iである．M人間の人間が参加する．それぞれの人間はb_iの得点の問題まで解ける．各人の目標得点c_iを超えるかどうか判定せよ．
+ 制約  
1≤N≤300,000  
1≤M≤300,000  
0≤a_i≤1,000, 000  
0≤b_i≤1,000,000  
0≤c_i≤Sum(a)
+ 時間制限: 2sec
+ メモリ制限: 65536KB
+ 解法:  
問題の難易度でソートして，二分探索．得点の総和を累積和で持っておくことでO(logN)で得点が求められる．全体としてO((N+M)logN).

##[C:Shopping](http://judge.u-aizu.ac.jp/onlinejudge/cdescription.jsp?cid=RitsCamp15Day1&pid=C)
+ タグ: Data_Structure
+ 概要:
N個の食材がある．N個の食材全てを使って料理を作る．各食材のコストはc\_iである．M個の魔法が使える．何度でも使うことができ食材a\_iをb\_iにb\_iをa\_iにすることができる．
+ 制約  
1≤xi≤1,000  
1≤N≤5,000  
0≤M≤min(N(N−1)/2,1000)  
si≠ti  
si,tiの組に重複はない  
si,tiは a0,…,aN−1 に含まれる  
+ 時間制限: 1sec
+ メモリ制限: 65536KB
+ 解法:  
Union-Findでa_iとb_iをマージする．その際，最小値もマージする．O(Nα(N))．

##[D:Hopping Hearts](http://judge.u-aizu.ac.jp/onlinejudge/cdescription.jsp?cid=RitsCamp15Day1&pid=D)
+ タグ: DP Cumulative_Sum
+ 概要:
N匹のウサギがいる．長さL-1の橋の上にいる．初期位置はx_iである．ここから右にa_iずつジャンプすることができる．ただし飛び越えることは許されない．橋から落ちることも許されない．並び方は何通りあるか．
+ 制約  
1≤N≤5 000  
N≤L≤5 000  
0≤xi<xi+1≤L−1  
0≤ai≤L−1
+ 時間制限: 1sec
+ メモリ制限: 131072KB
+ 解法:  
DP[i,j]をi番目のウサギがjの位置にいるような並べ方として持つ．DP[i+1,j]=Sum(Dp[i,k])(0<=k<=j-1)である．これを愚直に毎回計算するとTLEするのでDP[i,j]をi番目のウサギがjの位置までにいるような並べ方としてもってDPしていく．全体として_O(N\*L)_．
