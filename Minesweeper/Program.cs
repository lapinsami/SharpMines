namespace Minesweeper;

class Program
{
    // 40 by 12 field
    private static void Main()
    {
        PrintDisplay(InitializeDisplay(40, 12));
    }

    private static void PrintSquare(char symbol)
    {
        switch (symbol)
        {
            case 'o':
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                break;
            case 'F':
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            case '1':
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                break;
            case '2':
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                break;
            case '3':
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;
            case '4':
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;
            case '5':
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            case '6':
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
        }

        Console.Write(symbol);
        Console.ResetColor();
    }

    private static void PrintLine(string line)
    {
        for (int j = 0; j < line.Length; j++)
        {
            PrintSquare(line[j]);
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