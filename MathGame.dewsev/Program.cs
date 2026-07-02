int score = 0;
int questionCount = 15;
Random random = new Random();
bool randomizedOperations = false;

Play();


void Play()
{
    DisplayWelcomeMessage();
 
    OperationType operationType = GetOperationTypeChoiceFromUser();
    
    Console.Clear();
    for (int i = 0; i < questionCount; i++)
    {
        int firstNumber = random.Next(101);
        int secondNumber = random.Next(101);
        // TODO: Division should result in integer values, f.ex. 7/2 should not be displayed
        DisplayOperationString(operationType, firstNumber, secondNumber);

        string? readResult;
        do
        {
            readResult = Console.ReadLine()?.Trim();
            bool isValidInteger = int.TryParse(readResult, out int answer);
            if (isValidInteger)
            {
                CheckAnswer(operationType, firstNumber, secondNumber, answer);
                if (randomizedOperations)
                {
                    operationType = GetRandomOperationType();
                }
            }
            else
            {
                ClearCurrentConsoleLine();
                WriteColoredLine("Provide a valid integer.", ConsoleColor.Red);
            
                Console.Write($"{firstNumber} + {secondNumber} = ");
                readResult = "";
            }
        } while (string.IsNullOrEmpty(readResult));
    }
    
    Console.WriteLine($"\nYour score: {score}/{questionCount}");
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
    Console.WriteLine("  5.Random");
    Console.Write("\nYour choice: ");
    
    while (true)
    {
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


void CheckAnswer(OperationType operationType, int firstNumber, int secondNumber, int answer)
{
    ClearCurrentConsoleLine();

    int expectedResult = ComputeExpectedResult(firstNumber, secondNumber, operationType);
    
    DisplayOperationString(operationType, firstNumber, secondNumber, answer);
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


void DisplayOperationString(OperationType operationType, int firstNumber, int secondNumber, int? result = null)
{
    char op = operationType switch
    {
        OperationType.Addition => '+',
        OperationType.Subtraction => '-',
        OperationType.Multiplication => 'x',
        OperationType.Division => '/',
    };
    
    Console.Write($"{firstNumber} {op} {secondNumber} = {(result != null ? result : "")}");
}


int ComputeExpectedResult(int firstNumber, int secondNumber, OperationType operationType)
{
    return operationType switch
    {
        OperationType.Addition => firstNumber + secondNumber,
        OperationType.Subtraction => firstNumber - secondNumber,
        OperationType.Multiplication => firstNumber * secondNumber,
        OperationType.Division => firstNumber / secondNumber
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