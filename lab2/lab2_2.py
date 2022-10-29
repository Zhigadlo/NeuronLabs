# -*- coding: utf-8 -*-
"""lab2.2.ipynb

Automatically generated by Colaboratory.

Original file is located at
    https://colab.research.google.com/drive/1BuIA40E5mJO63K8bOqnWBuxJFgC6xgP-

Подключение библиотек
"""

import numpy as np
from PIL import Image, ImageOps
import os
import matplotlib.pyplot as plt
from google.colab import drive 
drive.mount('/content/drive')
!ls "/content/drive/My Drive/"

"""Объявление всех необходимых переменных"""

l = 0.1 #learning speed
leftTracksPath = "/content/drive/My Drive/NeuronLabs/LeftTracks"
rightTracksPath = "/content/drive/My Drive/NeuronLabs/RightTracks"
leftTestTracksPath = "/content/drive/My Drive/NeuronLabs/LeftTestTracks"
rightTestTracksPath = "/content/drive/My Drive/NeuronLabs/RightTestTracks"
t_x = []
t_y = []
test_x = []
test_y = []

"""Необходимые классы"""

class Neuron:
  def __init__(self, weights):
    self.weights = weights
    self.bias = 1
    self.biasWeight = 1
    self.sum = 0
    self.answer = 0
    self.grad = 0
    self.inputs = []

  def getAnswer(self, inputs):
    answer = np.dot(self.weights, inputs) + self.bias * self.biasWeight
    self.sum = answer
    self.answer = sigmoid(answer)
    self.inputs = inputs
    return sigmoid(answer)
  
  #def getGrad():
  #  self.grad = np.dot(prev_grad, prev_weights) * fun_der_sigm(self.sum)

  def fit(self, sum_w_grad):
    for k in range(len(self.weights)):
      self.weights[k] -= l * self.grad * self.inputs[k]
    
    #prev_weights = []
    #for neuron in prev_neurons:
    #  prev_weights.append(neuron.weights[neuron_num])
    #self.grad = np.dot(prev_grad, prev_weights) * fun_der_sigm(self.sum)
    #for i in range(len(self.weights)):
    #  self.weights[i] -= l * prev_grad[i] * next_neurons_answers[i]

class NeuronNetwork:
  def __init__(self, layerNeurons, inputCount):
    self.layerNeurons = layerNeurons
    self.layerCount = len(layerNeurons)
    self.inputCount = inputCount
    self.neuronCount = sum(layerNeurons)
    self.neurons = []
    self.losses = []
    self.epochs = []

  def fill_random_weights(self):
    inputs = self.inputCount
    for i in range(len(self.layerNeurons)):
      layer = []
      print(self.layerNeurons[i])
      for j in range(self.layerNeurons[i]):
        w = np.random.randint(-20,20, inputs)/3
        layer.append(Neuron(w))
      self.neurons.append(layer)
      inputs = self.layerNeurons[i]

  def predict(self, x_i):
    answers = []
    inputs = x_i
    for i in range(len(self.layerNeurons)):
      layerAnswers = []
      for j in range(self.layerNeurons[i]):
        layerAnswers.append(self.neurons[i][j].getAnswer(inputs))
      inputs = layerAnswers
      answers.append(layerAnswers)
    return answers[len(answers)-1][0]

  def loss(self, x,y):
    loss = 0
    for j in range(len(x)):
      answer = self.predict(x[j])
      loss += np.square(y[j] - answer)
    return loss/ len(x)
  #backprop
  def fit(self, inputs, expected, e):
    epoch = 1
    self.epochs = [1]
    loss = self.loss(inputs, expected)
    self.losses.append(loss)
    while epoch < 25000 and loss > e:
      if(epoch % 1000 == 0):
        print(epoch, "epoch has started.. Error:", self.loss(inputs, expected))
      n_rule = np.random.randint(0, len(t_x))
      x = t_x[n_rule]
      y = t_y[n_rule]
      #forward
      answer = self.predict(x)
      #back
      delt = answer - y
      grad = delt * fun_der_sigm(answer)
      prev_layer_grad = []
      cur_l = self.layerCount-1
      #слой 4
      #self.neurons[cur_l][0].fit(sums)
      for i in range(len(self.neurons[cur_l][0].weights)):
        self.neurons[cur_l][0].weights[i] -= l * grad * self.neurons[cur_l][0].inputs[i]
        self.neurons[cur_l][0].grad = grad * self.neurons[cur_l][0].weights[i]
        prev_layer_grad.append(self.neurons[cur_l][0].grad)
      
      sums = prev_layer_grad * self.neurons[cur_l][0].weights
      #внутренние слои
      cur_l -= 1
      while(cur_l > 0):
        new_layer_grad = []
        for i in range(len(self.neurons[cur_l])):
          self.neurons[cur_l][i].grad = sums[i] * fun_sigm_answ(self.neurons[cur_l][i].answer)
          new_layer_grad.append(self.neurons[cur_l][i].grad)
          #for k in range(len(self.neurons[cur_l][i].weights)):
          #  self.neurons[cur_l][i].weights[k] -= l * new_layer_grad[i] * self.neurons[cur_l][i].inputs[k]
        self.neurons[cur_l][i].fit(sums)
        #пересчет sums
        sums = []
        for i in range(len(self.neurons[cur_l][i].weights)):
          sum = 0
          for j in range(len(self.neurons[cur_l])):
            sum += self.neurons[cur_l][j].weights[i] * self.neurons[cur_l][j].grad
          sums.append(sum)
        prev_layer_grad = new_layer_grad
        cur_l -= 1

      #первый слой
      self.neurons[cur_l][i].fit(sums)
      #for i in range(len(self.neurons[cur_l])):
        #for j in range(len(self.neurons[cur_l][i].weights)):
          #self.neurons[cur_l][i].weights[j] -= l * sums[i] * x[j]
      
      loss = self.loss(inputs, expected)
      self.losses.append(loss)
      epoch += 1
      self.epochs.append(epoch)
    print("network was learned for",epoch,"generations")

