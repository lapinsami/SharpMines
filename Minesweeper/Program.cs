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
        
        gameField[3][4] = '¶';
        gameField[10][2] = '¶';
        gameField[1][7] = '¶';
        gameField[4][3] = '¶';
        gameField[6][10] = '¶';
        gameField[1][1] = '1';
        gameField[1][2] = '2';
        gameField[1][3] = '3';
        gameField[1][4] = '4';
        gameField[1][5] = '5';
        gameField[1][6] = '6';
        gameField[3][6] = 'o';

        gameField = PutBombsOnGamefield(gameField, 50);

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
            switch (input)
            {
                case ConsoleKey.F:
                    gameField[selection[0]][selection[1]] = '¶';
                    break;
                case ConsoleKey.Spacebar:
                    gameField[selection[0]][selection[1]] = 'o';
                    break;
                default:
                    selection = UpdateSelection(selection, input, width, height);
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
            case '¶':
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            case '#':
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.BackgroundColor = ConsoleColor.Black;
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
                row[j] = '#';
            }

            rows[i] = row;
        }

        return rows;
    }

    private static char[][] PutBombsOnGamefield(char[][] gamefield, int numberOfBombs)
    {
        Random rng = new();
        
        for (int i = 0; i < numberOfBombs; i++)
        {
            while (true)
            {
                int randomRow = rng.Next(gamefield.Length);
                int randomColumn = rng.Next(gamefield[0].Length);

                if (gamefield[randomRow][randomColumn] != 'o')
                {
                    gamefield[randomRow][randomColumn] = 'o';
                    break;
                }
            }
        }
        return gamefield;
    }

    private static ConsoleKey GetInputKey()
    {
        ConsoleKey[] allowedInputs =
        [
            ConsoleKey.Q,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.F,
            ConsoleKey.Spacebar
        ];
        
        while (true)
        {
            ConsoleKey input = Console.ReadKey(true).Key;

            if (allowedInputs.Contains(input))
            {
                return input;
            }
        }
    }

    private static int[] UpdateSelection(int[] selection, ConsoleKey pressedKey, int width, int height)
    {
        switch (pressedKey)
        {
            case ConsoleKey.UpArrow:
                if (selection[0] > 0) selection[0] -= 1;
                break;
            
            case ConsoleKey.DownArrow:
                if (selection[0] < height - 1) selection[0] += 1;
                break;
            
            case ConsoleKey.LeftArrow:
                if (selection[1] > 0) selection[1] -= 1;
                break;
            
            case ConsoleKey.RightArrow:
                if (selection[1] < width - 1) selection[1] += 1;
                break;
        }

        return selection;
    }
}