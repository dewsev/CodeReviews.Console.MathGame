Random random = new Random();

int score = 0;
int questionCount = 15;
bool randomizedOperations = false;

Play();


void Play()
{
    DisplayWelcomeMessage();
 
    OperationType operationType = GetOperationTypeChoiceFromUser();
    
    Console.Clear();
    for (int i = 0; i < questionCount; i++)
    {
        (int operand1, int operand2) = GetRandomOperands(operationType);
        DisplayOperationString(operationType, operand1, operand2);

        string? readResult;
        do
        {
            readResult = Console.ReadLine()?.Trim();
            bool isValidInteger = int.TryParse(readResult, out int answer);
            if (isValidInteger)
            {
                CheckAnswer(operationType, operand1, operand2, answer);
                if (randomizedOperations)
                {
                    operationType = GetRandomOperationType(operationType);
                }
            }
            else
            {
                ClearCurrentConsoleLine();
                WriteColoredLine("Provide a valid integer.", ConsoleColor.Red);
            
                Console.Write($"{operand1} + {operand2} = ");
                readResult = "";
            }
        } while (string.IsNullOrEmpty(readResult));
    }
    
    Console.WriteLine($"\nYour score: {score}/{questionCount}");
    Console.WriteLine("Press any key to exit.");
    Console.ReadLine();
}


(int, int) GetRandomOperands(OperationType operationType)
{
    int operand1 = random.Next(1, 101);
    int operand2 = random.Next(1, 101);

    if (operationType == OperationType.Division)
    {
        while (operand1 % operand2 != 0)
        {
            operand1 = random.Next(1, 101);
            operand2 = random.Next(1, 101);
        }
    }

    return (operand1, operand2);
}


void DisplayWelcomeMessage()
{
    Console.WriteLine("Welcome to the MathGame!");
    Console.WriteLine($"You need to answer {questionCount} questions.");
}


OperationType GetOperationTypeChoiceFromUser()
{
    Console.WriteLine("\nChoose an operation type:");
    Console.WriteLine("  1.Addition");
    Console.WriteLine("  2.Subtraction");
    Console.WriteLine("  3.Multiplication");
    Console.WriteLine("  4.Division");
    Console.WriteLine("  5.Random\n\n");
    
    while (true)
    {
        ClearCurrentConsoleLine();
        Console.Write("Your choice: ");
        string? choice = Console.ReadLine()?.Trim();

        bool isValidInteger = int.TryParse(choice, out int integerChoice);
        if (isValidInteger && integerChoice is > 0 and < 6)
        {
            return integerChoice switch
            {
                1 => OperationType.Addition,
                2 => OperationType.Subtraction,
                3 => OperationType.Multiplication,
                4 => OperationType.Division,
                5 => SetupRandomOperations(),
            };
        }
    }
}


OperationType SetupRandomOperations()
{
    randomizedOperations = true;
    return GetRandomOperationType();
}


OperationType GetRandomOperationType(OperationType? currentOperationType = null)
{
    while (true)
    {
        int randomIndex = random.Next(Enum.GetValues<OperationType>().Length);
        OperationType randomOperation = (OperationType)randomIndex;

        if (randomOperation != currentOperationType)
        {
            return randomOperation;
        }
    }
}


void CheckAnswer(OperationType operationType, int operand1, int operand2, int answer)
{
    ClearCurrentConsoleLine();

    int expectedResult = ComputeExpectedResult(operand1, operand2, operationType);
    
    DisplayOperationString(operationType, operand1, operand2, answer);
    Console.Write("\t");
    if (expectedResult == answer)
    {
        score++;
        WriteColoredLine("Correct!", ConsoleColor.Green);
    }
    else
    {
        WriteColoredLine($"Wrong! It is {expectedResult}.", ConsoleColor.Red);
    }
}


void DisplayOperationString(OperationType operationType, int operand1, int operand2, int? result = null)
{
    char op = operationType switch
    {
        OperationType.Addition => '+',
        OperationType.Subtraction => '-',
        OperationType.Multiplication => 'x',
        OperationType.Division => '/',
    };
    
    Console.Write($"{operand1} {op} {operand2} = {(result != null ? result : "")}");
}


int ComputeExpectedResult(int operand1, int operand2, OperationType operationType)
{
    return operationType switch
    {
        OperationType.Addition => operand1 + operand2,
        OperationType.Subtraction => operand1 - operand2,
        OperationType.Multiplication => operand1 * operand2,
        OperationType.Division => operand1 / operand2
    };
}


void ClearCurrentConsoleLine()
{
    Console.SetCursorPosition(0, Console.CursorTop - 1);
    Console.Write(new string(' ', Console.BufferWidth));
    Console.SetCursorPosition(0, Console.CursorTop);
}


void WriteColoredLine(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}


enum OperationType { Addition, Subtraction, Multiplication, Division }