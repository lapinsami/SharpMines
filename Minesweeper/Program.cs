using System.Diagnostics;

namespace Minesweeper;

class Program
{
    // 40 by 12 field
    private static void Main()
    {
        int width = 40;
        int height = 12;
        int[] selection = [6, 20];
        char[][] gameField = InitializeGamefield(width, height);
        
        gameField[3][4] = 'F';
        gameField[10][2] = 'F';
        gameField[1][7] = 'F';
        gameField[4][3] = 'F';
        gameField[6][10] = 'F';
        gameField[1][1] = '1';
        gameField[1][2] = '2';
        gameField[1][3] = '3';
        gameField[1][4] = '4';
        gameField[1][5] = '5';
        gameField[1][6] = '6';
        gameField[3][6] = 'o';

        int frameCounter = 0;
        
        PrintDisplay(gameField, selection);
        
        while (true)
        {
            Console.Clear();
            frameCounter++;
            Console.WriteLine($"Frame {frameCounter}");
            PrintDisplay(gameField, selection);
            
            ConsoleKey input = GetInputKey();

            if (input == ConsoleKey.Q)
            {
                break;
            }
        }
    }

    private static void PrintSquare(char symbol, bool selected = false)
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
            default:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
        }

        if (selected)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        Console.Write(symbol);
        Console.ResetColor();
    }

    private static void PrintLine(char[] line, int selection, bool selectionOnLine = false)
    {
        for (int j = 0; j < line.Length; j++)
        {
            PrintSquare(line[j], selectionOnLine && j == selection);
            
            if (j < line.Length - 1)
            {
                Console.Write(" ");
            }
        }
    }

    private static void PrintDisplay(char[][] lines, int[] selection)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            PrintLine(lines[i], selection[1], selection[0] == i);
            Console.Write("\n");
        }
    }

    private static char[][] InitializeGamefield(int width, int height)
    {
        char[][] rows = new char[height][];
        
        for (int i = 0; i < height; i++)
        {
            char[] row = new char[width];
            for (int j = 0; j < width; j++)
            {
                row[j] = ' ';
            }

            rows[i] = row;
        }

        return rows;
    }

    private static ConsoleKey GetInputKey()
    {
        return Console.ReadKey(true).Key;
    }
}