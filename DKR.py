'''
#1
import sympy as sp

mod = 29 #работаем в поле Z_29
n, k = 17, 103
m = (n ** 2 + (-1) ** k * (n + 2) - 5) % 60 #вычислим m
nu_values = range(29) #переберем v = 0, 1, 2, ..., 28
results = {}

for nu in nu_values:
    a = (5 + nu) % mod #строим матрицу A(v)
    A = sp.Matrix([[a, 1], [28, 12]])
    gamma = 26 # 2(-1)^k-1 mod 29

    basis = [sp.Matrix([[1, 0], [0, 0]]), sp.Matrix([[0, 1], [0, 0]]),
             sp.Matrix([[0, 0], [1, 0]]), sp.Matrix([[0, 0], [0, 1]])]

    columns = []
    for X in basis:
        Y = A * X + gamma * X.T * A.T #применим fv к каждому базисному X
        columns.append([int(e % mod) for e in [Y[0, 0], Y[0, 1], Y[1, 0], Y[1, 1]]])

    M = sp.Matrix(columns).T #полученная матрица 4x4 оператора
    charpoly = M.charpoly().as_expr().expand()
    results[nu] = sp.factor(charpoly, modulus=mod)

for nu, cp in results.items():
    print(f"v = {nu} mod 29: X(lambda) = {cp}")
'''

#2
import numpy as np

A = np.array([[1, 1, 1, 1, 2, 1, 1, 0],
              [2, 6, 8, 6, 0, 2, 6, 3],
              [3, 7, 9, 7, 2, 3, 7, 3]], dtype=float) #фамильная матрица

U, S, Vt = np.linalg.svd(A, full_matrices=False) #решение сингулярного разложения
print("U =\n", np.round(U, 3))
print("Sigma = diag", np.round(S, 3))
print("V^T =\n", np.round(Vt, 3))