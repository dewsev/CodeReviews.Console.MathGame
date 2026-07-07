const int questionCount = 10;

int score = 0;
int operandMin = 0;
int operandMax = 0;
bool randomizedOperations = false;

Random random = new Random();
OperationType operationType = OperationType.None;
DifficultyLevel difficulty = DifficultyLevel.Easy;
List<(OperationType OperationType, DifficultyLevel Difficulty, int Seconds, int Score)> gameHistory = [];

while (true)
{
    PlayGame();
}


void PlayGame()
{
    MainMenu();

    operationType = GetOperationTypeChoiceFromUser();
    difficulty = GetDifficultyChoiceFromUser();

    SetOperandRange();

    int secondsElapsed = 0;
    _ = new Timer(
        _ => secondsElapsed++,
        null,
        0,
        1000);
    for (int i = 0; i < questionCount; i++)
    {
        (int operand1, int operand2) = GetOperands();

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
    gameHistory.Add((operationType, difficulty, secondsElapsed, score));
    GameOver(secondsElapsed);
}


void CheckAnswer(int operand1, int operand2, int answer)
{
    ClearCurrentConsoleLine();

    int expectedResult = ComputeExpectedResult(operand1, operand2);
    
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


int ComputeExpectedResult(int operand1, int operand2)
{
    return operationType switch
    {
        OperationType.Addition => operand1 + operand2,
        OperationType.Subtraction => operand1 - operand2,
        OperationType.Multiplication => operand1 * operand2,
        OperationType.Division => operand1 / operand2
    };
}


(int, int) GetOperands()
{
    int operand1 = GetRandomOperand();
    int operand2 = GetRandomOperand();

    if (operationType == OperationType.Division)
    {
        while (operand1 % operand2 != 0)
        {
            operand2 = GetRandomOperand();
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
        case DifficultyLevel.Hard:
            operandMin = 10;
            operandMax = 101;
            break;
        case DifficultyLevel.VeryHard:
            operandMin = 100;
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
                    ResetGameState();
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
    
    Console.Clear();
}


void ResetGameState()
{
    score = 0;
    randomizedOperations = false;
    operationType = OperationType.None;
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
            
            DifficultyLevel difficultyLevel = gameHistory[i].Difficulty;
            string difficultyDisplayString = difficultyLevel == DifficultyLevel.VeryHard ? "Very hard" : difficulty.ToString();
            
            Console.Write($"{i + 1}.{gameHistory[i].OperationType} - ");
            Console.Write($"{difficultyDisplayString} - ");
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


void GameOver(int secondsElapsed)
{
    Console.WriteLine($"\nTime: {TimeSpan.FromSeconds(secondsElapsed)}");
    Console.WriteLine($"Your score: {score}/{questionCount}");
    Console.WriteLine("Press any key to go back to main menu.");
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
                5 => SetupRandomOperations()
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
    Console.WriteLine("2.Hard");
    Console.WriteLine("3.Very hard\n");
    
    while (true)
    {
        try
        {
            Console.Write("Your choice: ");
            string? readResult = Console.ReadLine();
            return readResult switch
            {
                "1" => DifficultyLevel.Easy,
                "2" => DifficultyLevel.Hard,
                "3" => DifficultyLevel.VeryHard,
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


void DisplayOperationString(int operand1, int operand2, int? result = null)
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


enum OperationType { None, Addition, Subtraction, Multiplication, Division }
enum DifficultyLevel { Easy, Hard, VeryHard }