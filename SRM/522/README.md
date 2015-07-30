#SRM522まとめ
##[Div1Easy]

+ 概要:
赤，緑，青色のボールがR,G,B個ある．N段の三角形にボールを配置し，同じ色が接触しないように配置するときこの三角形を何段作れるか？

+ 解法:  
二分探索で作れる上限を求める．N段の三角形を作るとき，N\*(N+1)/2のボールを使う．M=N\*(N+1)/2としたときに使うボールの数は3*M/3+M%3である．これを利用して解く．