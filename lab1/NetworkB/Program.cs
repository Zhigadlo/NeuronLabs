int[][] dataset = new int[][]
{
    new int[] {0, 0},
    new int[] {0, 1},
    new int[] {1, 1},
    new int[] {1, 0}
};

// [layer][number]
double[][] weights = new double[][]
{
    new double[4],
    new double[4],
    new double[2]
};

double T = 2; // порог
int k = 1; // поколение

double l = 1; // шаг обучения

WindrowHoffRules();

// Функция варианта 
int Function(int x, int y) => ~x & y | y;

int ActivatonFunction(double value)
{
    if (value <= T) return 0;
    else return 1;
}

void FillWeights()
{
    Random rnd = new Random();
    foreach (var w in weights)
    {
        for (int i = 0; i < w.Length; i++)
            w[i] = rnd.Next(-2, 2);
    }
}

void WindrowHoffRules()
{
    FillWeights();
    bool isEqual = false;

    while (!isEqual)
    {
        isEqual = true;
        foreach (var valuePair in dataset)
        {
            double valueForFirstNeuron = valuePair[0] * weights[0][0] + valuePair[1] * weights[0][2];

            int firstNeuronValue = ActivatonFunction(valueForFirstNeuron);

            double valueForSecondNeuron = valuePair[0] * weights[0][1] + valuePair[1] * weights[0][3];

            int secondNeuronValue = ActivatonFunction(valueForSecondNeuron);

            double valueForThirdNeuron = weights[1][0] * firstNeuronValue + secondNeuronValue * weights[1][2];

            int thirdNeuronValue = ActivatonFunction(valueForThirdNeuron);

            double valueForFourthNeuron = weights[1][1] * firstNeuronValue + secondNeuronValue * weights[1][3];

            int fourthNeuronValue = ActivatonFunction(valueForFourthNeuron);

            double valueForLastNeuron = thirdNeuronValue * weights[2][0] + fourthNeuronValue * weights[2][1] + thirdNeuronValue * weights[1][2];

            int answer = ActivatonFunction(valueForLastNeuron);
            int expected = Function(valuePair[0], valuePair[1]);

            if (answer != expected)
            {
                isEqual = false;

                weights[0][0] = WindrowHoff(weights[0][0], valuePair[0], firstNeuronValue, expected);
                weights[0][1] = WindrowHoff(weights[0][1], valuePair[0], secondNeuronValue, expected);
                weights[0][2] = WindrowHoff(weights[0][2], valuePair[1], firstNeuronValue, expected);
                weights[0][3] = WindrowHoff(weights[0][3], valuePair[1], secondNeuronValue, expected);
                weights[1][0] = WindrowHoff(weights[1][0], firstNeuronValue, thirdNeuronValue, expected);
                weights[1][1] = WindrowHoff(weights[1][1], firstNeuronValue, fourthNeuronValue, expected);
                weights[1][2] = WindrowHoff(weights[1][2], secondNeuronValue, thirdNeuronValue, expected);
                weights[1][3] = WindrowHoff(weights[1][3], secondNeuronValue, fourthNeuronValue, expected);
                weights[2][0] = WindrowHoff(weights[2][0], thirdNeuronValue, answer, expected);
                weights[2][1] = WindrowHoff(weights[2][1], fourthNeuronValue, answer, expected);

                k++;
            }

            Console.WriteLine($"{valuePair[0]} {valuePair[1]} {weights[0][0]} {weights[0][1]} {weights[0][2]} {weights[0][3]} {weights[1][0]} {weights[1][1]} {weights[1][2]} {weights[1][3]} {weights[2][0]} {weights[2][1]} {answer} {expected}");
        }
    }
}

double WindrowHoff(double weight, int x0, int y, int expected)
{
    return weight - l/k * (Math.Pow(y, k) - Math.Pow(expected, k)) * Math.Pow(x0, k);
}