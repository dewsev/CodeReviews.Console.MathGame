Console.WriteLine("Welcome to the MathGame!");

int score = 0;
int questionCount = 5;

Random random = new Random();

Console.WriteLine($"You need to answer {questionCount} questions.");
for (int i = 0; i < questionCount; i++)
{
    int firstNumber = random.Next(0, 101);
    int secondNumber = random.Next(0, 101);
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
            Console.WriteLine("Provide a valid integer.");
            readResult = "";
        }
    } while (string.IsNullOrEmpty(readResult));
}

Console.WriteLine($"Your score: {score}/{questionCount}.");


void CheckAnswer(int firstNumber, int secondNumber, int answer)
{
    ClearCurrentConsoleLine();
    int expectedResult = firstNumber + secondNumber;
    if (expectedResult == answer)
    {
        score++;
        Console.Write($"{firstNumber} + {secondNumber} = {answer}\t");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Correct!\n");
    }
    else
    {
        Console.Write($"{firstNumber} + {secondNumber} = {answer}\t");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"Wrong! It is {expectedResult}.\n");
    }
    
    Console.ResetColor();
}

void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop - 1;
    Console.SetCursorPosition(0, currentLineCursor);
    Console.Write(new string(' ', Console.BufferWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}