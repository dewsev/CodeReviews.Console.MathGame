// TODO: Display goodbye message when quitting the game
// TODO: Is ArgumentException a good Exception type?
// TODO: Extract all input gathering to separate methods
// TODO: Intercept keys in the menus, so the user does not need to press ENTER every time (maybe)


const int questionCount = 10;
Random random = new Random();

// TODO: You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.
// Keep score from each game inside a list, gameHistory[0] -> game 1, etc...
// Keep operation type for each game
List<int> gameHistory = [];

OperationType operationType = OperationType.Addition;
int score = 0;
bool randomizedOperations = false;
bool quit = false;

Run();


void Run()
{
    while (!quit)
    {
        Console.Clear();
        MainMenu();
        Console.Clear();
        PlayGame();
        Console.Clear();
        GameOver();
    }
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
}


void SetupNewGame()
{
    score = 0;
    randomizedOperations = false;
    operationType = GetOperationTypeChoiceFromUser();
}


void MainMenu()
{
    Console.WriteLine("Welcome to the MathGame!");
    Console.WriteLine($"You can test your *quick maths skills* by answering {questionCount} questions.");
    Console.WriteLine("\n1.Start new game");
    Console.WriteLine("2.Show game history");
    Console.WriteLine("3.Exit\n");

    try
    {
        Console.Write("Your choice: ");
        int choice = GetNumericInputFromUser(1, 3);

        switch (choice)
        {
            case 1:
                SetupNewGame();
                break;
            case 2:
                // DisplayGameHistory();
                break;
            case 3:
                quit = true;
                break;
        }
    }
    catch (ArgumentException ex)
    {
        WriteColoredLine(ex.Message, ConsoleColor.Red);
    }    
}


void GameOver()
{
    // TODO: Add score to gameHistory
    // TODO: Colorize the score based on how well the game went
    Console.WriteLine($"Your score: {score}/{questionCount}");
    Console.WriteLine("Press any key to go back to main menu.");
    Console.WriteLine("Type EXIT to quit.");

    string? readResult = Console.ReadLine()?.ToLower().Trim();
    if (readResult == "exit")
    {
        quit = true;
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
    Console.WriteLine("Choose an operation type:\n");
    Console.WriteLine("1.Addition");
    Console.WriteLine("2.Subtraction");
    Console.WriteLine("3.Multiplication");
    Console.WriteLine("4.Division");
    Console.WriteLine("5.Random\n\n");
    
    while (true)
    {
        ClearCurrentConsoleLine();
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
        int randomIndex = random.Next(Enum.GetValues<OperationType>().Length);
        OperationType randomOperation = (OperationType)randomIndex;
        
        // TODO: Fix this, gives the same operation few times in a row
        // the gameHistory.Count check is wrong
        if (gameHistory.Count == 0 || randomOperation != operationType)
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