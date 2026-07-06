const int questionCount = 10;

Random random = new Random();
List<(OperationType OperationType, int Score)> gameHistory = [];

OperationType operationType = OperationType.None;
int score = 0;
bool randomizedOperations = false;

while (true)
{
    PlayGame();
}


(int, int) GetRandomOperands()
{
    int operand1 = random.Next(1, 101);
    int operand2 = random.Next(1, 101);

    if (operationType == OperationType.Division)
    {
        while (operand1 % operand2 != 0)
        {
            operand2 = random.Next(1, 101);
        }
    }

    return (operand1, operand2);
}


void PlayGame()
{
    MainMenu();
    operationType = GetOperationTypeChoiceFromUser();
    
    for (int i = 0; i < questionCount; i++)
    {
        (int operand1, int operand2) = GetRandomOperands();

        bool invalidAnswer = true;
        while (invalidAnswer)
        {
            try
            {
                DisplayOperationString(operand1, operand2);
                int answer = GetNumericInputFromUser();

                CheckAnswer(operand1, operand2, answer);
                if (randomizedOperations)
                {
                    operationType = GetRandomOperationType();
                }

                invalidAnswer = false;
            }
            catch (ArgumentException)
            {
                invalidAnswer = true;
                ClearCurrentConsoleLine();
                WriteColoredLine("Please provide a valid integer.", ConsoleColor.Red);
            }
        }
    }
    
    gameHistory.Add((operationType, score));
    GameOver();
}


void ResetGameState()
{
    score = 0;
    randomizedOperations = false;
    operationType = OperationType.None;
}


void MainMenu()
{
    Console.Clear();
    Console.WriteLine("Welcome to the MathGame!");
    Console.WriteLine($"You can test your *quick maths skills* by answering {questionCount} questions.");
    Console.WriteLine("\n1.Start new game");
    Console.WriteLine("2.Show game history");
    Console.WriteLine("3.Exit\n");

    while (true)
    {
        try
        {
            Console.Write("Your choice: ");
            int choice = GetNumericInputFromUser(1, 3);

            switch (choice)
            {
                case 1:
                    ResetGameState();
                    break;
                case 2:
                    DisplayGameHistory();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
            break;
        }
        catch (ArgumentException ex)
        {
            ClearCurrentConsoleLine();
            WriteColoredLine(ex.Message, ConsoleColor.Red);
        }
    }
    
    Console.Clear();
}


void DisplayGameHistory()
{
    Console.Clear();

    if (gameHistory.Count == 0)
    {
        Console.WriteLine("You have not played any games yet.");
    }
    else
    {
        Console.WriteLine("Your game history:\n");
        for (int i = 0; i < gameHistory.Count; i++)
        {
            string pointPluralization = gameHistory[i].Score == 1 ? "point" : "points";
            Console.WriteLine($"{i + 1}.{gameHistory[i].OperationType} - {gameHistory[i].Score} {pointPluralization}");
        }    
    }
    
    Console.WriteLine("\nPress any key to go back to main menu.");
    Console.WriteLine("Type EXIT to quit.");
    
    string? readResult = Console.ReadLine()?.ToLower().Trim();
    if (readResult == "exit")
    {
        Environment.Exit(0);
    }
    else
    {
        MainMenu();
    }
}


void GameOver()
{
    Console.Clear();
    Console.WriteLine($"Your score: {score}/{questionCount}");
    Console.WriteLine("Press any key to go back to main menu.");
    Console.WriteLine("Type EXIT to quit.");
    
    string? readResult = Console.ReadLine()?.ToLower().Trim();
    if (readResult == "exit")
    {
        Environment.Exit(0);
    }
}


int GetNumericInputFromUser(int min = int.MinValue, int max = int.MaxValue)
{
    while (true)
    {
        string? input = Console.ReadLine()?.Trim();
        bool isNumericInput = int.TryParse(input, out int numericInput);
        if (isNumericInput && numericInput >= min && numericInput <= max)
        {
            return numericInput;
        }
        throw new ArgumentException("Invalid input.");
    }
}


OperationType GetOperationTypeChoiceFromUser()
{
    Console.Clear();
    Console.WriteLine("Choose an operation type:\n");
    Console.WriteLine("1.Addition");
    Console.WriteLine("2.Subtraction");
    Console.WriteLine("3.Multiplication");
    Console.WriteLine("4.Division");
    Console.WriteLine("5.Random\n");
    
    while (true)
    {
        try
        {
            Console.Write("Your choice: ");
            int choice = GetNumericInputFromUser(1, 5);
            return choice switch
            {
                1 => OperationType.Addition,
                2 => OperationType.Subtraction,
                3 => OperationType.Multiplication,
                4 => OperationType.Division,
                5 => SetupRandomOperations(),
            };
        }
        catch (ArgumentException ex)
        {
            ClearCurrentConsoleLine();
            WriteColoredLine(ex.Message, ConsoleColor.Red);
        }
    }
}


OperationType SetupRandomOperations()
{
    randomizedOperations = true;
    return GetRandomOperationType();
}


OperationType GetRandomOperationType()
{
    while (true)
    {
        int randomIndex = random.Next(1, Enum.GetValues<OperationType>().Length);
        OperationType randomOperation = (OperationType)randomIndex;
        
        if (operationType == OperationType.None || randomOperation != operationType)
        {
            return randomOperation;
        }
    }
}


void CheckAnswer(int operand1, int operand2, int answer)
{
    ClearCurrentConsoleLine();

    int expectedResult = ComputeExpectedResult(operand1, operand2, operationType);
    
    DisplayOperationString(operand1, operand2, answer);
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


void DisplayOperationString(int operand1, int operand2, int? result = null)
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
        OperationType.Division => operand1 / operand2,
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


enum OperationType { None, Addition, Subtraction, Multiplication, Division }