"""Вспомагательные функции"""

def sigmoid(value):
  return 1 / (1 + np.exp(-value))

def fun_der_sigm(value):
  return sigmoid(value) * (1 - sigmoid(value))

def fun_sigm_answ(answ):
  return answ * (1 - answ)

def AddVehicle(path):
  test_img = Image.open(path)
  test_img = test_img.resize((5,5))
  test_img = test_img.convert('L')
  #plt.imshow(test_img, cmap = 'gray')
  #plt.show()

  test_x = np.array(test_img, np.float32)
  test_x = test_x.reshape(-1,25)
  test_x = test_x / 255
  return test_x[0]

def AddLeftTrucks():
  for filename in os.listdir(leftTracksPath):
    path = os.path.join(leftTracksPath, filename)
    t_x.append(AddVehicle(path))
    t_y.append(0)

def AddRightTrucks():
  for filename in os.listdir(rightTracksPath):
    path = os.path.join(rightTracksPath, filename)
    t_x.append(AddVehicle(path))
    t_y.append(1)

def AddTestTrucks():
  for filename in os.listdir(leftTestTracksPath):
    path = os.path.join(leftTestTracksPath, filename)
    test_x.append(AddVehicle(path))
    test_y.append(0)
  for filename in os.listdir(rightTestTracksPath):
    path = os.path.join(rightTestTracksPath, filename)
    test_x.append(AddVehicle(path))
    test_y.append(1)

def output_weights(neuronNetwork : NeuronNetwork):
  for i in range(neuronNetwork.layerCount):
    for j in range(len(neuronNetwork.neurons[i])):
      print(str(i) + " " + str(j) + " " + str(len(neuronNetwork.neurons[i][j].weights)))# + str(neuronNetwork.neurons[i][j].weights))

"""Заполнение начальных данных"""

AddLeftTrucks()
AddRightTrucks()
AddTestTrucks()

"""Начальные веса"""

neuronNetwork = NeuronNetwork([10, 8, 6, 1], 25)
neuronNetwork.fill_random_weights()

"""Веса после обучения"""

neuronNetwork.fit(t_x, t_y, 0.01)

for i in range(len(test_y)):
  print(neuronNetwork.predict(test_x[i]), ": ", test_y[i])

plt.plot(neuronNetwork.epochs, neuronNetwork.losses)
plt.show()