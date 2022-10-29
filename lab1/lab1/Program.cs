int[][] dataset = new int[][]
{
    new int[] {0, 0},
    new int[] {0, 1},
    new int[] {1, 1},
    new int[] {1, 0}
};

double[][] weights = new double[][]
{
    new double[4],
    new double[4],
    new double[2]
};

double T = 1.6; // порог
const int offset = 1; // сдвиг

ErrorCorrectionMethodWithQuantization();

// Функция варианта 
int Function(int x, int y) => ~x & y | y;

void FillWeights()
{
    Random rnd = new Random();
    foreach (var w in weights)
    {
        for (int i = 0; i < w.Length; i++)
            w[i] = rnd.Next(-2, 2);
    }
}

int ActivatonFunction(double value)
{
    if (value < T) return 0;
    else return 1;
}

void ErrorCorrectionMethodWithQuantization()
{
    FillWeights();
    bool isEqual = false;

    while (!isEqual)
    {
        isEqual = true;
        
        foreach(var valuePair in dataset)
        {
            double valueForFirstNeuron = valuePair[0] * weights[0][0] + valuePair[1] * weights[0][2];
            int firstNeuronValue = ActivatonFunction(valueForFirstNeuron);

            double valueForSecondNeuron = valuePair[0] * weights[0][1] + valuePair[1] * weights[0][3];
            int secondNeuronValue = ActivatonFunction(valueForSecondNeuron);
            
            double valueForThirdNeuron = firstNeuronValue * weights[1][0] + offset * weights[1][2];
            int thirdNeuronValue = ActivatonFunction(valueForThirdNeuron);

            double valueForFourthNeuron = secondNeuronValue * weights[1][1] + offset + weights[1][3];
            int fourthNeuronValue = ActivatonFunction(valueForFourthNeuron);

            double valueForLastNeuron = thirdNeuronValue * weights[2][0] + fourthNeuronValue * weights[2][1];
            
            int answer = ActivatonFunction(valueForLastNeuron);
            int expected = Function(valuePair[0], valuePair[1]);

            if(answer != expected)
            {
                isEqual = false;

                weights[0][0] = CorrectionWithQuantization(weights[0][0], firstNeuronValue, expected);
                weights[0][1] = CorrectionWithQuantization(weights[0][1], secondNeuronValue, expected);
                weights[0][2] = CorrectionWithQuantization(weights[0][2], firstNeuronValue, expected);
                weights[0][3] = CorrectionWithQuantization(weights[0][3], secondNeuronValue, expected);
                weights[1][0] = CorrectionWithQuantization(weights[1][0], thirdNeuronValue, expected);
                weights[1][1] = CorrectionWithQuantization(weights[1][1], fourthNeuronValue, expected);
                weights[1][2] = CorrectionWithQuantization(weights[1][2], thirdNeuronValue, expected);
                weights[1][3] = CorrectionWithQuantization(weights[1][3], fourthNeuronValue, expected);
                weights[2][0] = CorrectionWithQuantization(weights[2][0], answer, expected);
                weights[2][1] = CorrectionWithQuantization(weights[2][1], answer, expected);
            }

            Console.WriteLine($"{valuePair[0]} {valuePair[1]} {weights[0][0]} {weights[0][1]} {weights[0][2]} {weights[0][3]} {weights[1][0]} {weights[1][1]} {weights[1][2]} {weights[1][3]} {weights[2][0]} {weights[2][1]} {answer} {expected}");
        }
    }
}

double CorrectionWithQuantization(double weight, int y, int expected)
{
    return weight + 2*(y - expected);
}