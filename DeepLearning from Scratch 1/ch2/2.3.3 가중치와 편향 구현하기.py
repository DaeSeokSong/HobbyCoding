"""2.3.3 가중치와 편향 구현하기"""

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