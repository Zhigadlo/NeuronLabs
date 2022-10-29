using ConsoleTables;

int[][] dataset = new int[][]
{
    new int[] {0, 0},
    new int[] {0, 1},
    new int[] {1, 1},
    new int[] {1, 0}
};

// [layer][number]
float[][] weights = new float[][] 
{ 
    new float[6], 
    new float[3] 
};

float T = 2; // порог

const float l = 0.2f; // шаг обучения

RosenblatRules();

// Функция варианта 
int Function(int x, int y) => ~x & y | y;

int ActivatonFunction(float value)
{
    if (value <= T) return 0;
    else return 1;
}

void FillWeights()
{
    Random rnd = new Random();
    foreach(var w in weights)
    {
        for (int i = 0; i < w.Length; i++)
            w[i] = rnd.Next(-2, 2);
    }
}

void RosenblatRules()
{
    ConsoleTable consoleTable = new ConsoleTable("x", "y", "wx1", "wx2", "wy1", "wy2", "wb1", "wb2", "w21", "w22", "w23", "y", "e");
    FillWeights();
    bool isEqual = false;

    while (!isEqual)
    {
        isEqual = true;
        foreach(var valuePair in dataset)
        {
            float valueForFirstNeuron = valuePair[0]*weights[0][0] + valuePair[1]*weights[0][3];

            int firstNeuronValue = ActivatonFunction(valueForFirstNeuron);

            float valueForSecondNeuron = valuePair[0] * weights[0][1] + valuePair[1] * weights[0][4];
        
            int secondNeuronValue = ActivatonFunction(valueForSecondNeuron);

            float valueForThirdNeuron = valuePair[0] * weights[0][2] + valuePair[1] * weights[0][5];

            int thirdNeuronValue = ActivatonFunction(valueForThirdNeuron);

            float valueForLastNeuron = firstNeuronValue * weights[1][0] + secondNeuronValue * weights[1][1] + thirdNeuronValue * weights[1][2];

            int answer = ActivatonFunction(valueForLastNeuron);
            int expected = Function(valuePair[0], valuePair[1]);
            
            if (answer != expected)
            {
                isEqual = false;
                weights[0][0] = Rosenblat(valuePair[0], weights[0][0], answer, expected);
                weights[0][1] = Rosenblat(valuePair[0], weights[0][1], answer, expected);
                weights[0][2] = Rosenblat(valuePair[0], weights[0][2], answer, expected);
                weights[0][3] = Rosenblat(valuePair[1], weights[0][3], answer, expected);
                weights[0][4] = Rosenblat(valuePair[1], weights[0][4], answer, expected);
                weights[0][5] = Rosenblat(valuePair[1], weights[0][5], answer, expected);
                weights[1][0] = Rosenblat(firstNeuronValue, weights[1][0], answer, expected);
                weights[1][1] = Rosenblat(secondNeuronValue, weights[1][1], answer, expected);
                weights[1][2] = Rosenblat(thirdNeuronValue, weights[1][2], answer, expected);
            }
            consoleTable.AddRow(valuePair[0], valuePair[1], weights[0][0], weights[0][1], weights[0][2], weights[0][3], weights[0][4], weights[0][5], weights[1][0], weights[1][1], weights[1][2], answer, expected);
        }
    }

    consoleTable.Write();
}

float Rosenblat(int x0, float weight, int y, int expected)
{
    return weight - l * x0 * (y - expected);
}