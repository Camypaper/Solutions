#Indeedなう予選2015Aまとめ
##[A:掛け算の筆算](http://indeednow-quala.contest.atcoder.jp/tasks/indeednow_2015_quala_1)
+ タグ: Implementation
+ 概要:
２つの数字A,Bの掛け算をしたい．答えは桁数*桁数で表される．
+ 制約:
1<=A,B<=1,000,000
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法:
問題文に書いてある通り，桁数の掛け算をする．文字列として受け取ると楽．
+ コメント:
コメントするほどの感想もない．

##[B:Indeedなう！](http://indeednow-quala.contest.atcoder.jp/tasks/indeednow_2015_quala_2)
+ タグ: Implementation
+ 概要:
**N**個の英小文字のみ文字列が与えられる．"indeednow"のアナグラムかどうか判定せよ．
+ 制約:
1<=**N**<=100
1<=|S_i|<=100
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法:
Sortしてチェックするかヒストグラムを作って判定するとか．
+ コメント:
まだ簡単

##[C:説明会](http://indeednow-quala.contest.atcoder.jp/tasks/indeednow_2015_quala_3)
+ タグ: Cumlative_Sum Binary_Search
+ 概要:
**N**人の人間と**M**個の会場がある．各人はa_i点の問題まで解ける．0点の人間は無視して良い．会場の広さはb_iである．b_i人以下になるような点数の最小の点数を求めよ．
+ 制約:
1<=**N**<=100,000
1<=a_i<=100,000
1<=**M**<=10,000
1<=b_i<=**N**
+ 時間制限: 2sec
+ メモリ制限: 256MB
+ 解法:
ある点までで何人入るかを管理したい．とりあえず座圧して，ヒストグラムを作って累積和で管理した．その際逆順にしておくと便利．
あとは二分探索でupper_boundを求めてupper_boundの値+1が答え．ただし，正の得点をとった人間を全員会場に入れる場合は0点でよい．
+ コメント:
なんかすごいバグらせた，もっと楽な解法あったんだろうなぁ

##[D:スライドパズル](http://indeednow-quala.contest.atcoder.jp/tasks/indeednow_2015_quala_4)
+ タグ: BFS
+ 概要:
**H**\***W**のスライドパズルがある．24手以内で解けるはずなので解け．
+ 制約:
1<=**H**,**W**<=6
+ 時間制限: 4sec
+ メモリ制限: 256MB
+ 解法:
解くための手数が24を超えるような移動と，巻き戻すような手をしないように枝刈りしながら幅優先探索した．かかる手数は0以外のマスの最終的な位置と現在の位置のマンハッタン距離の総和で求まる
+ コメント:
みんな解くの早すぎませんか．
