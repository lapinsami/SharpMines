namespace Minesweeper;

class Program
{
    // 40 by 12 field
    private static void Main()
    {
        PrintDisplay(InitializeDisplay(40, 12));
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

    private static void PrintDisplay(string[] lines)
    {
        foreach (string line in lines)
        {
            PrintLine(line);
            Console.Write("\n");
        }
    }

    private static string[] InitializeDisplay(int width, int height)
    {
        string[] rowArray = new string[height];
        
        for (int i = 0; i < height; i++)
        {
            string row = "";
            for (int j = 0; j < width; j++)
            {
                row += "#";
            }
            
            rowArray[i] = row;
        }

        return rowArray;
    }
}