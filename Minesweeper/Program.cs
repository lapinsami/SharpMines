namespace Minesweeper;

class Program
{
    // 40 by 12 field
    private static void Main(string[] args)
    {
        string line = "########################################";

        for (int i = 0; i < 12; i++)
        {
            PrintLine(line);
            Console.Write("\n");
        }
    }

    private static void PrintSquare(char symbol, string color)
    {
        if (color == "red")
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.Write(symbol);
    }

    private static void PrintLine(string line)
    {
        for (int j = 0; j < line.Length; j++)
        {
            PrintSquare(line[j], "red");
            if (j < line.Length - 1)
            {
                Console.Write(" ");
            }
        }
    }
}