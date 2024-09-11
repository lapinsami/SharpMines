namespace Minesweeper;

class Program
{
    // 40 by 12 field
    static void Main(string[] args)
    {
        string line = "########################################";

        for (int i = 0; i < 12; i++)
        {
            foreach (char c in line)
            {
                Console.Write(c + " ");
            }
            Console.Write("\n");
        }
    }

    static void PrintSquare(string backgroundColor, string textColor, char symbol)
    {
        if (backgroundColor == "gray")
        {
            Console.BackgroundColor = ConsoleColor.Gray;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }
        
        if (textColor == "red")
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.Write(symbol);
    }
}