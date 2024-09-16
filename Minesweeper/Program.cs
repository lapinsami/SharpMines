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
        bool[][] gameFieldMask = InitializeGamefieldMask(width, height);
        gameField = PutBombsOnGamefield(gameField, 60);
        gameField = FillNumbers(gameField);

        int frameCounter = 0;
        
        PrintDisplay(gameField, gameFieldMask, selection);
        
        while (true)
        {
            Console.Clear();
            frameCounter++;
            Console.WriteLine($"Frame {frameCounter}");
            PrintDisplay(gameField, gameFieldMask, selection);
            
            ConsoleKey input = GetInputKey();

            if (input == ConsoleKey.Q)
            {
                break;
            }
            switch (input)
            {
                case ConsoleKey.F:
                    gameField[selection[0]][selection[1]] = '¶';
                    gameFieldMask[selection[0]][selection[1]] = true;
                    break;
                case ConsoleKey.Spacebar:
                    gameFieldMask[selection[0]][selection[1]] = true;
                    break;
                default:
                    selection = UpdateSelection(selection, input, width, height);
                    break;
            }
        }
    }

    private static void PrintSquare(char symbol, bool visible, bool selected = false)
    {
        if (!visible)
        {
            symbol = '#';
        }
        
        switch (symbol)
        {
            case ' ':
                Console.ResetColor();
                break;
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

    private static void PrintLine(char[] line, bool[] mask, int selection, bool selectionOnLine = false)
    {
        for (int j = 0; j < line.Length; j++)
        {
            PrintSquare(line[j], mask[j], selectionOnLine && j == selection);
            
            if (j < line.Length - 1)
            {
                Console.Write(" ");
            }
        }
    }

    private static void PrintDisplay(char[][] lines, bool[][] mask, int[] selection)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            PrintLine(lines[i], mask[i], selection[1], selection[0] == i);
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

    private static bool[][] InitializeGamefieldMask(int width, int height)
    {
        bool[][] rows = new bool[height][];
        
        for (int i = 0; i < height; i++)
        {
            bool[] row = new bool[width];
            for (int j = 0; j < width; j++)
            {
                row[j] = false;
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

    private static char[][] FillNumbers(char[][] gameField)
    {
        for (int row = 0; row < gameField.Length; row++)
        {
            for (int square = 0; square < gameField[0].Length; square++)
            {
                if (gameField[row][square] == 'o')
                {
                    continue;
                }
                
                int[] selection = [row, square];
                int neighboringBombs = GetNumberOfNeighboringBombs(gameField, selection);
                if (neighboringBombs > 0)
                {
                    gameField[row][square] = char.Parse(neighboringBombs.ToString());
                }
            }
        }
        
        return gameField;
    }

    private static int GetNumberOfNeighboringBombs(char[][] gameField, int[] selection)
    {
        int neighboringBombs = 0;
        
        // Above left
        int[] neighbor = [0, 0];
        neighbor[0] = selection[0] - 1;
        neighbor[1] = selection[1] - 1;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Above mid
        neighbor[1]++;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Above right
        neighbor[1]++;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Mid left
        neighbor[0] = selection[0];
        neighbor[1] = selection[1] - 1;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Mid right
        neighbor[0] = selection[0];
        neighbor[1] = selection[1] + 1;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below left
        neighbor[0] = selection[0] + 1;
        neighbor[1] = selection[1] - 1;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below mid
        neighbor[1]++;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        // Below right
        neighbor[1]++;
        if (!SelectionOutOfBounds(gameField, neighbor))
        {
            if (gameField[neighbor[0]][neighbor[1]] == 'o')
            {
                neighboringBombs++;
            }
        }
        
        return neighboringBombs;

    }

    private static bool SelectionOutOfBounds(char[][] gameField, int[] selection)
    {
        return selection[0] < 0 ||
               selection[1] < 0 ||
               selection[0] >= gameField.Length ||
               selection[1] >= gameField[0].Length;
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
                if (selection[0] > 0) selection[0]--;
                break;
            
            case ConsoleKey.DownArrow:
                if (selection[0] < height - 1) selection[0]++;
                break;
            
            case ConsoleKey.LeftArrow:
                if (selection[1] > 0) selection[1]--;
                break;
            
            case ConsoleKey.RightArrow:
                if (selection[1] < width - 1) selection[1]++;
                break;
        }

        return selection;
    }
}