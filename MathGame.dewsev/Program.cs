const int questionCount = 10;

int operandMin = 0;
int operandMax = 0;

int score = 0;
int secondsElapsed = 0;

OperationType gameOperationType = OperationType.None;
DifficultyLevel difficulty = DifficultyLevel.Easy;

Random random = new Random();
List<(OperationType OperationType, DifficultyLevel Difficulty, int Seconds, int Score)> gameHistory = [];

while (true)
{
    Game();
}


void Game()
{
    MainMenu();
    
    _ = new Timer(
        _ => secondsElapsed++,
        null,
        0,
        1000);

    OperationType questionOperationType = OperationType.None;
    questionOperationType = gameOperationType == OperationType.Random ? GetRandomOperationType(questionOperationType) : gameOperationType;
    
    Console.Clear();
    
    for (int i = 0; i < questionCount; i++)
    {
        if (gameOperationType == OperationType.Random)
        {
            questionOperationType = GetRandomOperationType(questionOperationType);
        }
        
        (int operand1, int operand2) = GetOperands(questionOperationType, difficulty);

        AskQuestion(operand1, operand2, questionOperationType);
    }
    
    gameHistory.Add((gameOperationType, difficulty, secondsElapsed, score));
    GameOver();
}


void AskQuestion(int operand1, int operand2, OperationType operationType)
{
    while (true)
    {
        DisplayOperationString(operand1, operand2, operationType);
            
        try
        {
            int answer = GetNumericInputFromUser();
            bool correct = ValidateAnswer(operand1, operand2, operationType, answer);

            DisplayOperationString(operand1, operand2, operationType, answer);
            Console.Write("\t");
            if (correct)
            {
                score++;
                WriteColoredLine("Correct!", ConsoleColor.Green);
            }
            else
            {
                int expectedResult = ComputeExpectedResult(operand1, operand2, operationType);
                WriteColoredLine($"Wrong! It is {expectedResult}.", ConsoleColor.Red);
            }
                
            break;
        }
        catch (ArgumentException)
        {
            ClearCurrentConsoleLine();
            WriteColoredLine("Please provide a valid integer.", ConsoleColor.Red);
        }
    }
}


bool ValidateAnswer(int operand1, int operand2, OperationType operationType, int answer)
{
    ClearCurrentConsoleLine();

    int expected = ComputeExpectedResult(operand1, operand2, operationType);
    if (expected == answer)
    {
        return true;
    }
    
    return false;
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


(int, int) GetOperands(OperationType operationType, DifficultyLevel difficulty)
{
    int operand1 = GetRandomOperand();
    int operand2 = GetRandomOperand();

    if (difficulty == DifficultyLevel.Hard && operationType != OperationType.Division)
    {
        while (operand1 % 10 == 0)
        {
            operand1 = GetRandomOperand();
        }
        
        while (operand2 % 10 == 0)
        {
            operand2 = GetRandomOperand();
        }
    }

    if (operationType == OperationType.Division)
    {
        while (operand1 % operand2 != 0 || operand1 / operand2 == 1 || operand2 == 1)
        {
            operand1 = GetRandomOperand();
            operand2 = GetRandomOperand();
            
            if (difficulty == DifficultyLevel.Hard)
            {
                while (operand1 % 10 == 0)
                {
                    operand1 = GetRandomOperand();
                    operand2 = GetRandomOperand();
                }
            }
        }
    }

    return (operand1, operand2);
}


int GetRandomOperand()
{
    return random.Next(operandMin, operandMax);
}


void SetOperandRange()
{
    switch (difficulty)
    {
        case DifficultyLevel.Easy:
            operandMin = 1;
            operandMax = 11;
            break;
        case DifficultyLevel.Medium:
            operandMin = 1;
            operandMax = 101;
            break;
        case DifficultyLevel.Hard:
            operandMin = 1;
            operandMax = 1001;
            break;
    }
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
                    SetupGame();
                    break;
                case 2:
                    DisplayGameHistory();
                    break;
                case 3:
                    Quit();
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
}


void SetupGame()
{
    score = 0;
    secondsElapsed = 0;
    gameOperationType = GetOperationTypeChoiceFromUser();
    difficulty = GetDifficultyChoiceFromUser();
    SetOperandRange();
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
            
            Console.Write($"{i + 1}.{gameHistory[i].OperationType} - ");
            Console.Write($"{gameHistory[i].Difficulty} - ");
            Console.Write($"{TimeSpan.FromSeconds(gameHistory[i].Seconds)} - ");
            Console.Write($"{gameHistory[i].Score} {pointPluralization}\n");
        }    
    }
    
    Console.WriteLine("\nPress any key to go back to main menu.");
    Console.WriteLine("Type EXIT to quit.");
    
    string? readResult = Console.ReadLine()?.ToLowerInvariant().Trim();
    if (readResult == "exit")
    {
        Quit();
    }
    else
    {
        MainMenu();
    }
}


void GameOver()
{
    Console.WriteLine($"\nTime: {TimeSpan.FromSeconds(secondsElapsed)}");
    Console.WriteLine($"Score: {score}/{questionCount}");
    Console.WriteLine("\nPress any key to go back to main menu.");
    Console.WriteLine("Type EXIT to quit.");
    
    string? readResult = Console.ReadLine()?.ToLowerInvariant().Trim();
    if (readResult == "exit")
    {
        Quit();
    }
}


void Quit()
{
    Console.Clear();
    Console.WriteLine("Thank you for playing the Math Game!");
    Console.WriteLine("See you next time.");
    Environment.Exit(0);
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
                5 => OperationType.Random
            };
        }
        catch (ArgumentException ex)
        {
            ClearCurrentConsoleLine();
            WriteColoredLine(ex.Message, ConsoleColor.Red);
        }
    }
}


DifficultyLevel GetDifficultyChoiceFromUser()
{
    Console.Clear();
    Console.WriteLine("Choose a difficulty level:\n");
    Console.WriteLine("1.Easy");
    Console.WriteLine("2.Medium");
    Console.WriteLine("3.Hard\n");
    
    while (true)
    {
        try
        {
            Console.Write("Your choice: ");
            int choice = GetNumericInputFromUser(1, 3);
            return choice switch
            {
                1 => DifficultyLevel.Easy,
                2 => DifficultyLevel.Medium,
                3 => DifficultyLevel.Hard,
            };
        }
        catch (ArgumentException ex)
        {
            ClearCurrentConsoleLine();
            WriteColoredLine(ex.Message, ConsoleColor.Red);
        }
    }
}


OperationType GetRandomOperationType(OperationType currentOperationType)
{
    while (true)
    {
        // Should never return OperationType.None and OperationType.Random - skip them
        int randomIndex = random.Next(1, Enum.GetValues<OperationType>().Length - 1);
        OperationType randomOperation = (OperationType)randomIndex;
        
        if (currentOperationType == OperationType.None || randomOperation != currentOperationType)
        {
            return randomOperation;
        }
    }
}


void DisplayOperationString(int operand1, int operand2, OperationType operationType, int? result = null)
{
    char op = operationType switch
    {
        OperationType.Addition => '+',
        OperationType.Subtraction => '-',
        OperationType.Multiplication => 'x',
        OperationType.Division => '/'
    };
    
    Console.Write($"{operand1} {op} {operand2} = {(result != null ? result : "")}");
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


enum OperationType { None, Addition, Subtraction, Multiplication, Division, Random }
enum DifficultyLevel { Easy, Medium, Hard }