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
    new double[7],
    new double[2]
};

double T = 2; // порог
const int offset = 1; // сдвиг
double l = 1; // шаг обучения

HebbRules();

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

void HebbRules()
{
    FillWeights();
    bool isEqual = false;

    while (!isEqual)
    {
        isEqual = true;
        foreach (var valuePair in dataset)
        {
            double valueForFirstNeuron = valuePair[0] * weights[0][0] + valuePair[1] * weights[0][2] + offset * weights[0][4];

            int firstNeuronValue = ActivatonFunction(valueForFirstNeuron);

            double valueForSecondNeuron = valuePair[0] * weights[0][1] + valuePair[1] * weights[0][3] + offset * weights[0][5];

            int secondNeuronValue = ActivatonFunction(valueForSecondNeuron);

            double valueForThirdNeuron = weights[1][0] * firstNeuronValue + weights[1][1] * secondNeuronValue + offset * weights[0][6];

            int answer = ActivatonFunction(valueForThirdNeuron);
            int expected = Function(valuePair[0], valuePair[1]);

            if (answer != expected)
            {
                isEqual = false;

                weights[0][0] = Hebb(weights[0][0], valuePair[0], firstNeuronValue, expected);
                weights[0][1] = Hebb(weights[0][1], valuePair[0], secondNeuronValue, expected);
                weights[0][2] = Hebb(weights[0][2], valuePair[1], firstNeuronValue, expected);
                weights[0][3] = Hebb(weights[0][3], valuePair[1], secondNeuronValue, expected);
                weights[1][0] = Hebb(weights[1][0], firstNeuronValue, answer, expected);
                weights[1][1] = Hebb(weights[1][1], secondNeuronValue, answer, expected);
            }

            Console.WriteLine($"{valuePair[0]} {valuePair[1]} {weights[0][0]} {weights[0][1]} {weights[0][2]} {weights[0][3]} {weights[1][0]} {weights[1][1]} {answer} {expected}");
        }
    }
}

double Hebb(double weight, int x0, int y, int expected)
{
    return weight - l * x0 * (y - expected);
}