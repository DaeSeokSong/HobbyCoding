# 퍼셉트론은 직선 하나로 나눈 영역만 표현할 수 있다는 한계가 있다.
# 곡선의 영역은 '비선형 영역', 직선의 영역을 '선형 영역'이라고 한다.
# (단층) 퍼셉트론: 0층과 1층으로 구성된 퍼셉트론, 입력 후 별다른 값에 대한 공정 없이 바로 결과값이 나온다.
# 다층 퍼셉트론: 퍼셉트론을 여러 층을 쌓아놓은 것
# XOR 게이트는 '입력 → NAND, OR 층 → 각 층의 결과값에 대한 AND 연산결과' 과정을 거치는 다층(0~2층 또는 1~3층) 퍼셉트론이다. Ex. 0, 1 → NAND=1, OR=1 → AND=1 즉, 출력=1
"""2.5.2 XOR 게이트 구현하기"""

def AND(x1, x2):
    x = np.array([x1, x2])
    w = np.array([0.5, 0.5])
    b = -0.7

    tmp = np.sum(w*x) + b
    if tmp <= 0: return 0
    else: return 1

def NAND(x1, x2):
    x = np.array([x1, x2])
    # AND와 NAND는 가중치(w와 b)만 다르다
    w = np.array([-0.5, -0.5])
    b = 0.7

    tmp = np.sum(w*x) + b
    if tmp <= 0: return 0
    else: return 1

def OR(x1, x2):
    x = np.array([x1, x2])
    w = np.array([0.5, 0.5])
    b = -0.2

    tmp = np.sum(w*x) + b
    if tmp <= 0: return 0
    else: return 1

def XOR(x1, x2):
    s1 = NAND(x1, x2)
    s2 = OR(x1, x2)
    y = AND(s1, s2)
    return y

print(XOR(0, 0)) # 0
print(XOR(1, 0)) # 1
print(XOR(0, 1)) # 1
print(XOR(1, 1)) # 0