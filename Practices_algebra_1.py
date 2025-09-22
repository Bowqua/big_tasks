'''
#1
from math import *

sub = ((195 * (8 % abs(-3))) / 3) ** 2 - (1001 // 2 + 2 ** 4)
twoSeven = sub ** (2 / 7)

result = 1000 ** (1 / 3) * sqrt(twoSeven)
print(result)
'''
'''
#2
def fibs (num):
    if num == 0:
        return 0
    elif num == 1:
        return 1
    else:
        return fibs(num - 1) + fibs(num - 2)

print(fibs(20))
'''
'''
#3
##1
import numpy as np

rows = 7
columns = 7
num = 2

matrix = np.full((rows, columns), num)
print(matrix)

##2
for i in range(matrix.shape[0]):
    for j in range(matrix.shape[1]):
        matrix[i, j] = matrix[i, j] ** j + i
print(matrix)
'''
'''
#4
def poly(lists, value):
    summ = 0
    degree = len(lists) - 1
    for index, number in enumerate(lists):
        summ += number * value ** (degree - index)
    return summ

print(poly([1, 1, 1], 1))
print(poly([2, 1, 0], -1))
print(poly([-1, 1, -1, 1, -1], 2))
'''
'''
#5
def geom_mean(lists):
    if len(lists) < 2:
        return None

    start = 1
    for number in lists:
        start *= number
    
    return start ** (1 / len(lists))

print(geom_mean([1]))
print(geom_mean([2, 2]))
print(geom_mean([2, 3, 4]))
print(geom_mean([-1, -2, 3, 4, 0.5]))
'''
'''
#6
import numpy as np

def make_matrix(n):
    values = np.tile(np.arange(1, n + 1), (n ** 2 + n - 1) // n)
    matrix = values[:n * n].reshape(n, n)

    for i in range(matrix.shape[0]):
        matrix[i] = np.roll(matrix[i], -i)
    
    return matrix

print(make_matrix(2))
print(make_matrix(4))
print(make_matrix(10))
'''
'''
#7
import numpy as np

first = np.array([[1, 2, 3], [4, 5, 6]])
second = np.array([[-2, 0], [0, 2], [1, -1]])
third = np.array([[1, 0], [0, 1]])
fourth = np.array([[1, 1], [2, 2]])
fifth = np.array([[1, -1], [-1, 1]])

result = first @ second + 3 * third - fourth * fifth
print(result)
'''
'''
#8
import numpy as np

def creating_matrix(matrix):
    row_sum = matrix.sum(axis=1)
    row_sum_matrix = row_sum[: np.newaxis]
    new_matrix = matrix / row_sum_matrix
    return new_matrix

array_1 = np.array([[1, 2, 3], [4, 5, 6], [7, 8, 9]])
array_2 = np.array([[1, 2], [4, 5]])
array_3 = np.array([[1, 2]])

print(creating_matrix(array_1))
print(creating_matrix(array_2))
print(creating_matrix(array_3))
'''
'''
#9
import numpy as np

def sort_matrix_cols(matrix):
    column_sum = matrix.sum(axis=0)
    sorted_index = np.argsort(column_sum)
    matrix = matrix[:, sorted_index]
    return matrix

array_1 = np.array([[3, 4], [1, 5], [2, 6]])
array_2 = np.array([[4, 1, 6], [11, 0, 2], [4, 6, 2]])

print(sort_matrix_cols(array_1))
print(sort_matrix_cols(array_2))
'''