"""2.3.2 가중치와 편향 도입"""
import numpy as np

x = np.array([0, 1])        # 입력
w = np.array([0.5, 0.5])    # 가중치, 입력 신호가 결과에 주는 영향력(중요도) 조절하는 매개변수
b = -0.7                    # 편향, 뉴런이 얼마나 쉽게 활성화(결과 == 1)하느냐를 조정하는 매개변수

print(w*x)
print(np.sum(w*x))
print(np.sum(w*x) + b)