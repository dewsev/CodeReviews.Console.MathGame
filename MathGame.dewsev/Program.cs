Console.WriteLine("Welcome to the MathGame!");

int score = 0;
int questionCount = 5;

Random random = new Random();

Console.WriteLine($"You need to answer {questionCount} questions.");
for (int i = 0; i < questionCount; i++)
{
    int firstNumber = random.Next(101);
    int secondNumber = random.Next(101);
    Console.Write($"{firstNumber} + {secondNumber} = ");

    string? readResult;
    do
    {
        readResult = Console.ReadLine();
        readResult = readResult?.Trim();

        bool isValidInteger = int.TryParse(readResult, out int answer);
        if (isValidInteger)
        {
            CheckAnswer(firstNumber, secondNumber, answer);
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


void CheckAnswer(int firstNumber, int secondNumber, int answer)
{
    ClearCurrentConsoleLine();
    int expectedResult = firstNumber + secondNumber;
    if (expectedResult == answer)
    {
        score++;
        Console.Write($"{firstNumber} + {secondNumber} = {answer}\t");
        WriteColoredLine("Correct!", ConsoleColor.Green);
    }
    else
    {
        Console.Write($"{firstNumber} + {secondNumber} = {answer}\t");
        WriteColoredLine($"Wrong! It is {expectedResult}.", ConsoleColor.Red);
    }
}

void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop - 1;
    Console.SetCursorPosition(0, currentLineCursor);
    Console.Write(new string(' ', Console.BufferWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}

void WriteColoredLine(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}